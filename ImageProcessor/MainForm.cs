using ImageProcessor.Models;
using ImageProcessor.Services;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageProcessor;

public partial class MainForm : Form
{
    private readonly ImageService _imageService;
    private readonly PdfExportService _pdfExportService;
    private readonly PdfRenderService _pdfRenderService;
    private static readonly HttpClient _httpClient;

    static MainForm()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) ImageProcessor/1.0");
    }
    private SixLabors.ImageSharp.Image<Rgba32>? _processedImage;
    private string? _currentFilePath;
    private bool _isUpdatingAspectRatio;
    private ImagePreviewForm? _previewForm;

    public MainForm()
    {
        InitializeComponent();
        _imageService = new ImageService();
        _pdfExportService = new PdfExportService();
        _pdfRenderService = new PdfRenderService();
        _previewForm = new ImagePreviewForm();

        // Add mouse hover events for image preview
        pictureBox.MouseEnter += PictureBox_MouseEnter;
        pictureBox.MouseLeave += PictureBox_MouseLeave;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        // Set default values
        cmbRotation.SelectedIndex = 0;
        cmbFormat.SelectedIndex = 0;
        cmbBleedUnit.SelectedIndex = 0; // mm by default
    }

    private async void btnOpen_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            await LoadImageAsync(openFileDialog.FileName);
        }
    }

    private async Task LoadImageAsync(string filePath)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            _currentFilePath = filePath;
            await _imageService.LoadImageAsync(filePath);

            UpdatePreview();
            UpdateImageInfo();
            EnableControls(true);

            lblFileName.Text = Path.GetFileName(filePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnLoadUrl_Click(object sender, EventArgs e)
    {
        var url = txtUrl.Text.Trim();
        if (string.IsNullOrEmpty(url))
        {
            MessageBox.Show("Please enter a URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        await LoadImageFromUrlAsync(url);
    }

    private async Task LoadImageFromUrlAsync(string url)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            lblFileName.Text = "Downloading...";
            Application.DoEvents();

            // Validate URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
            {
                MessageBox.Show("Invalid URL format. Please enter a complete URL starting with http:// or https://",
                    "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblFileName.Text = "No file loaded";
                return;
            }

            // Check scheme
            if (uri.Scheme != "http" && uri.Scheme != "https")
            {
                MessageBox.Show("URL must start with http:// or https://",
                    "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblFileName.Text = "No file loaded";
                return;
            }

            // Download the image
            var response = await _httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Server returned: {(int)response.StatusCode} {response.ReasonPhrase}",
                    "Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblFileName.Text = "No file loaded";
                return;
            }

            // Determine file extension from content type or URL
            var contentType = response.Content.Headers.ContentType?.MediaType;
            var extension = GetExtensionFromContentType(contentType)
                           ?? Path.GetExtension(uri.LocalPath)
                           ?? ".png";

            // Save to temp file
            var tempPath = Path.Combine(Path.GetTempPath(), $"url_image_{Guid.NewGuid()}{extension}");
            var imageBytes = await response.Content.ReadAsByteArrayAsync();

            if (imageBytes.Length == 0)
            {
                MessageBox.Show("Downloaded file is empty.", "Download Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblFileName.Text = "No file loaded";
                return;
            }

            await File.WriteAllBytesAsync(tempPath, imageBytes);

            // Check if it's a PDF and render it first
            if (PdfRenderService.IsPdf(extension) || PdfRenderService.IsPdf(contentType))
            {
                lblFileName.Text = "Rendering PDF...";
                Application.DoEvents();

                // Render PDF to image
                var pngPath = Path.Combine(Path.GetTempPath(), $"pdf_render_{Guid.NewGuid()}.png");
                await _pdfRenderService.RenderFirstPageToFileAsync(tempPath, pngPath, 2.0);

                // Load the rendered image
                await LoadImageAsync(pngPath);

                var pageCount = _pdfRenderService.GetPageCount(tempPath);
                var fileName = Path.GetFileName(uri.LocalPath);
                if (string.IsNullOrEmpty(fileName)) fileName = "document.pdf";
                lblFileName.Text = $"PDF: {fileName} (page 1 of {pageCount})";
            }
            else
            {
                // Load using existing method
                await LoadImageAsync(tempPath);

                var fileName = Path.GetFileName(uri.LocalPath);
                if (string.IsNullOrEmpty(fileName)) fileName = "image" + extension;
                lblFileName.Text = $"URL: {fileName}";
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"Failed to download image:\n{ex.Message}", "Download Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblFileName.Text = "No file loaded";
        }
        catch (TaskCanceledException)
        {
            MessageBox.Show("Download timed out. Please try again.", "Timeout",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            lblFileName.Text = "No file loaded";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading image from URL:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblFileName.Text = "No file loaded";
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private static string? GetExtensionFromContentType(string? contentType)
    {
        return contentType?.ToLower() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/bmp" => ".bmp",
            "image/webp" => ".webp",
            "application/pdf" => ".pdf",
            _ => null
        };
    }

    private void UpdatePreview()
    {
        if (!_imageService.HasImage)
            return;

        pictureBox.Image?.Dispose();

        var bitmap = _imageService.ToWinFormsBitmap();
        if (bitmap != null)
        {
            pictureBox.Image = bitmap;
            CenterPreviewImage();
        }
    }

    private void UpdatePreviewWithProcessed()
    {
        if (_processedImage == null)
            return;

        pictureBox.Image?.Dispose();
        pictureBox.Image = ImageService.ToWinFormsBitmap(_processedImage);
        CenterPreviewImage();
    }

    private void CenterPreviewImage()
    {
        if (pictureBox.Image == null)
            return;

        // Adjust PictureBox size mode based on image size
        if (pictureBox.Image.Width > previewPanel.ClientSize.Width ||
            pictureBox.Image.Height > previewPanel.ClientSize.Height)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Dock = DockStyle.Fill;
        }
        else
        {
            pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox.Dock = DockStyle.Fill;
        }
    }

    private void UpdateImageInfo()
    {
        if (_imageService.HasImage)
        {
            lblImageInfo.Text = $"{_imageService.Width} x {_imageService.Height} pixels";

            // Pre-fill resize dimensions
            if (string.IsNullOrEmpty(txtWidth.Text))
                txtWidth.Text = _imageService.Width.ToString();
            if (string.IsNullOrEmpty(txtHeight.Text))
                txtHeight.Text = _imageService.Height.ToString();
        }
        else
        {
            lblImageInfo.Text = "";
        }
    }

    private void EnableControls(bool enabled)
    {
        btnSave.Enabled = enabled;
        btnSaveExport.Enabled = enabled;
        btnApply.Enabled = enabled;
        btnReset.Enabled = enabled;
        grpResize.Enabled = enabled;
        grpBleed.Enabled = enabled;
        grpFlip.Enabled = enabled;
        grpRotation.Enabled = enabled;
        grpOutput.Enabled = enabled;
    }

    private ProcessingOptions GetCurrentOptions()
    {
        var options = new ProcessingOptions
        {
            MaintainAspectRatio = chkMaintainRatio.Checked,
            BleedWidth = (double)numBleedWidth.Value,
            BleedHeight = (double)numBleedHeight.Value,
            BleedUnit = GetBleedUnit(),
            Dpi = (int)numDpi.Value,
            MirrorBleed = chkMirrorBleed.Checked,
            FlipHorizontal = chkFlipHorizontal.Checked,
            FlipVertical = chkFlipVertical.Checked,
            JpegQuality = (int)numQuality.Value,
            OutputFormat = GetOutputFormat()
        };

        // Parse width
        if (int.TryParse(txtWidth.Text, out int width) && width > 0)
        {
            options.TargetWidth = width;
        }

        // Parse height
        if (int.TryParse(txtHeight.Text, out int height) && height > 0)
        {
            options.TargetHeight = height;
        }

        // Get rotation angle
        options.RotationAngle = GetRotationAngle();

        return options;
    }

    private BleedUnit GetBleedUnit()
    {
        return cmbBleedUnit.SelectedIndex switch
        {
            0 => BleedUnit.Millimeters,
            1 => BleedUnit.Pixels,
            2 => BleedUnit.Inches,
            _ => BleedUnit.Millimeters
        };
    }

    private OutputFormat GetOutputFormat()
    {
        return cmbFormat.SelectedIndex switch
        {
            0 => OutputFormat.Png,
            1 => OutputFormat.Jpeg,
            2 => OutputFormat.Pdf,
            _ => OutputFormat.Png
        };
    }

    private float GetRotationAngle()
    {
        if (cmbRotation.SelectedIndex == 4) // Custom
        {
            return (float)numCustomAngle.Value;
        }

        return cmbRotation.SelectedIndex switch
        {
            1 => 90f,
            2 => 180f,
            3 => 270f,
            _ => 0f
        };
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
        if (!_imageService.HasImage)
            return;

        try
        {
            Cursor = Cursors.WaitCursor;

            var options = GetCurrentOptions();

            _processedImage?.Dispose();
            _processedImage = _imageService.ProcessImage(options);

            UpdatePreviewWithProcessed();

            lblImageInfo.Text = $"{_processedImage.Width} x {_processedImage.Height} pixels (processed)";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing image: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        _processedImage?.Dispose();
        _processedImage = null;

        // Reset controls
        txtWidth.Text = _imageService.Width.ToString();
        txtHeight.Text = _imageService.Height.ToString();
        numBleedWidth.Value = 0;
        numBleedHeight.Value = 0;
        cmbBleedUnit.SelectedIndex = 0;
        chkMirrorBleed.Checked = true;
        chkFlipHorizontal.Checked = false;
        chkFlipVertical.Checked = false;
        cmbRotation.SelectedIndex = 0;
        numCustomAngle.Value = 0;

        UpdatePreview();
        UpdateImageInfo();
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        var imageToSave = _processedImage ?? _imageService.CloneCurrentImage();
        if (imageToSave == null)
            return;

        try
        {
            var options = GetCurrentOptions();

            // Set default file extension based on format
            saveFileDialog.FilterIndex = options.OutputFormat switch
            {
                OutputFormat.Png => 1,
                OutputFormat.Jpeg => 2,
                OutputFormat.Pdf => 3,
                _ => 1
            };
            saveFileDialog.FileName = Path.GetFileNameWithoutExtension(_currentFilePath ?? "image");

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                if (options.OutputFormat == OutputFormat.Pdf)
                {
                    // Export to PDF
                    _pdfExportService.ExportToPdf(imageToSave, saveFileDialog.FileName, options.Dpi);
                }
                else
                {
                    // Export to image format
                    await _imageService.SaveImageAsync(imageToSave, saveFileDialog.FileName, options);
                }

                MessageBox.Show("File saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving file: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;

            // Dispose if it was a clone (not processed image)
            if (_processedImage == null)
                imageToSave.Dispose();
        }
    }

    private void txtWidth_TextChanged(object sender, EventArgs e)
    {
        if (_isUpdatingAspectRatio || !_imageService.HasImage || !chkMaintainRatio.Checked)
            return;

        if (int.TryParse(txtWidth.Text, out int width) && width > 0)
        {
            _isUpdatingAspectRatio = true;
            double aspectRatio = (double)_imageService.Width / _imageService.Height;
            txtHeight.Text = ((int)(width / aspectRatio)).ToString();
            _isUpdatingAspectRatio = false;
        }
    }

    private void txtHeight_TextChanged(object sender, EventArgs e)
    {
        if (_isUpdatingAspectRatio || !_imageService.HasImage || !chkMaintainRatio.Checked)
            return;

        if (int.TryParse(txtHeight.Text, out int height) && height > 0)
        {
            _isUpdatingAspectRatio = true;
            double aspectRatio = (double)_imageService.Width / _imageService.Height;
            txtWidth.Text = ((int)(height * aspectRatio)).ToString();
            _isUpdatingAspectRatio = false;
        }
    }

    private void cmbRotation_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Enable custom angle input only when "Custom" is selected
        numCustomAngle.Enabled = cmbRotation.SelectedIndex == 4;
    }

    private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Enable quality setting only for JPEG
        numQuality.Enabled = cmbFormat.SelectedIndex == 1;
    }

    private void PictureBox_MouseEnter(object? sender, EventArgs e)
    {
        if (pictureBox.Image == null) return;

        var cursorPos = Cursor.Position;
        var maxSize = new Size(800, 600); // Maximum preview size

        _previewForm?.ShowPreview(pictureBox.Image, cursorPos, maxSize);
    }

    private void PictureBox_MouseLeave(object? sender, EventArgs e)
    {
        // Preview form handles its own closing via mouse tracking
        // This allows smooth transition when moving to the preview window
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _previewForm?.Close();
        _previewForm?.Dispose();
        _imageService.Dispose();
        _pdfRenderService.Dispose();
        _processedImage?.Dispose();
        pictureBox.Image?.Dispose();
        base.OnFormClosing(e);
    }
}

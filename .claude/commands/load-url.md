Load an image file from a URL into the Windows Forms ImageProcessor application.

## Instructions

When the user provides a URL to an image file, perform the following steps:

1. **Add URL loading capability to MainForm.cs** by adding these elements:
   - A TextBox for entering the URL (`txtUrl`)
   - A Button to trigger URL loading (`btnLoadUrl`)
   - An async method `LoadImageFromUrlAsync(string url)` that:
     - Downloads the image using `HttpClient`
     - Saves it to a temp file
     - Calls the existing `LoadImageAsync(tempFilePath)` method

2. **Implementation pattern for URL loading**:

```csharp
private static readonly HttpClient _httpClient = new HttpClient();

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

        // Validate URL
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            MessageBox.Show("Invalid URL format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Download the image
        var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        // Determine file extension from content type or URL
        var extension = GetExtensionFromContentType(response.Content.Headers.ContentType?.MediaType)
                       ?? Path.GetExtension(uri.LocalPath)
                       ?? ".png";

        // Save to temp file
        var tempPath = Path.Combine(Path.GetTempPath(), $"url_image_{Guid.NewGuid()}{extension}");
        var imageBytes = await response.Content.ReadAsByteArrayAsync();
        await File.WriteAllBytesAsync(tempPath, imageBytes);

        // Load using existing method
        await LoadImageAsync(tempPath);

        lblFileName.Text = $"Loaded from URL: {Path.GetFileName(uri.LocalPath)}";
    }
    catch (HttpRequestException ex)
    {
        MessageBox.Show($"Failed to download image: {ex.Message}", "Download Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error loading image from URL: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        _ => null
    };
}
```

3. **UI changes in MainForm.Designer.cs**:
   - Add a TextBox `txtUrl` with placeholder text
   - Add a Button `btnLoadUrl` with text "Load URL"
   - Position them in the `topPanel` next to the existing Open button

4. **Designer code to add**:

```csharp
// In field declarations
private TextBox txtUrl;
private Button btnLoadUrl;

// In InitializeComponent()
this.txtUrl = new TextBox();
this.btnLoadUrl = new Button();

// txtUrl
this.txtUrl.Location = new Point(120, 14);
this.txtUrl.Name = "txtUrl";
this.txtUrl.Size = new Size(400, 23);
this.txtUrl.PlaceholderText = "Enter image URL...";
this.txtUrl.TabIndex = 1;

// btnLoadUrl
this.btnLoadUrl.Location = new Point(530, 12);
this.btnLoadUrl.Name = "btnLoadUrl";
this.btnLoadUrl.Size = new Size(80, 28);
this.btnLoadUrl.TabIndex = 2;
this.btnLoadUrl.Text = "Load URL";
this.btnLoadUrl.UseVisualStyleBackColor = true;
this.btnLoadUrl.Click += new EventHandler(this.btnLoadUrl_Click);

// Add to topPanel.Controls
this.topPanel.Controls.Add(this.txtUrl);
this.topPanel.Controls.Add(this.btnLoadUrl);

// Adjust lblFileName position
this.lblFileName.Location = new Point(620, 18);
```

## Usage

After implementing, users can:
1. Paste an image URL into the text box
2. Click "Load URL" to download and load the image
3. Process the image using existing tools (resize, bleed, rotate, etc.)

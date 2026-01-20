namespace ImageProcessor;

/// <summary>
/// A borderless preview window that displays an enlarged image when hovering over the main preview.
/// </summary>
public class ImagePreviewForm : Form
{
    private readonly PictureBox _pictureBox;
    private System.Windows.Forms.Timer? _closeTimer;

    public ImagePreviewForm()
    {
        // Form settings for a floating preview window
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.ShowInTaskbar = false;
        this.TopMost = true;
        this.BackColor = Color.FromArgb(30, 30, 30);
        this.Padding = new Padding(3);

        // Add a subtle border effect
        this.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(100, 100, 100), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
        };

        // PictureBox for displaying the image
        _pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(30, 30, 30),
            Margin = new Padding(3)
        };

        this.Controls.Add(_pictureBox);

        // Close when mouse leaves the preview window
        this.MouseLeave += (s, e) => ScheduleCloseCheck();
        _pictureBox.MouseLeave += (s, e) => ScheduleCloseCheck();

        // Allow clicking the preview to close it
        _pictureBox.Click += (s, e) => this.Hide();
        this.Click += (s, e) => this.Hide();
    }

    /// <summary>
    /// Shows the preview window with the specified image.
    /// </summary>
    /// <param name="image">The image to display.</param>
    /// <param name="location">The cursor position to position the window near.</param>
    /// <param name="maxSize">Maximum size for the preview window.</param>
    public void ShowPreview(Image image, Point location, Size maxSize)
    {
        if (image == null) return;

        // Cancel any pending close
        _closeTimer?.Stop();

        // Calculate size maintaining aspect ratio
        var aspectRatio = (double)image.Width / image.Height;
        int width = Math.Min(image.Width, maxSize.Width);
        int height = (int)(width / aspectRatio);

        if (height > maxSize.Height)
        {
            height = maxSize.Height;
            width = (int)(height * aspectRatio);
        }

        // Ensure minimum size for small images
        width = Math.Max(width, 250);
        height = Math.Max(height, 200);

        // Add padding for border
        this.Size = new Size(width + 6, height + 6);

        // Position the window near the cursor but keep on screen
        var screen = Screen.FromPoint(location);
        int x = location.X + 25;
        int y = location.Y + 25;

        // Adjust if would go off right edge
        if (x + this.Width > screen.WorkingArea.Right)
            x = location.X - this.Width - 15;

        // Adjust if would go off bottom edge
        if (y + this.Height > screen.WorkingArea.Bottom)
            y = location.Y - this.Height - 15;

        // Ensure we don't go off the left or top
        this.Location = new Point(
            Math.Max(screen.WorkingArea.Left, x),
            Math.Max(screen.WorkingArea.Top, y)
        );

        _pictureBox.Image = image;

        if (!this.Visible)
        {
            this.Show();
        }
    }

    /// <summary>
    /// Hides the preview window.
    /// </summary>
    public void HidePreview()
    {
        _closeTimer?.Stop();
        this.Hide();
    }

    private void ScheduleCloseCheck()
    {
        // Use a small delay to prevent flickering when moving between controls
        _closeTimer?.Stop();
        _closeTimer?.Dispose();
        _closeTimer = new System.Windows.Forms.Timer { Interval = 150 };
        _closeTimer.Tick += (s, e) =>
        {
            _closeTimer?.Stop();
            // Only close if mouse is not over this window
            if (!this.Bounds.Contains(Cursor.Position))
            {
                this.Hide();
            }
        };
        _closeTimer.Start();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _closeTimer?.Stop();
        _closeTimer?.Dispose();
        _pictureBox.Image = null; // Don't dispose - it's shared with main form
        base.OnFormClosing(e);
    }

    // Prevent the form from stealing focus
    protected override bool ShowWithoutActivation => true;

    // Make the window non-focusable
    protected override CreateParams CreateParams
    {
        get
        {
            const int WS_EX_NOACTIVATE = 0x08000000;
            const int WS_EX_TOOLWINDOW = 0x00000080;
            var cp = base.CreateParams;
            cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
            return cp;
        }
    }
}

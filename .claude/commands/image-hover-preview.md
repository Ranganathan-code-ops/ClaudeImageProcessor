Open a new preview window when the mouse hovers over an image in the Windows Forms ImageProcessor application.

## Instructions

When implementing the image hover preview feature, follow these steps:

### 1. Create a new Preview Form (ImagePreviewForm.cs)

Create a new form that displays the image in a larger, separate window:

```csharp
namespace ImageProcessor;

public class ImagePreviewForm : Form
{
    private readonly PictureBox _pictureBox;
    private System.Windows.Forms.Timer? _closeTimer;

    public ImagePreviewForm()
    {
        // Form settings
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.ShowInTaskbar = false;
        this.TopMost = true;
        this.BackColor = Color.Black;
        this.Padding = new Padding(2);

        // PictureBox for displaying the image
        _pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Black
        };

        this.Controls.Add(_pictureBox);

        // Close when mouse leaves the preview window
        this.MouseLeave += (s, e) => CheckAndClose();
        _pictureBox.MouseLeave += (s, e) => CheckAndClose();
    }

    public void ShowPreview(Image image, Point location, Size maxSize)
    {
        if (image == null) return;

        // Calculate size maintaining aspect ratio
        var aspectRatio = (double)image.Width / image.Height;
        int width = Math.Min(image.Width, maxSize.Width);
        int height = (int)(width / aspectRatio);

        if (height > maxSize.Height)
        {
            height = maxSize.Height;
            width = (int)(height * aspectRatio);
        }

        // Ensure minimum size
        width = Math.Max(width, 200);
        height = Math.Max(height, 150);

        this.Size = new Size(width + 4, height + 4); // +4 for border padding

        // Position the window near the cursor but keep on screen
        var screen = Screen.FromPoint(location);
        int x = location.X + 20;
        int y = location.Y + 20;

        // Adjust if would go off screen
        if (x + this.Width > screen.WorkingArea.Right)
            x = location.X - this.Width - 10;
        if (y + this.Height > screen.WorkingArea.Bottom)
            y = location.Y - this.Height - 10;

        this.Location = new Point(
            Math.Max(screen.WorkingArea.Left, x),
            Math.Max(screen.WorkingArea.Top, y)
        );

        _pictureBox.Image = image;
        this.Show();
    }

    private void CheckAndClose()
    {
        // Use a small delay to prevent flickering
        _closeTimer?.Stop();
        _closeTimer?.Dispose();
        _closeTimer = new System.Windows.Forms.Timer { Interval = 100 };
        _closeTimer.Tick += (s, e) =>
        {
            _closeTimer?.Stop();
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
        base.OnFormClosing(e);
    }
}
```

### 2. Update MainForm.cs

Add the preview form and mouse event handlers:

```csharp
// Add field
private ImagePreviewForm? _previewForm;

// In constructor, after InitializeComponent():
_previewForm = new ImagePreviewForm();

// Add event handlers for pictureBox (in InitializeComponent or MainForm constructor):
pictureBox.MouseEnter += PictureBox_MouseEnter;
pictureBox.MouseLeave += PictureBox_MouseLeave;

// Add these methods to MainForm:
private void PictureBox_MouseEnter(object sender, EventArgs e)
{
    if (pictureBox.Image == null) return;

    var cursorPos = Cursor.Position;
    var maxSize = new Size(800, 600); // Maximum preview size

    _previewForm?.ShowPreview(pictureBox.Image, cursorPos, maxSize);
}

private void PictureBox_MouseLeave(object sender, EventArgs e)
{
    // Preview form handles its own closing via mouse tracking
}

// Update OnFormClosing:
protected override void OnFormClosing(FormClosingEventArgs e)
{
    _previewForm?.Close();
    _previewForm?.Dispose();
    // ... existing dispose code
}
```

### 3. Designer Updates (MainForm.Designer.cs)

Add event subscriptions in InitializeComponent():

```csharp
// pictureBox
this.pictureBox.MouseEnter += new EventHandler(this.PictureBox_MouseEnter);
this.pictureBox.MouseLeave += new EventHandler(this.PictureBox_MouseLeave);
```

## Features

- **Hover to Preview**: Mouse over the image to open a larger preview window
- **Auto-Position**: Preview window positions itself near cursor, staying on screen
- **Auto-Close**: Preview closes when mouse leaves both the main image and preview
- **Aspect Ratio**: Preview maintains original image aspect ratio
- **Maximum Size**: Preview limited to 800x600 to avoid filling entire screen

## Usage

After implementing, users can:
1. Load an image (from file or URL)
2. Hover mouse over the preview area
3. A larger preview window appears near the cursor
4. Move mouse away to close the preview

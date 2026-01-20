namespace ImageProcessor.Models;

/// <summary>
/// Represents the user-selected image processing options.
/// </summary>
public class ProcessingOptions
{
    // Resize options
    public int? TargetWidth { get; set; }
    public int? TargetHeight { get; set; }
    public bool MaintainAspectRatio { get; set; } = true;

    // Bleed options - separate width (left/right) and height (top/bottom)
    public double BleedWidth { get; set; } = 0;  // Left + Right bleed
    public double BleedHeight { get; set; } = 0; // Top + Bottom bleed
    public BleedUnit BleedUnit { get; set; } = BleedUnit.Millimeters;
    public int Dpi { get; set; } = 300;
    public bool MirrorBleed { get; set; } = true;

    // Mirror/Flip options
    public bool FlipHorizontal { get; set; } = false;
    public bool FlipVertical { get; set; } = false;

    // Rotation options
    public float RotationAngle { get; set; } = 0;

    // Output options
    public OutputFormat OutputFormat { get; set; } = OutputFormat.Png;
    public int JpegQuality { get; set; } = 90;

    /// <summary>
    /// Calculates bleed width (left/right) in pixels based on unit and DPI.
    /// </summary>
    public int BleedWidthPixels
    {
        get
        {
            if (BleedWidth <= 0) return 0;

            return BleedUnit switch
            {
                BleedUnit.Pixels => (int)BleedWidth,
                BleedUnit.Millimeters => (int)Math.Round(BleedWidth * Dpi / 25.4),
                BleedUnit.Inches => (int)Math.Round(BleedWidth * Dpi),
                _ => (int)BleedWidth
            };
        }
    }

    /// <summary>
    /// Calculates bleed height (top/bottom) in pixels based on unit and DPI.
    /// </summary>
    public int BleedHeightPixels
    {
        get
        {
            if (BleedHeight <= 0) return 0;

            return BleedUnit switch
            {
                BleedUnit.Pixels => (int)BleedHeight,
                BleedUnit.Millimeters => (int)Math.Round(BleedHeight * Dpi / 25.4),
                BleedUnit.Inches => (int)Math.Round(BleedHeight * Dpi),
                _ => (int)BleedHeight
            };
        }
    }

    /// <summary>
    /// Returns true if any bleed is configured.
    /// </summary>
    public bool HasBleed => BleedWidth > 0 || BleedHeight > 0;

    /// <summary>
    /// Returns true if any processing operation is configured.
    /// </summary>
    public bool HasOperations =>
        TargetWidth.HasValue ||
        TargetHeight.HasValue ||
        HasBleed ||
        FlipHorizontal ||
        FlipVertical ||
        Math.Abs(RotationAngle) > 0.01f;
}

public enum OutputFormat
{
    Jpeg,
    Png,
    Pdf
}

public enum BleedUnit
{
    Pixels,
    Millimeters,
    Inches
}

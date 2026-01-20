using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageProcessor.Services;

/// <summary>
/// Provides PDF export functionality for processed images.
/// </summary>
public class PdfExportService
{
    /// <summary>
    /// Exports an image to a PDF file.
    /// </summary>
    /// <param name="image">The image to export.</param>
    /// <param name="filePath">The output PDF file path.</param>
    /// <param name="dpi">The DPI setting for the image (affects physical size calculation).</param>
    public void ExportToPdf(Image<Rgba32> image, string filePath, int dpi = 300)
    {
        using var document = new PdfDocument();
        document.Info.Title = "Image Export";
        document.Info.Creator = "Image Processor";

        // Calculate page size based on image dimensions and DPI
        // Convert pixels to points (72 points = 1 inch)
        double widthInInches = (double)image.Width / dpi;
        double heightInInches = (double)image.Height / dpi;
        double widthInPoints = widthInInches * 72;
        double heightInPoints = heightInInches * 72;

        // Create page with image dimensions
        var page = document.AddPage();
        page.Width = XUnit.FromPoint(widthInPoints);
        page.Height = XUnit.FromPoint(heightInPoints);

        using var gfx = XGraphics.FromPdfPage(page);

        // Save image to temp file for PDFsharp (it needs a file path or stream)
        var tempPath = Path.Combine(Path.GetTempPath(), $"img_export_{Guid.NewGuid()}.png");
        try
        {
            image.SaveAsPng(tempPath);

            using var xImage = XImage.FromFile(tempPath);
            gfx.DrawImage(xImage, 0, 0, page.Width.Point, page.Height.Point);
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }

        document.Save(filePath);
    }

    /// <summary>
    /// Exports an image to a PDF file with custom page size.
    /// </summary>
    /// <param name="image">The image to export.</param>
    /// <param name="filePath">The output PDF file path.</param>
    /// <param name="pageWidthMm">Page width in millimeters.</param>
    /// <param name="pageHeightMm">Page height in millimeters.</param>
    /// <param name="fitToPage">If true, scales image to fit page; otherwise uses actual size.</param>
    public void ExportToPdf(Image<Rgba32> image, string filePath, double pageWidthMm, double pageHeightMm, bool fitToPage = true)
    {
        using var document = new PdfDocument();
        document.Info.Title = "Image Export";
        document.Info.Creator = "Image Processor";

        // Convert mm to points (1 mm = 2.834645669 points)
        double widthInPoints = pageWidthMm * 2.834645669;
        double heightInPoints = pageHeightMm * 2.834645669;

        var page = document.AddPage();
        page.Width = XUnit.FromPoint(widthInPoints);
        page.Height = XUnit.FromPoint(heightInPoints);

        using var gfx = XGraphics.FromPdfPage(page);

        var tempPath = Path.Combine(Path.GetTempPath(), $"img_export_{Guid.NewGuid()}.png");
        try
        {
            image.SaveAsPng(tempPath);

            using var xImage = XImage.FromFile(tempPath);

            if (fitToPage)
            {
                // Calculate scaling to fit within page while maintaining aspect ratio
                double imageAspect = (double)image.Width / image.Height;
                double pageAspect = widthInPoints / heightInPoints;

                double drawWidth, drawHeight, drawX, drawY;

                if (imageAspect > pageAspect)
                {
                    // Image is wider - fit to width
                    drawWidth = widthInPoints;
                    drawHeight = widthInPoints / imageAspect;
                    drawX = 0;
                    drawY = (heightInPoints - drawHeight) / 2;
                }
                else
                {
                    // Image is taller - fit to height
                    drawHeight = heightInPoints;
                    drawWidth = heightInPoints * imageAspect;
                    drawX = (widthInPoints - drawWidth) / 2;
                    drawY = 0;
                }

                gfx.DrawImage(xImage, drawX, drawY, drawWidth, drawHeight);
            }
            else
            {
                // Center image on page at actual size
                double drawX = (widthInPoints - xImage.PointWidth) / 2;
                double drawY = (heightInPoints - xImage.PointHeight) / 2;
                gfx.DrawImage(xImage, drawX, drawY);
            }
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }

        document.Save(filePath);
    }
}

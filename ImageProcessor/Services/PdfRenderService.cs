using Docnet.Core;
using Docnet.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageProcessor.Services;

/// <summary>
/// Service to render PDF pages as images using Docnet.Core (PDFium wrapper).
/// </summary>
public class PdfRenderService : IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Renders the first page of a PDF file as an image.
    /// </summary>
    /// <param name="pdfPath">Path to the PDF file.</param>
    /// <param name="scale">Scale factor for rendering (1.0 = 72 DPI, 2.0 = 144 DPI, etc.).</param>
    /// <returns>The rendered page as an ImageSharp Image.</returns>
    public Image<Rgba32> RenderFirstPage(string pdfPath, double scale = 2.0)
    {
        return RenderPage(pdfPath, 0, scale);
    }

    /// <summary>
    /// Renders a specific page of a PDF file as an image.
    /// </summary>
    /// <param name="pdfPath">Path to the PDF file.</param>
    /// <param name="pageIndex">Zero-based page index.</param>
    /// <param name="scale">Scale factor for rendering (1.0 = 72 DPI, 2.0 = 144 DPI, etc.).</param>
    /// <returns>The rendered page as an ImageSharp Image.</returns>
    public Image<Rgba32> RenderPage(string pdfPath, int pageIndex, double scale = 2.0)
    {
        using var docReader = DocLib.Instance.GetDocReader(pdfPath, new PageDimensions(scale));

        if (pageIndex < 0 || pageIndex >= docReader.GetPageCount())
            throw new ArgumentOutOfRangeException(nameof(pageIndex), $"Page index must be between 0 and {docReader.GetPageCount() - 1}");

        using var pageReader = docReader.GetPageReader(pageIndex);

        var width = pageReader.GetPageWidth();
        var height = pageReader.GetPageHeight();
        var rawBytes = pageReader.GetImage();

        // Docnet returns BGRA format, we need to convert to RGBA
        var image = new Image<Rgba32>(width, height);

        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < height; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < width; x++)
                {
                    var offset = (y * width + x) * 4;
                    var b = rawBytes[offset];
                    var g = rawBytes[offset + 1];
                    var r = rawBytes[offset + 2];
                    var a = rawBytes[offset + 3];
                    row[x] = new Rgba32(r, g, b, a);
                }
            }
        });

        return image;
    }

    /// <summary>
    /// Gets the number of pages in a PDF file.
    /// </summary>
    public int GetPageCount(string pdfPath)
    {
        using var docReader = DocLib.Instance.GetDocReader(pdfPath, new PageDimensions(1.0));
        return docReader.GetPageCount();
    }

    /// <summary>
    /// Renders the first page and saves it as an image file.
    /// </summary>
    /// <param name="pdfPath">Path to the PDF file.</param>
    /// <param name="outputPath">Path for the output image.</param>
    /// <param name="scale">Scale factor for rendering.</param>
    public async Task RenderFirstPageToFileAsync(string pdfPath, string outputPath, double scale = 2.0)
    {
        using var image = RenderFirstPage(pdfPath, scale);
        await image.SaveAsPngAsync(outputPath);
    }

    /// <summary>
    /// Checks if a file is a PDF based on extension or content type.
    /// </summary>
    public static bool IsPdf(string pathOrContentType)
    {
        if (string.IsNullOrEmpty(pathOrContentType))
            return false;

        var lower = pathOrContentType.ToLowerInvariant();
        return lower.EndsWith(".pdf") || lower == "application/pdf";
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

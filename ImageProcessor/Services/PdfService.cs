using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ImageProcessor.Services;

/// <summary>
/// Provides PDF handling functionality.
/// Note: PDFsharp does not support rendering PDF pages to images directly.
/// This service provides PDF metadata and page information.
/// For full PDF to image conversion, consider using PDFium or similar libraries.
/// </summary>
public class PdfService : IDisposable
{
    private PdfDocument? _document;
    private bool _disposed;

    /// <summary>
    /// Gets the number of pages in the loaded PDF.
    /// </summary>
    public int PageCount => _document?.PageCount ?? 0;

    /// <summary>
    /// Gets whether a PDF is currently loaded.
    /// </summary>
    public bool HasDocument => _document != null;

    /// <summary>
    /// Loads a PDF document from the specified file path.
    /// </summary>
    public void LoadPdf(string filePath)
    {
        _document?.Dispose();
        _document = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
    }

    /// <summary>
    /// Gets information about a specific page.
    /// </summary>
    public PdfPageInfo? GetPageInfo(int pageIndex)
    {
        if (_document == null || pageIndex < 0 || pageIndex >= _document.PageCount)
            return null;

        var page = _document.Pages[pageIndex];
        return new PdfPageInfo
        {
            PageNumber = pageIndex + 1,
            Width = page.Width.Point,
            Height = page.Height.Point,
            Orientation = page.Width > page.Height ? PageOrientation.Landscape : PageOrientation.Portrait
        };
    }

    /// <summary>
    /// Checks if a file is a PDF based on its extension.
    /// </summary>
    public static bool IsPdfFile(string filePath)
    {
        return Path.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _document?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Information about a PDF page.
/// </summary>
public class PdfPageInfo
{
    public int PageNumber { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public PageOrientation Orientation { get; set; }
}

public enum PageOrientation
{
    Portrait,
    Landscape
}

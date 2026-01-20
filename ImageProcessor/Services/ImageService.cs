using ImageProcessor.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SharpImage = SixLabors.ImageSharp.Image;
using SharpSize = SixLabors.ImageSharp.Size;
using SharpPoint = SixLabors.ImageSharp.Point;

namespace ImageProcessor.Services;

/// <summary>
/// Provides image processing operations including resize, bleed, flip, and rotation.
/// </summary>
public class ImageService : IDisposable
{
    private Image<Rgba32>? _currentImage;
    private bool _disposed;

    public int Width => _currentImage?.Width ?? 0;
    public int Height => _currentImage?.Height ?? 0;
    public bool HasImage => _currentImage != null;

    public async Task LoadImageAsync(string filePath)
    {
        _currentImage?.Dispose();
        _currentImage = await SharpImage.LoadAsync<Rgba32>(filePath);
    }

    public async Task LoadImageAsync(Stream stream)
    {
        _currentImage?.Dispose();
        _currentImage = await SharpImage.LoadAsync<Rgba32>(stream);
    }

    public Image<Rgba32>? CloneCurrentImage()
    {
        return _currentImage?.Clone();
    }

    /// <summary>
    /// Applies all processing options and returns a new processed image.
    /// </summary>
    public Image<Rgba32> ProcessImage(ProcessingOptions options)
    {
        if (_currentImage == null)
            throw new InvalidOperationException("No image loaded.");

        var processed = _currentImage.Clone();

        // Apply resize if specified
        if (options.TargetWidth.HasValue || options.TargetHeight.HasValue)
        {
            processed = ResizeImage(processed, options.TargetWidth, options.TargetHeight, options.MaintainAspectRatio);
        }

        // Apply flip/mirror if specified
        if (options.FlipHorizontal || options.FlipVertical)
        {
            processed = FlipImage(processed, options.FlipHorizontal, options.FlipVertical);
        }

        // Apply bleed if specified (separate width and height)
        if (options.HasBleed)
        {
            processed = AddBleed(processed, options.BleedWidthPixels, options.BleedHeightPixels, options.MirrorBleed);
        }

        // Apply rotation if specified
        if (Math.Abs(options.RotationAngle) > 0.01f)
        {
            processed = RotateImage(processed, options.RotationAngle);
        }

        return processed;
    }

    private Image<Rgba32> ResizeImage(Image<Rgba32> image, int? targetWidth, int? targetHeight, bool maintainAspectRatio)
    {
        int newWidth = targetWidth ?? image.Width;
        int newHeight = targetHeight ?? image.Height;

        if (maintainAspectRatio)
        {
            double aspectRatio = (double)image.Width / image.Height;

            if (targetWidth.HasValue && !targetHeight.HasValue)
            {
                newHeight = (int)(newWidth / aspectRatio);
            }
            else if (targetHeight.HasValue && !targetWidth.HasValue)
            {
                newWidth = (int)(newHeight * aspectRatio);
            }
            else if (targetWidth.HasValue && targetHeight.HasValue)
            {
                double targetAspect = (double)targetWidth.Value / targetHeight.Value;
                if (aspectRatio > targetAspect)
                {
                    newHeight = (int)(newWidth / aspectRatio);
                }
                else
                {
                    newWidth = (int)(newHeight * aspectRatio);
                }
            }
        }

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new SharpSize(newWidth, newHeight),
            Mode = ResizeMode.Stretch,
            Sampler = KnownResamplers.Lanczos3
        }));

        return image;
    }

    private Image<Rgba32> FlipImage(Image<Rgba32> image, bool horizontal, bool vertical)
    {
        image.Mutate(ctx =>
        {
            if (horizontal)
                ctx.Flip(FlipMode.Horizontal);
            if (vertical)
                ctx.Flip(FlipMode.Vertical);
        });
        return image;
    }

    /// <summary>
    /// Adds bleed to all four sides of the image.
    /// </summary>
    /// <param name="source">Source image</param>
    /// <param name="bleedWidth">Bleed for left and right sides (each side gets this amount)</param>
    /// <param name="bleedHeight">Bleed for top and bottom sides (each side gets this amount)</param>
    /// <param name="mirror">If true, mirror edge pixels; otherwise extend edge pixels</param>
    private Image<Rgba32> AddBleed(Image<Rgba32> source, int bleedWidth, int bleedHeight, bool mirror)
    {
        // Each side gets the bleed amount
        int newWidth = source.Width + (bleedWidth * 2);
        int newHeight = source.Height + (bleedHeight * 2);

        var result = new Image<Rgba32>(newWidth, newHeight);

        // Copy original image to center
        result.Mutate(ctx => ctx.DrawImage(source, new SharpPoint(bleedWidth, bleedHeight), 1f));

        if (mirror)
        {
            MirrorEdges(source, result, bleedWidth, bleedHeight);
        }
        else
        {
            ExtendEdges(source, result, bleedWidth, bleedHeight);
        }

        source.Dispose();
        return result;
    }

    /// <summary>
    /// Mirrors the edge pixels into the bleed area.
    /// </summary>
    private void MirrorEdges(Image<Rgba32> source, Image<Rgba32> result, int bleedW, int bleedH)
    {
        int srcWidth = source.Width;
        int srcHeight = source.Height;

        // Top edge (mirrored vertically)
        for (int y = 0; y < bleedH; y++)
        {
            int srcY = Math.Min(bleedH - 1 - y, srcHeight - 1);
            if (srcY < 0) srcY = 0;

            for (int x = 0; x < srcWidth; x++)
            {
                result[x + bleedW, y] = source[x, srcY];
            }
        }

        // Bottom edge (mirrored vertically)
        for (int y = 0; y < bleedH; y++)
        {
            int srcY = Math.Max(srcHeight - 1 - y, 0);

            for (int x = 0; x < srcWidth; x++)
            {
                result[x + bleedW, srcHeight + bleedH + y] = source[x, srcY];
            }
        }

        // Left edge (mirrored horizontally)
        for (int x = 0; x < bleedW; x++)
        {
            int srcX = Math.Min(bleedW - 1 - x, srcWidth - 1);
            if (srcX < 0) srcX = 0;

            for (int y = 0; y < srcHeight; y++)
            {
                result[x, y + bleedH] = source[srcX, y];
            }
        }

        // Right edge (mirrored horizontally)
        for (int x = 0; x < bleedW; x++)
        {
            int srcX = Math.Max(srcWidth - 1 - x, 0);

            for (int y = 0; y < srcHeight; y++)
            {
                result[srcWidth + bleedW + x, y + bleedH] = source[srcX, y];
            }
        }

        // Top-left corner
        for (int y = 0; y < bleedH; y++)
        {
            int srcY = Math.Min(bleedH - 1 - y, srcHeight - 1);
            if (srcY < 0) srcY = 0;

            for (int x = 0; x < bleedW; x++)
            {
                int srcX = Math.Min(bleedW - 1 - x, srcWidth - 1);
                if (srcX < 0) srcX = 0;
                result[x, y] = source[srcX, srcY];
            }
        }

        // Top-right corner
        for (int y = 0; y < bleedH; y++)
        {
            int srcY = Math.Min(bleedH - 1 - y, srcHeight - 1);
            if (srcY < 0) srcY = 0;

            for (int x = 0; x < bleedW; x++)
            {
                int srcX = Math.Max(srcWidth - 1 - x, 0);
                result[srcWidth + bleedW + x, y] = source[srcX, srcY];
            }
        }

        // Bottom-left corner
        for (int y = 0; y < bleedH; y++)
        {
            int srcY = Math.Max(srcHeight - 1 - y, 0);

            for (int x = 0; x < bleedW; x++)
            {
                int srcX = Math.Min(bleedW - 1 - x, srcWidth - 1);
                if (srcX < 0) srcX = 0;
                result[x, srcHeight + bleedH + y] = source[srcX, srcY];
            }
        }

        // Bottom-right corner
        for (int y = 0; y < bleedH; y++)
        {
            int srcY = Math.Max(srcHeight - 1 - y, 0);

            for (int x = 0; x < bleedW; x++)
            {
                int srcX = Math.Max(srcWidth - 1 - x, 0);
                result[srcWidth + bleedW + x, srcHeight + bleedH + y] = source[srcX, srcY];
            }
        }
    }

    /// <summary>
    /// Extends edge pixels into the bleed area (no mirror).
    /// </summary>
    private void ExtendEdges(Image<Rgba32> source, Image<Rgba32> result, int bleedW, int bleedH)
    {
        int srcWidth = source.Width;
        int srcHeight = source.Height;

        // Top edge
        for (int y = 0; y < bleedH; y++)
        {
            for (int x = 0; x < srcWidth; x++)
            {
                result[x + bleedW, y] = source[x, 0];
            }
        }

        // Bottom edge
        for (int y = 0; y < bleedH; y++)
        {
            for (int x = 0; x < srcWidth; x++)
            {
                result[x + bleedW, srcHeight + bleedH + y] = source[x, srcHeight - 1];
            }
        }

        // Left edge
        for (int x = 0; x < bleedW; x++)
        {
            for (int y = 0; y < srcHeight; y++)
            {
                result[x, y + bleedH] = source[0, y];
            }
        }

        // Right edge
        for (int x = 0; x < bleedW; x++)
        {
            for (int y = 0; y < srcHeight; y++)
            {
                result[srcWidth + bleedW + x, y + bleedH] = source[srcWidth - 1, y];
            }
        }

        // Corners
        var topLeft = source[0, 0];
        var topRight = source[srcWidth - 1, 0];
        var bottomLeft = source[0, srcHeight - 1];
        var bottomRight = source[srcWidth - 1, srcHeight - 1];

        for (int y = 0; y < bleedH; y++)
        {
            for (int x = 0; x < bleedW; x++)
            {
                result[x, y] = topLeft;
                result[srcWidth + bleedW + x, y] = topRight;
                result[x, srcHeight + bleedH + y] = bottomLeft;
                result[srcWidth + bleedW + x, srcHeight + bleedH + y] = bottomRight;
            }
        }
    }

    private Image<Rgba32> RotateImage(Image<Rgba32> image, float angle)
    {
        image.Mutate(x => x.Rotate(angle));
        return image;
    }

    public async Task SaveImageAsync(Image<Rgba32> image, string filePath, ProcessingOptions options)
    {
        switch (options.OutputFormat)
        {
            case OutputFormat.Jpeg:
                await image.SaveAsJpegAsync(filePath, new JpegEncoder { Quality = options.JpegQuality });
                break;
            case OutputFormat.Png:
                await image.SaveAsPngAsync(filePath, new PngEncoder { CompressionLevel = PngCompressionLevel.DefaultCompression });
                break;
            case OutputFormat.Pdf:
                throw new InvalidOperationException("Use PdfExportService for PDF output.");
        }
    }

    public MemoryStream SaveToStream(Image<Rgba32> image)
    {
        var ms = new MemoryStream();
        image.SaveAsPng(ms);
        ms.Position = 0;
        return ms;
    }

    public System.Drawing.Bitmap? ToWinFormsBitmap()
    {
        if (_currentImage == null) return null;
        using var ms = new MemoryStream();
        _currentImage.SaveAsPng(ms);
        ms.Position = 0;
        return new System.Drawing.Bitmap(ms);
    }

    public static System.Drawing.Bitmap ToWinFormsBitmap(Image<Rgba32> image)
    {
        using var ms = new MemoryStream();
        image.SaveAsPng(ms);
        ms.Position = 0;
        return new System.Drawing.Bitmap(ms);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _currentImage?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

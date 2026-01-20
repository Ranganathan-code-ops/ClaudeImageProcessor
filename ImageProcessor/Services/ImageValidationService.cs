namespace ImageProcessor.Services;

/// <summary>
/// Service to validate image quality and aspect ratio.
/// </summary>
public class ImageValidationService
{
    /// <summary>
    /// Validates an image and returns the validation result.
    /// </summary>
    public ImageValidationResult Validate(int width, int height, long fileSizeBytes)
    {
        var result = new ImageValidationResult
        {
            Width = width,
            Height = height,
            FileSizeBytes = fileSizeBytes
        };

        // Calculate aspect ratio
        var gcd = GCD(width, height);
        result.AspectRatioWidth = width / gcd;
        result.AspectRatioHeight = height / gcd;
        result.AspectRatioName = GetAspectRatioName(width, height);

        // Calculate megapixels
        result.Megapixels = (width * height) / 1_000_000.0;

        // Validate resolution quality
        ValidateResolution(result);

        // Validate aspect ratio
        ValidateAspectRatio(result);

        // Validate file size
        ValidateFileSize(result);

        // Calculate overall quality
        CalculateOverallQuality(result);

        return result;
    }

    private void ValidateResolution(ImageValidationResult result)
    {
        var minDimension = Math.Min(result.Width, result.Height);
        var maxDimension = Math.Max(result.Width, result.Height);

        if (minDimension >= 1080 && result.Megapixels >= 2.0)
        {
            result.ResolutionQuality = QualityLevel.Good;
            result.ResolutionMessage = $"High resolution ({result.Width}x{result.Height}, {result.Megapixels:F1}MP)";
        }
        else if (minDimension >= 720 && result.Megapixels >= 0.9)
        {
            result.ResolutionQuality = QualityLevel.Acceptable;
            result.ResolutionMessage = $"Medium resolution ({result.Width}x{result.Height}, {result.Megapixels:F1}MP)";
        }
        else if (minDimension >= 480)
        {
            result.ResolutionQuality = QualityLevel.Poor;
            result.ResolutionMessage = $"Low resolution ({result.Width}x{result.Height}, {result.Megapixels:F2}MP)";
        }
        else
        {
            result.ResolutionQuality = QualityLevel.Poor;
            result.ResolutionMessage = $"Very low resolution ({result.Width}x{result.Height})";
        }
    }

    private void ValidateAspectRatio(ImageValidationResult result)
    {
        var ratio = (double)result.Width / result.Height;

        // Check for standard aspect ratios
        var standardRatios = new[]
        {
            (1.0, "1:1 (Square)"),
            (4.0/3.0, "4:3 (Standard)"),
            (3.0/2.0, "3:2 (Classic)"),
            (16.0/9.0, "16:9 (Widescreen)"),
            (16.0/10.0, "16:10 (Display)"),
            (21.0/9.0, "21:9 (Ultra-wide)"),
            (9.0/16.0, "9:16 (Portrait)"),
            (3.0/4.0, "3:4 (Portrait)"),
            (2.0/3.0, "2:3 (Portrait)")
        };

        var closestMatch = standardRatios
            .Select(r => (ratio: r, diff: Math.Abs(ratio - r.Item1)))
            .OrderBy(x => x.diff)
            .First();

        if (closestMatch.diff < 0.02)
        {
            result.AspectRatioQuality = QualityLevel.Good;
            result.AspectRatioMessage = $"Standard ratio: {closestMatch.ratio.Item2}";
        }
        else if (closestMatch.diff < 0.1)
        {
            result.AspectRatioQuality = QualityLevel.Acceptable;
            result.AspectRatioMessage = $"Near {closestMatch.ratio.Item2} ({result.AspectRatioWidth}:{result.AspectRatioHeight})";
        }
        else
        {
            result.AspectRatioQuality = QualityLevel.Acceptable;
            result.AspectRatioMessage = $"Custom ratio: {result.AspectRatioWidth}:{result.AspectRatioHeight}";
        }
    }

    private void ValidateFileSize(ImageValidationResult result)
    {
        var fileSizeMB = result.FileSizeBytes / (1024.0 * 1024.0);
        result.FileSizeMB = fileSizeMB;

        // Expected file size based on resolution (rough estimate for compressed images)
        var expectedMinMB = result.Megapixels * 0.1; // ~100KB per megapixel minimum
        var expectedMaxMB = result.Megapixels * 2.0; // ~2MB per megapixel maximum

        if (fileSizeMB >= expectedMinMB && fileSizeMB <= expectedMaxMB)
        {
            result.FileSizeQuality = QualityLevel.Good;
            result.FileSizeMessage = $"Good file size ({fileSizeMB:F2} MB)";
        }
        else if (fileSizeMB < expectedMinMB)
        {
            result.FileSizeQuality = QualityLevel.Poor;
            result.FileSizeMessage = $"Possibly over-compressed ({fileSizeMB:F2} MB)";
        }
        else
        {
            result.FileSizeQuality = QualityLevel.Acceptable;
            result.FileSizeMessage = $"Large file size ({fileSizeMB:F2} MB)";
        }
    }

    private void CalculateOverallQuality(ImageValidationResult result)
    {
        var qualities = new[] { result.ResolutionQuality, result.AspectRatioQuality, result.FileSizeQuality };

        if (qualities.All(q => q == QualityLevel.Good))
        {
            result.OverallQuality = QualityLevel.Good;
            result.OverallMessage = "Excellent image quality";
        }
        else if (qualities.Any(q => q == QualityLevel.Poor))
        {
            result.OverallQuality = QualityLevel.Poor;
            result.OverallMessage = "Image quality needs improvement";
        }
        else
        {
            result.OverallQuality = QualityLevel.Acceptable;
            result.OverallMessage = "Acceptable image quality";
        }
    }

    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static string GetAspectRatioName(int width, int height)
    {
        var ratio = (double)width / height;

        return ratio switch
        {
            >= 0.98 and <= 1.02 => "Square (1:1)",
            >= 1.31 and <= 1.35 => "Standard (4:3)",
            >= 1.48 and <= 1.52 => "Classic (3:2)",
            >= 1.76 and <= 1.80 => "Widescreen (16:9)",
            >= 1.58 and <= 1.62 => "Display (16:10)",
            >= 2.33 and <= 2.40 => "Ultra-wide (21:9)",
            >= 0.55 and <= 0.58 => "Portrait (9:16)",
            >= 0.74 and <= 0.76 => "Portrait (3:4)",
            >= 0.65 and <= 0.68 => "Portrait (2:3)",
            _ => "Custom"
        };
    }
}

/// <summary>
/// Result of image validation.
/// </summary>
public class ImageValidationResult
{
    public int Width { get; set; }
    public int Height { get; set; }
    public long FileSizeBytes { get; set; }
    public double FileSizeMB { get; set; }
    public double Megapixels { get; set; }

    public int AspectRatioWidth { get; set; }
    public int AspectRatioHeight { get; set; }
    public string AspectRatioName { get; set; } = "";

    public QualityLevel ResolutionQuality { get; set; }
    public string ResolutionMessage { get; set; } = "";

    public QualityLevel AspectRatioQuality { get; set; }
    public string AspectRatioMessage { get; set; } = "";

    public QualityLevel FileSizeQuality { get; set; }
    public string FileSizeMessage { get; set; } = "";

    public QualityLevel OverallQuality { get; set; }
    public string OverallMessage { get; set; } = "";
}

/// <summary>
/// Quality level for validation results.
/// </summary>
public enum QualityLevel
{
    Poor,       // Red
    Acceptable, // Yellow/Orange
    Good        // Green
}

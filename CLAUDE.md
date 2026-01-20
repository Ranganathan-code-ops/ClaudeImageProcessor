# ClaudeImageProcessor

Windows Forms Image Processor application with PDF support, URL loading, and image validation.

## Project Structure

```
ClaudeImageTest/
├── ImageProcessor/
│   ├── MainForm.cs              # Main application form
│   ├── MainForm.Designer.cs     # UI designer code
│   ├── ImagePreviewForm.cs      # Hover preview popup window
│   ├── Program.cs               # Entry point
│   ├── Models/
│   │   └── ProcessingOptions.cs # Image processing options
│   └── Services/
│       ├── ImageService.cs           # Core image operations
│       ├── ImageValidationService.cs # Quality & aspect ratio validation
│       ├── PdfService.cs             # PDF metadata handling
│       ├── PdfRenderService.cs       # PDF to image rendering
│       └── PdfExportService.cs       # Export images to PDF
├── .claude/
│   └── commands/                # Claude Code skills
│       ├── load-url.md          # Load images from URL
│       ├── image-hover-preview.md # Mouse hover preview feature
│       ├── r1.md                # Print Ranganathan
│       └── p1.md                # Read clipboard
└── ImageProcessor.sln           # Solution file
```

## Features

### Image Loading
- **Open File**: Load images from local filesystem (JPG, PNG, BMP, GIF)
- **Load from URL**: Download and load images from web URLs
- **PDF Support**: Load PDF files - renders first page as image using Docnet.Core

### Image Processing
- **Resize**: Change dimensions with aspect ratio lock
- **Bleed**: Add bleed margins (mm, px, or inches) with mirror edge option
- **Flip/Mirror**: Horizontal and vertical flipping
- **Rotation**: Preset angles (90°, 180°, 270°) or custom angle

### Image Validation (Color-Coded)
- **Resolution Quality**: Checks megapixels and minimum dimensions
  - Green: High resolution (1080p+, 2MP+)
  - Orange: Medium resolution (720p+)
  - Red: Low resolution
- **Aspect Ratio**: Identifies standard ratios (16:9, 4:3, 1:1, etc.)
- **File Size**: Validates compression quality
- **Overall Status**: Combined quality assessment with background highlight

### Export Options
- **PNG**: Lossless format
- **JPEG**: With quality control (1-100)
- **PDF**: Export as PDF document

### UI Features
- **Image Preview**: Zoomable preview panel
- **Hover Preview**: Mouse over image to see enlarged popup (800x600 max)
- **Validation Panel**: Real-time quality feedback below preview

## Dependencies

- **.NET 8.0 Windows Forms**
- **SixLabors.ImageSharp** (3.1.7) - Image processing
- **SixLabors.ImageSharp.Drawing** (2.1.5) - Drawing operations
- **PDFsharp** (6.1.1) - PDF creation/export
- **Docnet.Core** (2.6.0) - PDF rendering to images

## Claude Code Skills

### /load-url
Load an image from a URL into the application.

### /image-hover-preview
Implement mouse hover preview functionality.

### /r1
Print "Ranganathan" to the user.

### /p1
Read clipboard content using PowerShell.

## Build & Run

```bash
cd ImageProcessor
dotnet build
dotnet run
```

## GitHub Repository

https://github.com/Ranganathan-code-ops/ClaudeImageProcessor

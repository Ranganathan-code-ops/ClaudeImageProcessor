@echo off
REM ============================================
REM ImageProcessor Build Script
REM ============================================

setlocal enabledelayedexpansion

set PROJECT_NAME=ImageProcessor
set SOLUTION_FILE=ImageProcessor.sln
set PUBLISH_DIR=publish
set CONFIGURATION=Release

echo.
echo ============================================
echo Building %PROJECT_NAME%
echo ============================================
echo.

REM Clean previous builds
if exist "%PUBLISH_DIR%" (
    echo Cleaning previous build...
    rmdir /s /q "%PUBLISH_DIR%"
)

REM Restore dependencies
echo.
echo [1/4] Restoring dependencies...
dotnet restore %SOLUTION_FILE%
if errorlevel 1 (
    echo ERROR: Restore failed!
    exit /b 1
)

REM Build the solution
echo.
echo [2/4] Building solution...
dotnet build %SOLUTION_FILE% --configuration %CONFIGURATION% --no-restore
if errorlevel 1 (
    echo ERROR: Build failed!
    exit /b 1
)

REM Publish self-contained executable
echo.
echo [3/4] Publishing self-contained executable...
dotnet publish %PROJECT_NAME%\%PROJECT_NAME%.csproj ^
    --configuration %CONFIGURATION% ^
    --runtime win-x64 ^
    --self-contained true ^
    --output %PUBLISH_DIR%\win-x64 ^
    -p:PublishSingleFile=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -p:EnableCompressionInSingleFile=true

if errorlevel 1 (
    echo ERROR: Publish failed!
    exit /b 1
)

REM Create version file
echo.
echo [4/4] Creating version info...
echo Build Date: %DATE% %TIME% > "%PUBLISH_DIR%\win-x64\version.txt"
echo Configuration: %CONFIGURATION% >> "%PUBLISH_DIR%\win-x64\version.txt"

echo.
echo ============================================
echo Build completed successfully!
echo ============================================
echo.
echo Output: %PUBLISH_DIR%\win-x64\%PROJECT_NAME%.exe
echo.

REM List output files
dir /b "%PUBLISH_DIR%\win-x64"

endlocal

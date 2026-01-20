@echo off
REM ============================================
REM One-Click Deploy to Production Server
REM ============================================

setlocal enabledelayedexpansion

REM ============================================
REM CONFIGURATION - Edit these settings
REM ============================================
set SSH_USER=administrator
set SSH_HOST=YOUR_SERVER_IP
set SSH_PORT=22
set REMOTE_PATH=/c/Deploy/ImageProcessor
set LOCAL_PUBLISH_DIR=publish\win-x64

REM ============================================
REM DO NOT EDIT BELOW THIS LINE
REM ============================================

set PROJECT_NAME=ImageProcessor
set SOLUTION_FILE=ImageProcessor.sln

echo.
echo ============================================
echo One-Click Deploy: %PROJECT_NAME%
echo ============================================
echo.
echo Server: %SSH_USER%@%SSH_HOST%
echo Path:   %REMOTE_PATH%
echo.

REM Check if server is configured
if "%SSH_HOST%"=="YOUR_SERVER_IP" (
    echo ERROR: Please configure SSH_HOST in deploy.bat
    echo Edit the file and set your server IP address.
    exit /b 1
)

REM Step 1: Build
echo [1/4] Building project...
call build.bat
if errorlevel 1 (
    echo ERROR: Build failed!
    exit /b 1
)

REM Step 2: Create remote directory
echo.
echo [2/4] Preparing remote directory...
ssh -p %SSH_PORT% %SSH_USER%@%SSH_HOST% "mkdir -p %REMOTE_PATH%"
if errorlevel 1 (
    echo ERROR: Failed to create remote directory!
    exit /b 1
)

REM Step 3: Stop running application (if any)
echo.
echo [3/4] Stopping existing application...
ssh -p %SSH_PORT% %SSH_USER%@%SSH_HOST% "taskkill /F /IM %PROJECT_NAME%.exe 2>nul || echo No running instance found"

REM Step 4: Deploy files
echo.
echo [4/4] Deploying files to server...
scp -P %SSH_PORT% -r %LOCAL_PUBLISH_DIR%\* %SSH_USER%@%SSH_HOST%:%REMOTE_PATH%/
if errorlevel 1 (
    echo ERROR: Deployment failed!
    exit /b 1
)

echo.
echo ============================================
echo Deployment completed successfully!
echo ============================================
echo.
echo Application deployed to: %SSH_USER%@%SSH_HOST%:%REMOTE_PATH%
echo.
echo To start the application on the server, run:
echo   ssh %SSH_USER%@%SSH_HOST% "%REMOTE_PATH%/%PROJECT_NAME%.exe"
echo.

endlocal

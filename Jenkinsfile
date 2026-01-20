pipeline {
    agent {
        label 'windows'  // Requires a Windows agent with .NET SDK installed
    }

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'
        PROJECT_NAME = 'ImageProcessor'
        SOLUTION_FILE = 'ImageProcessor.sln'
        PUBLISH_DIR = 'publish'
        ARTIFACT_NAME = "ImageProcessor-${env.BUILD_NUMBER}"
    }

    options {
        buildDiscarder(logRotator(numToKeepStr: '10'))
        timestamps()
        timeout(time: 30, unit: 'MINUTES')
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
                echo "Building branch: ${env.BRANCH_NAME ?: 'master'}"
                echo "Build number: ${env.BUILD_NUMBER}"
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore %SOLUTION_FILE%'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build %SOLUTION_FILE% --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                echo 'Running tests...'
                // Uncomment when tests are added
                // bat 'dotnet test %SOLUTION_FILE% --configuration Release --no-build --verbosity normal'
                echo 'No tests configured yet - skipping'
            }
        }

        stage('Publish') {
            steps {
                echo 'Publishing self-contained executable...'
                bat '''
                    dotnet publish %PROJECT_NAME%\\%PROJECT_NAME%.csproj ^
                        --configuration Release ^
                        --runtime win-x64 ^
                        --self-contained true ^
                        --output %PUBLISH_DIR%\\win-x64 ^
                        -p:PublishSingleFile=true ^
                        -p:IncludeNativeLibrariesForSelfExtract=true ^
                        -p:EnableCompressionInSingleFile=true
                '''
            }
        }

        stage('Archive Artifacts') {
            steps {
                echo 'Archiving build artifacts...'

                // Create version info file
                bat '''
                    echo Build: %BUILD_NUMBER% > %PUBLISH_DIR%\\win-x64\\version.txt
                    echo Date: %DATE% %TIME% >> %PUBLISH_DIR%\\win-x64\\version.txt
                    echo Branch: %BRANCH_NAME% >> %PUBLISH_DIR%\\win-x64\\version.txt
                '''

                // Archive the published files
                archiveArtifacts artifacts: "${PUBLISH_DIR}/win-x64/**/*", fingerprint: true

                // Create a zip file for distribution
                bat '''
                    powershell -Command "Compress-Archive -Path '%PUBLISH_DIR%\\win-x64\\*' -DestinationPath '%PUBLISH_DIR%\\%ARTIFACT_NAME%.zip' -Force"
                '''

                archiveArtifacts artifacts: "${PUBLISH_DIR}/${ARTIFACT_NAME}.zip", fingerprint: true
            }
        }

        stage('Deploy to Production') {
            when {
                anyOf {
                    branch 'master'
                    branch 'main'
                }
            }
            steps {
                echo 'Deploying to production server...'

                // Option 1: Copy to network share
                // bat 'xcopy /Y /E %PUBLISH_DIR%\\win-x64\\* \\\\production-server\\apps\\ImageProcessor\\'

                // Option 2: Copy to local production folder
                // bat 'xcopy /Y /E %PUBLISH_DIR%\\win-x64\\* C:\\Production\\ImageProcessor\\'

                // Option 3: Use SSH/SCP to deploy to remote server
                // bat 'scp -r %PUBLISH_DIR%\\win-x64\\* user@production-server:/opt/apps/ImageProcessor/'

                echo 'Deployment target not configured - update Jenkinsfile with your production server details'
                echo "Artifacts available at: ${env.BUILD_URL}artifact/${PUBLISH_DIR}/"
            }
        }
    }

    post {
        success {
            echo 'Build completed successfully!'
            echo "Download artifacts from: ${env.BUILD_URL}artifact/"
        }
        failure {
            echo 'Build failed!'
            // Uncomment to enable email notifications
            // mail to: 'team@example.com',
            //      subject: "Build Failed: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
            //      body: "Check console output at ${env.BUILD_URL}"
        }
        cleanup {
            echo 'Cleaning up workspace...'
            // cleanWs()  // Uncomment to clean workspace after build
        }
    }
}

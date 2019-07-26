param(
    [string] $VersionSuffix,
    [switch] $RunTests
)
Push-Location $PSScriptRoot

function Write-Message{
    param([string]$message)
    Write-Host
    Write-Host $message
    Write-Host
}
function Confirm-PreviousCommand {
    param([string]$errorMessage)
    if ( $LASTEXITCODE -ne 0) { 
        if( $errorMessage) {
            Write-Message $errorMessage
        }    
        exit $LASTEXITCODE 
    }
}

function Confirm-Process {
    param ([System.Diagnostics.Process]$process,[string]$errorMessage)
    $process.WaitForExit()
    if($process.ExitCode -ne 0){
        Write-Host $process.ExitCode
        if( $errorMessage) {
            Write-Message $errorMessage
        }    
        exit $process.ExitCode 
    }
}

Write-Host "Parameters"
Write-Host "=========="
Write-Host "Version suffix: $VersionSuffix"
Write-Host "Run tests: $RunTests"

# Check prerequisites
$proc = Start-Process "node" -ArgumentList "-v" -PassThru
Confirm-Process $proc "Could not find node.js, please install and run again ..."

$proc = Start-Process "npm" -ArgumentList "-v" -PassThru
Confirm-Process $proc "Could not find npm, please install and run again ..."

$proc = Start-Process "dotnet" -ArgumentList "--version" -PassThru
Confirm-Process $proc "Could not find dotnet sdk, please install and run again ..."


if ( $RunTests ) {
    # Start selenium
    Write-Message "Installing selenium-standalone ..."
    npx selenium-standalone install
    
    Write-Message "Starting selenium-standalone ..."
    $selenium = Start-Process "npx" -ArgumentList "selenium-standalone start" -PassThru
}

Write-Message "Installing npm dependencies ..."
Push-Location BlazorContextMenu
npm install
Confirm-PreviousCommand

Write-Message "Minifying ..."
npm run minify
Confirm-PreviousCommand

Pop-Location

Write-Message "Building ..."
dotnet build BlazorContextMenu.sln -c Release
Confirm-PreviousCommand

if ( $RunTests ) {
    Write-Message "Running Tests (Client-Side) ..."
    dotnet test Tests/BlazorContextMenu.BlazorE2ETests -c Release --no-build 
    Confirm-PreviousCommand

    Write-Message "Running Tests (Server-Side) ..."
    dotnet test Tests/BlazorContextMenu.RazorComponentsE2ETests -c Release --no-build 
    Confirm-PreviousCommand

    Write-Message "Stopping selenium-standalone ..."
    Stop-Process $selenium
    # TODO: Find a better way to kill child proc
    Stop-Process -Name "node"
}

Write-Message "Creating nuget package ..."
dotnet pack BlazorContextMenu -c Release --no-build /p:VersionSuffix="$VersionSuffix" -o artifacts/nuget
Confirm-PreviousCommand

Write-Message "Build completed successfully"
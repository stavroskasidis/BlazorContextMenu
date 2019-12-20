@echo off
cd "%~dp0"
set version-suffix=%1

dotnet build BlazorContextMenu.sln -c Release
if %errorlevel% neq 0 exit /b %errorlevel%

dotnet test Tests/BlazorContextMenu.BlazorE2ETests -c Release --no-build 
if %errorlevel% neq 0 exit /b %errorlevel%

dotnet test Tests/BlazorContextMenu.RazorComponentsE2ETests -c Release --no-build 
if %errorlevel% neq 0 exit /b %errorlevel%

dotnet pack BlazorContextMenu -c Release --no-build /p:VersionSuffix="%version-suffix%" -o artifacts/nuget
if %errorlevel% neq 0 exit /b %errorlevel%

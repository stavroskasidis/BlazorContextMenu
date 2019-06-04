@echo off
cd "%~dp0"

dotnet build DemoApp\BlazorContextMenu.DemoApp.Server -c Release
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet publish DemoApp\BlazorContextMenu.DemoApp.Server -c Release -o artifacts\demoapp --no-build
if %errorlevel% neq 0 exit /b %errorlevel%

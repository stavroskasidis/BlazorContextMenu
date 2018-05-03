@echo off
cd "%~dp0"


dotnet publish BlazorContextMenu.DemoApp -c Release
if %errorlevel% neq 0 exit /b %errorlevel%
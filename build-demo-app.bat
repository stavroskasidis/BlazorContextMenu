@echo off
cd "%~dp0"


dotnet publish DemoApp\BlazorContextMenu.DemoApp -c Release -o artifacts\demoapp
if %errorlevel% neq 0 exit /b %errorlevel%
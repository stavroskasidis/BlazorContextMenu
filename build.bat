@echo off
cd "%~dp0"
set version-suffix=%1
echo %version-suffix%
dotnet build BlazorContextMenu.sln -c Release
dotnet test BlazorContextMenu.E2ETests -c Release --no-build 
dotnet pack BlazorContextMenu -c Release --no-build --version-suffix %version-suffix%

dotnet build BlazorContextMenu.DemoApp.sln -c Release
dotnet publish BlazorContextMenu.DemoApp -c Release
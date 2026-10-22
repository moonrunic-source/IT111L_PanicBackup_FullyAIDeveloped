@echo off
echo Building Agimat ng Baybayin...
dotnet publish AgimatNgBaybayin.csproj -c Release -o "%~dp0Game"
if errorlevel 1 (
  echo.
  echo Build failed. Make sure the .NET 8 SDK is installed: https://dotnet.microsoft.com/download/dotnet/8.0
  pause
  exit /b 1
)
echo.
echo Done! Your game is at: %~dp0Game\AgimatNgBaybayin.exe
start "" "%~dp0Game"
pause

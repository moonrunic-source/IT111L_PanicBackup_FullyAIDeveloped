@echo off
setlocal
cd /d "%~dp0"

echo Preparing BayBayinFont
if not exists "Assets" mkdir "Assets"
rem If the font sits next to this .bat file, copy it into assets
if exist "NotoSansTagalog-Regular.ttf" copy /Y "NotoSansTagalog-Regular.ttf" "Assets/NotoSansTagalog-Regular.ttf" >nul

if not exist "Assets/NotoSansTagalog-Regular.ttf" (
  echo.
  echo Error: NotoSansTagalog-Regular.ttf was not found.
  echo Either download a NotoSansTagalog.ttf file in https://fonts.google.com/noto/specimen/Noto+Sans+Tagalog?preview.script=Tglg&preview.lang=fil_Tglg, and insert it in the Assets folder
  pause
  exit /b 1 
)
echo Font Found
echo.

echo Building the BayBayin game
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

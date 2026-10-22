# Agimat ng Baybayin (Windows Forms, C#)

Open the folder in Visual Studio 2022 (".NET desktop development" workload) and press F5,
or run `dotnet run` inside the folder (requires the .NET 8 SDK on Windows).

## Where to change things
| I want to change...                      | File                       |
|------------------------------------------|----------------------------|
| Lives, points, speeds, sizes, fonts      | Config/GameConfig.cs       |
| Level speed / timer / blanks / mission   | Config/LevelCatalog.cs     |
| Words and meanings                       | Models/WordBank.cs         |
| Scoring rules                            | Systems/ScoreManager.cs    |
| Life rules                               | Systems/LifeManager.cs     |
| Timer rules                              | Systems/CountdownTimer.cs  |
| Orb spawning                             | Systems/OrbSpawner.cs      |
| Orb / player look                        | Entities/*.cs              |
| Screens and buttons                      | Screens/*.cs, UI/*.cs      |

## Controls
W / S or Up / Down arrows to move, mouse for buttons, Esc to return to the menu.

## Making the .exe
Double-click `BUILD_EXE.bat` (needs the .NET 8 SDK once, on the PC that builds it).
It creates `Game\AgimatNgBaybayin.exe`: a single standalone file you can double-click to play
or copy to any Windows 10/11 PC (no .NET install needed there).
In Visual Studio: Build > Publish, or just press F5 to run while developing.

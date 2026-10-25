using System.Drawing;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Core;
using AgimatNgBaybayin.Models;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Screens
{
    /// <summary>
    /// Shown before every stage: explains the rules and lets the player
    /// play the tutorial (Codex + practice) or skip it.
    /// </summary>
    public class StageIntroScreen : GameScreen
    {
        private readonly int _levelNumber;
        private readonly LevelSettings _settings;

        public StageIntroScreen(IGameHost host, int levelNumber) : base(host)
        {
            _levelNumber = levelNumber;
            _settings = LevelCatalog.GetLevel(levelNumber);

            Buttons.Add(new MenuButton("Play Tutorial", new Rectangle(180, 470, 280, 52), StartTutorial));
            Buttons.Add(new MenuButton("Skip Tutorial", new Rectangle(500, 470, 280, 52), StartLevel));
            Buttons.Add(new MenuButton("Main Menu", new Rectangle(380, 535, 200, 42), BackToMenu));
        }

        private void StartTutorial()
        {
            Host.ShowScreen(new CodexScreen(
                Host,
                "Baybayin Codex",
                "Study the characters of this level, then try a short practice warm-up.",
                WordBank.GetUniqueSyllablesForLevel(_settings),
                "Start Practice",
                StartPractice));
        }

        private void StartPractice()
        {
            Host.ShowScreen(new PlayScreen(Host, LevelCatalog.GetPracticeVersion(_levelNumber)));
        }

        private void StartLevel()
        {
            Host.ShowScreen(new PlayScreen(Host, LevelCatalog.GetLevel(_levelNumber)));
        }

        private void BackToMenu()
        {
            Host.ShowScreen(new MenuScreen(Host));
        }

        public override void Draw(Graphics g)
        {
            Theme.DrawBackdrop(g, GameConfig.WindowWidth, GameConfig.WindowHeight);

            DrawCenteredLine(g, "Level " + _settings.LevelNumber + ": " + _settings.Title,
                             FontCache.Get(GameConfig.TextFontName, 32f, FontStyle.Bold), Theme.Gold, 40, 60);
            DrawCenteredLine(g, _settings.Description, FontCache.Get(GameConfig.TextFontName, 15f), Theme.Cream, 105, 30);

            string[] ruleLines =
            {
                "Mission: complete " + _settings.WordsToComplete + " words.",
                _settings.HasTimer ? "Timer: " + _settings.SecondsPerWord.ToString("0") + " seconds per word."
                                   : "Timer: none on this level.",
                "Missing characters per word: " + _settings.BlanksPerWord + ".",
                _settings.ShowTranslationGuide ? "The Latin guide is shown on every orb."
                                               : "No translation guide. Trust your memory!",
                _settings.ShowWordInBaybayin ? "Words are written in Baybayin. Use the meaning clue."
                                             : "Words are written in Latin letters.",
                "Move with W / S or the Up / Down arrows. Wrong characters cost a life."
            };

            for (int i = 0; i < ruleLines.Length; i++)
                DrawCenteredLine(g, ruleLines[i], FontCache.Get(GameConfig.TextFontName, 14f), Theme.Cream, 170 + i * 34, 30);

            DrawCenteredLine(g, "Would you like to play the tutorial first?",
                             FontCache.Get(GameConfig.TextFontName, 14f, FontStyle.Bold), Theme.Gold, 425, 30);

            DrawButtons(g);
        }
    }
}

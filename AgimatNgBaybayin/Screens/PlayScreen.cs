using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Core;
using AgimatNgBaybayin.Entities;
using AgimatNgBaybayin.Models;
using AgimatNgBaybayin.Systems;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Screens
{
    /// <summary>
    /// The arcade gameplay of one level (or one practice warm-up).
    /// This class only coordinates: the rules live in LifeManager, ScoreManager,
    /// CountdownTimer, WordSession and OrbSpawner.
    /// </summary>
    public class PlayScreen : GameScreen
    {
        // ---------- Level data ----------
        private readonly LevelSettings _settings;
        private readonly Random _rng = new Random();

        // ---------- Game objects ----------
        private readonly Player _player = new Player();
        private readonly List<Orb> _orbs = new List<Orb>();

        // ---------- Systems ----------
        private readonly OrbSpawner _spawner;
        private readonly LifeManager _lives = new LifeManager();
        private readonly ScoreManager _score = new ScoreManager();
        private readonly CountdownTimer _timer = new CountdownTimer();

        // ---------- Current progress ----------
        private WordSession _session;
        private Word _previousWord;
        private int _wordsCompleted;
        private bool _isFinished;

        // ---------- Feedback message ----------
        private string _flashText = "";
        private Color _flashColor = Color.White;
        private float _flashSecondsLeft;

        public PlayScreen(IGameHost host, LevelSettings settings) : base(host)
        {
            _settings = settings;
            _spawner = new OrbSpawner(_rng, WordBank.GetUniqueSyllablesForLevel(settings));

            // Practice never touches the real score. Retrying a level restarts from the score the level began with.
            int startingScore = settings.IsPractice ? GameConfig.StartingScore : host.Progress.TotalScore;
            _score.Reset(startingScore);

            LoadNextWord();
        }

        // =====================================================================
        //  UPDATE
        // =====================================================================
        public override void Update(float deltaSeconds)
        {
            if (_isFinished)
                return;

            _player.Update(deltaSeconds);
            SpawnOrbs(deltaSeconds);
            UpdateOrbs(deltaSeconds);
            HandleCollisions();
            UpdateTimer(deltaSeconds);
            UpdateFlashMessage(deltaSeconds);
            CheckEndConditions();
        }

        private void SpawnOrbs(float deltaSeconds)
        {
            Orb newOrb = _spawner.TrySpawn(deltaSeconds, _settings, _session);
            if (newOrb != null)
                _orbs.Add(newOrb);
        }

        private void UpdateOrbs(float deltaSeconds)
        {
            foreach (Orb orb in _orbs)
                orb.Update(deltaSeconds);

            _orbs.RemoveAll(orb => !orb.IsActive);   // collected or skipped orbs disappear
        }

        private void HandleCollisions()
        {
            foreach (Orb orb in _orbs)
            {
                if (orb.IsActive && orb.Intersects(_player))
                    HandleOrbCollected(orb);
            }

            _orbs.RemoveAll(orb => !orb.IsActive);
        }

        private void HandleOrbCollected(Orb orb)
        {
            orb.IsActive = false;

            AgimatOrb agimat = orb as AgimatOrb;
            if (agimat != null)
            {
                CollectAgimat();
                return;
            }

            LetterOrb letterOrb = (LetterOrb)orb;
            if (_session.TryFill(letterOrb.Syllable))
                OnCorrectLetter();
            else
                OnWrongLetter();
        }

        // ---------- Rules ----------
        private void OnCorrectLetter()
        {
            _score.AddCorrectLetter();
            ShowFlash("+" + GameConfig.PointsPerCorrectLetter, Theme.Good);

            if (_session.IsComplete)
                OnWordCompleted();
        }

        private void OnWordCompleted()
        {
            _timer.Stop();
            _score.AddCompletedWord();
            _wordsCompleted++;
            ShowFlash(_session.Word.Latin + " complete! +" + GameConfig.PointsPerCompletedWord, Theme.Gold);

            if (_wordsCompleted < _settings.WordsToComplete)
                LoadNextWord();
        }

        private void OnWrongLetter()
        {
            _score.PenalizeWrongLetter();

            if (!_settings.IsPractice)
                _lives.LoseLife();

            ShowFlash("Wrong character! -" + GameConfig.PenaltyWrongLetter, Theme.Bad);
        }

        private void CollectAgimat()
        {
            if (_lives.TryRestoreLife())
            {
                ShowFlash("Agimat! +1 life", Theme.Gold);
            }
            else
            {
                _score.AddAgimatBonus();
                ShowFlash("Agimat! +" + GameConfig.BonusAgimatAtFullLives + " bonus", Theme.Gold);
            }
        }

        private void UpdateTimer(float deltaSeconds)
        {
            _timer.Update(deltaSeconds);

            if (_timer.IsExpired)
            {
                _lives.LoseLife();
                ShowFlash("Time's up! Life lost", Theme.Bad);
                LoadNextWord();   // a new word restarts the timer
            }
        }

        private void LoadNextWord()
        {
            Word word = WordBank.PickRandom(_settings, _rng, _previousWord);
            _previousWord = word;
            _session = new WordSession(word, _settings.BlanksPerWord, _rng);
            _timer.Start(_settings.SecondsPerWord);
        }

        private void CheckEndConditions()
        {
            if (_lives.IsOutOfLives)
                FinishGameOver();
            else if (_wordsCompleted >= _settings.WordsToComplete)
                FinishLevelSuccess();
        }

        // =====================================================================
        //  END OF LEVEL
        // =====================================================================
        private void FinishGameOver()
        {
            _isFinished = true;
            int levelNumber = _settings.LevelNumber;

            Host.ShowScreen(new ResultScreen(
                Host, "Game Over", Theme.Bad,
                new[] { "You ran out of lives.", "Score this attempt: " + _score.Score },
                ("Retry Level", () => Host.ShowScreen(new PlayScreen(Host, LevelCatalog.GetLevel(levelNumber)))),
                ("Main Menu", () => Host.ShowScreen(new MenuScreen(Host)))));
        }

        private void FinishLevelSuccess()
        {
            _isFinished = true;

            if (_settings.IsPractice)
            {
                ShowPracticeComplete();
                return;
            }

            Host.Progress.TotalScore = _score.Score;

            if (_settings.LevelNumber >= LevelCatalog.TotalLevels)
                ShowVictory();
            else
                ShowLevelComplete();
        }

        private void ShowPracticeComplete()
        {
            int levelNumber = _settings.LevelNumber;

            Host.ShowScreen(new ResultScreen(
                Host, "Warm-up Complete!", Theme.Gold,
                new[] { "You are ready for the real stage." },
                ("Start Level " + levelNumber, () => Host.ShowScreen(new PlayScreen(Host, LevelCatalog.GetLevel(levelNumber)))),
                ("Main Menu", () => Host.ShowScreen(new MenuScreen(Host)))));
        }

        private void ShowLevelComplete()
        {
            int nextLevel = _settings.LevelNumber + 1;

            Host.ShowScreen(new ResultScreen(
                Host, "Level Complete!", Theme.Gold,
                new[] { "Kultura Score: " + _score.Score, "Lives left: " + _lives.Lives },
                ("Next Level", () => Host.ShowScreen(new StageIntroScreen(Host, nextLevel))),
                ("Main Menu", () => Host.ShowScreen(new MenuScreen(Host)))));
        }

        private void ShowVictory()
        {
            int finalScore = _score.Score;

            Action openFullCodex = () => Host.ShowScreen(new CodexScreen(
                Host,
                "The Full Baybayin Codex",
                "Every character you have learned. Final score: " + finalScore,
                WordBank.GetUniqueSyllables(WordBank.GetAllWords()),
                "Main Menu",
                () => Host.ShowScreen(new MenuScreen(Host))));

            Host.ShowScreen(new ResultScreen(
                Host, "Codex Unlocked!", Theme.Gold,
                new[] { "You finished every level.", "Final Kultura Score: " + finalScore },
                ("View Full Codex", openFullCodex),
                ("Main Menu", () => Host.ShowScreen(new MenuScreen(Host)))));
        }

        // =====================================================================
        //  INPUT
        // =====================================================================
        public override void OnKeyDown(Keys key)
        {
            if (key == Keys.W || key == Keys.Up) _player.IsMovingUp = true;
            if (key == Keys.S || key == Keys.Down) _player.IsMovingDown = true;
            if (key == Keys.Escape) Host.ShowScreen(new MenuScreen(Host));
        }

        public override void OnKeyUp(Keys key)
        {
            if (key == Keys.W || key == Keys.Up) _player.IsMovingUp = false;
            if (key == Keys.S || key == Keys.Down) _player.IsMovingDown = false;
        }

        // =====================================================================
        //  FEEDBACK MESSAGE
        // =====================================================================
        private void ShowFlash(string text, Color color)
        {
            _flashText = text;
            _flashColor = color;
            _flashSecondsLeft = GameConfig.FlashMessageSeconds;
        }

        private void UpdateFlashMessage(float deltaSeconds)
        {
            if (_flashSecondsLeft > 0f)
                _flashSecondsLeft -= deltaSeconds;
        }

        // =====================================================================
        //  DRAW
        // =====================================================================
        public override void Draw(Graphics g)
        {
            Theme.DrawBackdrop(g, GameConfig.WindowWidth, GameConfig.WindowHeight);

            foreach (Orb orb in _orbs)
                orb.Draw(g);

            _player.Draw(g);

            DrawHud(g);
            DrawTopBar(g);
        }

        private void DrawTopBar(Graphics g)
        {
            string label = _settings.IsPractice
                ? "Practice Warm-up  -  Words " + _wordsCompleted + "/" + _settings.WordsToComplete
                : "Level " + _settings.LevelNumber + "  -  Words " + _wordsCompleted + "/" + _settings.WordsToComplete;

            using (var brush = new SolidBrush(Theme.Cream))
                g.DrawString(label, FontCache.Get(GameConfig.TextFontName, 12f, FontStyle.Bold), brush, 12, 8);

            if (_flashSecondsLeft > 0f)
            {
                using (var brush = new SolidBrush(_flashColor))
                    Theme.DrawCentered(g, _flashText, FontCache.Get(GameConfig.TextFontName, 18f, FontStyle.Bold),
                                       brush, new RectangleF(0, 4, GameConfig.WindowWidth, 30));
            }
        }

        private void DrawHud(Graphics g)
        {
            var hudArea = new Rectangle(0, GameConfig.HudTop, GameConfig.WindowWidth, GameConfig.WindowHeight - GameConfig.HudTop);
            using (var fill = new SolidBrush(Theme.WoodDark))
            using (var edge = new Pen(Theme.WoodLight, 3))
            {
                g.FillRectangle(fill, hudArea);
                g.DrawLine(edge, 0, GameConfig.HudTop, GameConfig.WindowWidth, GameConfig.HudTop);
            }

            DrawWordSlots(g);
            DrawLives(g);
            DrawMeaningAndTimer(g);
            DrawScore(g);
        }

        private void DrawWordSlots(Graphics g)
        {
            const int boxSize = 48;
            const int boxGap = 6;
            int startX = 20;
            int slotY = GameConfig.HudTop + 8;

            for (int i = 0; i < _session.SlotCount; i++)
            {
                var box = new Rectangle(startX + i * (boxSize + boxGap), slotY, boxSize, boxSize);
                SlotState state = _session.GetState(i);
                Syllable syllable = _session.GetSyllable(i);

                Color boxColor = state == SlotState.Missing ? Color.FromArgb(120, 90, 60)
                               : state == SlotState.Filled ? Color.FromArgb(90, 150, 80)
                               : Color.FromArgb(200, 190, 160);
                Color textColor = state == SlotState.Revealed ? Theme.WoodDark : Color.White;

                string text;
                Font font;
                if (state == SlotState.Missing)
                {
                    text = "_";
                    font = FontCache.Get(GameConfig.TextFontName, 20f, FontStyle.Bold);
                }
                else if (_settings.ShowWordInBaybayin)
                {
                    text = syllable.Glyph;
                    font = FontCache.Get(GameConfig.GlyphFontName, 22f);
                }
                else
                {
                    text = syllable.Latin;
                    font = FontCache.Get(GameConfig.TextFontName, 16f, FontStyle.Bold);
                }

                using (var fill = new SolidBrush(boxColor))
                using (var border = new Pen(Theme.WoodMid, 2))
                using (var brush = new SolidBrush(textColor))
                {
                    g.FillRectangle(fill, box);
                    g.DrawRectangle(border, box);
                    Theme.DrawCentered(g, text, font, brush, box);
                }

                // Translation guide: the Baybayin character under every known slot.
                if (_settings.ShowTranslationGuide && state != SlotState.Missing)
                {
                    using (var guideBrush = new SolidBrush(Theme.Cream))
                        Theme.DrawCentered(g, syllable.Glyph, FontCache.Get(GameConfig.GlyphFontName, 18f), guideBrush,
                                           new RectangleF(box.X, box.Bottom + 2, box.Width, 30));
                }
            }
        }

        private void DrawLives(Graphics g)
        {
            Font heartFont = FontCache.Get(GameConfig.SymbolFontName, 22f);

            for (int i = 0; i < _lives.MaxLives; i++)
            {
                Color heartColor = i < _lives.Lives ? Color.FromArgb(220, 40, 60) : Color.FromArgb(90, 70, 70);
                using (var brush = new SolidBrush(heartColor))
                    g.DrawString("\u2665", heartFont, brush, 18 + i * 36, GameConfig.HudTop + 94);
            }
        }

        private void DrawMeaningAndTimer(Graphics g)
        {
            int panelX = 290;

            using (var cream = new SolidBrush(Theme.Cream))
            using (var gold = new SolidBrush(Theme.Gold))
            {
                g.DrawString("Meaning:", FontCache.Get(GameConfig.TextFontName, 11f), cream, panelX, GameConfig.HudTop + 12);
                g.DrawString(_session.Word.Meaning, FontCache.Get(GameConfig.TextFontName, 20f, FontStyle.Bold), gold,
                             panelX, GameConfig.HudTop + 30);
            }

            DrawTimerBar(g, panelX, GameConfig.HudTop + 84);
        }

        private void DrawTimerBar(Graphics g, int x, int y)
        {
            const int barWidth = 300;
            const int barHeight = 18;

            using (var cream = new SolidBrush(Theme.Cream))
            {
                if (!_settings.HasTimer)
                {
                    g.DrawString("No timer", FontCache.Get(GameConfig.TextFontName, 11f), cream, x, y);
                    return;
                }

                Color barColor = _timer.Ratio > 0.25f ? Theme.Good : Theme.Bad;
                using (var back = new SolidBrush(Color.FromArgb(40, 25, 15)))
                using (var fill = new SolidBrush(barColor))
                {
                    g.FillRectangle(back, x, y, barWidth, barHeight);
                    g.FillRectangle(fill, x, y, barWidth * _timer.Ratio, barHeight);
                }

                g.DrawString(_timer.SecondsLeft.ToString("0.0") + "s", FontCache.Get(GameConfig.TextFontName, 11f, FontStyle.Bold),
                             cream, x + barWidth + 8, y);
            }
        }

        private void DrawScore(Graphics g)
        {
            using (var cream = new SolidBrush(Theme.Cream))
            using (var gold = new SolidBrush(Theme.Gold))
            {
                g.DrawString("KULTURA SCORE", FontCache.Get(GameConfig.TextFontName, 11f, FontStyle.Bold), cream, 720, GameConfig.HudTop + 20);
                g.DrawString(_score.Score.ToString(), FontCache.Get(GameConfig.TextFontName, 34f, FontStyle.Bold), gold, 716, GameConfig.HudTop + 40);
            }
        }
    }
}

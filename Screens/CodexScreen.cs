using System;
using System.Collections.Generic;
using System.Drawing;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Core;
using AgimatNgBaybayin.Models;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Screens
{
    /// <summary>
    /// The Baybayin Codex: a grid of cards (glyph on top, Latin sound below).
    /// Used for the tutorial AND for the full Codex unlocked at the end of the game.
    /// </summary>
    public class CodexScreen : GameScreen
    {
        private const int CardsPerRow = 10;
        private const int CardWidth = 84;
        private const int CardHeight = 84;
        private const int CardGap = 8;
        private const int GridTop = 130;

        private readonly string _title;
        private readonly string _subtitle;
        private readonly List<Syllable> _syllables;

        public CodexScreen(IGameHost host, string title, string subtitle,
                           List<Syllable> syllables, string continueButtonText, Action onContinue)
            : base(host)
        {
            _title = title;
            _subtitle = subtitle;
            _syllables = syllables;

            Buttons.Add(new MenuButton(continueButtonText, new Rectangle(340, 520, 280, 50), onContinue));
        }

        public override void Draw(Graphics g)
        {
            Theme.DrawBackdrop(g, GameConfig.WindowWidth, GameConfig.WindowHeight);

            DrawCenteredLine(g, _title, FontCache.Get(GameConfig.TextFontName, 30f, FontStyle.Bold), Theme.Gold, 30, 50);
            DrawCenteredLine(g, _subtitle, FontCache.Get(GameConfig.TextFontName, 13f), Theme.Cream, 85, 28);

            DrawCards(g);
            DrawButtons(g);
        }

        private void DrawCards(Graphics g)
        {
            int columns = Math.Min(CardsPerRow, _syllables.Count);
            int gridWidth = columns * CardWidth + (columns - 1) * CardGap;
            int startX = (GameConfig.WindowWidth - gridWidth) / 2;

            for (int i = 0; i < _syllables.Count; i++)
            {
                int column = i % CardsPerRow;
                int row = i / CardsPerRow;
                var card = new Rectangle(startX + column * (CardWidth + CardGap),
                                         GridTop + row * (CardHeight + CardGap),
                                         CardWidth, CardHeight);

                using (var fill = new SolidBrush(Theme.WoodMid))
                using (var border = new Pen(Theme.WoodDark, 3))
                using (var glyphBrush = new SolidBrush(Theme.Cream))
                using (var latinBrush = new SolidBrush(Theme.Gold))
                {
                    g.FillRectangle(fill, card);
                    g.DrawRectangle(border, card);

                    var glyphArea = new RectangleF(card.X, card.Y + 2, card.Width, 52);
                    var latinArea = new RectangleF(card.X, card.Y + 54, card.Width, 26);
                    Theme.DrawCentered(g, _syllables[i].Glyph, FontCache.Get(GameConfig.GlyphFontName, 26f), glyphBrush, glyphArea);
                    Theme.DrawCentered(g, _syllables[i].Latin, FontCache.Get(GameConfig.TextFontName, 12f, FontStyle.Bold), latinBrush, latinArea);
                }
            }
        }
    }
}

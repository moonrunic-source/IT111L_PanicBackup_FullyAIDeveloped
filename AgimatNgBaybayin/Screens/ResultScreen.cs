using System;
using System.Drawing;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Core;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Screens
{
    /// <summary>
    /// One reusable screen for: warm-up complete, level complete, game over and victory.
    /// The caller decides the title, the text lines and the buttons.
    /// </summary>
    public class ResultScreen : GameScreen
    {
        private readonly string _title;
        private readonly string[] _lines;
        private readonly Color _titleColor;

        public ResultScreen(IGameHost host, string title, Color titleColor, string[] lines,
                            params (string Text, Action OnClick)[] buttons)
            : base(host)
        {
            _title = title;
            _titleColor = titleColor;
            _lines = lines;

            int buttonWidth = 280;
            int buttonHeight = 50;
            int buttonX = (GameConfig.WindowWidth - buttonWidth) / 2;

            for (int i = 0; i < buttons.Length; i++)
            {
                var bounds = new Rectangle(buttonX, 340 + i * 64, buttonWidth, buttonHeight);
                Buttons.Add(new MenuButton(buttons[i].Text, bounds, buttons[i].OnClick));
            }
        }

        public override void Draw(Graphics g)
        {
            Theme.DrawBackdrop(g, GameConfig.WindowWidth, GameConfig.WindowHeight);

            DrawCenteredLine(g, _title, FontCache.Get(GameConfig.TextFontName, 40f, FontStyle.Bold), _titleColor, 70, 80);

            for (int i = 0; i < _lines.Length; i++)
                DrawCenteredLine(g, _lines[i], FontCache.Get(GameConfig.TextFontName, 18f), Theme.Cream, 170 + i * 40, 36);

            DrawButtons(g);
        }
    }
}

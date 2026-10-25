using System.Drawing;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Core;
using AgimatNgBaybayin.Models;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Screens
{
    /// <summary>Title screen.</summary>
    public class MenuScreen : GameScreen
    {
        public MenuScreen(IGameHost host) : base(host)
        {
            int buttonWidth = 280;
            int buttonHeight = 52;
            int buttonX = (GameConfig.WindowWidth - buttonWidth) / 2;

            Buttons.Add(new MenuButton("Start Game", new Rectangle(buttonX, 300, buttonWidth, buttonHeight), StartNewGame));
            Buttons.Add(new MenuButton("Quit", new Rectangle(buttonX, 370, buttonWidth, buttonHeight), Host.QuitGame));
        }

        private void StartNewGame()
        {
            Host.Progress.Reset();
            Host.ShowScreen(new StageIntroScreen(Host, 1));
        }

        public override void Draw(Graphics g)
        {
            Theme.DrawBackdrop(g, GameConfig.WindowWidth, GameConfig.WindowHeight);

            // Decorative row of vowel characters: A  I/E  O/U
            string decoration = BaybayinConverter.ToGlyph("A") + "   " +
                                BaybayinConverter.ToGlyph("I") + "   " +
                                BaybayinConverter.ToGlyph("U");
            DrawCenteredLine(g, decoration, FontCache.Get(GameConfig.GlyphFontName, 40f), Theme.Gold, 40, 70);

            DrawCenteredLine(g, "Agimat ng Baybayin", FontCache.Get(GameConfig.TextFontName, 40f, FontStyle.Bold), Theme.Cream, 120, 70);
            DrawCenteredLine(g, "Catch the characters. Complete the words. Unlock the Codex.",
                             FontCache.Get(GameConfig.TextFontName, 14f), Theme.Cream, 195, 30);

            DrawButtons(g);
        }
    }
}

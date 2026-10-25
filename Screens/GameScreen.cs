using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AgimatNgBaybayin.Core;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Screens
{
    /// <summary>
    /// Base class of every screen (menu, tutorial, gameplay, results).
    /// Handles button hover / click so child screens only add what is different.
    /// </summary>
    public abstract class GameScreen
    {
        protected readonly IGameHost Host;
        protected readonly List<MenuButton> Buttons = new List<MenuButton>();

        protected GameScreen(IGameHost host)
        {
            Host = host;
        }

        public virtual void OnEnter() { }
        public virtual void Update(float deltaSeconds) { }
        public abstract void Draw(Graphics g);
        public virtual void OnKeyDown(Keys key) { }
        public virtual void OnKeyUp(Keys key) { }

        public virtual void OnMouseMove(Point mousePosition)
        {
            foreach (MenuButton button in Buttons)
                button.IsHovered = button.Contains(mousePosition);
        }

        public virtual void OnMouseDown(Point mousePosition)
        {
            foreach (MenuButton button in Buttons)
            {
                if (button.Contains(mousePosition))
                {
                    button.OnClick();
                    return;   // the click may have switched screens, stop here
                }
            }
        }

        protected void DrawButtons(Graphics g)
        {
            foreach (MenuButton button in Buttons)
                button.Draw(g);
        }

        /// <summary>Draws a line of text centered across the whole window width.</summary>
        protected void DrawCenteredLine(Graphics g, string text, Font font, Color color, float y, float height)
        {
            using (var brush = new SolidBrush(color))
                Theme.DrawCentered(g, text, font, brush, new RectangleF(0, y, Config.GameConfig.WindowWidth, height));
        }
    }
}

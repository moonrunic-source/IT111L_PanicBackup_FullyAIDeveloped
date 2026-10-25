using System;
using System.Drawing;

namespace AgimatNgBaybayin.UI
{
    /// <summary>A clickable wooden button drawn with GDI+.</summary>
    public class MenuButton
    {
        public Rectangle Bounds { get; private set; }
        public string Text { get; private set; }
        public Action OnClick { get; private set; }
        public bool IsHovered { get; set; }

        public MenuButton(string text, Rectangle bounds, Action onClick)
        {
            Text = text;
            Bounds = bounds;
            OnClick = onClick;
        }

        public bool Contains(Point mousePosition)
        {
            return Bounds.Contains(mousePosition);
        }

        public void Draw(Graphics g)
        {
            Color fillColor = IsHovered ? Theme.WoodLight : Theme.WoodMid;

            using (var fill = new SolidBrush(fillColor))
            using (var border = new Pen(Theme.WoodDark, 3))
            using (var textBrush = new SolidBrush(Theme.Cream))
            {
                g.FillRectangle(fill, Bounds);
                g.DrawRectangle(border, Bounds);
                Theme.DrawCentered(g, Text, FontCache.Get("Segoe UI", 14f, FontStyle.Bold), textBrush, Bounds);
            }
        }
    }
}

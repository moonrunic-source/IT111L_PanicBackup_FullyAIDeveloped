using System.Drawing;
using System.Drawing.Drawing2D;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Entities
{
    /// <summary>
    /// The rare golden "Agimat" orb.
    /// Restores 1 life, or gives bonus points when lives are already full.
    /// </summary>
    public class AgimatOrb : Orb
    {
        public AgimatOrb(float x, float y, float speed) : base(x, y, speed)
        {
        }

        public override void Draw(Graphics g)
        {
            DrawHangingString(g);

            var circle = new RectangleF(X, Y, Width, Height);
            using (var fill = new LinearGradientBrush(circle, Color.FromArgb(255, 235, 140), Color.FromArgb(200, 140, 20), 90f))
            using (var ring = new Pen(Color.FromArgb(120, 80, 10), 4))
            using (var starBrush = new SolidBrush(Color.FromArgb(255, 255, 240)))
            {
                g.FillEllipse(fill, circle);
                g.DrawEllipse(ring, circle);
                Theme.DrawCentered(g, "\u2605", FontCache.Get(GameConfig.SymbolFontName, 26f), starBrush, circle);
            }
        }
    }
}

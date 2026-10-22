using System.Drawing;
using System.Drawing.Drawing2D;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Models;
using AgimatNgBaybayin.UI;

namespace AgimatNgBaybayin.Entities
{
    /// <summary>A wooden orb carrying one Baybayin character.</summary>
    public class LetterOrb : Orb
    {
        private readonly bool _showLatinHint;

        public Syllable Syllable { get; private set; }

        public LetterOrb(float x, float y, float speed, Syllable syllable, bool showLatinHint)
            : base(x, y, speed)
        {
            Syllable = syllable;
            _showLatinHint = showLatinHint;
        }

        public override void Draw(Graphics g)
        {
            DrawHangingString(g);

            var circle = new RectangleF(X, Y, Width, Height);
            using (var fill = new LinearGradientBrush(circle, Theme.WoodLight, Theme.WoodMid, 90f))
            using (var ring = new Pen(Theme.WoodDark, 4))
            using (var textBrush = new SolidBrush(Theme.Cream))
            {
                g.FillEllipse(fill, circle);
                g.DrawEllipse(ring, circle);

                // With the guide on, the glyph moves up a little to make room for the Latin label.
                RectangleF glyphArea = _showLatinHint
                    ? new RectangleF(X, Y + 2, Width, Height - 22)
                    : circle;
                Theme.DrawCentered(g, Syllable.Glyph, FontCache.Get(GameConfig.GlyphFontName, 26f), textBrush, glyphArea);

                if (_showLatinHint)
                {
                    var labelArea = new RectangleF(X, Y + Height - 26, Width, 18);
                    Theme.DrawCentered(g, Syllable.Latin, FontCache.Get(GameConfig.TextFontName, 10f, FontStyle.Bold), textBrush, labelArea);
                }
            }
        }
    }
}

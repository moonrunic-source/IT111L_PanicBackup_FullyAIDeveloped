using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AgimatNgBaybayin.UI
{
    /// <summary>Colors and drawing helpers shared by every screen.</summary>
    public static class Theme
    {
        // ---------- Colors ----------
        public static readonly Color WoodDark = Color.FromArgb(60, 38, 22);
        public static readonly Color WoodMid = Color.FromArgb(110, 72, 42);
        public static readonly Color WoodLight = Color.FromArgb(190, 140, 90);
        public static readonly Color Cream = Color.FromArgb(255, 245, 220);
        public static readonly Color Gold = Color.FromArgb(255, 210, 90);
        public static readonly Color Good = Color.FromArgb(120, 220, 120);
        public static readonly Color Bad = Color.FromArgb(255, 110, 110);

        private static readonly StringFormat CenterFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            FormatFlags = StringFormatFlags.NoWrap
        };

        private static readonly PointF[] Stars = BuildStars();

        private static PointF[] BuildStars()
        {
            var random = new Random(7);
            var stars = new PointF[40];
            for (int i = 0; i < stars.Length; i++)
                stars[i] = new PointF(random.Next(0, 960), random.Next(0, 300));
            return stars;
        }

        /// <summary>Draws text centered inside a rectangle.</summary>
        public static void DrawCentered(Graphics g, string text, Font font, Brush brush, RectangleF area)
        {
            g.DrawString(text, font, brush, area, CenterFormat);
        }

        /// <summary>Night sky + terraces backdrop used behind every screen.</summary>
        public static void DrawBackdrop(Graphics g, int width, int height)
        {
            var area = new Rectangle(0, 0, width, height);
            using (var sky = new LinearGradientBrush(area, Color.FromArgb(25, 10, 50), Color.FromArgb(85, 40, 105), 90f))
                g.FillRectangle(sky, area);

            using (var starBrush = new SolidBrush(Color.FromArgb(180, 255, 255, 255)))
                foreach (PointF star in Stars)
                    g.FillEllipse(starBrush, star.X, star.Y, 2.5f, 2.5f);

            // Simple rice-terrace hills.
            using (var farHills = new SolidBrush(Color.FromArgb(70, 45, 90)))
                g.FillPolygon(farHills, new[]
                {
                    new PointF(0, 470), new PointF(0, 330), new PointF(180, 260), new PointF(360, 340),
                    new PointF(560, 250), new PointF(780, 330), new PointF(960, 270), new PointF(960, 470)
                });
            using (var nearHills = new SolidBrush(Color.FromArgb(45, 28, 70)))
                g.FillPolygon(nearHills, new[]
                {
                    new PointF(0, 470), new PointF(0, 400), new PointF(250, 360), new PointF(500, 410),
                    new PointF(760, 365), new PointF(960, 405), new PointF(960, 470)
                });
        }
    }
}

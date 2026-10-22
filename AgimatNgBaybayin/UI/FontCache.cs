using System.Collections.Generic;
using System.Drawing;

namespace AgimatNgBaybayin.UI
{
    /// <summary>Creates each font once and reuses it (drawing runs 60 times per second).</summary>
    public static class FontCache
    {
        private static readonly Dictionary<string, Font> Cache = new Dictionary<string, Font>();

        public static Font Get(string fontName, float size, FontStyle style = FontStyle.Regular)
        {
            string key = fontName + "|" + size + "|" + (int)style;

            Font font;
            if (!Cache.TryGetValue(key, out font))
            {
                font = new Font(fontName, size, style, GraphicsUnit.Point);
                Cache[key] = font;
            }
            return font;
        }
    }
}

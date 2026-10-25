using System;
using System.Drawing;
using AgimatNgBaybayin.Config;

namespace AgimatNgBaybayin.Entities
{
    /// <summary>The winged hero. Moves only UP and DOWN.</summary>
    public class Player : GameObject
    {
        public bool IsMovingUp { get; set; }
        public bool IsMovingDown { get; set; }

        public Player()
        {
            Width = GameConfig.PlayerWidth;
            Height = GameConfig.PlayerHeight;
            X = GameConfig.PlayerStartX;
            Y = (GameConfig.PlayAreaTop + GameConfig.HudTop) / 2f - Height / 2f;
        }

        public override void Update(float deltaSeconds)
        {
            float direction = 0f;
            if (IsMovingUp) direction -= 1f;
            if (IsMovingDown) direction += 1f;

            Y += direction * GameConfig.PlayerSpeed * deltaSeconds;

            // Keep the player inside the play area.
            float minY = GameConfig.PlayAreaTop;
            float maxY = GameConfig.HudTop - Height;
            Y = Math.Max(minY, Math.Min(maxY, Y));
        }

        public override void Draw(Graphics g)
        {
            float centerX = X + Width / 2f;
            float centerY = Y + Height / 2f;

            PointF[] wings =
            {
                new PointF(X, centerY),
                new PointF(centerX, Y),
                new PointF(X + Width, centerY - 6),
                new PointF(centerX, centerY + 8)
            };

            using (var wingBrush = new SolidBrush(Color.FromArgb(220, 70, 140, 70)))
            using (var bodyBrush = new SolidBrush(Color.FromArgb(205, 175, 130)))
            using (var outline = new Pen(Color.FromArgb(40, 30, 20), 2))
            {
                g.FillPolygon(wingBrush, wings);
                g.DrawPolygon(outline, wings);

                g.FillEllipse(bodyBrush, centerX - 12, centerY - 8, 24, 30);
                g.DrawEllipse(outline, centerX - 12, centerY - 8, 24, 30);

                g.FillEllipse(bodyBrush, centerX - 9, centerY - 22, 18, 18);   // head
                g.DrawEllipse(outline, centerX - 9, centerY - 22, 18, 18);
            }
        }
    }
}

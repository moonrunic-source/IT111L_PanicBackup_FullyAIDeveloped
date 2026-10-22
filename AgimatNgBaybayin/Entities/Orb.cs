using System.Drawing;
using AgimatNgBaybayin.Config;

namespace AgimatNgBaybayin.Entities
{
    /// <summary>
    /// Base class of every floating orb. Orbs hang from the top by a string
    /// and drift left toward the player. Child classes decide what the orb looks like.
    /// </summary>
    public abstract class Orb : GameObject
    {
        public float Speed { get; set; }

        protected Orb(float x, float y, float speed)
        {
            X = x;
            Y = y;
            Speed = speed;
            Width = GameConfig.OrbSize;
            Height = GameConfig.OrbSize;
        }

        public override void Update(float deltaSeconds)
        {
            X -= Speed * deltaSeconds;

            // Orb left the screen: the player skipped it.
            if (X + Width < 0)
                IsActive = false;
        }

        protected void DrawHangingString(Graphics g)
        {
            using (var pen = new Pen(Color.FromArgb(150, 90, 60, 40), 2))
                g.DrawLine(pen, X + Width / 2f, 0, X + Width / 2f, Y + 4);
        }
    }
}

using System.Drawing;

namespace AgimatNgBaybayin.Entities
{
    /// <summary>
    /// Base class of everything that lives in the play area (player, orbs...).
    /// Child classes decide how they update and how they look.
    /// </summary>
    public abstract class GameObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public bool IsActive { get; set; }

        public RectangleF Bounds
        {
            get { return new RectangleF(X, Y, Width, Height); }
        }

        protected GameObject()
        {
            IsActive = true;
        }

        public bool Intersects(GameObject other)
        {
            return Bounds.IntersectsWith(other.Bounds);
        }

        public abstract void Update(float deltaSeconds);
        public abstract void Draw(Graphics g);
    }
}

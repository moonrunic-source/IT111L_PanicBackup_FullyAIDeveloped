using AgimatNgBaybayin.Config;

namespace AgimatNgBaybayin.Systems
{
    /// <summary>Owns the player's hearts. Nothing else changes lives directly.</summary>
    public class LifeManager
    {
        public int Lives { get; private set; }
        public int MaxLives { get { return GameConfig.MaxLives; } }
        public bool IsFull { get { return Lives >= MaxLives; } }
        public bool IsOutOfLives { get { return Lives <= 0; } }

        public LifeManager()
        {
            Reset();
        }

        public void Reset()
        {
            Lives = GameConfig.StartingLives;
        }

        public void LoseLife()
        {
            if (Lives > 0)
                Lives--;
        }

        /// <summary>Gives +1 life. Returns false (and does nothing) when already full.</summary>
        public bool TryRestoreLife()
        {
            if (IsFull)
                return false;

            Lives++;
            return true;
        }
    }
}

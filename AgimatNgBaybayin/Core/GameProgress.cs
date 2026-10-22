using AgimatNgBaybayin.Config;

namespace AgimatNgBaybayin.Core
{
    /// <summary>Progress that survives from one screen to the next during a run.</summary>
    public class GameProgress
    {
        /// <summary>Score the player had when the current level started (used when retrying).</summary>
        public int TotalScore { get; set; }

        public GameProgress()
        {
            Reset();
        }

        public void Reset()
        {
            TotalScore = GameConfig.StartingScore;
        }
    }
}

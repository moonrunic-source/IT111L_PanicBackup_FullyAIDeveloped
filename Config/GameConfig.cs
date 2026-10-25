namespace AgimatNgBaybayin.Config
{
    /// <summary>
    /// EVERY tweakable number of the game lives here.
    /// Change a value in this file and the whole game follows,
    /// no need to hunt through other classes.
    /// </summary>
    public static class GameConfig
    {
        // ---------- Window ----------
        public const int WindowWidth = 960;
        public const int WindowHeight = 600;
        public const int FramesPerSecond = 60;

        // ---------- Layout (the play area sits between these two Y values) ----------
        public const int PlayAreaTop = 40;
        public const int HudTop = 470;          // everything below this is the HUD (hearts, word, score)

        // ---------- Lives ----------
        public const int StartingLives = 3;
        public const int MaxLives = 3;

        // ---------- Scoring (from the proposal) ----------
        public const int StartingScore = 0;
        public const int PointsPerCorrectLetter = 10;
        public const int PointsPerCompletedWord = 50;
        public const int PenaltyWrongLetter = 25;
        public const int BonusAgimatAtFullLives = 15;
        public const bool ScoreCannotGoBelowZero = true;

        // ---------- Player ----------
        public const int PlayerStartX = 70;
        public const int PlayerWidth = 70;
        public const int PlayerHeight = 60;
        public const float PlayerSpeed = 360f;   // pixels per second (up / down only)

        // ---------- Orbs ----------
        public const int OrbSize = 68;
        public const double AgimatSpawnChance = 0.08;        // 8% of spawns are golden Agimat orbs
        public const double NeededLetterSpawnChance = 0.45;  // chance a spawned orb is a letter the word needs

        // ---------- Feedback ----------
        public const float FlashMessageSeconds = 1.4f;

        // ---------- Fonts ----------
        // "Segoe UI Historic" ships with Windows 10/11 and can draw Baybayin (Tagalog block).
        public const string GlyphFontName = "Noto Sans Tagalog";
        public const string TextFontName = "Times New Roman";
        public const string SymbolFontName = "Segoe UI Symbol";
    }
}

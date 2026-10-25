namespace AgimatNgBaybayin.Config
{
    /// <summary>
    /// All the rules of ONE level. Create / edit levels in LevelCatalog.
    /// </summary>
    public class LevelSettings
    {
        public int LevelNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // ---------- Pace ----------
        public float OrbSpeed { get; set; }              // how fast orbs fly toward the player (px/sec)
        public float SpawnIntervalSeconds { get; set; }  // seconds between new orbs

        // ---------- Timer ----------
        public float SecondsPerWord { get; set; }        // 0 = no timer
        public bool HasTimer { get { return SecondsPerWord > 0f; } }

        // ---------- Words ----------
        public int MinSyllablesPerWord { get; set; }
        public int MaxSyllablesPerWord { get; set; }
        public int BlanksPerWord { get; set; }           // how many characters are missing in each word
        public int WordsToComplete { get; set; }         // the "mission" of the level

        // ---------- Help shown to the player ----------
        public bool ShowTranslationGuide { get; set; }   // Level 1: Baybayin <-> Latin guide
        public bool ShowWordInBaybayin { get; set; }     // Level 4: the word itself is drawn in Baybayin

        // ---------- Practice warm-up (tutorial) ----------
        public bool IsPractice { get; set; }             // practice never costs lives

        /// <summary>Makes an independent copy so editing a copy never changes the catalog.</summary>
        public LevelSettings Clone()
        {
            return (LevelSettings)MemberwiseClone();
        }
    }
}

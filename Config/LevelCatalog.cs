using System;

namespace AgimatNgBaybayin.Config
{
    /// <summary>
    /// The 4 levels from the proposal. To rebalance the game, edit the numbers below.
    /// </summary>
    public static class LevelCatalog
    {
        public const int TotalLevels = 4;

        private static readonly LevelSettings[] Levels =
        {
            // LEVEL 1 - common words, translation guide on, no timer
            new LevelSettings
            {
                LevelNumber = 1,
                Title = "Unang Hakbang",
                Description = "Common words. The Baybayin-to-Latin guide is shown.",
                OrbSpeed = 110f,
                SpawnIntervalSeconds = 1.4f,
                SecondsPerWord = 0f,
                MinSyllablesPerWord = 2,
                MaxSyllablesPerWord = 2,
                BlanksPerWord = 1,
                WordsToComplete = 5,
                ShowTranslationGuide = true,
                ShowWordInBaybayin = false
            },

            // LEVEL 2 - faster, timer appears, guide removed
            new LevelSettings
            {
                LevelNumber = 2,
                Title = "Bilis at Alaala",
                Description = "The game speeds up, a timer appears and the guide is gone.",
                OrbSpeed = 170f,
                SpawnIntervalSeconds = 1.2f,
                SecondsPerWord = 20f,
                MinSyllablesPerWord = 2,
                MaxSyllablesPerWord = 3,
                BlanksPerWord = 1,
                WordsToComplete = 6,
                ShowTranslationGuide = false,
                ShowWordInBaybayin = false
            },

            // LEVEL 3 - longer words, more blanks, faster, shorter timer
            new LevelSettings
            {
                LevelNumber = 3,
                Title = "Mahabang Salita",
                Description = "Longer words, more missing characters, faster pace, shorter timer.",
                OrbSpeed = 230f,
                SpawnIntervalSeconds = 1.0f,
                SecondsPerWord = 16f,
                MinSyllablesPerWord = 3,
                MaxSyllablesPerWord = 4,
                BlanksPerWord = 2,
                WordsToComplete = 6,
                ShowTranslationGuide = false,
                ShowWordInBaybayin = false
            },

            // LEVEL 4 - words become full Baybayin, slower pace, longer timer
            new LevelSettings
            {
                LevelNumber = 4,
                Title = "Agimat ng Baybayin",
                Description = "Words are written in Baybayin. Slower pace and a longer timer.",
                OrbSpeed = 140f,
                SpawnIntervalSeconds = 1.3f,
                SecondsPerWord = 28f,
                MinSyllablesPerWord = 3,
                MaxSyllablesPerWord = 4,
                BlanksPerWord = 3,
                WordsToComplete = 6,
                ShowTranslationGuide = false,
                ShowWordInBaybayin = true
            }

        };

        /// <summary>Returns a fresh copy of a level (1-based number).</summary>
        public static LevelSettings GetLevel(int levelNumber)
        {
            return Levels[levelNumber - 1].Clone();
        }

        /// <summary>Gentle warm-up version of a level used by the tutorial phase.</summary>
        public static LevelSettings GetPracticeVersion(int levelNumber)
        {
            LevelSettings practice = GetLevel(levelNumber);
            practice.IsPractice = true;
            practice.Title = "Practice Warm-up";
            practice.OrbSpeed = Math.Min(practice.OrbSpeed, 100f);
            practice.SpawnIntervalSeconds = 1.6f;
            practice.SecondsPerWord = 0f;
            practice.ShowTranslationGuide = true;
            practice.ShowWordInBaybayin = false;
            practice.WordsToComplete = 2;
            return practice;
        }
    }
}

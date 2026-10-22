using System;
using System.Collections.Generic;
using System.Linq;
using AgimatNgBaybayin.Models;

namespace AgimatNgBaybayin.Systems
{
    /// <summary>State of one letter slot of the word being built.</summary>
    public enum SlotState
    {
        Revealed,   // already shown to the player
        Missing,    // blank, waiting for the player to catch the right character
        Filled      // the player caught the right character
    }

    /// <summary>
    /// The word the player is currently completing:
    /// which slots are blank, which are filled, and whether the word is done.
    /// </summary>
    public class WordSession
    {
        private readonly SlotState[] _slots;

        public Word Word { get; private set; }
        public int SlotCount { get { return _slots.Length; } }
        public bool IsComplete { get { return !_slots.Contains(SlotState.Missing); } }

        public WordSession(Word word, int blanksWanted, Random rng)
        {
            Word = word;
            _slots = new SlotState[word.Syllables.Count];

            for (int i = 0; i < _slots.Length; i++)
                _slots[i] = SlotState.Revealed;

            // Randomly choose which slots become blanks.
            int blanks = Math.Min(blanksWanted, _slots.Length);
            List<int> blankIndexes = Enumerable.Range(0, _slots.Length)
                                               .OrderBy(i => rng.Next())
                                               .Take(blanks)
                                               .ToList();
            foreach (int index in blankIndexes)
                _slots[index] = SlotState.Missing;
        }

        public SlotState GetState(int slotIndex)
        {
            return _slots[slotIndex];
        }

        public Syllable GetSyllable(int slotIndex)
        {
            return Word.Syllables[slotIndex];
        }

        /// <summary>The characters the player still has to catch.</summary>
        public List<Syllable> GetMissingSyllables()
        {
            var missing = new List<Syllable>();
            for (int i = 0; i < _slots.Length; i++)
                if (_slots[i] == SlotState.Missing)
                    missing.Add(Word.Syllables[i]);
            return missing;
        }

        /// <summary>Fills the first blank that matches. Returns false if the character is wrong.</summary>
        public bool TryFill(Syllable caughtSyllable)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == SlotState.Missing && Word.Syllables[i].Matches(caughtSyllable))
                {
                    _slots[i] = SlotState.Filled;
                    return true;
                }
            }
            return false;
        }
    }
}

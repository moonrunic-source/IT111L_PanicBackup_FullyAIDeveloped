using System;
using System.Collections.Generic;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Entities;
using AgimatNgBaybayin.Models;

namespace AgimatNgBaybayin.Systems
{
    /// <summary>Creates new orbs at the right edge of the screen on a timer.</summary>
    public class OrbSpawner
    {
        private readonly Random _rng;
        private readonly List<Syllable> _allSyllables;   // pool used for wrong / distraction orbs
        private float _secondsUntilNextSpawn;

        public OrbSpawner(Random rng, List<Syllable> syllablePool)
        {
            _rng = rng;
            _allSyllables = syllablePool;
        }

        /// <summary>Returns a new orb when it is time to spawn one, otherwise null.</summary>
        public Orb TrySpawn(float deltaSeconds, LevelSettings settings, WordSession session)
        {
            _secondsUntilNextSpawn -= deltaSeconds;
            if (_secondsUntilNextSpawn > 0f)
                return null;

            _secondsUntilNextSpawn = settings.SpawnIntervalSeconds;

            float spawnX = GameConfig.WindowWidth + 10;
            float minY = GameConfig.PlayAreaTop;
            float maxY = GameConfig.HudTop - GameConfig.OrbSize;
            float spawnY = minY + (float)_rng.NextDouble() * (maxY - minY);

            // Rare golden Agimat orb.
            if (_rng.NextDouble() < GameConfig.AgimatSpawnChance)
                return new AgimatOrb(spawnX, spawnY, settings.OrbSpeed);

            Syllable syllable = ChooseSyllable(session);
            return new LetterOrb(spawnX, spawnY, settings.OrbSpeed, syllable, settings.ShowTranslationGuide);
        }

        /// <summary>Sometimes a needed character (so the level is winnable), otherwise a random one.</summary>
        private Syllable ChooseSyllable(WordSession session)
        {
            List<Syllable> needed = session.GetMissingSyllables();

            if (needed.Count > 0 && _rng.NextDouble() < GameConfig.NeededLetterSpawnChance)
                return needed[_rng.Next(needed.Count)];

            return _allSyllables[_rng.Next(_allSyllables.Count)];
        }
    }
}

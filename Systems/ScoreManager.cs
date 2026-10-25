using AgimatNgBaybayin.Config;

namespace AgimatNgBaybayin.Systems
{
    /// <summary>Owns the score. All point values come from GameConfig.</summary>
    public class ScoreManager
    {
        public int Score { get; private set; }

        public void Reset(int startingScore)
        {
            Score = startingScore;
        }

        public void AddCorrectLetter()   { ChangeScore(GameConfig.PointsPerCorrectLetter); }
        public void AddCompletedWord()   { ChangeScore(GameConfig.PointsPerCompletedWord); }
        public void AddAgimatBonus()     { ChangeScore(GameConfig.BonusAgimatAtFullLives); }
        public void PenalizeWrongLetter(){ ChangeScore(-GameConfig.PenaltyWrongLetter); }

        private void ChangeScore(int points)
        {
            Score += points;

            if (GameConfig.ScoreCannotGoBelowZero && Score < 0)
                Score = 0;
        }
    }
}

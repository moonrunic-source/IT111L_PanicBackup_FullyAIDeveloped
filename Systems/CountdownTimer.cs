namespace AgimatNgBaybayin.Systems
{
    /// <summary>Counts down the time to complete one word.</summary>
    public class CountdownTimer
    {
        private float _totalSeconds;

        public float SecondsLeft { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsExpired { get; private set; }

        /// <summary>Fraction of time left (1 = full, 0 = empty).</summary>
        public float Ratio
        {
            get { return _totalSeconds > 0f ? SecondsLeft / _totalSeconds : 1f; }
        }

        /// <summary>Starts the timer. Passing 0 means "no timer" for this word.</summary>
        public void Start(float seconds)
        {
            _totalSeconds = seconds;
            SecondsLeft = seconds;
            IsExpired = false;
            IsRunning = seconds > 0f;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Update(float deltaSeconds)
        {
            if (!IsRunning)
                return;

            SecondsLeft -= deltaSeconds;

            if (SecondsLeft <= 0f)
            {
                SecondsLeft = 0f;
                IsRunning = false;
                IsExpired = true;
            }
        }
    }
}

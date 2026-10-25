using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using AgimatNgBaybayin.Config;
using AgimatNgBaybayin.Screens;

namespace AgimatNgBaybayin.Core
{
    /// <summary>
    /// The one and only window. It runs the game loop (update + redraw ~60 times a second)
    /// and forwards keyboard / mouse input to whichever screen is active.
    /// </summary>
    public class GameForm : Form, IGameHost
    {
        private readonly Timer _gameLoopTimer = new Timer();
        private readonly Stopwatch _clock = new Stopwatch();
        private GameScreen _currentScreen;
        private double _lastFrameSeconds;

        public GameProgress Progress { get; private set; }

        public GameForm()
        {
            Progress = new GameProgress();

            Text = "Agimat ng Baybayin";
            ClientSize = new Size(GameConfig.WindowWidth, GameConfig.WindowHeight);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true;

            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            _gameLoopTimer.Interval = 1000 / GameConfig.FramesPerSecond;
            _gameLoopTimer.Tick += OnGameLoopTick;

            ShowScreen(new MenuScreen(this));

            _clock.Start();
            _gameLoopTimer.Start();
        }

        // ---------- IGameHost ----------
        public void ShowScreen(GameScreen screen)
        {
            _currentScreen = screen;
            _currentScreen.OnEnter();
        }

        public void QuitGame()
        {
            Close();
        }

        // ---------- Game loop ----------
        private void OnGameLoopTick(object sender, EventArgs e)
        {
            double nowSeconds = _clock.Elapsed.TotalSeconds;
            float deltaSeconds = (float)Math.Min(nowSeconds - _lastFrameSeconds, 0.05);   // cap big hiccups
            _lastFrameSeconds = nowSeconds;

            _currentScreen.Update(deltaSeconds);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            _currentScreen.Draw(e.Graphics);
        }

        // ---------- Input forwarding ----------
        protected override void OnKeyDown(KeyEventArgs e)
        {
            _currentScreen.OnKeyDown(e.KeyCode);
            e.Handled = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _currentScreen.OnKeyUp(e.KeyCode);
            e.Handled = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _currentScreen.OnMouseMove(e.Location);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _currentScreen.OnMouseDown(e.Location);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _gameLoopTimer.Stop();
            base.OnFormClosed(e);
        }
    }
}

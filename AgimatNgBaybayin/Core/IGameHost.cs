using AgimatNgBaybayin.Screens;

namespace AgimatNgBaybayin.Core
{
    /// <summary>What a screen is allowed to ask of the game window.</summary>
    public interface IGameHost
    {
        GameProgress Progress { get; }
        void ShowScreen(GameScreen screen);
        void QuitGame();
    }
}

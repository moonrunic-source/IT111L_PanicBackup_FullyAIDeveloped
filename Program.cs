using System;
using System.Windows.Forms;
using AgimatNgBaybayin.Core;

namespace AgimatNgBaybayin
{
    /// <summary>Entry point of the game.</summary>
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
        }
    }
}

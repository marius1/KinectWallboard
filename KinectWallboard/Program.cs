using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KinectWallboard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool debugMode = false;
            bool noFullscreen = false;

            foreach (string arg in args)
            {
                if (arg == "/debug")
                    debugMode = true;

                if (arg == "/nofullscreen")
                    noFullscreen = true;
            }

            Application.Run(new Main(debugMode, !noFullscreen));
        }
    }
}

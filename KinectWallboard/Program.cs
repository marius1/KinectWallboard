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

            bool debugMode = args.Length > 0 && args[0] == "/debug";

            Application.Run(new Main(debugMode));
        }
    }
}

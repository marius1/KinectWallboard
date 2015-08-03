using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            KinectServer server = new KinectServer();
            server.Start();            

            while (ShouldQuit())
            {                
            }

            server.Stop();
        }

        private static bool ShouldQuit()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    return false;
            }

            return true;
        }
    }
}

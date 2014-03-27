using System;
using System.Collections.Generic;
using System.Text;

namespace KinectWallboard
{
    public class WebPage
    {
        public string Address { get; set; }

        public JavaScriptAction OnReady { get; set; }

        public int Timeout { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public WebPage()
        {
            Timeout = 20;
        }
    }
}

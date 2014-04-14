using System;
using System.Collections.Generic;
using System.Text;

namespace KinectWallboard
{
    [Serializable]
    public class WebPage
    {
        public string Address { get; set; }

        public string JavaScript { get; set; }
        public bool InjectJQuery { get; set; }
        public bool AllowUpdates { get; set; }
        public int Timeout { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public WebPage()
        {
            Timeout = 20;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWallboard.Gesture
{
    public class GestureRecognizedEvent : EventArgs
    {
        public GestureType Type { get; set; }

        public GestureRecognizedEvent(GestureType type)
        {
            Type = type;
        }
    }
}

using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWallboard.Gesture.Gestures
{
    public interface IGestureSegment
    {
        GestureState CheckGesture(Skeleton skeleton);
    }
}

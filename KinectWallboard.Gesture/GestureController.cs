using KinectWallboard.Gesture.Gestures;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWallboard.Gesture
{
    public class GestureController
    {
        private List<GestureBase> _gestures = new List<GestureBase>();

        public event EventHandler<GestureRecognizedEvent> GestureRecognized;

        public GestureController()
        {

        }

        public void UpdateAllGestures(Skeleton skeleton)
        {
            foreach (var gesture in _gestures)
            {
                gesture.UpdateGesture(skeleton);
            }
        }

        public void AddGesture(GestureType type, IGestureSegment[] gestureSegments)
        {
            var gesture = new GestureBase(type, gestureSegments);
            gesture.GestureRecognized += GestureOnGestureRecognized;
            _gestures.Add(gesture);
        }

        public void AddGesture(GestureBase gesture)
        {
            _gestures.Add(gesture);
        }

        private void GestureOnGestureRecognized(object sender, GestureRecognizedEvent e)
        {
            if (GestureRecognized != null)
                GestureRecognized(sender, e);
        }
    }
}

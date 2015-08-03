using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using KinectWallboard.Gesture.Gestures;

namespace KinectWallboard.Gesture
{
    public class GestureBase
    {
        private IGestureSegment[] _gestureSegments;
        private GestureType _type;

        public GestureBase(GestureType type, IGestureSegment[] gestureSegments)
        {
            _type = type;
            _gestureSegments = gestureSegments;
        }

        private bool _paused = false;
        private int _currentSegment = 0;
        private int _frameCount = 0;
        private int _pausedFrameCount = 10;

        public event EventHandler<GestureRecognizedEvent> GestureRecognized;
        
        public void UpdateGesture(Skeleton skeleton)
        {
            if (_paused)
            {
                if (_frameCount == _pausedFrameCount)
                {
                    _paused = false;
                }
                _frameCount++;
            }

            var result = _gestureSegments[_currentSegment].CheckGesture(skeleton);

            if (result == GestureState.Success)
            {
                if (_currentSegment + 1 < this._gestureSegments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                    _pausedFrameCount = 10;
                    _paused = true;
                }
                else
                {
                    if (GestureRecognized != null)
                        GestureRecognized(this, new GestureRecognizedEvent(_type));

                    Reset();
                }
            }
            else if (result == GestureState.Fail || _frameCount == 50)
            {
                Reset();
            }
            else
            {
                _frameCount++;
                _pausedFrameCount = 5;
                _paused = true;
            }
        }

        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
            _pausedFrameCount = 5;
            _paused = true;
        }
    }
}

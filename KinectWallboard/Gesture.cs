using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectWallboard
{
    public class GestureController
    {
        private List<Gesture> _gestures = new List<Gesture>();

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
            var gesture = new Gesture(type, gestureSegments);
            gesture.GestureRecognized += GestureOnGestureRecognized;
            _gestures.Add(gesture);
        }

        private void GestureOnGestureRecognized(object sender, GestureRecognizedEvent e)
        {
            if (GestureRecognized != null)
                GestureRecognized(sender, e);
        }
    }

    public class GestureRecognizedEvent : EventArgs
    {
        public GestureType Type { get; set; }

        public GestureRecognizedEvent(GestureType type)
        {
            Type = type;
        }
    }

    public class Gesture
    {
        private IGestureSegment[] _gestureSegments;
        private GestureType _type;

        public Gesture(GestureType type, IGestureSegment[] gestureSegments)
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

    public class SwipeRightStartGesture : IGestureSegment
    {
        public GestureState CheckGesture(Skeleton skeleton)
        {
            // hand above elbow
            if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ElbowRight].Position.Y)
            {
                // hand left of elbow
                if (skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.ElbowRight].Position.X)
                {
                    return GestureState.Success;
                }

                // hand not dropped but not left enough
                return GestureState.Paused;
            }

            // hand dropped, failed
            return GestureState.Fail;
        }
    }

    public class SwipeRightEndGesture : IGestureSegment
    {
        public GestureState CheckGesture(Skeleton skeleton)
        {
            // hand above elbow
            if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ElbowRight].Position.Y)
            {
                // hand right of elbow
                if (skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ElbowRight].Position.X)
                {
                    return GestureState.Success;
                }

                // hand not dropped but not right enough
                return GestureState.Paused;
            }

            // hand dropped, failed
            return GestureState.Fail;
        }
    }

    public class SwipeLeftStartGesture : IGestureSegment
    {
        public GestureState CheckGesture(Skeleton skeleton)
        {
            // hand above elbow
            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ElbowLeft].Position.Y)
            {
                // hand left of elbow
                if (skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.ElbowLeft].Position.X)
                {
                    return GestureState.Success;
                }

                // hand not dropped but not left enough
                return GestureState.Paused;
            }

            // hand dropped, failed
            return GestureState.Fail;
        }
    }

    public class SwipeLeftEndGesture : IGestureSegment
    {
        public GestureState CheckGesture(Skeleton skeleton)
        {
            // hand above elbow
            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ElbowLeft].Position.Y)
            {
                // hand right of elbow
                if (skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ElbowLeft].Position.X)
                {
                    return GestureState.Success;
                }

                // hand not dropped but not right enough
                return GestureState.Paused;
            }

            // hand dropped, failed
            return GestureState.Fail;
        }
    }

    public interface IGestureSegment
    {
        GestureState CheckGesture(Skeleton skeleton);
    }

    public enum GestureState
    {
        Success,
        Paused,
        Fail
    }

    public enum GestureType
    {
        SwipeLeft,
        SwipeRight
    }
}

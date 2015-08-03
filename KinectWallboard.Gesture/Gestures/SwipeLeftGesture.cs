using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWallboard.Gesture.Gestures
{
    public class SwipeLeftGesture : GestureBase
    {
        public SwipeLeftGesture()
            : base(GestureType.SwipeLeft, new IGestureSegment[2]
                    {
                        new SwipeLeftStartGesture(), 
                        new SwipeLeftEndGesture()
                    })
        {
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
}

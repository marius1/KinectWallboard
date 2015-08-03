using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWallboard.Gesture.Gestures
{
    public class SwipeRightGesture : GestureBase
    {
        public SwipeRightGesture()
            : base(GestureType.SwipeRight, new IGestureSegment[2]
                    {
                        new SwipeRightStartGesture(), 
                        new SwipeRightEndGesture()
                    })
        {
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
}

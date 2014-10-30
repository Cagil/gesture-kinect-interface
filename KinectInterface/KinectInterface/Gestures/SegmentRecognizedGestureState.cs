using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class SegmentRecognizedGestureState : GestureState
    {
        private Gesture gesture;

        public SegmentRecognizedGestureState(Gesture g)
        {
            this.gesture = g;
        }

        public bool Update(Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> bodyJoints, float delta)
        {
            return false;
        }

        public void changeState(GestureState newState)
        {
            this.gesture.State = newState;
        }
    }
}

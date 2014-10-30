using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class TimedOutGestureState : GestureState
    {
        private Gesture gesture;

        public TimedOutGestureState(Gesture g)
        {
            this.gesture = g;
        }

        public bool Update(Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> bodyJoints, float delta)
        {
            this.gesture.Reset();
            this.changeState(new IdleGestureState(this.gesture));
            return true;
        }

        public void changeState(GestureState newState)
        {
            this.gesture.State = newState;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class PausedGestureState : GestureState
    {
        public bool Update(Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> bodyJoints, float delta)
        {
            throw new NotImplementedException();
        }

        public void changeState(GestureState newState)
        {
            throw new NotImplementedException();
        }
    }
}

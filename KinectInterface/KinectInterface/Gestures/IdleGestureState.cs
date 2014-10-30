using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class IdleGestureState : GestureState
    {
        private Gesture gesture;

        public IdleGestureState(Gesture g)
        {
            this.gesture = g;
        }

        public bool Update(Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> bodyJoints, float delta)
        {
            
  


            GestureSegment segment = this.gesture.Segments.ElementAt(this.gesture.CurrentSegmentIndex);

            Boolean result = segment.Update(bodyJoints, null);

            if (result)
            {
                if (this.gesture.HasNextSegment)
                {
                    this.gesture.MoveToNextSegment();
                    return true;
                }
                else
                {
                    // success
                }
            }
            else
            {
                // segmentFailedState
            }

            return false;
        }


        public void changeState(GestureState newState)
        {
            this.gesture.State = newState;
        }
    }
}

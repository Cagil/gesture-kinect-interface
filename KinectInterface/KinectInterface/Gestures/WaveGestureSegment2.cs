using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class WaveGestureSegment2 : GestureSegment
    {
        public WaveGestureSegment2() { }

        public bool Update(Dictionary<JointType, Joint> skeleton, Dictionary<JointType, Joint> prev = null)
        {
            Joint handRight;
            Joint elbowRight;
            skeleton.TryGetValue(JointType.HandRight, out handRight);
            skeleton.TryGetValue(JointType.ElbowRight, out elbowRight);

            if (handRight.Position.Y > elbowRight.Position.Y)
            {

                // Hand left of elbow
                if (handRight.Position.X > elbowRight.Position.X)
                {
                    return true;
                }
            }

            // Hand dropped
            return false;

        }
    }
}

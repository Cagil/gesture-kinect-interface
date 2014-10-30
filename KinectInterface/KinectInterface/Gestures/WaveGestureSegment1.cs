using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class WaveGestureSegment1 : GestureSegment
    {
        public WaveGestureSegment1()
        {

        }

        public Boolean Update(Dictionary<JointType, Joint> skeleton, Dictionary<JointType, Joint> prev = null)
        {
            // Hand above elbow
            Joint handRight;
            Joint elbowRight;
            skeleton.TryGetValue(JointType.HandRight, out handRight);
            skeleton.TryGetValue(JointType.ElbowRight, out elbowRight);

            // Hand above elbow
            if (handRight.Position.Y > elbowRight.Position.Y)
            {
                Console.WriteLine("HAND IS ABOVE ELBOW 1.1");
                // Hand right of elbow
                if (handRight.Position.X < elbowRight.Position.X)
                {
                    Console.WriteLine("HAND IS RIGHT OF ELBOW 1.2");
                    return true;
                }
            }

            // Hand dropped
            return false;
        }

    }
}

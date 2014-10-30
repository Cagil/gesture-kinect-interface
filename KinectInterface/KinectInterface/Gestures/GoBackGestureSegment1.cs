using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    class GoBackGestureSegment1 : GestureSegment
    {
        public GoBackGestureSegment1() : base() { }

        public bool Update(Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> skeleton, Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> prev = null)
        {
            // Hand above elbow
            Joint handRight;
            Joint elbowRight;
            Joint shoulderRight;
            Joint shoulderCenter;
            Joint head;
            Joint hipRight;

            skeleton.TryGetValue(JointType.ShoulderCenter, out shoulderCenter);
            skeleton.TryGetValue(JointType.HipRight, out hipRight);
            skeleton.TryGetValue(JointType.HandRight, out handRight);
            skeleton.TryGetValue(JointType.ElbowRight, out elbowRight);
            skeleton.TryGetValue(JointType.ShoulderRight, out shoulderRight);
            skeleton.TryGetValue(JointType.Head, out head);

            String errString = "";
            bool err = false;
            //if (handRight.Position.X < head.Position.X ||
            //    handRight.Position.Y < elbowRight.Position.Y ||
            //    handRight.Position.Y > shoulderRight.Position.Y ||
            //    handRight.Position.X > shoulderRight.Position.X * 1.25f)
            //{
            //     Console.WriteLine("PRE-REQUISITES DID NOT MEET - GO BACK GESTURE");
            //    return false;
            //}

            if (handRight.Position.X < head.Position.X)
            {
                errString += "RIGHR HAND IS AT LEFT OF HEAD\n";
                err = true;
            }

            float handShoulderDx = handRight.Position.X - shoulderRight.Position.X;
            float rightCenterShoulderDx = shoulderRight.Position.X - shoulderCenter.Position.X;

            //if (handRight.Position.X > shoulderRight.Position.X )
            if (handShoulderDx > rightCenterShoulderDx)
            {
                errString += "RIGHT HAND IS TOO FAR RIGHT OF THE RIGHT SHOULDER\n";
                err = true;
            }

            if (handRight.Position.Y > head.Position.Y)
            {
                errString += "RIGHT HAND IS ABOVE HEAD\n";
                err = true;
            }

            if (handRight.Position.Y < hipRight.Position.Y)
            {
                errString += "RIGHT HAND IS BELOW RIGHT HIP\n";
                err = true;
            }

         //   Console.WriteLine(errString);
            if (err == true) return false;

            if (handRight.Position.X > head.Position.X)
            {

                //if (handRight.Position.Y > elbowRight.Position.Y)
               // {
                    if (handRight.Position.Y < shoulderRight.Position.Y)
                    {

                        return true;
                    }
                    else
                    {
                        return false;
                    }

              //  }
              //  else
              //  {
              //      return false;
              //  }
            }
            else
            {
                return false;
            }
        }
    }
}

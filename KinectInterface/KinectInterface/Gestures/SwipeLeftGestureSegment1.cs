using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class SwipeLeftGestureSegment1 : GestureSegment
    {
        public SwipeLeftGestureSegment1() : base() { }

        public bool Update(Dictionary<JointType, Joint> skeleton, Dictionary<JointType, Joint> prev = null)
        {
            // Hand above elbow
            Joint handRight;
            Joint elbowRight;
            Joint shoulderRight;
            Joint shoulderCenter;
            Joint hipCenter;
            Joint head;
            Joint hipRight;
            skeleton.TryGetValue(JointType.HipRight, out hipRight);
            skeleton.TryGetValue(JointType.Head, out head);
            skeleton.TryGetValue(JointType.HipCenter, out hipCenter);
            skeleton.TryGetValue(JointType.HandRight, out handRight);
            skeleton.TryGetValue(JointType.ElbowRight, out elbowRight);
            skeleton.TryGetValue(JointType.ShoulderRight, out shoulderRight);
            skeleton.TryGetValue(JointType.ShoulderCenter, out shoulderCenter);

            // cancel if
            // right hand is above head
            // right hand is below right elbow
            // right hand is below hip
            // right hand is above shoulder

            if (//handRight.Position.Y > head.Position.Y ||
               // handRight.Position.Y < elbowRight.Position.Y ||
                handRight.Position.Y < hipRight.Position.Y ||
                handRight.Position.Y > shoulderRight.Position.Y)
            {
                //Console.WriteLine("PRE-REQUISTES TO START GESTURE DID NOT MEET!!!");
                return false;
            }

            //if (handRight.Position.Y > shoulderRight.Position.Y)
            //{
            //    Console.Write("RETURNING FALSE == HAND IS ABOVE SHOULDER");
            //    return false;

            //}
            // Hand infront of elbow
            if (true || handRight.Position.Z < elbowRight.Position.Z)
            {
                //  Console.WriteLine("HAND IS INFRONT OF ELBOW 1.1");
                // Hand right of shoulder
                if (handRight.Position.X > shoulderRight.Position.X)
                {
                    //      Console.WriteLine("HAND IS RIGHT OF ELBOW 1.2");
                    float handShoulderDx = handRight.Position.X - shoulderRight.Position.X;
                    float shoulderTorsoDx = shoulderRight.Position.X - shoulderCenter.Position.X;
                  //  Console.WriteLine("HAND - SHOULDER DISTANCE === " + handShoulderDx);
                 //   Console.WriteLine("SHOULDER - TORSO DISTANCE === " + shoulderTorsoDx);
                    if (handShoulderDx < shoulderTorsoDx)
                    {
                        return true;
                    }
                    else
                    {
                      //  Console.WriteLine("DISTANCE IS NOT ENOUGH");
                        return false;
                    }
                }
                else
                {

                    return false;
                }
            }
            else
            {
             //   Console.WriteLine("RETURNING FALSE === HAND IS BEHIND ELBoW");
                // Hand dropped
                return false;
            }
        }
    }
}

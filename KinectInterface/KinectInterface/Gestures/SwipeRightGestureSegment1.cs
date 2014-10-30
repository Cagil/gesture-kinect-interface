using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class SwipeRightGestureSegment1 : GestureSegment
    {
        public SwipeRightGestureSegment1() : base() { }

        public bool Update(Dictionary<JointType, Joint> skeleton, Dictionary<JointType, Joint> prev = null)
        {
            // Hand above elbow
            Joint handLeft;
            Joint elbowLeft;
            Joint shoulderLeft;
            Joint shoulderCenter;
            Joint hipCenter;
            Joint head;
            Joint hipLeft;
            skeleton.TryGetValue(JointType.HipLeft, out hipLeft);
            skeleton.TryGetValue(JointType.Head, out head);
            skeleton.TryGetValue(JointType.HipCenter, out hipCenter);
            skeleton.TryGetValue(JointType.HandLeft, out handLeft);
            skeleton.TryGetValue(JointType.ElbowLeft, out elbowLeft);
            skeleton.TryGetValue(JointType.ShoulderLeft, out shoulderLeft);
            skeleton.TryGetValue(JointType.ShoulderCenter, out shoulderCenter);

            // cancel if
            // right hand is above head
            // right hand is below right elbow
            // right hand is below hip
            // right hand is above shoulder

            if (//handRight.Position.Y > head.Position.Y ||
                // handRight.Position.Y < elbowRight.Position.Y ||
                handLeft.Position.Y < hipLeft.Position.Y ||
                handLeft.Position.Y > shoulderLeft.Position.Y)
            {
              //  Console.WriteLine("PRE-REQUISTES TO START GESTURE DID NOT MEET!!!");
                return false;
            }

            //if (handRight.Position.Y > shoulderRight.Position.Y)
            //{
            //    Console.Write("RETURNING FALSE == HAND IS ABOVE SHOULDER");
            //    return false;

            //}
            // Hand infront of elbow
            if (true || handLeft.Position.Z < elbowLeft.Position.Z)
            {
                //  Console.WriteLine("HAND IS INFRONT OF ELBOW 1.1");
                // Hand right of shoulder
                if (handLeft.Position.X < shoulderLeft.Position.X)
                {
                    //      Console.WriteLine("HAND IS RIGHT OF ELBOW 1.2");
                    float handShoulderDx = handLeft.Position.X - shoulderLeft.Position.X;
                    float shoulderTorsoDx = shoulderLeft.Position.X - shoulderCenter.Position.X;
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

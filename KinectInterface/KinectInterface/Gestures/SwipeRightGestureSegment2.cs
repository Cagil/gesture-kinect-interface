using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class SwipeRightGestureSegment2 : GestureSegment
    {

        public bool Update(Dictionary<JointType, Joint> skeleton, Dictionary<JointType, Joint> prev = null)
        {

            // Hand above elbow
            Joint handLeft;
            Joint elbowLeft;
            Joint hipCenter;
            Joint shoulderCenter;
            Joint shoulderLeft;
            skeleton.TryGetValue(JointType.ShoulderLeft, out shoulderLeft);
            skeleton.TryGetValue(JointType.ShoulderCenter, out shoulderCenter);
            skeleton.TryGetValue(JointType.HandLeft, out handLeft);
            skeleton.TryGetValue(JointType.ElbowLeft, out elbowLeft);
            skeleton.TryGetValue(JointType.HipCenter, out hipCenter);

            //if (prev == null) return false;

            // Hand infront of elbow
            if (true || handLeft.Position.Z < elbowLeft.Position.Z)
            {

                // Hand left of elbow
                if (handLeft.Position.X > shoulderLeft.Position.X)
                {
                    ////Console.WriteLine("HAND IS RIGHT OF ELBOW 2.2 X == " + handRight.Position.X);
                    //if (prev != null)
                    //{
                    //    Joint prevhandRight;

                    //    prev.TryGetValue(JointType.HandRight, out prevhandRight);
                    //    Console.WriteLine("PREV n CURR RIGHT HAND DIFFERENCE ::: " + handRight.Position.X + "  "  + prevhandRight.Position.X);
                    //    float dist = Math.Abs(handRight.Position.X - prevhandRight.Position.X);
                    //    if (dist >= 0.40f)
                    //    {
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("!!!!!PREV DATA FOUND BUT NOT GOOD ENOUGH");
                    //        return false;
                    //    }

                    //}
                    //else
                    //{
                    //    Console.WriteLine("NO PREVIOUS DATA");
                    //    return false;
                    //}

                    //return true;
                    //Joint centerHip;
                    //skeleton.TryGetValue(JointType.HipCenter, out centerHip);
                    float handShoulderRightDx = shoulderLeft.Position.X - handLeft.Position.X;
                    float shoulderRightCenterDx = shoulderLeft.Position.X - shoulderCenter.Position.X;
                   // Console.WriteLine("Hand - Shoulder Right Distance == " + handShoulderRightDx);
                  //  Console.WriteLine("Shoulder RIght vs Center Distance == " + shoulderRightCenterDx);
                    if (handShoulderRightDx > shoulderRightCenterDx)
                    {
                        return true;
                    }

          //          Console.WriteLine("ERROR 2.2 DISTANCE ERROR!!!");
                    return false;
                }
                else
                {
            //        Console.WriteLine("ERROR HAND - SHoulder X VALUE 2.1");
                    // Console.WriteLine("RETURNING FALSE == HAND IS LEFT OF ELBOW");
                    return false;
                }
            }
            else
            {
          //      Console.WriteLine("ERROR HAND Z VALUE 2.1");
                //   Console.WriteLine("RETURNING FALSE === HAND IS BEHIND ELBOW");
                return false;
            }
        }
    }
}

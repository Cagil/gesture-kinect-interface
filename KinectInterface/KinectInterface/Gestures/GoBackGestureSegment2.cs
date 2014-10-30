using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    class GoBackGestureSegment2 : GestureSegment
    {
        public GoBackGestureSegment2() : base() { }

        public bool Update(Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> skeleton, Dictionary<Microsoft.Kinect.JointType, Microsoft.Kinect.Joint> prev = null)
        {
        //    Console.WriteLine("GO BACK GESTURE PART 2 INITIATED");
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

            if (handRight.Position.X < head.Position.X ||
                handRight.Position.Y < hipRight.Position.Y ||
                handRight.Position.Y < shoulderRight.Position.Y ||
                //handRight.Position.X > shoulderRight.Position.X ||
                handRight.Position.Z > head.Position.Z)
            {
                // Console.WriteLine("PRE-REQUISITES DID NOT MEET");
                return false;
            }



            if (handRight.Position.X > head.Position.X)
            {
                if (handRight.Position.Y > elbowRight.Position.Y)
                {
                    if (handRight.Position.Y > shoulderRight.Position.Y)
                    {
                        if (handRight.Position.Y > head.Position.Y)
                        {
                            //float headShoulderDx = head.Position.X - shoulderRight.Position.X;
                            //float headElbowDx = head.Position.X - elbowRight.Position.X;
                            //if (headElbowDx > headShoulderDx * 1.50f)
                            //{
                            //    return false;
                            //}
                           // Console.WriteLine("ELBOW RIGHT X :: " + elbowRight.Position.X);
                          //  Console.WriteLine("SHOULDER RIGHT X :: " + shoulderRight.Position.X);
                            //if (elbowRight.Position.X > shoulderRight.Position.X * 1.25f) return false;
                          //  Console.WriteLine("HAND Z :: " + handRight.Position.Z + "\t HEAD Z :: " + head.Position.Z);
                           // if(handRight.Position.Z > head.Position.Z)
                                return true;
                          //  return false;
                        }
                        else
                        {
                            return false;
                        }
                        //float shoulderHandDx = handRight.Position.X - shoulderRight.Position.X;
                        //float shoulderHeadDx = shoulderRight.Position.X - head.Position.X;
                        //if (shoulderHandDx < shoulderHeadDx)
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    return false;
                        //}
                    }
                    else
                    {
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
                return false;
            }

        }
    }
}

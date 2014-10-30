using KinectInterface.Gestures;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface
{
    public interface GestureRecognizer
    {
        //void updateInterestedGestureList(List<String> gestureNames);
        void updateGestureList(List<Gesture> gestures);
        bool lookForGestures(Dictionary<JointType, Joint> bodyJoints, float dt);
    }
}

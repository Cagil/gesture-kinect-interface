using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public interface GestureState
    {
        


        
        Boolean Update(Dictionary<JointType, Joint> bodyJoints, float delta);

        void changeState(GestureState newState);


    }

    
}

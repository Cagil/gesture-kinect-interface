using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public interface GestureSegment
    {
        Boolean Update(Dictionary<JointType, Joint> skeleton, Dictionary<JointType, Joint> prev = null);
    }
}

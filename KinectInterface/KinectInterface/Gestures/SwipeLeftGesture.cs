using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class SwipeLeftGesture : Gesture
    {
        public SwipeLeftGesture() : base(-1)
        {
            this.Name = "SWIPE LEFT GESTURE";

            SwipeLeftGestureSegment1 gSegment1 = new SwipeLeftGestureSegment1();
            SwipeLeftGestureSegment2 gSegment2 = new SwipeLeftGestureSegment2();

            List<GestureSegment> segs = new List<GestureSegment>();
            segs.Add(gSegment1);
            segs.Add(gSegment2);
            

            this.GestureSegments = segs;
            
        }
    }
}

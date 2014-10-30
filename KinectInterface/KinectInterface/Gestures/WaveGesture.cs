using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class WaveGesture : Gesture
    {
        public WaveGesture() : base(-1)
        {
            this.Name = "WAVE GESTURE";

            WaveGestureSegment1 wgs1 = new WaveGestureSegment1();
            WaveGestureSegment2 wgs2 = new WaveGestureSegment2();

            List<GestureSegment> segs = new List<GestureSegment>();
            segs.Add(wgs1);
            segs.Add(wgs2);
            segs.Add(wgs1);
            segs.Add(wgs2);
            segs.Add(wgs1);
            segs.Add(wgs2);

            this.GestureSegments = segs;
            
        }

    }
}

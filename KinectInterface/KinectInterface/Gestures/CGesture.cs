using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class CGesture : Gesture
    {
        public CGesture(int id, String name, List<GestureSegment> segments) : base(id)
        {
            Console.WriteLine("CGESTURE CONSTRUCTOR BEGIN NAME == " + name);
            this.Name = name;

            if (segments != null)
            {

                for (int i = 0; i < segments.Count; i++)
                {
                    this.GestureSegments.Add(segments.ElementAt(i));
                }
            }
           // if (true) Console.WriteLine("GESTURE SEGMENT COUNT  " + segments.Count);
        }
    }
}

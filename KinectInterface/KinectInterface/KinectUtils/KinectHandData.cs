using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.KinectUtils
{
    public class KinectHandData
    {
        private float time;
        private Point location;

        public Point Position { get { return this.location; } }
        public float Time { get { return this.time; } }

        public KinectHandData(Point p, float t)
        {
            this.location = p;
            this.time = t;
        }



    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public abstract class BoundingShape
    {
        //private Vector2 center;
        private Vector2 center;

        //public BoundingShape() { center = new Vector2(); }
        //public BoundingShape(int x, int y) { center = new Vector2(x, y); }
       
        public Vector2 Center { get { return center; } set { this.center = value; } }
        public abstract Rectangle XNARectangle{get;}

        public abstract bool Intersect(ref BoundingCircle circle);
        public abstract bool Intersect(Vector2 point);
        public abstract bool Intersect(Point point);
        public abstract bool Intersect(ref BoundingRectangle rect);
       

    }
}

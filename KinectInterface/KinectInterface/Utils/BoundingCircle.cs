using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public class BoundingCircle : BoundingShape
    {
        private int radius;

        public int Radius { get { return this.radius; } set { this.radius = value; } }
        //public Vector2 Center { get { return this.center; } }

        public override Rectangle XNARectangle { get { return new Rectangle((int)this.Center.X, (int)this.Center.Y, this.radius, this.radius); } }

        public BoundingCircle()
        {
            this.radius = 0;
            this.Center = new Vector2(0, 0);
        }


        public BoundingCircle(int x, int y, int r)
        {
            this.radius = r;
            this.Center = new Vector2(x, y);
        }

        public override bool Intersect(ref BoundingCircle circle)
        {


        
            int dx = (int)(circle.Center.X - this.Center.X);
            int dy = (int)(circle.Center.Y - this.Center.Y);
            int radii = this.radius + circle.radius;
            if ((dx * dx) + (dy * dy) < radii * radii)
            {
                return true;
            }

            return false;
        }

        public override bool Intersect(ref BoundingRectangle rect)
        {
            return false;
        }


        public override bool Intersect(Vector2 point) { return false; }
        public override bool Intersect(Point point) { return false; }

       
    }
}

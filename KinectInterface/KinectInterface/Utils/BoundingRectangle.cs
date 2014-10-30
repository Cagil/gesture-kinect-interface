using KinectInterface.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public class BoundingRectangle : BoundingShape
    {
        private int width, height;
        private Point pos;

        public int Width { get { return this.width; } set { this.width = value; } }
        public int Height { get { return this.height; } set { this.height = value; } }
        public Point Position { get { return this.pos; } set { this.pos = value; } }
        public Point Dimension { get { return new Point(this.width, this.height); } set { this.width = value.X; this.height = value.Y; } }
        //public Vector2 Center { get { return this.center; } }
        //public Rectangle BoundingRectangle { get { return this.rect; } set { this.rect = value; } }

        public BoundingRectangle()
        {
            this.pos.X = 0;
            this.pos.Y = 0;
            this.width = 0;
            this.height = 0;

            this.Center = new Vector2(0, 0);
        }

        public BoundingRectangle(Rectangle r) 
        {
            this.pos.X = r.X;
            this.pos.Y = r.Y;
            this.width = r.Width;
            this.height = r.Height;

            this.Center = new Vector2(r.Center.X, r.Center.Y);
        }

        public BoundingRectangle(int x, int y, int width, int height) 
        {
            this.pos.X = x;
            this.pos.Y = y;
            this.width = width;
            this.height = height;

            this.Center = new Vector2((x + width) / 2, (y + height) / 2);
            
        }

        public static BoundingRectangle CreateFromGroup(List<AbstractUI> elements)
        {
            List<BoundingRectangle> rects = new List<BoundingRectangle>();
            foreach(AbstractUI ui in elements)
                rects.Add(ui.BoundingRectangle);
            
            return CreateFromGroup(rects);
        }

        public static BoundingRectangle CreateFromGroup(List<BoundingRectangle> rects)
        {
            BoundingRectangle bigbox = null;

            int x = 0, y = 0, w = 0, h = 0;

            for (int i = 0; i < rects.Count; i++)
            {
                if (i == 0)
                {
                    x = rects.ElementAt(i).Position.X;
                    y = rects.ElementAt(i).Position.Y;
                }

                if (x > rects.ElementAt(i).Position.X)
                {
                    x = rects.ElementAt(i).Position.Y;
                }

                if (y > rects.ElementAt(i).Position.Y)
                {
                    y = rects.ElementAt(i).Position.Y;
                }

                if (w < rects.ElementAt(i).Position.X + rects.ElementAt(i).Dimension.X)
                {
                    w = rects.ElementAt(i).Position.X + rects.ElementAt(i).Dimension.X;
                }

                if (h < rects.ElementAt(i).Position.Y + rects.ElementAt(i).Dimension.Y)
                {
                    h = rects.ElementAt(i).Position.Y + rects.ElementAt(i).Dimension.Y;
                }
            }

            x = (int)(x * 0.75);
            y = (int)(y * 0.75);
            w = (int)(w * 1.25);
            h = (int)(h * 1.25);

            bigbox = new BoundingRectangle(x, y, w, h);

            return bigbox;
        }

        public override Rectangle XNARectangle {get{ return new Rectangle(this.pos.X, this.pos.Y, this.width, this.height);}}

        public override bool Intersect(ref BoundingRectangle rect)
        {
            return false;
        }

        public override bool Intersect(ref BoundingCircle circle)
        {
            return false;

        }

        private bool checkRectvsPoint(int x, int y)
        {
            if (x > this.width + this.pos.X ||
               x < this.pos.X ||
                y > this.height + this.pos.Y ||
                y < this.pos.Y) return false;

            return true;
        }

        public override bool Intersect(Point vec)
        {
            return checkRectvsPoint(vec.X, vec.Y);
        }

        public override bool Intersect(Vector2 vec)
        {
           return checkRectvsPoint((int)vec.X, (int)vec.Y);
        }


        
    }
}

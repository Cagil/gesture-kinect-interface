using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Layouts
{
    public class LayoutParams
    {
        private Point inbetweenEleSpace;
      //  private Point padding;
        private Vector2 marginLT;
        private Vector2 marginRB;
        private Vector2 eleRatio;

        public Vector2 ElementRatio { get { return this.eleRatio; } }
      //  public Point Padding { get { return this.padding; } }
        public Vector2 MarginLeftTop { get { return this.marginLT; } }
        public Vector2 MarginRightBottom { get { return this.marginRB; } }
        public Point InBetweenElementSpace { get { return this.inbetweenEleSpace; } set { this.inbetweenEleSpace = value; } }

        public LayoutParams(Vector2 eleRatio)
        {
            this.inbetweenEleSpace = new Point(0,0);
            this.eleRatio = eleRatio;
          //  this.padding = padding;
            this.marginLT = new Vector2(0, 0);
            this.marginRB = new Vector2(0, 0);
        }

        public void setMargin(float left, float top, float right, float bottom){
            left = Math.Max(0.0f, left);
            left = Math.Min(1.0f, left);

            top = Math.Max(0.0f, top);
            top = Math.Min(1.0f, top);

            right = Math.Max(0.0f, right);
            right = Math.Min(1.0f, right);

            bottom = Math.Max(0.0f, bottom);
            bottom = Math.Min(1.0f, bottom);

            this.marginLT = new Vector2(left, top);
            this.marginRB = new Vector2(right, bottom);
        }


    }
}

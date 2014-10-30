using KinectInterface.UI;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Messages
{
    public class InputTouchMessage :Message<AbstractUI>
    {
        public InputTouchMessage(int size)
            : base(size)
        {

        }

        public override void open(AbstractUI receipent)
        {
            //Console.WriteLine("reading the message");
            Vector2 coords = new Vector2(float.Parse(this.Data[0]), float.Parse(this.Data[1]));
            receipent.onTouch(coords);
        }
    }
}

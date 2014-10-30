using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinectInterface.Utils;
using KinectInterface.UI;
using Microsoft.Xna.Framework;

namespace KinectInterface.Messages
{
    public class InputMoveMessage : Message<AbstractUI>
    {
        private InputManager sender;
        public InputMoveMessage(int size, InputManager im) :base(size)
        {
            this.sender = im;
        }

        public override void open(AbstractUI receipent)
        {
           
            Vector2 coords = new Vector2(float.Parse(this.Data[0]), float.Parse(this.Data[1]));
            Boolean r = receipent.onFocus(coords);
            //if (receipent is UIButton)
            //{
            //    Console.WriteLine("ON FOCUS RESULT == " + r);
            //}
            //this.sender.IsOnElement = r;
            this.sender.CanResetPushTimer = !r;
            
        }
    }
}

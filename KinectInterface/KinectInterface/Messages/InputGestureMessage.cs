using KinectInterface.UI;
using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Messages
{
    public class InputGestureMessage : Message<AbstractUI>
    {

        public InputGestureMessage(int size)
                : base(size)
            {

            }

            public override void open(AbstractUI receipent)
            {

                
                receipent.onGestureNoticed(this.Data[0]);
            }
       
    }
}

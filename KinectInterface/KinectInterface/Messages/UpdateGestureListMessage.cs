using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Messages
{
    class UpdateGestureListMessage : Message<InputManager>
    {
        private List<String> gestureNames;

        public UpdateGestureListMessage(List<String> names)
            : base(0)
        {
            this.gestureNames = names;
        }

        public override void open(InputManager receipent)
        {
            //receipent.
        }
    }
}

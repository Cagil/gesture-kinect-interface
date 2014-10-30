using KinectInterface.UI;
using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Messages
{
    class InitializeMessage : Message<AbstractUI>
    {
        public InitializeMessage() : base(0) { }

        public override void open(AbstractUI receipent)
        {
            receipent.Initialize();
        }
    }
}

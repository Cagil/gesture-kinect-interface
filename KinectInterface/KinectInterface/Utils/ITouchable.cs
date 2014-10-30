using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    interface ITouchable
    {
        void setOnTouchCommand(ref Command command);
        void onTouch();
    }
}

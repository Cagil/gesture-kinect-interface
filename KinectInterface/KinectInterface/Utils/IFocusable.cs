using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public interface IFocusable
    {
        void setOnFocusCommand(ref Command command);
        void onFocus();
    }
}

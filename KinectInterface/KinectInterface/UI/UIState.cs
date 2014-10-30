using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    public class UIState
    {
        public enum InteractionState { IDLE, FOCUSED, TOUCHED };
        public enum VisibilityState { VISIBLE, HIDDEN, FLEEING };
    }
}

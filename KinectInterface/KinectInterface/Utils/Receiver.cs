using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public interface Receiver<S>
    {
        void Receive(Message<S> message);
    }
}

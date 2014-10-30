using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KinectInterface.Utils
{
    public interface Sender<S>
    {
        

        void addReceiver(Receiver<S> newReceiver);
        void removeReceiver(Receiver<S> receiver);
        void broadcast(Message<S> message);
    }
}

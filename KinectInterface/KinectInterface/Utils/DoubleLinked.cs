using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public interface DoubleLinked<T>
    {

        bool hasNext();
        bool hasPrev();
        T getNext();
        T getPrev();
        void setNext(T obj);
        void setPrev(T obj);

    }
}

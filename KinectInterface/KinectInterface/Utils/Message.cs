using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public abstract class Message<S> 
    {
        private String[] data;
        private int index = 0;
        private int size = 0;

        public Message(int size)
        {
            this.size = size;
            this.data = new String[this.size];
        }

        public int Size { get { return this.size; } }
        public String[] Data { get { return this.data; } }

        public void addData(String newdata)
        {
            if (this.index < this.size)
            {
                this.data[this.index] = newdata;
                this.index++;
            }
        }

        public abstract void open(S receipent);
        
    }
}

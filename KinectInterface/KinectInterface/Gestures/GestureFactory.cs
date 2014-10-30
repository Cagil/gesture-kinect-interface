using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public class GestureFactory
    {
        private IMediator hub;
        private Dictionary<String, Gesture> gestureCollection;
        private int idc;
        public GestureFactory() { 
            this.idc = 0;
            this.gestureCollection = new Dictionary<String, Gesture>();
            this.tmpSegments = new List<GestureSegment>();
            this.hub = null;
        }

        private List<GestureSegment> tmpSegments;

        public IMediator Hub { set { this.hub = value; } }

        public void addNewSegment(GestureSegment gs)
        {
            this.tmpSegments.Add(gs);
        }

        public void addSegmentList(List<GestureSegment> gsl)
        {
            this.tmpSegments = gsl;
        }

        public Gesture MakeGesture(String name, float timeoutlimit, Command command = null)
        {
            Gesture g = null;
            if (name != null)
                this.gestureCollection.TryGetValue(name, out g);
            else return null;

            if (g != null) return g;

            idc++;
            g = new CGesture(idc, name, this.tmpSegments);
            g.TimeOutLimit = timeoutlimit;
            //for (int i = 0; i < this.tmpSegments.Count; i++)
            //{
            //    g.addGestureSegment(this.tmpSegments.ElementAt(i));
            //}
            g.SucceedCommand = command;
            this.gestureCollection.Add(name, g);
            this.tmpSegments.Clear();
         //   Gesture k;
        //    this.gestureCollection.TryGetValue(name, out k);
        //    Console.WriteLine("GESTURE SEGMENT COUNT " + k.GestureSegments.Count);
            return g;     
        }

        public List<Gesture> GetGestures(List<String> gestureNames)
        {
            List<Gesture> gestures = new List<Gesture>();

            for(int i = 0; i < gestureNames.Count; i++){
                Gesture g = null;
                this.gestureCollection.TryGetValue(gestureNames.ElementAt(i), out g);
                if(g != null){
                    gestures.Add(g);
                }
            }

            return gestures;

        }
    }
}

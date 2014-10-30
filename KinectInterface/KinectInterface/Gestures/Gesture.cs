using KinectInterface.Utils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Gestures
{
    public abstract class Gesture
    {
        private String name;
        private int id;
        
        private List<GestureSegment> segments;
        private GestureState state;

        private Boolean failed;
        private Boolean paused;
        private Command succeedCommand;
        private int segIndex;

        private float timeOutLimit;
        private float timeOutCounter;

        public List<GestureSegment> Segments { get { return this.segments; } }
        public float TimeOutCouter { get { return this.timeOutCounter; } set { this.timeOutCounter = value; } }
        public int CurrentSegmentIndex { get { return this.segIndex; } set { this.segIndex = value; } }
        public Boolean IsFailed { get { return this.failed; } }
        public Boolean IsPaused { get { return this.paused; } }
        public Command SucceedCommand { get { return this.succeedCommand; } set{ this.succeedCommand = value; } }
        public GestureState State { get { return this.state; } set { this.state = value; } }
        public String Name { get { return this.name; } set { this.name = value; } }
        public int ID { get { return this.id; } }
        public float TimeOutLimit { get { return this.timeOutLimit; } set { this.timeOutLimit = value; } }

        public List<GestureSegment> GestureSegments { set { this.segments = value; } get { return this.segments; } }

        private Dictionary<JointType, Joint> prevBodyJoints;

        public Gesture(int id)
        {
            this.failed = false;
            this.paused = false;
            
            this.segments = new List<GestureSegment>();
            this.segIndex = 0;
            this.timeOutCounter = 0.0f;
            this.timeOutLimit = 0.16f;
            this.name = "DEFAULT GESTURE NAME";
            //this.state = new 

            Console.WriteLine("GESTURE BASE CONSTRUCTOR  " + this.name);
            this.id = id;

            prevBodyJoints = null;
        }

        public void addGestureSegment(GestureSegment gs){
            this.segments.Add(gs);
        }

        //public Gesture(List<GestureSegment> gestureSegments)
        //{
        //    this.failed = false;
        //    this.paused = false;
        //    this.segments = gestureSegments;

        //    this.segIndex = 0;
        //    this.timeOutCounter = 0.0f;
        //    this.timeOutLimit = 1.0f;
        //    this.name = "DEFAULT GESTURE NAME";
        //    this.state = GestureState.NONE;
        //}
        

        public Boolean Update(Dictionary<JointType, Joint> bodyJoints, float delta){
            if (this.segments.Count <= 0) { Console.WriteLine("NO SEGMENTS FOUND RETURNING FALSE"); return false; }



            if (this.HasTimedOut)
            {
                this.Reset();

                this.failed = true;
                this.paused = false;
                Console.WriteLine("GESTURE TIMING OUT RETURNING FALSE");
                return false;
            }

            this.timeOutCounter += delta;

            //this.state.update(bodyJoints, delta);

            GestureSegment segment = this.segments.ElementAt(this.segIndex);

            Boolean result = segment.Update(bodyJoints, prevBodyJoints);

            if (result == true)
            {
             //   Console.WriteLine(this.Name + "  SEGMENT " + this.segIndex + "  TRUE ");
                if (this.segIndex + 1 < this.segments.Count)
                {

                    this.segIndex++;
                    this.prevBodyJoints = bodyJoints;
                    this.failed = false;
                    this.paused = true;
              //      Console.WriteLine("MOVING ONTO NEXT SEGMENT" + this.segIndex);
                    return true;
                }
                else
                {
      //              Console.WriteLine("RUNNING SUCCES COMMAND");
                    //if (this.succeedCommand != null)
                    //{
                    //    //this.succeedCommand.run();
                    //    this.Reset();
                    //    return true;
                    //}
                    this.Reset();
                    return true;
                }
            }
            else
            {
                if (this.segIndex == 0)
                {

                    this.Reset();
                    return false;
                }
                else
                {
                    this.paused = true;
                    return false;
                }
            }

            //this.prevBodyJoints = null;
            //this.failed = true;
            //this.paused = false;
            //return false;
        }


        public void Reset()
        {
            this.segIndex = 0;
            this.timeOutCounter = 0.0f;
            this.paused = false;
            this.failed = false;
            this.prevBodyJoints = null;
        }

        public void MoveToNextSegment()
        {
            this.segIndex++;
        }

        public Boolean HasNextSegment
        {
            get
            {
                return (this.segIndex + 1) < this.segments.Count;
            }
        }

        public Boolean HasTimedOut
        {
            get
            {
                return this.timeOutCounter >= this.timeOutLimit;
            }
        }

        public Boolean CanContinue
        {
            get
            {
                return this.paused;
                //if (this.paused == true)
                //{
                //    return true;
                //}
                //else
                //{
                //    if (this.failed == true)
                //    {
                //        return false;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}



            }
        }

        


    }
}

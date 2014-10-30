using KinectInterface.Gestures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public class Mediator : IMediator
    {
        private Boolean interestedGesturesUpdated;

        private GestureFactory gestureFactory;
        private InputManager inputManager;
        private SceneManager sceneManager;
        private KinectManager kinectManager;

        public Mediator() { 
            this.inputManager = null;
            this.sceneManager = null;
            this.kinectManager = null;
            this.gestureFactory = null;

            this.interestedGesturesUpdated = false;
        }

        public void sendInterestedGestureList(List<String> interestedGestures)
        {
            this.inputManager.updateGestureList(this.gestureFactory.GetGestures(interestedGestures));
        }

        public void sendNoticedGesture(String gestureName)
        {
            this.sceneManager.ReceiveRecognizedGesture(gestureName);
        }

        public void sendAutoPilotStartRequest()
        {
            this.sceneManager.AutoPilot = true;
        }

        public void sendAutoPilotStopRequest()
        {
            this.sceneManager.AutoPilot = false;
        }

        //public void resetPushTimer()
        //{
        //    this.inputManager.resetPushTimer();
        //}


        //public void sendInputOnMove(int x, int y)
        //{
        //    this.sceneManager.checkOnMove(x, y);
        //}

        //public void sendInputOnPush(int x, int y)
        //{
        //    this.sceneManager.checkOnPush(x, y);
        //}

        public Dictionary<Microsoft.Kinect.JointType,Microsoft.Kinect.Joint> getSkeleton()
        {
            return this.kinectManager.BodyJoints;
        }

        public Point get2DCursorPosition()
        {
            return this.kinectManager.PrimaryHandPos2D;
        }

        public bool IsPersonDetected()
        {
            return this.kinectManager.IsTracking;
        }

        public bool IsKinectReady()
        {
            if (this.kinectManager != null && this.kinectManager.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void registerInputManager(ref InputManager im)
        {
            this.inputManager = im;
            this.inputManager.Hub = this;
        }

        public void registerSceneManager(ref SceneManager sm)
        {
            this.sceneManager = sm;
            this.sceneManager.Hub = this;
        }

        public void registerKinectManager(ref KinectManager km)
        {
            this.kinectManager = km;
            this.kinectManager.Hub = this;
        }

        public void registerGesturePool(ref GestureFactory gf)
        {
            this.gestureFactory = gf;
            this.gestureFactory.Hub = this;
        }





    }
}

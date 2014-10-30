using KinectInterface.UI;
using KinectInterface.Utils;
using KinectInterface.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using KinectInterface.Gestures;
using Microsoft.Kinect;

namespace KinectInterface
{
    public class InputManager: GestureRecognizer, Sender<AbstractUI>
    {
        private IMediator hub;

        private List<Receiver<AbstractUI>> receipients;

        private KinectManager kinectManager;

        private Point lastPrimaryHandCoord;
        private Point currentPrimaryHandCoord;

        private Point lastMouseCoord;
        private Point currentMouseCoord;

        private Boolean leftMousePressed;

        private int pushTime;
        private int pushTimeLimit;
        private Boolean canResetPushTimer;

        private int noSkeletonTime;
        private int noSkeletonTimeLimit;
        private Boolean oldSkeletonTrackState;

       // public KinectManager KinectManager { set { this.KinectManager = value; } }
        private Point maxScreen;

        private Boolean onElement;
        //private Keys exitKey;
        //private Command exitKeyCommand;

        //private Keys fullscreenKey;
        //private Command fullscreenKeyCommand;

        //public Keys ExitKey { set { this.exitKey = value; } }
        //public Command ExitKeyCommand { set { this.exitKeyCommand = value; } }

        //public Keys FullscreenKey { set { this.fullscreenKey = value; } }
        //public Command FullscreenKeyCommand { set { this.fullscreenKeyCommand = value; } }

        private List<Gesture> gestures;
        private int gestureIndex;
        private float gestureWaitTimeMax;
        private float gestureTotalWaitTime;
        private Boolean gestureRecognitionPaused;

        private float zDelta;
        private float zTouchStart;
        private float zTouchEnd;
        private float zStartPercent;
        private float zEndPercent;
        private Point cursorHitOffset;


        private float initZ;
        private String recognizedGesture;

        private Dictionary<Keys, Command> keyCommandCollection;
        public IMediator Hub { set { this.hub = value; } }
        public int PushTime { get { return this.pushTime; } }
        public Point CursorHitPointOffset { get { return this.cursorHitOffset;} set{ this.cursorHitOffset = value;} }

        public InputManager(int w, int h)
        {

            //if (runWithKinect == false)
            //{
            //    kinectManager = null;
            //}
            //else
            //{
                
                
            //}

            receipients = new List<Receiver<AbstractUI>>();

            currentPrimaryHandCoord = new Point();
            lastPrimaryHandCoord = new Point();

            currentMouseCoord = new Point();
            lastMouseCoord = new Point();

            leftMousePressed = false;

            this.pushTime = 0;
            this.pushTimeLimit = 1000;

            this.noSkeletonTime = 0;
            this.noSkeletonTimeLimit = 3000;
            this.oldSkeletonTrackState = false;

            this.maxScreen = new Point(w, h);

            this.keyCommandCollection = new Dictionary<Keys, Command>();

            this.gestureIndex = 0;
            this.gestureTotalWaitTime = 0.0f;
            this.gestureWaitTimeMax = 0.75f;
            this.gestureRecognitionPaused = false;

            this.gestures = new List<Gesture>();
            this.recognizedGesture = null;
            initZ = 0.0f;
            this.zDelta = 0.0f;
            zTouchStart = 0.0f;
          zTouchEnd = 0.0f;
          zStartPercent = 0.95f;
          zEndPercent = 1.10f;
            //this.exitKey = Keys.X;
            //this.exitKeyCommand = null;

            //this.fullscreenKey = Keys.F;
            //this.fullscreenKeyCommand = null;
            this.onElement = false;
            this.canResetPushTimer = false;
        }

        public float getZChange()
        {
            if (this.pushTime <= 0.0f) return 0.0f;
            else
            {
                return (this.pushTime * 1.0f) / this.pushTimeLimit;
            }
        }

        public void resetPushTimer(){
           
            this.pushTime = 0;
        }

        public Boolean IsOnElement { get { return this.onElement; } set { this.onElement = value; } }
        public Boolean CanResetPushTimer { get { return this.canResetPushTimer; } set { this.canResetPushTimer = value; } }

        public Point getCursorRectangle()
        {
            if (this.hub.IsKinectReady())
                return new Point(currentPrimaryHandCoord.X, currentPrimaryHandCoord.Y);
            else
                return new Point(currentMouseCoord.X, currentMouseCoord.Y);

        }

        public void LoadContent(Driver driver)
        {
            if (kinectManager != null)
                this.kinectManager.loadKinectResources(driver.GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //functionality of dynamically updatable list of keys and bindings(commands)
            //improve that functionality and make it able to work with kinect gestures not just mouse and keyboard.
            for (int i = 0; i < this.keyCommandCollection.ToList().Count; i++)
            {
                Keys key = this.keyCommandCollection.ToList().ElementAt(i).Key;
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(key))
                {
                    Command command = this.keyCommandCollection.ToList().ElementAt(i).Value;
                    if(command != null)
                        command.run();
                }
            }

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Y))
            {
                this.hub.sendNoticedGesture("SWIPE_LEFT_GESTURE");
                return;
            }else if(Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.U)){
                this.hub.sendNoticedGesture("SWIPE_RIGHT_GESTURE");
                return;
            }else if(Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.P)){
                Console.WriteLine("CURRENTLY LOOKED GESTURES :: " + this.gestures.Count);
                for(int i = 0; i < this.gestures.Count; i++){
                    Console.WriteLine(this.gestures.ElementAt(i).Name);
                }
                return;
            }



            ////////////////////////////////////////


            if (this.hub.IsKinectReady())
            {
                if (this.checkSkeletonTrackingState(this.hub.IsPersonDetected(), gameTime.ElapsedGameTime.Milliseconds) == false) return;
                //kinectManager.update(gameTime);
                Dictionary<JointType, Joint> skeleton = this.hub.getSkeleton();
                // Joint primeHand3D KinectManager.getJointPos(KinectManager.
                //currentPrimaryHandCoord = kinectManager.PrimaryHandPos2D;
                

                //checks for gestures
                //if recoggnized prevent move / push checks
                if (this.lookForGestures(skeleton, dt) == true)
                {
                    this.zDelta = 0.0f;
                    return;
                }
                currentPrimaryHandCoord = configureKinectCursorPos(this.hub.get2DCursorPosition());

                //currentPrimaryHandCoord = kinectManager.get
                //@TODO get Head and hand position difference for push .
                //IF it is not a push then move on to move event.
                //if(
                Joint hand;
                Joint hipCenter;
                skeleton.TryGetValue(JointType.HipCenter, out hipCenter);
                skeleton.TryGetValue(JointType.HandRight, out hand);

                if (hand.Position.Y < hipCenter.Position.Y) return;
                //12 -> 15
                BoundingCircle currPosCircle = new BoundingCircle((int)currentPrimaryHandCoord.X , (int)currentPrimaryHandCoord.Y , 15);
                if (new BoundingCircle((int)lastPrimaryHandCoord.X, (int)lastPrimaryHandCoord.Y, 18).Intersect(ref currPosCircle) == false)
                {
                    Message<AbstractUI> moveMessage = new InputMoveMessage(2, this);
                    Point processed_points = new Point(
                        currentPrimaryHandCoord.X + this.cursorHitOffset.X,
                        currentPrimaryHandCoord.Y + this.cursorHitOffset.Y);
                    moveMessage.addData(processed_points.X.ToString());
                    moveMessage.addData(processed_points.Y.ToString());

                    broadcast(moveMessage);

                    //if (this.canResetPushTimer)
                        this.resetPushTimer();
                        this.zDelta = 0.0f;
                    this.lastPrimaryHandCoord = currentPrimaryHandCoord;
                    //this.resetPushTimer();
                   // Console.WriteLine(currentPrimaryHandCoord.X + "    " + currentPrimaryHandCoord.Y);
                }
                else
                {
                    //if (this.onElement == false) return;
                    //Console.WriteLine(this.onElement);
                   
                    Joint head;
                    
                    skeleton.TryGetValue(JointType.Head, out head);
                    

                    float armZ = head.Position.Z - 0.40f;

                    this.zTouchStart = armZ;
                    this.zTouchEnd = armZ - 0.175f * armZ;

                    float handlocalMaxZ = this.zTouchStart - hand.Position.Z;
                    float handlocalMinZ = hand.Position.Z - this.zTouchStart;
                    // if it has not reached beyond that point
                    if (hand.Position.Z > this.zTouchEnd)
                    {
                        if (hand.Position.Z < this.zTouchStart)
                        {
                            float range = Math.Abs(zTouchEnd - zTouchStart);
                            this.zDelta = (handlocalMaxZ / range);
                        }
                        else
                        {
                            this.zDelta = 0.0f;
                        }
                    }
                    else
                    {
                        //issue touch command;
                        this.zDelta = 1.0f;
                    }
                    
                    if (this.pushTime == 0)
                    {
                        initZ = hand.Position.Z;
                    }
                    this.pushTime += gameTime.ElapsedGameTime.Milliseconds;
                    float zChange = initZ - hand.Position.Z;

                   // Console.WriteLine(hand.Position.X);
              //     Console.WriteLine("START ::: " + this.zTouchStart + "   END :: " + this.zTouchEnd  + " HAND :: " + hand.Position.Z); 
              //      Console.WriteLine("HEAD Z :: " + head.Position.Z + "      HAND Z :: " + hand.Position.Z);
                  //  Console.WriteLine((head.Position.Z - 0.65f > hand.Position.Z) + "   HEAD :: " + head.Position.Z + "  HAND :: " + hand.Position.Z);
               //     Console.WriteLine("DX ::::: " + zChange);



                    //&& zChange >= 0.075f
                    //this.pushTime >= this.pushTimeLimit
                    //head.Position.Z - 0.65f > hand.Position.Z
                    //if (this.zDelta >= 1.0f)
                    if(this.pushTime >= this.pushTimeLimit)
                    {
                        Point processed_points = new Point(
                        currentPrimaryHandCoord.X + this.cursorHitOffset.X,
                        currentPrimaryHandCoord.Y + this.cursorHitOffset.Y);
                        Message<AbstractUI> touchMessage = new InputTouchMessage(2);
                        touchMessage.addData(processed_points.X.ToString());
                        touchMessage.addData(processed_points.Y.ToString());

                        broadcast(touchMessage);
                        this.onElement = false;
                        this.resetPushTimer();
                        this.zDelta = 0.0f;
                    }
                    

                }

               

                

            }
            else
            {
                if(Mouse.GetState().LeftButton == ButtonState.Pressed){
                    leftMousePressed = true;
                }
                
                if (leftMousePressed && Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    currentMouseCoord = new Point(Mouse.GetState().X, Mouse.GetState().Y);

                    Message<AbstractUI> touchMessage = new InputTouchMessage(2);
                    touchMessage.addData(currentMouseCoord.X.ToString());
                    touchMessage.addData(currentMouseCoord.Y.ToString());

                    broadcast(touchMessage);

                    this.lastMouseCoord = currentMouseCoord;

                    leftMousePressed = false;

                    return;
                }
                
                currentMouseCoord = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                BoundingCircle currMousePosCircle = new BoundingCircle(currentMouseCoord.X, currentMouseCoord.Y, 1);
                //Console.WriteLine(currentMouseCoord);
               // if (new BoundingCircle(lastMouseCoord.X, lastMouseCoord.Y, 1).Intersect(ref currMousePosCircle))
               // {
                    Message<AbstractUI> moveMessage = new InputMoveMessage(2, this);
                    moveMessage.addData(currentMouseCoord.X.ToString());
                    moveMessage.addData(currentMouseCoord.Y.ToString());

                    broadcast(moveMessage);

                    
               // }

                this.lastMouseCoord = currentMouseCoord;
            }

            
        }


        public Rectangle getLastCoord()
        {
            return new Rectangle(lastPrimaryHandCoord.X, lastPrimaryHandCoord.Y, 24, 24);
        }
        //private updateLast

        public void addKeybind(Keys key, Command command)
        {
          //  if (key == null) return;

            this.keyCommandCollection.Add(key, command);
        }

        public void Draw(ref SpriteBatch sp, GameTime gameTime)
        {
            if (this.kinectManager != null)
            {
                this.kinectManager.draw(sp);
            }
            
        }

        //public String ConsumeRecognizedGesture()
        //{
        //    String s = this.recognizedGesture;
        //    this.recognizedGesture = null;

        //    return s;
        //}

        public void addReceiver(Receiver<AbstractUI> newReceiver)
        {
            receipients.Add(newReceiver);
        }

        public void removeReceiver(Receiver<AbstractUI> receiver)
        {
            receipients.Remove(receiver);
        }

        public void broadcast(Message<AbstractUI> message)
        {
            for (int i = 0; i < receipients.Count; i++)
            {
                receipients.ElementAt(i).Receive(message);
               // Console.WriteLine("broadcasting");
            }
            
            
        }

        private bool checkSkeletonTrackingState(Boolean state, int dt)
        {
            if (state == false)
            {
                this.noSkeletonTime += dt;
                if (this.noSkeletonTime >= this.noSkeletonTimeLimit/3)
                {
                    //this.hub.toggleAutoPilot();
                    //notify SceneManager to stop auto pilot
                    this.hub.sendAutoPilotStartRequest();
                    this.noSkeletonTime = 0;
                    //this.oldSkeletonTrackState = true;
                    return false;
                }

                return false;
            }
            else
            {
               // if (this.isSkeletonTimerStarted)
               // {
                    this.noSkeletonTime += dt;
                    if (this.noSkeletonTime >= this.noSkeletonTimeLimit)
                    {
                        //this.hub.toggleAutoPilot();
                        //notify SceneManager to stop auto pilot
                        this.hub.sendAutoPilotStopRequest();
                        this.noSkeletonTime = 0;
                        //this.oldSkeletonTrackState = true;
                        return true;
                    }
                //}
                
                return true;

            }
            //if (!state)
            //{
               

            //    return false;
            //}
            
            //return true;
        }

        private Point configureKinectCursorPos(Point kinectCursorInput)
        {
            int nX = 0, nY = 0;
            //if (kinectCursorInput.X > 0) Console.WriteLine(kinectCursorInput.X);
    //        nX = kinectCursorInput.X + (this.maxScreen.X / 2);
   //         nY = kinectCursorInput.Y + (this.maxScreen.Y / 2);

           // Vector3( w / 640.0f, h / 480.0f, 1.0f); 
            Vector2 ratio = new Vector2(((2.0f * this.maxScreen.X) / (640.0f)), ((2.0f * this.maxScreen.Y) / (480.0f)));

            nX = (int)(kinectCursorInput.X * ratio.X) - (int)((2.0f * this.maxScreen.X) - this.maxScreen.X) /  2;
            nY = (int)(kinectCursorInput.Y * ratio.Y) - (int)((2.0f * this.maxScreen.Y) - this.maxScreen.Y) / 2;

            if (nX < 0) { nX = 0; }
            if (nX > this.maxScreen.X - 24) { nX = this.maxScreen.X -24; }
            if (nY < 0) { nY = 0; }
            if (nY > this.maxScreen.Y - 24) { nY = this.maxScreen.Y - 24; }


           // Console.WriteLine(nX + "    " + nY);
            return new Point(nX, nY);

        }

        public void updateGestureList(List<Gesture> gestureList)
        {
            //if (this.kinectManager != null && gestures != null)
            if (gestureList != null)
            {
                //this.kinectManager.GestureCollection = gestures;
                this.gestures = gestureList;
                this.gestureRecognitionPaused = true;
                Console.WriteLine( gestureList.Count + " GESTURES ADDED");
            }
            this.gestureIndex = 0;
        }



        //public void updateInterestedGestureList(List<String> gestureNames)
        //{
        //    this.
        //}

        public bool lookForGestures(Dictionary<JointType, Joint> bodyJoints, float dt)
        {
            if (this.gestureRecognitionPaused == false)
            {
                if (this.gestures.Count <= 0) return false;




                Gesture currentGesture = this.gestures.ElementAt(this.gestureIndex);

                Boolean result = currentGesture.Update(bodyJoints, dt);
                //Console.WriteLine("Current GESTURE RESULT == " + result);


                if (result)
                {
                    // #2 in the table
              //      Console.Write("POSITIVE GESTURE " + currentGesture.Name);
                    if (currentGesture.IsPaused == false)
                    {
                        if (this.gestureIndex + 1 < this.gestures.Count)
                        {
                            this.gestureIndex++;
                        }
                        else
                        {
                            this.gestureIndex = 0;
                        }
                        this.gestureRecognitionPaused = true;
                //        Console.WriteLine("GESTURE RECOGNIZED!!! " + currentGesture.Name);
                        this.hub.sendNoticedGesture(currentGesture.Name);
                        return true;
                    }
                    else // #1 in the table
                    {

                        return true;
                    }
                }
                else
                {
                    if (currentGesture.IsPaused == false)
                    {
                        if (this.gestureIndex + 1 < this.gestures.Count)
                        {
                            this.gestureIndex++;
                        }
                        else
                        {
                            this.gestureIndex = 0;
                        }
                       // this.gestureRecognitionPaused = true;
                        return false;
                    }

                    return false;
                }

                //if (result == true)
                //{
                //    if (currentGesture.CanContinue)
                //    {
                //        Console.WriteLine("WAITINF FOR CONTINUATION");
                //        return true;
                //    }
                //    else
                //    {
                //        if (this.gestureIndex + 1 < this.gestures.Count)
                //        {
                            
                //            this.gestureIndex++;
                //            this.gestureRecognitionPaused = true;
                //            this.hub.sendNoticedGesture(currentGesture.Name);
                //            Console.WriteLine("!!!!!!!!!!!!!SUCCESSMOVING ONTO NEXT GESTURE" + this.gestureIndex);
                //            return true;
                //        }
                //        else
                //        {
                //            this.gestureIndex = 0;
                //            this.gestureRecognitionPaused = true;
                //            this.hub.sendNoticedGesture(currentGesture.Name);
                //            Console.WriteLine("GESTURE RECOGNITION PAUSED == NO MORE GESTURES");
                //            return true;

                //        }
                //    }
                //}
                //else
                //{
                //    this.gestureRecognitionPaused = false;
                //    Console.WriteLine("GESTURE RECOGNITION PAUSED == GESTURE FAILED");
                //    return false;
                //}

            }
            else
            {
                this.gestureTotalWaitTime += dt;
                //Console.WriteLine(gestureTotalWaitTime);
                if (this.gestureTotalWaitTime >= this.gestureWaitTimeMax)
                {
                    this.gestureTotalWaitTime = 0.0f;
                    this.gestureRecognitionPaused = false;
                    //Console.WriteLine("GESTURE RECOGNITION RESUMEED");
                    return false;
                }
            }

            return false;
        }
    }
}

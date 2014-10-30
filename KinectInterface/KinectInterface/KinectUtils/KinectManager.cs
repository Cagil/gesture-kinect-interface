using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using KinectInterface.KinectUtils;
using KinectInterface.Gestures;
using KinectInterface.Utils;


namespace KinectInterface
{
    public class KinectManager
    {
        private IMediator hub;
        private static KinectManager instance = null;
        Driver mGame;
        private KinectSensor mKinect;

        private Dictionary<JointType, Joint> bodyJoints;
        private List<Gesture> gestures;
        private int gestureIndex;
        private float gestureWaitTimeMax;
        private float gestureTotalWaitTime;
        private Boolean gestureRecognitionPaused;
       
        private Color[] mLatestColorData;
        private Texture2D mColorImage;
        private Texture2D mDepthImage;
        private DepthImagePixel[] depthPixels;
        private Skeleton[] skeletons;

        private Texture2D rTexture;
        private Rectangle rect;

        private Vector3 ScreenVGARatio;

        //Hands hands;
        KinectBody body;
        JointType primaryHand;
        Boolean isConnected;
        float reconnectTimeCount;
        int reconnectTime;

        int chest_width;

        Boolean isTracking;
        Boolean bigMovement;

        List<KinectHandData> prevHands;
        int maxStoreValue;
        float timeCounter;
        int dxCounter;
        int dyCounter;

        float preHandUpdateCount;
        float preHandUpdateLimit;

        KinectHandData preHand;

        float tiltTimeLimit;
        float tiltTotalTime;
        Boolean canTilt;
        Boolean needTilt;

        public float aa;

        public String PrimaryHand { 
            get { return this.primaryHand.ToString(); } 
            set {
                if (value.Equals("Right")) primaryHand = JointType.HandRight;
                else if (value.Equals("Left")) primaryHand = JointType.HandLeft;
                } 
        }

        public Boolean IsConnected { get { return this.isConnected; } }
        public List<Gesture> GestureCollection { get { return this.gestures; } set { this.gestures = value; } }
        public Dictionary<JointType, Joint> BodyJoints { get { return this.bodyJoints; } }

        public IMediator Hub { set { this.hub = value; } }
        public Boolean IsTracking { get { return this.isTracking; } }


        public static KinectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new KinectManager();
                }

                return instance;
            }
        }

        public KinectManager()
        {
            aa = 0.0f;
            chest_width = 0;
            //Default Primary Hand set to Right

            this.PrimaryHand = "Right";
            this.body = KinectBody.Instance;
            this.reconnectTime = 5000;

            this.mDepthImage = null;

            this.ScreenVGARatio = Vector3.One;

            this.isConnected = InitKinect();

            this.bodyJoints = new Dictionary<JointType, Joint>();
            
            this.bodyJoints.Add(JointType.Head, new Joint());
            this.bodyJoints.Add(JointType.Spine, new Joint());
            this.bodyJoints.Add(JointType.ShoulderCenter, new Joint());
            this.bodyJoints.Add(JointType.HipCenter, new Joint());
            this.bodyJoints.Add(JointType.ShoulderLeft, new Joint());
            this.bodyJoints.Add(JointType.ElbowLeft, new Joint());
            this.bodyJoints.Add(JointType.WristLeft, new Joint());
            this.bodyJoints.Add(JointType.HandLeft, new Joint());
            this.bodyJoints.Add(JointType.HipLeft, new Joint());
            this.bodyJoints.Add(JointType.KneeLeft, new Joint());
            this.bodyJoints.Add(JointType.AnkleLeft, new Joint());
            this.bodyJoints.Add(JointType.FootLeft, new Joint());
            this.bodyJoints.Add(JointType.ShoulderRight, new Joint());
            this.bodyJoints.Add(JointType.ElbowRight, new Joint());
            this.bodyJoints.Add(JointType.WristRight, new Joint());
            this.bodyJoints.Add(JointType.HandRight, new Joint());
            this.bodyJoints.Add(JointType.HipRight, new Joint());
            this.bodyJoints.Add(JointType.KneeRight, new Joint());
            this.bodyJoints.Add(JointType.AnkleRight, new Joint());
            this.bodyJoints.Add(JointType.FootRight, new Joint());

            bigMovement = false;
            maxStoreValue = 30;
            timeCounter = 0.0f;
            dxCounter = 0;
            dyCounter = 0;

            preHand = new KinectHandData(new Point(0,0), 0.0f);

            preHandUpdateCount = 0.0f;
            preHandUpdateLimit = 1.0f;

            this.gestureIndex = 0;
            this.gestureTotalWaitTime = 0.0f;
            this.gestureWaitTimeMax = 2.0f;
            this.gestureRecognitionPaused = false;

            this.isTracking = false;

            this.gestures = new List<Gesture>();
            this.prevHands = new List<KinectHandData>(maxStoreValue);

            this.tiltTimeLimit = 1000;
            this.tiltTotalTime = 0; ;
            this.canTilt = false;
            this.needTilt = false;
        }


        // OLD INSTANCE AND CONSTRUCTOR 
        // IF DEPTH OR COLOR IMAGE NEEDED CREATE A METHOD FOR LOADING the DRIVER instance.

        //public static KinectManager getInstance(Driver game){
        //    if (instance == null)
        //    {
        //        instance = new KinectManager(game);
        //    }

        //    return instance;
        //}

        //private KinectManager()
        //{
        //    mGame = game;      
        //}
        
        public void setScreenDim(int w, int h){
            this.ScreenVGARatio = new Vector3( w / 640.0f, h / 480.0f, 1.0f); 
        }

        public Boolean InitKinect()
        {


            if (KinectSensor.KinectSensors.Count == 0)
            {
                Console.WriteLine("KINECT DEVICE NOT FOUND!");
                return false;//"Error: No kinect sensors!";
            }

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.mKinect = potentialSensor;
                    break;
                }
            }


            try
            {
                mKinect = KinectSensor.KinectSensors[0];
                
                mKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                mKinect.DepthStream.Enable(DepthImageFormat.Resolution80x60Fps30);
                
                mKinect.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                
                mKinect.SkeletonStream.Enable(new TransformSmoothParameters()
                {
                    Smoothing = 0.5f,
                    Correction = 0.01f,
                    Prediction = 0.01f,
                    JitterRadius = 0.1f,
                    MaxDeviationRadius = 0.05f
                    
                });
                mKinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(mKinect_SkeletonFrameReady);
               // mDepthImage = new Texture2D(mGame.GraphicsDevice, mKinect.DepthStream.FrameWidth, mKinect.DepthStream.FrameHeight);
               // mKinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(mKinect_ColorFrameReady);
         
                this.mKinect.DepthFrameReady += mKinect_DepthFrameReady;
                if (this.mKinect != null)
                {
                   // this.mKinect.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(mKinect_DepthFrameReady);
                    this.mKinect.Start();
                    
                   // this.hands = Hands.getInstance();
                    this.body = KinectBody.Instance;
                }
              //  mColorImage = new Texture2D(mGame.GraphicsDevice, mKinect.ColorStream.FrameWidth, mKinect.ColorStream.FrameHeight);
                
                //Debug.WriteLineIf(debugging, kinect.Status);
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e.ToString());
            }


            Console.WriteLine("KINECT DEVICE FOUND AND CONNECTED");
            return true;//"";
        }

    



        

        ////Inverse the positions

        //public Vector2 getRightHandPos2D()
        //{
        //    CoordinateMapper mapper = mKinect.CoordinateMapper;
        //    var point = mapper.MapSkeletonPointToColorPoint(body.RightHandJoint.Position, mKinect.ColorStream.Format);
        //    return new Vector2(point.X, point.Y);
        //}


        ////inverse the positions.
        //public Vector2 getLeftHandPos2D()
        //{
        //    CoordinateMapper mapper = mKinect.CoordinateMapper;
        //    var point = mapper.MapSkeletonPointToColorPoint(body.LeftHandJoint.Position, mKinect.ColorStream.Format);
        //    return new Vector2(point.X, point.Y);

        //}

        private Point virtual2dpos(JointType type)
        {
            // CoordinateMapper mapper = mKinect.CoordinateMapper;

            // var point = mapper.MapSkeletonPointToColorPoint(body.getJoint(type).Position, mKinect.ColorStream.Format);

            // Vector2 p = new Vector2(point.X, point.Y);
            //// Console.WriteLine("BEFORE RATIO APPLI
            // p.X = (int)(p.X * this.ScreenVGARatio.X);
            // p.Y = (int)(p.Y * this.ScreenVGARatio.Y);

            CoordinateMapper mapper = mKinect.CoordinateMapper;
            Joint skelJoint = new Joint();
            Joint shoulderJoint = new Joint();
            Joint shoulderLeft = new Joint();
            Joint head = new Joint();
            this.bodyJoints.TryGetValue(JointType.Head, out head);
            this.bodyJoints.TryGetValue(JointType.ShoulderRight, out shoulderJoint);
            this.bodyJoints.TryGetValue(JointType.ShoulderLeft, out shoulderLeft);
            this.bodyJoints.TryGetValue(type, out skelJoint);


            var head_point = mapper.MapSkeletonPointToColorPoint(head.Position, mKinect.ColorStream.Format);
            var shoulder_left_point = mapper.MapSkeletonPointToColorPoint(shoulderLeft.Position, mKinect.ColorStream.Format);
            var point = mapper.MapSkeletonPointToColorPoint(skelJoint.Position, mKinect.ColorStream.Format);
            var shoulder_point = mapper.MapSkeletonPointToColorPoint(shoulderJoint.Position, mKinect.ColorStream.Format);
            if (chest_width == 0)
                chest_width = shoulder_point.X - shoulder_left_point.X;
            //int shoulderDX = shoulder_point.X - shoulder_left_point.X;
            //Console.WriteLine(shoulderJoint.Position.X - shoulderLeft.Position.X);
            Point p = new Point(point.X, point.Y);
            return p;
            BoundingRectangle rec = new BoundingRectangle(shoulder_left_point.X + chest_width - 640 / 2, shoulder_left_point.Y - 480 / 2, 640, 480);
            //Rectangle r = new Rectangle(shoulder_point.X - 320/2, shoulder_point.Y, 320, 240);

            if (rec.Intersect(p) == false)
            {

                if (rec.Position.X > p.X / 2)
                {
                    p.X = 0;
                }


                if (rec.Position.X + rec.Width < p.X / 2)
                {
                    p.X = 640;
                }

                if (rec.Position.Y > p.Y / 2)
                {
                    p.Y = 0;
                }

                if (rec.Position.Y + rec.Height < p.Y / 2)
                {
                    p.Y = 480;
                }
            }
            else
            {
                p.X = p.X - rec.Position.X;
                p.Y = p.Y - rec.Position.Y;
                p.X = (640 * p.X) / rec.Width;
                p.Y = (480 * p.Y) / rec.Height;
                // point.X = 320;
                // point.Y = 240;
            }

            //  Console.WriteLine(r);
            //  p.X = (int)(p.X * this.ScreenVGARatio.X);
            //   p.Y = (int)(p.Y * this.ScreenVGARatio.Y);

            // return p;
        }

        public Point get2DPos(JointType type)
        {
            CoordinateMapper mapper = mKinect.CoordinateMapper;
            Joint skelJoint = new Joint();
            Joint elbowJoint = new Joint();

            this.bodyJoints.TryGetValue(JointType.Head, out elbowJoint);
            this.bodyJoints.TryGetValue(type, out skelJoint);

            

            var elbow = mapper.MapSkeletonPointToColorPoint(elbowJoint.Position, mKinect.ColorStream.Format);
            var point = mapper.MapSkeletonPointToColorPoint(skelJoint.Position, mKinect.ColorStream.Format);

            int vW = 240;
            int vH = 160;
            BoundingRectangle vScreen = new BoundingRectangle(elbow.X - (vW/3) , elbow.Y, vW, vH);
           
            Point newPos = new Point(point.X - elbow.X, point.Y - elbow.Y);

            //left of pivot
            if (newPos.X < 0)
            {
                if (newPos.X < vScreen.Position.X)
                {
                    newPos.X = vScreen.Position.X + 1;
                }
            }
            else
            {
                if (newPos.X > vScreen.Position.X + vScreen.Width)
                {
                    newPos.X = vScreen.Position.X + vScreen.Width - 2;
                }
            }

            if (newPos.Y < 0)
            {
                if (newPos.Y < vScreen.Position.Y)
                {
                    newPos.Y = vScreen.Position.Y +1;
                }

            }
            else
            {
                if (newPos.Y > vScreen.Position.Y + vScreen.Height)
                {
                    newPos.Y = vScreen.Position.Y + vScreen.Height + 2;
                }
            }

            newPos.X = (this.mKinect.ColorStream.FrameWidth * newPos.X) / vScreen.Width;
            newPos.Y = (this.mKinect.ColorStream.FrameHeight * newPos.Y) / vScreen.Height;
            Point p = new Point(newPos.X, newPos.Y);


            return p;
        }

        //Default method for getting interacting hand position 
        public Point PrimaryHandPos2D { get {
            
            return get2DPos(this.primaryHand);    
        } }

        //@TODO
        public Point CursorPos { get { return new Point(); } }

        //public DepthImagePoint getRightHandPos()
        //public Vector3 getRightHandPos(float scaleFactor)
        //{          
        //    return new Vector3(-1*hands.RightJoint.Position.X * scaleFactor, hands.RightJoint.Position.Y * scaleFactor, hands.RightJoint.Position.Z * scaleFactor);
        //}

        public Vector3 getJointPos(JointType t, Vector3 s)
        {
            Joint skelJoint = new Joint();
            this.bodyJoints.TryGetValue(t, out skelJoint);

            Vector3 pos = new Vector3(
                skelJoint.Position.X * s.X,
                skelJoint.Position.Y * s.Y,
                skelJoint.Position.Z * s.Z);


            return pos;
        }



        //public Vector3 getLeftHandPos(float scaleFactor)
        //{        
        //    return new Vector3(-1*hands.LeftJoint.Position.X * scaleFactor, hands.LeftJoint.Position.Y *scaleFactor , hands.LeftJoint.Position.Z * scaleFactor);
        //}


        public void loadKinectResources(GraphicsDevice gd)
        {
           // Console.WriteLine("KINECT INIT");
             this.rTexture = new Texture2D(gd, 1, 1);
              this.rTexture.SetData(new[] { Color.Chocolate });

              this.rect = new Rectangle(0, 0, 24, 24);

            if(this.IsConnected)
              this.mDepthImage = new Texture2D(gd, this.mKinect.DepthStream.FrameWidth, this.mKinect.DepthStream.FrameHeight);
            body.init(gd);
        }

        public void DisposeResources()
        {
            // this.rTexture.Dispose();

            
            body.Dispose();
            // this.mColorImage.Dispose();
            //this.mDepthImage.Dispose();
        }

        public void draw(SpriteBatch sb)
        {
            //body.drawHands(sb);

            sb.Draw(this.rTexture, this.rect, Color.Black);
        }


        public void update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float tt = (float)gameTime.TotalGameTime.TotalSeconds;
            if (this.isConnected != true)
            {
                this.reconnectTimeCount += dt;
                if (this.reconnectTimeCount >= this.reconnectTime)
                {
                    Console.WriteLine("TRYING TO LOOK FOR A KINECT DEVICE");
                    this.isConnected = InitKinect();
                    this.reconnectTimeCount = 0;
                }
                return;
            }

            if (this.needTilt == true)
            {
                this.tiltTotalTime += dt;
                if (this.tiltTotalTime >= this.tiltTimeLimit)
                {
                    this.canTilt = true;
                }
                else
                {
                    this.canTilt = false;
                }
            }


            //if (this.preHand != null)
            //{
            //   // Console.WriteLine(this.preHand.Position.X + "        " + this.PrimaryHandPos2D.X);
            //    BoundingCircle lastHandCircle = new BoundingCircle((int)this.preHand.Position.X, (int)this.preHand.Position.Y, 2);
            //    if (new BoundingCircle((int)this.PrimaryHandPos2D.X, (int)this.PrimaryHandPos2D.Y, 3).Intersect(ref lastHandCircle) == true)
            //    {
            //        //if (Math.Abs(tt - this.preHand.Time) > 2.0f)
            //        // {
            //        // Console.WriteLine("LAST AND CURRENT HAND IS TOO CLOSE");

            //        return;
            //        // }
            //    }
            //    else
            //    {
                    
            //        if (this.preHandUpdateCount >= this.preHandUpdateLimit)
            //        {
            //            Console.WriteLine("UPDATING PRE  HAND");
            //            this.preHand = new KinectHandData(this.PrimaryHandPos2D, tt);
            //            this.preHandUpdateCount = 0.0f;
            //        }
            //        else
            //        {
            //            this.preHandUpdateCount += dt;
            //        }

            //    }

            //}
            //else
            //{
            //    Console.WriteLine("FIRST TIME SETTING PRE  HAND");
            //    this.preHand = new KinectHandData(this.PrimaryHandPos2D, tt);
            //}


            //if (this.gestureRecognitionPaused == false)
            //{
            //    if (this.gestures.Count <= 0) return;


                

            //    Gesture currentGesture = this.gestures.ElementAt(this.gestureIndex);

            //    Boolean result = currentGesture.Update(this.bodyJoints, dt);
            //    Console.WriteLine("Current GESTURE RESULT == " + result);
                
               
            //        if (result == true)
            //        {
            //            if (currentGesture.CanContinue)
            //            {
            //                return;
            //            }
            //            else
            //            {
            //                if (this.gestureIndex + 1 < this.gestures.Count)
            //                {
            //                    this.gestureIndex++;
            //                    this.gestureRecognitionPaused = true;
            //                    Console.WriteLine("MOVING ONTO NEXT GESTURE" + this.gestureIndex);
            //                }
            //                else
            //                {
            //                    this.gestureIndex = 0;
            //                    this.gestureRecognitionPaused = true;
            //                    Console.WriteLine("GESTURE RECOGNITION PAUSED == NO MORE GESTURES");

            //                }
            //            }
            //        }
            //        else
            //        {
            //            this.gestureRecognitionPaused = true;
            //            Console.WriteLine("GESTURE RECOGNITION PAUSED == GESTURE FAILED");
            //        }
               
            //}
            //else
            //{
            //    this.gestureTotalWaitTime += dt;
            //    //Console.WriteLine(gestureTotalWaitTime);
            //    if (this.gestureTotalWaitTime >= this.gestureWaitTimeMax)
            //    {
            //        this.gestureTotalWaitTime = 0.0f;
            //        this.gestureRecognitionPaused = false;
            //        Console.WriteLine("GESTURE RECOGNITION RESUMEED");
            //        return;
            //    }
            //}

            

        }

     

        private byte[] ConvertDepthFrame(short[] depthFrame, DepthImageStream depthStream)
        {
            int RedIndex = 0, GreenIndex = 1, BlueIndex = 2, AlphaIndex = 3;

            byte[] depthFrame32 = new byte[depthStream.FrameWidth * depthStream.FrameHeight * 4];

            for (int i16 = 0, i32 = 0; i16 < depthFrame.Length && i32 < depthFrame32.Length; i16++, i32 += 4)
            {
                int player = depthFrame[i16] & DepthImageFrame.PlayerIndexBitmask;
                int realDepth = depthFrame[i16] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                byte intensity = (byte)(~(realDepth >> 4));

                depthFrame32[i32 + RedIndex] = (byte)(intensity);
                depthFrame32[i32 + GreenIndex] = (byte)(intensity);
                depthFrame32[i32 + BlueIndex] = (byte)(intensity);
                depthFrame32[i32 + AlphaIndex] = 255;
            }

            return depthFrame32;
        }

        private void mKinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            DepthImageFrame depthVideoFrame = e.OpenDepthImageFrame();

            

            if (depthVideoFrame != null)
            {
                //Debug.WriteLineIf(debugging, "Frame");
                //Create array for pixel data and copy it from the image frame
                short[] pixelData = new short[depthVideoFrame.PixelDataLength];
                depthVideoFrame.CopyPixelDataTo(pixelData);

                for (int i = 0; i < 10; i++)
                {
                    //Debug.WriteLineIf(debugging, pixelData[i]); 
                }

                // Convert the Depth Frame
                // Create a texture and assign the realigned pixels
                //
               // mDepthImage = new Texture2D(mGame.GraphicsDevice, depthVideoFrame.Width, depthVideoFrame.Height);
                mDepthImage.SetData(ConvertDepthFrame(pixelData, mKinect.DepthStream));

              

            }
        }

        void mKinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame colorVideoFrame = e.OpenColorImageFrame();

            if (colorVideoFrame != null)
            {
                //Create array for pixel data and copy it from the image frame
                Byte[] pixelData = new Byte[colorVideoFrame.PixelDataLength];
                colorVideoFrame.CopyPixelDataTo(pixelData);

                //Convert RGBA to BGRA
                Byte[] bgraPixelData = new Byte[colorVideoFrame.PixelDataLength];
                for (int i = 0; i < pixelData.Length; i += 4)
                {
                    bgraPixelData[i] = pixelData[i + 2];
                    bgraPixelData[i + 1] = pixelData[i + 1];
                    bgraPixelData[i + 2] = pixelData[i];
                    bgraPixelData[i + 3] = (Byte)255; //The video comes with 0 alpha so it is transparent
                }

                // Create a texture and assign the realigned pixels
                mColorImage = new Texture2D(mGame.GraphicsDevice, colorVideoFrame.Width, colorVideoFrame.Height);
                mColorImage.SetData(bgraPixelData);
            }

        }

        public void DrawColorImage(SpriteBatch batch, GraphicsDevice device, Rectangle bounds)
        {

            if (mColorImage == null)
            {
                return;
            }

            // mColorImage = new Texture2D(device, 640, 480);

            // mColorImage.SetData<Color>(mLatestColorData);

            batch.Draw(mColorImage, bounds, Color.White);
        }

        

        public void DrawDepthImage(SpriteBatch batch, GraphicsDevice device, Rectangle bounds)
        {
            if (mDepthImage == null)
            {
                return;
            }

            batch.Draw(mDepthImage, bounds, Color.White);

            
        }

        void mKinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;

                if (skeletons == null ||
                    skeletons.Length != skeletonFrame.SkeletonArrayLength)
                {

                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                }

                skeletonFrame.CopySkeletonDataTo(skeletons);

            }

            Skeleton closestSkeleton = skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked)
                                                .OrderBy(s => s.Position.Z )
                                                .FirstOrDefault();






            if (closestSkeleton == null)
            {
              //  Console.WriteLine("NO SKELETON FOUND!!!");
                this.isTracking = false;
              //  Console.WriteLine(this.mKinect.ElevationAngle);
                //this.hub.notifyTrackingState(this.isTracking);
                return;
            }

           // if(closestSkeleton.ClippedEdges
            //if (this.canTilt == true)
            //{
            //    this.adjustElevationAngle(closestSkeleton.ClippedEdges);
            //    this.needTilt = false;
            //}
            //if (this.isTracking == false)
            //{
            //    Console.WriteLine("HEAD POS :: " + closestSkeleton.Joints[JointType.Head].Position.Y);
            //    if (closestSkeleton.ClippedEdges != FrameEdges.None)
            //    {
            //        Console.WriteLine("FRAME EDGE :: " + closestSkeleton.ClippedEdges);
            //        if (closestSkeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            //        {
            //            this.mKinect.ElevationAngle += 3;

            //        }
            //    }
            //}

            //if (closestSkeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            //{
            //    Console.WriteLine("TOP ");
            //    //this.mKinect.ElevationAngle += 2;
            //    this.needTilt = true;
            //    return;
            //}
         //   Console.WriteLine("SKELETON FOUND!!!");
            //if (this.isTracking == false)
            //{
            //    while (closestSkeleton.ClippedEdges != FrameEdges.None)
            //    {
            //        adjustElevationAngle(closestSkeleton.ClippedEdges);
            //    }
                
            //}

            this.isTracking = true;
           // this.hub.notifyTrackingState(this.isTracking);
            this.updateJoints(closestSkeleton);
            //this.body.updateJoints(closestSkeleton);
            // this.body.RightHan
        }

        private void adjustElevationAngle(FrameEdges edges)
        {
            switch (edges)
            {
                case FrameEdges.Top:
                    this.mKinect.ElevationAngle++;
                    break;

                case FrameEdges.Bottom:
                    this.mKinect.ElevationAngle++;
                    break;

                case FrameEdges.Left:
                    break;

                case FrameEdges.Right:
                    break;
                default:
                    break;

            }


        }

        private void updateJoints(Skeleton skeleton)
        {

            for (int i = 0; i < skeleton.Joints.Count; i++)
            {
                this.bodyJoints[skeleton.Joints.ElementAt(i).JointType] = skeleton.Joints.ElementAt(i);
            }


        }

        public void addGesture(Gesture gest)
        {
            this.gestures.Add(gest);
        }

        public void removeGesture(Gesture gest)
        {
            this.gestures.Remove(gest);
        }

     


      

    }
}

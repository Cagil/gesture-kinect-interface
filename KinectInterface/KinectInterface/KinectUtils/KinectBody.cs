using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KinectInterface
{
    class KinectBody
    {
        private static KinectBody instance = null;
        private Joint head;
        private Joint spine;
        private Joint centerShoulder;
        private Joint centerHip;

        private Joint rightHand; public Vector3 prevRightHand; public Vector2 RightHandSelectPos; public Vector3 prevCenterShoulder;
        private Joint rightWrist;
        private Joint rightElbow;
        private Joint rightShoulder;
        private Joint rightHip;
        private Joint rightKnee;
        private Joint rightAnkle;
        private Joint rightFoot;

        private Joint leftHand; public Vector3 prevLeftHand; public Vector2 LeftHandSelectPos;
        private Joint leftWrist;
        private Joint leftElbow;
        private Joint leftShoulder;
        private Joint leftHip;
        private Joint leftKnee;
        private Joint leftAnkle;
        private Joint leftFoot;

        //private int Screenwidth;
        //private int Screenheight;

        //public int ScreenWidth { get { return this.Screenwidth; } set { this.Screenwidth = value; } }
        //public int ScreenHeight { get { return this.Screenheight; } set { this.Screenheight = value; } }

        //Left and RIght Hand Position and Rendering fields
        private Vector3 rightPosition;
        private Vector3 leftPosition;

        private int width;
        private int height;
        private Color leftColor;
        private Color rightColor;

        private Texture2D lTexture;
        private Texture2D rTexture;

        private Rectangle leftRect;
        private Rectangle rightRect;

        public static KinectBody Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new KinectBody();

                }

                return instance;
            }
        }

        private KinectBody()
        {
            this.width = 24;
            this.height = 24;


            this.leftRect = new Rectangle(0,0, 24, 24);
            this.rightRect = new Rectangle(0, 0, 24, 24);

            this.leftColor = Color.Brown;
            this.rightColor = Color.Black;

          
        }

        public void init(GraphicsDevice gd)
        {
            Console.WriteLine("KINECT BODY INIT");
            this.lTexture = new Texture2D(gd, 1, 1);
            this.lTexture.SetData(new[] { this.leftColor });
            this.rTexture = new Texture2D(gd, 1,1);
            this.rTexture.SetData(new[] { this.rightColor });
        }

        public Joint HeadJoint { set { this.head = value; } get { return this.head; } }
        public Joint SpineJoint { set { this.spine = value; } get { return this.spine; } }
        public Joint CenterShoulderJoint { set { this.centerShoulder = value; } get { return this.centerShoulder; } }
        public Joint CenterHipJoint { set { this.centerHip = value; } get { return this.centerHip; } }

        public Joint LeftHandJoint { set { this.leftHand = value; } get { return this.leftHand; } }
        public Joint LeftWristJoint { set { this.leftWrist = value; } get { return this.leftWrist; } }
        public Joint LeftElbowJoint { set { this.leftElbow = value; } get { return this.leftElbow; } }
        public Joint LeftShoulderJoint { set { this.leftShoulder = value; } get { return this.leftShoulder; } }
        public Joint LeftHipJoint { set { this.leftHip = value; } get { return this.leftHip; } }
        public Joint LeftKneeJoint { set { this.leftKnee = value; } get { return this.leftKnee; } }
        public Joint LeftAnkleJoint { set { this.leftAnkle = value; } get { return this.leftAnkle; } }
        public Joint LeftFootJoint { set { this.leftFoot = value; } get { return this.leftFoot; } }       

        public Joint RightHandJoint { set { this.rightHand = value; } get { return this.rightHand; } }
        public Joint RightWristJoint { set { this.rightWrist = value; } get { return this.rightWrist; } }
        public Joint RightElbowJoint { set { this.rightElbow = value; } get { return this.rightElbow; } }
        public Joint RightShoulderJoint { set { this.rightShoulder = value; } get { return this.rightShoulder; } }
        public Joint RightHipJoint { set { this.rightHip = value; } get { return this.rightHip; } }
        public Joint RightKneeJoint { set { this.rightKnee = value; } get { return this.rightKnee; } }
        public Joint RightAnkleJoint { set { this.rightAnkle = value; } get { return this.rightAnkle; } }
        public Joint RightFootJoint { set { this.rightFoot = value; } get { return this.rightFoot; } }


        public Vector3 getRightHandPos(Vector3 s) { return getPosition3D(JointType.HandRight, s); }
        public Vector3 getRightWristPos(Vector3 s) { return getPosition3D(JointType.WristRight, s); }
        public Vector3 getRightElbowPos(Vector3 s) { return getPosition3D(JointType.ElbowRight, s); }
        public Vector3 getRightShoulderPos(Vector3 s) { return getPosition3D(JointType.ShoulderRight, s); }
        public Vector3 getRightHipPos(Vector3 s) { return getPosition3D(JointType.HipRight, s); }
        public Vector3 getRightKneePos(Vector3 s) { return getPosition3D(JointType.KneeRight, s); }
        public Vector3 getRightFootPos(Vector3 s) { return getPosition3D(JointType.FootRight, s); }
        public Vector3 getLeftHandPos(Vector3 s) { return getPosition3D(JointType.HandLeft, s); }
        public Vector3 getLeftWristPos(Vector3 s) { return getPosition3D(JointType.WristLeft, s); }
        public Vector3 getLeftElbowPos(Vector3 s) { return getPosition3D(JointType.ElbowLeft, s); }
        public Vector3 getLeftShoulderPos(Vector3 s) { return getPosition3D(JointType.ShoulderLeft, s); }
        public Vector3 getLeftHipPos(Vector3 s) { return getPosition3D(JointType.HipLeft, s); }
        public Vector3 getLeftKneePos(Vector3 s) { return getPosition3D(JointType.KneeLeft, s); }
        public Vector3 getLeftFootPos(Vector3 s) { return getPosition3D(JointType.FootLeft, s); }
        public Vector3 getHeadPos(Vector3 s) { return getPosition3D(JointType.Head, s); }
        public Vector3 getSpinePos(Vector3 s) { return getPosition3D(JointType.Spine, s); }
        public Vector3 getCenterShoulderPos(Vector3 s) { return getPosition3D(JointType.ShoulderCenter, s); }
        public Vector3 getCenterHipPos(Vector3 s) { return getPosition3D(JointType.HipCenter, s); }

        public Vector3 RightHandPos { get {return getPosition3D(JointType.HandRight, Vector3.One); } }
        public Vector3 RightWristPos { get{ return getPosition3D(JointType.WristRight, Vector3.One); }}
        public Vector3 RightElbowPos { get{return getPosition3D(JointType.ElbowRight, Vector3.One); }}
        public Vector3 RightShoulderPos {  get{return getPosition3D(JointType.ShoulderRight, Vector3.One); }}
        public Vector3 RightHipPos {  get{return getPosition3D(JointType.HipRight, Vector3.One); }}
        public Vector3 RightKneePos {  get{return getPosition3D(JointType.KneeRight, Vector3.One); }}
        public Vector3 RightFootPos {  get{return getPosition3D(JointType.FootRight, Vector3.One); }}
        public Vector3 LeftHandPos {  get{return getPosition3D(JointType.HandLeft, Vector3.One); }}
        public Vector3 LeftWristPos {  get{return getPosition3D(JointType.WristLeft, Vector3.One); }}
        public Vector3 LeftElbowPos {  get{return getPosition3D(JointType.ElbowLeft, Vector3.One); }}
        public Vector3 LeftShoulderPos {  get{return getPosition3D(JointType.ShoulderLeft, Vector3.One); }}
        public Vector3 LeftHipPos {  get{return getPosition3D(JointType.HipLeft, Vector3.One); }}
        public Vector3 LeftKneePos { get{ return getPosition3D(JointType.KneeLeft, Vector3.One); }}
        public Vector3 LeftFootPos {  get{return getPosition3D(JointType.FootLeft, Vector3.One); }}
        public Vector3 HeadPos {  get{return getPosition3D(JointType.Head, Vector3.One); }}
        public Vector3 SpinePos {  get{return getPosition3D(JointType.Spine, Vector3.One); }}
        public Vector3 CenterShoulderPos {  get{return getPosition3D(JointType.ShoulderCenter, Vector3.One); }}
        public Vector3 CenterHipPos { get { return getPosition3D(JointType.HipCenter, Vector3.One); } }
        public bool getPrevHands;

        public Vector2 getPosition2D(JointType type){
            return new Vector2();
        }

        public Vector3 getPosition3D(JointType type, Vector3 scale_factor){
            Joint j = getJoint(type);
            Vector3 tmp = new Vector3(
                j.Position.X * scale_factor.X,
                j.Position.Y * scale_factor.Y,
                j.Position.Z * scale_factor.Z);


            //Vector3 skeletonDetectSize = new Vector3(640, 480, 1);
            //Vector3 currentWindowSize = new Vector3(this.Screenwidth, this.Screenheight, 1);

            //tmp = (tmp * currentWindowSize) / skeletonDetectSize;

            return tmp;

        }

        public Joint getJoint(JointType type)
        {
            switch (type)
            {
                case JointType.Head: return HeadJoint;
                case JointType.Spine: return SpineJoint;
                case JointType.ShoulderCenter: return CenterShoulderJoint;
                case JointType.HipCenter: return CenterHipJoint;
                case JointType.ShoulderLeft: return LeftShoulderJoint;
                case JointType.ElbowLeft: return LeftElbowJoint;
                case JointType.WristLeft: return LeftWristJoint;
                case JointType.HandLeft: return LeftHandJoint;
                case JointType.HipLeft: return LeftHipJoint;
                case JointType.KneeLeft: return LeftKneeJoint;
                case JointType.FootLeft: return LeftFootJoint;
                case JointType.ShoulderRight: return RightShoulderJoint;
                case JointType.ElbowRight: return RightElbowJoint;
                case JointType.WristRight: return RightWristJoint;
                case JointType.HandRight: return RightHandJoint;
                case JointType.HipRight: return RightHipJoint;
                case JointType.KneeRight: return RightKneeJoint;
                case JointType.AnkleRight: return RightAnkleJoint;
                case JointType.FootRight: return RightFootJoint;
                default: return HeadJoint;
            }
        }

        public void updateJoints(Skeleton skeleton)
        {
            HeadJoint = skeleton.Joints[JointType.Head];
            SpineJoint = skeleton.Joints[JointType.Spine];
            CenterShoulderJoint = skeleton.Joints[JointType.ShoulderCenter];
            CenterHipJoint = skeleton.Joints[JointType.HipCenter];
            LeftShoulderJoint = skeleton.Joints[JointType.ShoulderLeft];
            LeftElbowJoint = skeleton.Joints[JointType.ElbowLeft];
            LeftWristJoint = skeleton.Joints[JointType.WristLeft];
            LeftHandJoint = skeleton.Joints[JointType.HandLeft];
            LeftHipJoint = skeleton.Joints[JointType.HipLeft];
            LeftKneeJoint = skeleton.Joints[JointType.KneeLeft];
            LeftAnkleJoint = skeleton.Joints[JointType.AnkleLeft];
            LeftFootJoint = skeleton.Joints[JointType.FootLeft];
            RightShoulderJoint = skeleton.Joints[JointType.ShoulderRight];
            RightElbowJoint = skeleton.Joints[JointType.ElbowRight];
            RightWristJoint = skeleton.Joints[JointType.WristRight];
            RightHandJoint = skeleton.Joints[JointType.HandRight];
            RightHipJoint = skeleton.Joints[JointType.HipRight];
            RightKneeJoint = skeleton.Joints[JointType.KneeRight];
            RightAnkleJoint = skeleton.Joints[JointType.AnkleRight];
            RightFootJoint = skeleton.Joints[JointType.FootRight];
           
        }




        //public Vector3 LeftHandPosition2D
        //{
        //    set
        //    {
        //        this.LeftHandSelectPos = new Vector2(value.X, value.Y);
        //        this.leftPosition = value;
        //        Vector3 tmp = this.leftPosition;
        //        Vector3 skeletonDetectSize = new Vector3(640, 480, 1);
        //        Vector3 currentWindowSize = new Vector3(this.Screenwidth, this.Screenheight, this.LeftHandPos.Z);

        //        tmp = (tmp * currentWindowSize) / skeletonDetectSize;
        //        this.leftRect = new Rectangle((int)Math.Ceiling(tmp.X), (int)Math.Ceiling(tmp.Y), this.width, this.height);
        //        //this.leftRect = new Rectangle((int)Math.Ceiling(this.leftPosition.X), (int)Math.Ceiling(this.leftPosition.Y), this.width, this.height);
        //        this.leftPosition = new Vector3((float)Math.Ceiling(tmp.X), (float)Math.Ceiling(tmp.Y), 0.0f);

        //        if (getPrevHands == true)
        //        {
        //            prevLeftHand = this.leftPosition;
                   
        //        }
        //    }
        //    get 
        //    {

        //        return this.leftPosition; 
        //    }
        //}
        //public Vector3 RightHandPosition2D
        //{
        //    set
        //    {
        //        //this.RightHandSelectPos = new Vector2(value.X, value.Y);
        //        this.rightPosition = value;

        //        //Vector3 tmp = this.rightPosition;
        //        //Vector3 skeletonDetectSize = new Vector3(640, 480, 1);
        //        //Vector3 currentWindowSize = new Vector3(this.Screenwidth, this.Screenheight, this.RightHandPos.Z);

        //        //tmp = (tmp * currentWindowSize) / skeletonDetectSize;
        //        this.rightRect = new Rectangle((int)Math.Ceiling(value.X), (int)Math.Ceiling(value.Y), this.width, this.height);
        //        //this.rightPosition = new Vector3((float)Math.Ceiling(tmp.X), (float)Math.Ceiling(tmp.Y), 0.0f);
        //        //// this.rightRect = new Rectangle((int)Math.Ceiling(this.rightPosition.X), (int)Math.Ceiling(this.rightPosition.Y), this.width, this.height);

        //        //if (getPrevHands == true)
        //        //{
        //        //    prevRightHand = this.rightPosition;

        //        //}
                
        //    }
        //    get 
        //    {

        //       // return tmp;
        //        return this.rightPosition; 
        //    }
        //}

        public void drawHands(SpriteBatch spriteBatch)
        {
           // spriteBatch.Draw(lTexture, this.leftRect, this.leftColor);
          //  Console.WriteLine(this.rightRect.X + "   " + this.rightRect.Y);
            spriteBatch.Draw(rTexture, this.rightRect, this.rightColor);
        }

        public void Dispose()
        {
            this.lTexture.Dispose();
            this.rTexture.Dispose();
            
        }

        //public void setGameWindowSize(int w, int h)
        //{
        //    this.Screenheight = h;
        //    this.Screenwidth = w;
        //}

         
    }
}

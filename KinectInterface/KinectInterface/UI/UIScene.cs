using KinectInterface.Messages;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KinectInterface.UI
{
    public class UIScene : AbstractUI, DoubleLinked<UIScene>, Sender<AbstractUI>
    {
        public List<Receiver<AbstractUI>> elements;
        private UIScene next;
        private UIScene prev;
        private UIScene parent;
        private Boolean isSlide;
        public List<Texture2D> gestureIndicators;
        private List<BoundingRectangle> gestureRects;
        private UIPanel rightGestureIndicator;
        private UIPanel leftGestureIndicator;
        public UIText rightGestLabel;
        public UIText leftGestLabel;

        private BoundingRectangle topTextRect;
        private UIText bottomText;
        private BoundingRectangle bottomTextRect;
        private UIPanel bottomTextPanel;

        public UIText BottomText { get { return this.bottomText; } set { this.bottomText = value; } }
        public BoundingRectangle BottomTextRectangle { get { return this.bottomTextRect; } set { this.bottomTextRect = value; } }
        public BoundingRectangle TopTextRectangle { get { return this.topTextRect; } set { this.topTextRect = value; } }

        private Boolean isAutoPilotDraw;
        public Boolean isAutoPilotOn { get { return this.isAutoPilotDraw; } set{this.isAutoPilotDraw = value;}}

        //private this.
        public Boolean IsASlide { get { return this.isSlide; } set { this.isSlide = value; } }


        public UIScene(Driver driver, String label = "Scene")
            : base(driver)
        {
            elements = new List<Receiver<AbstractUI>>();
            this.Label = new UIText(driver, label);
            this.parent = null;
            this.next = null;
            this.prev = null;

            this.isSlide = false;
            this.bottomText = null;
            this.bottomTextRect = null;
            this.topTextRect = null;

            this.rightGestLabel = new UIText(driver, "Right Hand Gestures");
            this.leftGestLabel = new UIText(driver, "Left Hand Gestures");

            this.rightGestureIndicator = new UIPanel(driver);
            this.leftGestureIndicator = new UIPanel(driver);
            this.bottomTextPanel = new UIPanel(driver);
            this.gestureIndicators = new List<Texture2D>();
            this.gestureRects = new List<BoundingRectangle>();

            isAutoPilotDraw = false;
            //this.gestureIndicator = null;
        }

        public override void LoadContent(ref ResourceManager resources, GraphicsDevice gd)
        {
            this.Label.LoadContent(ref resources, gd);

            if (this.topTextRect != null)
            {
                this.Label.PlaceAndAlign(this.topTextRect);
            }
            else
            {
                this.Label.PlaceAndAlign(new BoundingRectangle(0, 0, this.BoundingRectangle.Width, 50));
            }

            if (this.bottomText != null)
            {
                if (this.bottomTextRect != null)
                {
                    this.bottomText.LoadContent(ref resources, gd);

                    this.bottomText.PlaceAndAlign(this.bottomTextRect);

                    //this.bottomTextPanel.CurrentColor = Color.Black;
                    this.bottomTextPanel.addReceiver(bottomText);
                }
            }

            
             

            int leftY = (this.BoundingRectangle.Height * 10)/100;
            int rightY = (this.BoundingRectangle.Height * 10)/100;
            int dim = (this.BoundingRectangle.Width * 10) / 100;
            int dividedHeight = 0;
            if (this.InterestedGestures.Count > 0)
                dividedHeight = this.BoundingRectangle.Height / this.InterestedGestures.Count;
            for (int i = 0; i < this.InterestedGestures.Count; i++)
            {
               // Texture2D currTex = this.gestureIndicators.ElementAt(i);

                

                String currGestureName = this.InterestedGestures.ElementAt(i);
                if (currGestureName == "GO_BACK_GESTURE")
                {
                //    Console.WriteLine("GESTURE INDI FOUND " + currGestureName);
                    UIImage icn = new UIImage(this.Driver);
                    icn.Texture = ResourceManager.loadTextureOnce(gd, "arrow_up.png");
                    icn.BoundingRectangle = new BoundingRectangle(this.BoundingRectangle.Width - dim, rightY, dim, dim);
                    this.rightGestureIndicator.addReceiver(icn);
                    this.gestureIndicators.Add(ResourceManager.loadTextureOnce(gd, "arrow_up.png"));
                    this.gestureRects.Add(new BoundingRectangle(this.BoundingRectangle.Width - dim, rightY, dim, dim));
                    rightY += (int)(dim * 1.25f);
                }
                else if (currGestureName == "SWIPE_LEFT_GESTURE")
                {
                    UIImage icn = new UIImage(this.Driver);
                    icn.Texture = ResourceManager.loadTextureOnce(gd, "arrow_left.png");
                    icn.BoundingRectangle = new BoundingRectangle(this.BoundingRectangle.Width - dim, rightY, dim, dim);
                    this.rightGestureIndicator.addReceiver(icn);
                    //    categoryScenes.ElementAt(g)
                   // Console.WriteLine("GESTURE INDI FOUND " + currGestureName);
                    this.gestureIndicators.Add(ResourceManager.loadTextureOnce(gd, "arrow_left.png"));
                    this.gestureRects.Add(new BoundingRectangle(this.BoundingRectangle.Width - dim, rightY, dim, dim));
                    rightY += (int)(dim * 1.25f);
                }
                else if (currGestureName == "SWIPE_RIGHT_GESTURE")
                {
                    UIImage icn = new UIImage(this.Driver);
                    icn.Texture = ResourceManager.loadTextureOnce(gd, "arrow_right.png");
                    icn.BoundingRectangle =new BoundingRectangle(0, leftY, dim, dim);
                    this.leftGestureIndicator.addReceiver(icn);
                   this.gestureIndicators.Add(ResourceManager.loadTextureOnce(gd, "arrow_right.png"));
                  //  Console.WriteLine("GESTURE INDI FOUND " + currGestureName);
                    this.gestureRects.Add(new BoundingRectangle(0, leftY, dim, dim));
                    leftY += (int)(dim * 1.25f);
                }

                

            }

            if (this.isSlide == false)
            {
                UIImage icn = new UIImage(this.Driver);
                icn.Texture = ResourceManager.loadTextureOnce(gd, "hold_still.png");
                icn.BoundingRectangle = new BoundingRectangle(this.BoundingRectangle.Width - dim, rightY, dim, dim);
                this.rightGestureIndicator.addReceiver(icn);
                this.gestureIndicators.Add(ResourceManager.loadTextureOnce(gd, "hold_still.png"));
                this.gestureRects.Add(new BoundingRectangle(this.BoundingRectangle.Width - dim, rightY, dim, dim));
                rightY += (int)(dim * 1.25f);
            }

            Console.WriteLine("GESTURE INDI COUNT :: " + this.gestureIndicators.Count);
            if (this.gestureIndicators.Count > 0)
            {
               
                for (int i = 0; i < this.gestureIndicators.Count; i++)
                {
                    
                }
            }

            LoadContentMessage message = new LoadContentMessage(ref resources, gd);
            broadcast(message);
        }

        public override void Initialize()
        {
            

            InitializeMessage message = new InitializeMessage();
            this.leftGestureIndicator.Receive(message);
            this.rightGestureIndicator.Receive(message);
            broadcast(message);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            UpdateMessage message = new UpdateMessage(gameTime);
             this.leftGestureIndicator.Receive(message);
            this.rightGestureIndicator.Receive(message);
            broadcast(message);
        }

        public override void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch sp, Microsoft.Xna.Framework.GameTime gameTime)
        {
            DrawMessage message = new DrawMessage(ref sp, gameTime);
            broadcast(message);

            if (this.isSlide == false)
            {
                this.Label.Draw(ref sp, gameTime);
               // this.bottomTextPanel.Draw(ref sp, gameTime);
                
            }

            if (this.isAutoPilotOn == false)
            {
                this.leftGestureIndicator.Draw(ref sp, gameTime);
                this.rightGestureIndicator.Draw(ref sp, gameTime);
            }

           // this.bottomTextPanel.Draw(ref sp, gameTime);
            if (this.bottomText != null)
            {
                this.bottomText.Draw(ref sp, gameTime);
            }

                //if (this.gestureIndicators.Count > 0)
                //{

                //    for (int i = 0; i < this.gestureIndicators.Count; i++)
                //    {
                //        sp.Draw(this.gestureIndicators.ElementAt(i), this.gestureRects.ElementAt(i).XNARectangle, Color.White);
                //    }
                //}

            
            
        }

       

        public override void Receive(Message<AbstractUI> message)
        {
            
            broadcast(message);
        }

        public void addReceiver(Receiver<AbstractUI> newReceiver)
        {
            this.elements.Add(newReceiver);
        }

        public void removeReceiver(Receiver<AbstractUI> receiver)
        {
            this.elements.Remove(receiver);
        }

        public void broadcast(Message<AbstractUI> message)
        {
            for (int i = 0; i < this.elements.Count; i++)
            {
                this.elements.ElementAt(i).Receive(message);
            }
        }

        public override void Reset()
        {
            base.Reset();


            broadcast(new ResetMessage());
        }

        public bool hasNext()
        {
            if (this.next != null) return true;
            return false;
        }

        public bool hasPrev()
        {
            if (this.prev != null) return true;
            return false;
        }

        public bool hasParent()
        {
            if (this.parent != null) return true;
            return false;
        }

        public UIScene getNext()
        {
            return this.next;
        }

        public UIScene getPrev()
        {
            return this.prev;
        }

        public UIScene getParent()
        {
            return this.parent;
        }

        public void setParent(UIScene obj)
        {
            this.parent = obj;
        }

        //@TODO change the lazy assignments
        public void setNext(UIScene obj)
        {
            this.next = obj;
          //  obj.setPrev(this);
            Console.WriteLine("SCENE " + this.UIID + " HAS  SCENE " + obj.UIID + " AS THE NEXT");
            
        }

        public void setPrev(UIScene obj)
        {
            this.prev = obj;
         //   obj.setNext(this);
            Console.WriteLine("SCENE " + this.UIID + " HAS SCENE " + obj.UIID + " AS THE PREV");
         
        }

        
    }
}

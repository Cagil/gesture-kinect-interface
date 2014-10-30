using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    public abstract class AbstractUI : Receiver<AbstractUI>
    {
        private int ID;
        private Driver driver;

        private Point position;
        private Point dimension;
        private Texture2D currentTexture;
        private String textureString;


        private Point fontPosition;
        private SpriteFont labelFont;

        //private String label;

        private UIText uitext;

        private static int instanceCount = 0;

        /**
         * @TODO
         * Debug to be change
         * 
         * */

        Color focusColor;
        Color idleColor;
        Color clickColor;

        private Color currentColor;

        private UIState.InteractionState interactionState;
        private UIState.VisibilityState visibilityState;

        private BoundingRectangle boundRectangle;

        private Command touchCommand;
        private Command focusCommand;
        private Dictionary<String, Command> gestureCommandCollection;


        public AbstractUI(Driver dr)
        {
            idleColor = Color.Green;
            focusColor = Color.Yellow;
            clickColor = Color.Black;

            currentColor = idleColor;

            this.touchCommand = null;
            this.focusCommand = null;
            this.gestureCommandCollection = new Dictionary<String, Command>();
            

            this.driver = dr;

            instanceCount++;
            this.ID = instanceCount;
        }

        public Point Position { get { return this.position; } set { this.position = value; } }
        public Point Dimension { get { return this.dimension; } set { this.dimension = value; } }
        public Texture2D Texture { get { return this.currentTexture; } set { this.currentTexture = value; } }
        public String MediaFilename { get { return this.textureString; } set { this.textureString = value; } }

        public UIState.InteractionState State { get { return this.interactionState; } }
        public UIState.VisibilityState Visibility { get { return this.visibilityState; } }

        public BoundingRectangle BoundingRectangle { get { return this.boundRectangle; } set { this.boundRectangle = value; } }

        public Command TouchCommand { get { return this.touchCommand; } set { this.touchCommand = value; } }
        public Command FocusCommand { get { return this.focusCommand; } set { this.focusCommand = value; } }

        public Driver Driver { get { return this.driver; } }

        public Color CurrentColor { get { return this.currentColor; } set { this.currentColor = value; } }

        public UIText Label { get { return this.uitext; } set { this.uitext = value; } }
        public int UIID { get { return this.ID; } }

        public SpriteFont Font { get { return this.labelFont; } set { this.labelFont = value; } }
        public Point FontPosition { get { return this.fontPosition; } set { this.fontPosition = value; } }
        public List<String> InterestedGestures
        {
            get
            {
                List<String> gestureNames = new List<String>();

                for (int i = 0; i < this.gestureCommandCollection.Count; i++)
                {
                    gestureNames.Add(this.gestureCommandCollection.ElementAt(i).Key);
                }

                return gestureNames;
            }
        }


        public abstract void LoadContent(ref ResourceManager resources, GraphicsDevice gd);
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(ref SpriteBatch sp, GameTime gameTime);

        public abstract void Receive(Message<AbstractUI> message);
        

        public virtual bool onFocus(Vector2 coords)
        {      
            if (this.boundRectangle.Intersect(coords))
            {
                if (this.State != UIState.InteractionState.FOCUSED)
                {
                 //   Console.WriteLine("in on focus " + this.Label.Text + " " + this.UIID);
                    changeInteractionState(UIState.InteractionState.FOCUSED);

                    if (this.focusCommand != null)
                    {
                        this.focusCommand.run();
                    }
                    return true;
                }
                //if (this is UIButton)
                //{
                //    Console.WriteLine("ON ELEMENT");
                //}
                return true;
            }
            else
            {
                
                if(this.State != UIState.InteractionState.IDLE)
                    changeInteractionState(UIState.InteractionState.IDLE);
                return false;
            }
        }

        public void onTouch(Vector2 coords)
        {
            
            if (this.boundRectangle.Intersect(coords))
            {
             //   Console.WriteLine("in on touch " + this.Label.Text + " " + this.UIID);
                if (this.State == UIState.InteractionState.FOCUSED)
                {
                    //@TODO check if touched or focused works.
                    changeInteractionState(UIState.InteractionState.TOUCHED);

                    
                    if (this.touchCommand != null)
                    {
                        
                        this.touchCommand.run();
                    }

                    
                }
                
            }
            else
            {
                changeInteractionState(UIState.InteractionState.IDLE);
            }
        }

        public void onGestureNoticed(String gestureName)
        {
            Console.WriteLine("in gesture noticed ABSTRACT UI");
            Command gestureCommand = null;
            this.gestureCommandCollection.TryGetValue(gestureName, out gestureCommand);

            if (gestureCommand != null)
            {
                gestureCommand.run();
            }
            else
            {
                //Console.WriteLine("Command NUILL  " + gestureName);
            }

        }

        private void changeInteractionState(UIState.InteractionState state)
        {
            switch (state)
            {
                case UIState.InteractionState.IDLE:
                    this.interactionState = UIState.InteractionState.IDLE;
                    this.currentColor = idleColor;
                    break;
                case UIState.InteractionState.FOCUSED:
                    this.interactionState = UIState.InteractionState.FOCUSED;
                    this.currentColor = focusColor;
                    break;
                case UIState.InteractionState.TOUCHED:
                    this.interactionState = UIState.InteractionState.TOUCHED;
                    this.currentColor = clickColor;
                    break;
                default:
                    this.interactionState = UIState.InteractionState.IDLE;
                    this.currentColor = idleColor;
                    break;
            }
        }

        public virtual void Reset()
        {
            changeInteractionState(UIState.InteractionState.IDLE);
        }

        public void setGestureCommand(String name, Command command)
        {
            this.gestureCommandCollection.Add(name, command);
        }
        
    }
}

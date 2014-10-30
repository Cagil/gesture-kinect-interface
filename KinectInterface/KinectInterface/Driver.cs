using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using KinectInterface.Utils;
using KinectInterface.UI;
using KinectInterface.KinectUtils.Commands;
using System.IO;
using KinectInterface.Models;
using KinectInterface.Commands;
using KinectInterface.Gestures;
using KinectInterface.Layouts;

namespace KinectInterface
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Driver : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GraphicsDevice gDevice;

        Texture2D texture;
        BoundingRectangle rectangle, r2;
        Color color;

        Point rectdim;
        Point dimratio;
        

        UIButton button, button2, button3;
        UIPanel panel, panel2;
        UIScene scene, scene2;

        Dictionary<int, UIScene> helpScenes;
        private UIVideo gest1, gest2, gest3;

        private Texture2D cursorTexture;
        private Rectangle cursorRectangle;
        private Color cursorColor;

        Dictionary<int,UIScene> tempScenes;
        List<Message<SceneManager>> sceneMessages;

        private Cursor Cursor;
        private IMediator Hub;
        private ResourceManager ResourceManager;
        private InputManager InputManager;
        private SceneManager SceneManager;
        private KinectManager KinectManager;
        private LayoutManager LayoutManager;
        private Parser Parser;
        private GestureFactory GestureFactory;

        private String contentLocation;

        Texture2D testSlide;
        Video testVideo;

        private UIText countdown;

        private Texture2D testboxbg;
        private Texture2D testboxfg;
        private Rectangle testboxbgrect;
        private Rectangle testboxfgrect;

        //PARSER REQUIRED FIELDS
        int categoryNum;
        List<String> categories;
        List<String[]> catFilenames;

        int MAIN_MENU_ID;
        private Texture2D cursorTex;
        Boolean init;
        Boolean isFullScreen;
        //SETTINGS PARAMS
        public Boolean IsFullScreen { set { this.isFullScreen = value; this.toggleFullScreen(); } get { return this.isFullScreen; } }
        //public Boolean IsMouseVisible { set { this.IsMouseVisible = value; } get { return this.IsMouseVisible; } }

        public Driver()
        {
            //graphics = new GraphicsDeviceManager(this);
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };
            Content.RootDirectory = "Content";
            //for Debug purposes
            this.IsMouseVisible = true;

            this.IsFullScreen = true;
            init = false;
           
            int cursorWidth = (int)(this.graphics.PreferredBackBufferWidth * 0.06f);
            int cursorHeight = (int)(this.graphics.PreferredBackBufferHeight * 0.08f);

            cursorWidth = 48;
            cursorHeight = 48;

            Cursor = new Cursor(cursorWidth, cursorHeight);

            ////////////////////////////////////////

            this.cursorColor = Color.Black;
            this.cursorRectangle = new Rectangle(0, 0, 24, 24);

            this.testboxbgrect = new Rectangle(0, 0, 10, this.graphics.PreferredBackBufferHeight);
            this.testboxfgrect = new Rectangle(0, 0, 10, this.graphics.PreferredBackBufferHeight);
            
            FileStream stream = new FileStream("C:\\Users\\Cagil\\Documents\\dissertation-kinect-interface\\KinectInterface\\KinectInterfaceContent\\Textures\\" + "hand.png", FileMode.Open);
            cursorTex = Texture2D.FromStream(this.GraphicsDevice, stream);
            /////////////////////////////////////////////////////////////////////////////////
            
            categoryNum = 0;
            categories = new List<String>();
            catFilenames = new List<String[]>();

            countdown = new UIText(this, "5");

            helpScenes = new Dictionary<int, UIScene>();
            gest1 = new UIVideo("swipeleftgestureprev", "HELP : SWIPE LEFT GESTURE", this); 
      //      gest1.IsLooping = true;
            gest2 = new UIVideo("swiperightgestureprev", "HELP : SWIPE RIGHT GESTURE", this);
         //   gest1.IsLooping = true;
            gest3 = new UIVideo("gobackgestureprev", "HELP : GO BACK GESTURE", this);
     //       gest3.IsLooping = true;
            

            contentLocation = @"C:\\Users\\Cagil\\Documents\\dissertation-kinect-interface\\KinectInterface\\KinectInterfaceContent\\Data\\";
            /**
             * Resource Manager will have List of cateogies String list
             * and List of String arrays
             * as well as Dictionary of Texture2D and Video ( String, Texture2D and String, Video)
             * resource manager will have a loadContent() method to load all the content into the Dictionaries.
             * resource manager will have a method to getImage and getVideo by the filename.
             **/
            //ResourceManager = new ResourceManager();
            //Parser.parseContent(String contentURL, out List<String>, out List<String[]>)
            //Parser.parseContentInfo(String contentURL, String filename) //text file for noww to prove the concept.
            //text file will include filename and title underneath and a description below that.

      
            
            //dynamic changing controling fullscreen with the current screen resolution
            //toggleFullScreen();
            Hub = new Mediator();
            Parser = new Parser(this.contentLocation);
            ParsedData categoryData = Parser.parseContent();
            ResourceManager = new ResourceManager(this.graphics.GraphicsDevice, this.contentLocation);
            
            GestureFactory = new Gestures.GestureFactory();
            Hub.registerGesturePool(ref GestureFactory);
            ResourceManager.CategoryModel = categoryData.Categories;
            //ResourceManager.LoadContent();


            LayoutManager = new LayoutManager(this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            LayoutParams mainMenulparams = new LayoutParams(new Vector2(0.95f, 0.92f));
            mainMenulparams.setMargin(0.10f, 0.08f, 0.10f, 0.07f);

            LayoutParams categoryMenulparams = new LayoutParams(new Vector2(0.95f, 0.92f));
            categoryMenulparams.setMargin(0.10f, 0.08f, 0.10f, 0.07f);

            LayoutParams helpLayoutParams = new LayoutParams(new Vector2(0.95f, 0.92f));
            helpLayoutParams.setMargin(0.05f, 0.10f, 0.05f, 0.10f);

           // LayoutManager.MainMenuLayout = new GridLayout(3, 1, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), mainMenulparams);
           // LayoutManager.MainMenuLayout = new GridLayout(1, 2, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), mainMenulparams);

            LayoutManager.MainMenuLayout = new GridLayout(2, 2, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), mainMenulparams);
            LayoutManager.CategoryMenuLayout = new GridLayout(3, 2, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), categoryMenulparams);
            LayoutManager.HelpMenuLayout = new GridLayout(1, 2, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), helpLayoutParams);

            sceneMessages = new List<Message<SceneManager>>();

            InputManager = new InputManager(this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            Hub.registerInputManager(ref InputManager);
            InputManager.addKeybind(Keys.Escape, new ExitProgramCommand(this));
            //InputManager.ExitKey = Keys.Escape;
            //InputManager.ExitKeyCommand = new ExitProgramCommand(this);
            InputManager.addKeybind(Keys.F, new FullScreenToggleCommand(this));
            //InputManager.FullscreenKey = Keys.F;
            //InputManager.FullscreenKeyCommand = new FullScreenToggleCommand(this);
            InputManager.CursorHitPointOffset = this.Cursor.GetOffSet(111, 11, 256, 256); // from left top, @TODO change this read from settings folder


            KinectManager =KinectManager.Instance;
            KinectManager.setScreenDim(this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            Hub.registerKinectManager(ref KinectManager);

           // WaveGesture waveGesture = new WaveGesture();
           // waveGesture.SucceedCommand = new ExitProgramCommand(this);
            List<Gesture> gestList = new List<Gesture>();

            
            //InputManager.KinectManager = KinectManager.Instance;
            SceneManager = new SceneManager();
            Hub.registerSceneManager(ref SceneManager);


            //tempScenes = createSceneCollectionFromContentNew();
            tempScenes = createScenes();
           
            SceneManager.Scenes = tempScenes;
            SceneManager.HelpScenes = helpScenes;
            
            GestureFactory.addNewSegment(new SwipeLeftGestureSegment1());
            GestureFactory.addNewSegment(new SwipeLeftGestureSegment2());
            GestureFactory.MakeGesture("SWIPE_LEFT_GESTURE", 0.20f, null);

            GestureFactory.addNewSegment(new SwipeRightGestureSegment1());
            GestureFactory.addNewSegment(new SwipeRightGestureSegment2());
            GestureFactory.MakeGesture("SWIPE_RIGHT_GESTURE", 0.24f, null);

            GestureFactory.addNewSegment(new GoBackGestureSegment1());
            GestureFactory.addNewSegment(new GoBackGestureSegment2());
            GestureFactory.MakeGesture("GO_BACK_GESTURE", 0.24f, null);

            //GestureFactory.addNewSegment(new WaveGestureSegment1());
            //GestureFactory.addNewSegment(new WaveGestureSegment2());
            //GestureFactory.addNewSegment(new WaveGestureSegment1());
            //GestureFactory.addNewSegment(new WaveGestureSegment2());
            //GestureFactory.MakeGesture("WAVE_GESTURE", 0.20f, null);

           // SwipeLeftGesture swipeLeftGesture = new SwipeLeftGesture();
           // swipeLeftGesture.SucceedCommand = new SceneChangeCommand(ref this.SceneManager, tempScenes.ElementAt(2).Value.UIID);
          //  gestList.Add(swipeLeftGesture);
            //InputManager.addGestureList(gestList);



            //rectdim = new Point((int)(graphics.PreferredBackBufferWidth * 0.16f), (int)(graphics.PreferredBackBufferHeight * 0.10f));

            rectangle = new BoundingRectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            

            SceneManager.MainMenuSceneId = MAIN_MENU_ID;
            SceneManager.AutoPilot = false;

            Console.WriteLine("CONSTRUCT");
            InputManager.addReceiver(SceneManager);
            init = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //screen vs kinect skeleton tracking approximation
            //graphics.PreferredBackBufferHeight = 480;
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            //texture = new Texture2D(GraphicsDevice, (int) rectdim.X, (int)rectdim.Y);

          //  testSlide = ResourceManager.getTexture("Events_slide11.png");
            for (int i = 0; i < helpScenes.Count; i++)
            {
                helpScenes.ElementAt(i).Value.Initialize();
            }


            for (int i = 0; i < tempScenes.Count; i++)
            {
                tempScenes.ElementAt(i).Value.Initialize();
            }

            Console.WriteLine("INIT");
           // button.Initialize();
            //color = Color.Black;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Console.WriteLine("LOADCONTENT");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);

          //  this.cursorTexture = new Texture2D(this.GraphicsDevice, 1, 1);
          //  this.cursorTexture.SetData(new[] { this.cursorColor });
            this.cursorTexture = new Texture2D(this.GraphicsDevice, 10, 10);
            this.cursorTexture = Content.Load<Texture2D>("hand_pointer");
            //this.cursorTexture.SetData(

            this.testboxbg = new Texture2D(this.GraphicsDevice, 1, 1);
            this.testboxbg.SetData(new[] { Color.Brown });

            this.testboxfg = new Texture2D(this.GraphicsDevice, 1, 1);
            this.testboxfg.SetData(new[] { Color.Yellow });
            
            //this.cursorTexture = c;
            countdown.LoadContent(ref this.ResourceManager, this.GraphicsDevice);

            Cursor.LoadContent(this.GraphicsDevice);

            InputManager.LoadContent(this);
            KinectManager.loadKinectResources(this.graphics.GraphicsDevice);

            for (int i = 0; i < helpScenes.Count; i++)
            {
                helpScenes.ElementAt(i).Value.LoadContent(ref this.ResourceManager, this.GraphicsDevice);
            }

          //  this.testVideo = Content.Load<Video>("video");
            for (int i = 0; i < tempScenes.Count; i++)
            {
                tempScenes.ElementAt(i).Value.LoadContent(ref this.ResourceManager, this.GraphicsDevice);
            }
           // SceneManager.CurrentScene.LoadContent();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            ///// DEBUG KEYBINDS
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.R))
            {
                SceneManager.jumpTo(MAIN_MENU_ID);
            }

            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.B))
            {
                SceneManager.changeToPrev();
            }
            


            
            KinectManager.update(gameTime);

            InputManager.Update(gameTime);
            SceneManager.Update(gameTime);
            this.Cursor.Update(gameTime);

            ////////////////////////
        //    this.testboxfgrect = new Rectangle(0, 0, 10, (int)(this.graphics.PreferredBackBufferHeight * InputManager.getZChange()));
         //   countdown.Text = (1000 - InputManager.PushTime)/1000.0f + "";
         //   this.cursorRectangle = InputManager.getCursorRectangle(32, 32);
            this.Cursor.UpdatePosition(InputManager.getCursorRectangle());
            this.Cursor.PushIndicatorValue = InputManager.getZChange();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Transparent, 0, 0);
            this.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
           
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            SceneManager.CurrentScene.Draw(ref spriteBatch, gameTime);
            //KinectManager.draw(spriteBatch);
            //InputManager.Draw(ref spriteBatch, gameTime);

           // cursor.Draw(spriteBatch, gameTime);
          //  spriteBatch.Draw(cursorTexture, cursorRectangle, Color.Green);
          //  spriteBatch.Draw(cursorTexture, this.InputManager.getLastCoord(), Color.Orange);
            KinectManager.DrawDepthImage(spriteBatch,
                this.GraphicsDevice,
                new Rectangle(
                    this.graphics.PreferredBackBufferWidth - 80,
                    0,
                    80, 60));

            spriteBatch.End();
            Cursor.Draw(spriteBatch, gameTime, this.graphics);

         //   spriteBatch.Draw(this.testboxbg, this.testboxbgrect, Color.Brown);
         //   spriteBatch.Draw(this.testboxfg, this.testboxfgrect, Color.Yellow);

            //countdown.Draw(ref spriteBatch, gameTime);
            
            base.Draw(gameTime);
        }

        private void toggleFullScreen()
        {
            if (isFullScreen == true)
            {
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; // 480
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; // 800
                graphics.IsFullScreen = true;
            }
            else
            {

                graphics.PreferredBackBufferHeight = 768;//480;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2; // 480
                graphics.PreferredBackBufferWidth = 1366;//800;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2; // 800
                graphics.IsFullScreen = false;
            }

            if (init == true)
            {
                rectdim = new Point((int)(graphics.PreferredBackBufferWidth * 0.16f), (int)(graphics.PreferredBackBufferHeight * 0.10f));
            //    button.Dimension = new Point((int)rectdim.X, (int)rectdim.Y);
            }

            graphics.ApplyChanges();
        }

        private Dictionary<int, UIScene> createScenes()
        {
            Dictionary<int, UIScene> SceneSet = new Dictionary<int, UIScene>();
            //Dictionary of Scenes by Category name
            //Dictionary of Scenes by Category name + filename
            //List of MainScenes
            //List of CategoryScenes
            //List of CategoryMediaScenes

            Dictionary<String, UIScene> scenesByCategory = new Dictionary<String, UIScene>();
            Dictionary<String, UIScene> scenesByFilename = new Dictionary<String, UIScene>();
            List<UIScene> mainMenuScenes = new List<UIScene>();

            
            List<UIScene> categoryScenes = new List<UIScene>();
            List<UIScene> mediaScenes = new List<UIScene>();
            List<UIScene> videoScenes = new List<UIScene>();
            

            //Resource manager has Category models
            //    Category Model has 
            //name of the category, 
            //list of filenames associated


            //create Help Scenes aka Tutorial
            UIScene help1 = new UIScene(this, "HELP : SWIPE LEFT GESTURE");
            UIScene help2 = new UIScene(this, "HELP : SWIPE RIGHT GESTURE");
            UIScene help3 = new UIScene(this, "HELP : GO BACK GESTURE");
            UIScene help4 = new UIScene(this, "HELP : MAKING SELECTION");

            help1.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            help2.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            help3.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            help4.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);

            help1.setNext(help2);
            help2.setPrev(help1);     

            help2.setNext(help3);
            help3.setPrev(help2);

            help3.setNext(help4);
            help4.setPrev(help3);

            BoundingRectangle hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();
            int xspace = (int)(this.graphics.PreferredBackBufferWidth * 0.05f);
            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace*2, hmrec.Height);

           

            UIDiscMedia videoDisc = new UIDiscMedia(this, hmrec, 0.66f);
           // videoDisc.Media = gest1;
            videoDisc.Media = new UIImage(this);
            videoDisc.Media.MediaFilename = "arrow_left.png";
            videoDisc.Discription = new UIText(this, "The icon indicates SWIPE LEFT GESTURE is available to be performed.");
            videoDisc.Discription.TextColor = Color.Black;
            //gest1.BoundingRectangle = hmrec;

            UIPanel u1 = new UIPanel(this);
            u1.addReceiver(videoDisc);
            help1.addReceiver(u1);

            hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();
           
            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace * 2, hmrec.Height);
            UIDiscMedia iconDisc = new UIDiscMedia(this, hmrec, 0.66f);
            iconDisc.Media = gest1;
            iconDisc.Discription = new UIText(this, "Swipe your right hand from right to left to perform SWIPE LEFT GESTURE as shown.");
            iconDisc.Discription.TextColor = Color.Black;

            UIPanel u11 = new UIPanel(this);
            u11.addReceiver(iconDisc);
            help1.addReceiver(u11);

            help1.BottomTextRectangle = LayoutManager.HelpMenuLayout.getBottomTextRect();
            help1.BottomText = new UIText(this, "<Swipe your right hand as shown to ADVANCE or Lift your right hand up above your head to SKIP>");
            help1.TopTextRectangle = LayoutManager.HelpMenuLayout.getTopTextRect();

            //////////////////////////
            LayoutManager.HelpMenuLayout.reset();

            hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();
            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace * 2, hmrec.Height);
            videoDisc = new UIDiscMedia(this, hmrec, 0.66f);
            // videoDisc.Media = gest1;
            videoDisc.Media = new UIImage(this);
            videoDisc.Media.MediaFilename = "arrow_right.png";
            videoDisc.Discription = new UIText(this, "The icon indicates SWIPE RIGHT GESTURE is available to be performed.");
            videoDisc.Discription.TextColor = Color.Black;

            //UIPanel u1 = new UIPanel(this);
            //u1.addReceiver(videoDisc);
            //help1.addReceiver(u1);

           // gest2.BoundingRectangle = hmrec;
            UIPanel u2 = new UIPanel(this);
            u2.addReceiver(videoDisc);
            help2.addReceiver(u2);

            hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();

            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace * 2, hmrec.Height);
            iconDisc = new UIDiscMedia(this, hmrec, 0.66f);
            iconDisc.Media = gest2;
            iconDisc.Discription = new UIText(this, "Swipe your left hand from left to right to perform SWIPE RIGHT GESTURE as shown.");
            iconDisc.Discription.TextColor = Color.Black;

            UIPanel u22 = new UIPanel(this);
            u22.addReceiver(iconDisc);
            help2.addReceiver(u22);

            help2.BottomTextRectangle = LayoutManager.HelpMenuLayout.getBottomTextRect();
            help2.BottomText = new UIText(this, "<Swipe your right hand to ADVANCE, Swipe your left hand to GO BACK or Lift your right hand up above your head to SKIP>");
            help2.TopTextRectangle = LayoutManager.HelpMenuLayout.getTopTextRect();

            ////////////////////////////
            LayoutManager.HelpMenuLayout.reset();

            //hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();
            //hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width, hmrec.Height);
            //UIPanel u3 = new UIPanel(this);
            //   UIImage icon = new UIImage(this);
            //icon.MediaFilename = "arrow_up.png";
            //icon.BoundingRectangle = hmrec;
            //u3.addReceiver(icon);

            hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();
            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace * 2, hmrec.Height);
            videoDisc = new UIDiscMedia(this, hmrec, 0.66f);
            // videoDisc.Media = gest1;
            videoDisc.Media = new UIImage(this);
            videoDisc.Media.MediaFilename = "arrow_up.png";
            videoDisc.Discription = new UIText(this, "The icon indicates GO BACK GESTURE is available to be performed.");
            videoDisc.Discription.TextColor = Color.Black;

            UIPanel u3 = new UIPanel(this);
            u3.addReceiver(videoDisc);
            help3.addReceiver(u3);


            hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();

            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace * 2, hmrec.Height);
            iconDisc = new UIDiscMedia(this, hmrec, 0.66f);
            iconDisc.Media = gest3;
            iconDisc.Discription = new UIText(this, "Lift your right hand from hip level to up above your head to perform GO BACK GESTURE as shown.");
            iconDisc.Discription.TextColor = Color.Black;

            UIPanel u33 = new UIPanel(this);
            u33.addReceiver(iconDisc);
            help3.addReceiver(u33);

            help3.BottomTextRectangle = LayoutManager.HelpMenuLayout.getBottomTextRect();
            help3.BottomText = new UIText(this, "<Swipe your left hand for previous screen, Swipe your right hand for next screen or Lift your right hand up above your head to SKIP/CLOSE HELP SECTION>");
            help3.TopTextRectangle = LayoutManager.HelpMenuLayout.getTopTextRect();


            ////////////////////////////
            LayoutManager.HelpMenuLayout.reset();
            hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();

            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace * 2, hmrec.Height);
            iconDisc = new UIDiscMedia(this, hmrec, 0.66f);
            iconDisc.Media = new UIImage(this);
            iconDisc.Media.MediaFilename = "hold_still.png";
            iconDisc.Discription = new UIText(this, "This icon indicates you hold your right hand on buttons on the screen to make a selection.");
            iconDisc.Discription.TextColor = Color.Black;



            UIPanel u4 = new UIPanel(this);
            u4.addReceiver(iconDisc);
            help4.addReceiver(u4);

            hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();

            hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width - xspace * 2, hmrec.Height);
            iconDisc = new UIDiscMedia(this, hmrec, 0.66f);
            iconDisc.Media = new UIButton(iconDisc.MediaContainer.Position.X, iconDisc.MediaContainer.Position.Y, iconDisc.MediaContainer.Width, iconDisc.MediaContainer.Height, "", this, "Move cursor here and hold still");
            iconDisc.Discription = new UIText(this, "Try interacting with the button above, it will turn black when you made the selection.");
            iconDisc.Discription.TextColor = Color.Black;

            UIPanel u44 = new UIPanel(this);
            u44.addReceiver(iconDisc);
            help4.addReceiver(u44);

            help4.BottomTextRectangle = LayoutManager.HelpMenuLayout.getBottomTextRect();
            help4.BottomText = new UIText(this, "<Swipe your left hand for previous screen or Lift your right hand up above your head to SKIP/CLOSE HELP SECTION>");
            help4.TopTextRectangle = LayoutManager.HelpMenuLayout.getTopTextRect();

            //hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();
            //UIText videoText = new UIText(this, "You can swipe your Right hand from right to left, when you see this icon.");
            //videoText.BoundingRectangle = hmrec;
            //videoText.PlaceAndAlign(videoText.BoundingRectangle);
            //u3.addReceiver(videoText);

            //hmrec = LayoutManager.HelpMenuLayout.getNextElementPos();
            //hmrec = new BoundingRectangle(hmrec.Position.X + xspace, hmrec.Position.Y, hmrec.Width, hmrec.Height);
            //   gest3.BoundingRectangle = hmrec;
            
            //u3.addReceiver(gest3);


            //help3.addReceiver(u3);


            ////////////////////////
            // Hardcoding videos for extras category
            ////////////////////////////////////////

            for(int g = 0; g < 1; g++){
                String fn = "movie" + (g+1);
            UIScene v1 = new UIScene(this);
            UIPanel vpanel = new UIPanel(this);
            UIVideo video = new UIVideo(fn, fn, this);
            vpanel.addReceiver(video);
            v1.addReceiver(vpanel);
            v1.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
            videoScenes.Add(v1);
            }


            ////////////////////////
         

            


            //create Media Scenes
            //    - 	create Scenes and add to the Dictionary.
            //    -	add Go back gesture
            for (int i = 0; i < ResourceManager.CategoryModel.Count; i++)
            {
                KinectInterface.Models.Category currentCategory = ResourceManager.CategoryModel.ElementAt(i);
                for (int j = 0; j < currentCategory.Filenames.Count; j++)
                {
                    UIScene mediaScene = new UIScene(this, currentCategory.Filenames[j] + "_SLIDE_SCENE");
                    mediaScene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                    UISlide slide = new UISlide(currentCategory.Name + "_" + currentCategory.Filenames[j], "SLIDE", this);
                    slide.Texture = ResourceManager.getTexture(currentCategory.Name, currentCategory.Filenames[j]);
                    UIPanel mediaPanel = new UIPanel(this);
                    mediaPanel.addReceiver(slide);
                    mediaScene.IsASlide = true;
                    mediaScene.addReceiver(mediaPanel);
                    mediaScene.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));

                    scenesByFilename.Add(currentCategory.Name + "_" + currentCategory.Filenames[j], mediaScene);

                    //UIScene slideScene = new UIScene(this, currentCategory.Filenames[j] + "_SLIDE_SCENE");
                    //slideScene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                    //UISlide slidE = new UISlide(currentCategory.Name + "_" + currentCategory.Filenames[j], "SLIDE", this);
                    //slidE.Texture = ResourceManager.getTexture(currentCategory.Name, currentCategory.Filenames[j]);
                    ////UIPanel mediaPanel2 = new UIPanel(this);
                    //slideScene.IsASlide = true;
                    //slideScene.addReceiver(slidE);
                    //slides.Add(slideScene);
                }
                //ADDING MOVIES MANUALLY..
                if (currentCategory.Name == "Extras")
                {
                    for (int g = 0; g < 5; g++)
                    {
                        String fn = "movie" + (g + 1);
                        currentCategory.Filenames.Add(fn);
                        
                        UIScene v1 = new UIScene(this);
                        v1.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                        //v1.IsASlide = true;
                        UIPanel vpanel = new UIPanel(this);
                        UIVideo video = new UIVideo(fn, fn, this);
                        vpanel.addReceiver(video);
                        v1.addReceiver(vpanel);
                        v1.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
                        //videoScenes.Add(v1);
                        scenesByFilename.Add(currentCategory.Name + "_" + fn, v1);


                    }

                }
            }

            //create Category Scenes
            //    - 	go through category models
            //    -	use grid layout to create buttons
            //    -	link buttons to the associated media scenes using category name + file
            //    -	if it fits into 1 screen create one scene else more
            //    -	add scenes into a list 
            //    - 	go through the list and if there are more than one scene, add navigation 

            //int helpWidth = (int)(this.graphics.PreferredBackBufferWidth * 0.15f);
            //int helpHeight = (int)(this.graphics.PreferredBackBufferHeight * 0.10f);
            BoundingRectangle helpRect = LayoutManager.CategoryMenuLayout.getHelpTextRect();

            UIButton helpButton = new UIButton(helpRect.Position.X, helpRect.Position.Y, helpRect.Width, helpRect.Height, "", this, "HELP");
            helpButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, help1.UIID);
            
            for (int i = 0; i < ResourceManager.CategoryModel.Count; i++)
            {
                KinectInterface.Models.Category currentCategory = ResourceManager.CategoryModel.ElementAt(i);
                List<AbstractUI> categoryElements = new List<AbstractUI>();
                LayoutManager.CategoryMenuLayout.reset();
                BoundingRectangle cmrec = LayoutManager.CategoryMenuLayout.getNextElementPos();
                for (int j = 0; j < currentCategory.Filenames.Count; j++)
                {
                    if (cmrec != null)
                    {
                        UIButton catFileButton = new UIButton(cmrec.Position.X, cmrec.Position.Y, cmrec.Width, cmrec.Height, "", this, currentCategory.Filenames[j]);
                        //Console.WriteLine("CREATED AT :: " + catFileButton.Position);
                        //Console.WriteLine("DIM :: " + catFileButton.Dimension);
                     //   Console.WriteLine("X == " + cmrec.Position.X + "  Y == " + cmrec.Position.Y);
                     //   Console.WriteLine("W == " + cmrec.Width + "   H == " + cmrec.Height);

                        UIScene associatedScene = null;
                        scenesByFilename.TryGetValue(currentCategory.Name + "_" + currentCategory.Filenames[j], out associatedScene);

                        if(associatedScene != null)
                            catFileButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, associatedScene.UIID);

                        //processedScenes.Add(mediaScene);

                        categoryElements.Add(catFileButton);
                    }


                    cmrec = LayoutManager.CategoryMenuLayout.getNextElementPos();
                    if (j + 1 == ResourceManager.CategoryModel[i].Filenames.Count || cmrec == null)
                    {
                        categoryElements.Add(helpButton);
                        UIScene s = new UIScene(this, currentCategory.Name);
                        Console.WriteLine("NEW SCENE ID == " + s.UIID);
                       // prevcatButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s.UIID);
                        UIPanel p = new UIPanel(this, categoryElements);
                        s.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                        Console.WriteLine("CREATING CATEGORY SCENE " + currentCategory.Name);
                        s.addReceiver(p);
                        //currentCatScenes.Add(s);
                        categoryScenes.Add(s);
                        categoryElements = new List<AbstractUI>();
                        LayoutManager.CategoryMenuLayout.reset();
                        if (cmrec == null)
                            cmrec = LayoutManager.CategoryMenuLayout.getNextElementPos();
                    }


                }
                //@TODO add gesture images
                for (int g = 0; g < categoryScenes.Count; g++)
                {

                    for (int h = 0; h < categoryScenes.ElementAt(g).InterestedGestures.Count; h++)
                    {
                        String currGestureName = categoryScenes.ElementAt(g).InterestedGestures.ElementAt(h);
                        if (currGestureName == "GO_BACK_GESTURE")
                        {
                            Console.WriteLine("GESTURE INDI FOUND " + currGestureName);
                        }
                        else if (currGestureName == "SWIPE_LEFT_GESTURE")
                        {
                        //    categoryScenes.ElementAt(g)
                            Console.WriteLine("GESTURE INDI FOUND " + currGestureName);
                        }
                        else if (currGestureName == "SWIPE_RIGHT_GESTURE")
                        {
                            categoryScenes.ElementAt(g).gestureIndicators.Add(ResourceManager.loadTextureOnce(this.GraphicsDevice, "arrow_right.png"));
                            Console.WriteLine("GESTURE INDI FOUND " + currGestureName);
                        }
                    }
                    
                }

                //process category scenes and link them
                if (categoryScenes.Count > 1)
                {
                    String bottomTextSuffix = " of " + categoryScenes.Count;
                    for (int k = 0; k < categoryScenes.Count; k++)
                    {
                        categoryScenes.ElementAt(k).BottomText = new UIText(this, "Page " + (k + 1) + bottomTextSuffix);
                        categoryScenes.ElementAt(k).BottomTextRectangle = LayoutManager.CategoryMenuLayout.getBottomTextRect();

                    }

                    UIScene pS = categoryScenes.First();
                    for (int k = 1; k < categoryScenes.Count; k++)
                    {


                        categoryScenes.ElementAt(k - 1).setNext(categoryScenes.ElementAt(k));
                        categoryScenes.ElementAt(k).setPrev(categoryScenes.ElementAt(k - 1));

                      //  categoryScenes.ElementAt(k).gestureIndicators.Add(ResourceManager.loadTextureOnce(this.GraphicsDevice, "arrow_right.png"));
                      //  categoryScenes.ElementAt(k-1).gestureIndicators.Add(ResourceManager.loadTextureOnce(this.GraphicsDevice, "arrow_left.png"));
                        categoryScenes.ElementAt(k).setGestureCommand("SWIPE_RIGHT_GESTURE", new SceneChangeCommand(ref this.SceneManager, categoryScenes.ElementAt(k - 1).UIID));
                        categoryScenes.ElementAt(k - 1).setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, categoryScenes.ElementAt(k).UIID));
                        /////
                     //   categoryScenes.ElementAt(k - 1).setNext(categoryScenes.ElementAt(k));
                    //    categoryScenes.ElementAt(k).setPrev(categoryScenes.ElementAt(k - 1));

                        //@TODO LOOK INTO THIS
                        //categoryScenes.ElementAt(k-1).gestureIndicators.Add(ResourceManager.loadTextureOnce(this.GraphicsDevice, "arrow_right.png"));
                        categoryScenes.ElementAt(k - 1).setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
                    //    categoryScenes.ElementAt(k).setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, categoryScenes.ElementAt(k - 1).UIID));
                        //nextScene.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, s.UIID));
                        SceneSet.Add(categoryScenes.ElementAt(k - 1).UIID, categoryScenes.ElementAt(k - 1));
                        if (k + 1 >= categoryScenes.Count)
                        {
                            categoryScenes.ElementAt(k).setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
                            SceneSet.Add(categoryScenes.ElementAt(k).UIID, categoryScenes.ElementAt(k));
                        }
                        //pS = currentCatScenes.ElementAt(k);
                    }
                    scenesByCategory.Add(currentCategory.Name, pS);
                }
                else
                {
                    //for (int k = 0; k < categoryScenes.Count; k++)
                    //{
                    //    categoryScenes.ElementAt(k).setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
                    //    processedScenes.Add(categoryScenes.ElementAt(k));
                    //}
                    if (categoryScenes.Count > 0)
                    {
                        UIScene pS = categoryScenes.First();
                        pS.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
                        scenesByCategory.Add(currentCategory.Name, pS);
                        SceneSet.Add(pS.UIID, pS);
                    }
                }
                categoryScenes = new List<UIScene>();



            }

        //    MainMenuScene Creation	
        //-	needs category names to create buttons
        //-	if there is more than one menu link them
        //- 	add any neccessary ui elements
            helpRect = LayoutManager.MainMenuLayout.getHelpTextRect();

            helpButton = new UIButton(helpRect.Position.X, helpRect.Position.Y, helpRect.Width, helpRect.Height, "", this, "HELP");
            helpButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, help1.UIID);


            Console.WriteLine("!!!!!!!!!!MAIN MENU CREATION");
            List<AbstractUI> elements = new List<AbstractUI>();
            BoundingRectangle mmrec = LayoutManager.MainMenuLayout.getNextElementPos();
            for (int i = 0; i < ResourceManager.CategoryModel.Count; i++)
            {
                KinectInterface.Models.Category currentCategory = ResourceManager.CategoryModel.ElementAt(i);
                
                if (mmrec != null)
                {
                    Console.WriteLine(currentCategory.Name);
                    Console.WriteLine("X == " + mmrec.Position.X + "  Y == " + mmrec.Position.Y);
                    Console.WriteLine("W == " + mmrec.Width + "   H == " + mmrec.Height);
                    UIScene associatedScene = null;
                    UIButton button = new UIButton(mmrec.Position.X, mmrec.Position.Y, mmrec.Width, mmrec.Height, "", this, currentCategory.Name);

                    Console.WriteLine("LOOKING FOR CATEGORY SCENE :: " + currentCategory.Name);
                    scenesByCategory.TryGetValue(currentCategory.Name, out associatedScene);
                    if (associatedScene != null)
                    {
                        Console.WriteLine("FOUND!!! = " + associatedScene.Label.Text + "  " + associatedScene.UIID);
                        button.TouchCommand = new SceneChangeCommand(ref this.SceneManager, associatedScene.UIID);
                    }
                    else
                    {
                        for (int g = 0; g < scenesByCategory.Count; g++)
                        {
                            Console.WriteLine("AVAILABLE CATEGORY SCENE ::: " + scenesByCategory.ElementAt(g).Key);
                        }
                    }

                    
                    elements.Add(button);
                    Console.WriteLine("ADDED " + currentCategory.Name + "  Button ");
                }

                mmrec = LayoutManager.MainMenuLayout.getNextElementPos();
                if (i + 1 == ResourceManager.CategoryModel.Count || mmrec == null)
                {
                    elements.Add(helpButton);
                    UIScene s = new UIScene(this, "Categories");
                    //elements.First().TouchCommand = new SceneChangeCommand(ref this.SceneManager, 11);
                    UIPanel p = new UIPanel(this, elements);


                    s.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                    Console.WriteLine("MAIN MENU SCENE CREATED!!!");
                    s.addReceiver(p);
                    mainMenuScenes.Add(s);

                    //SceneSet.Add(s.UIID, s);
                    elements = new List<AbstractUI>();

                    LayoutManager.MainMenuLayout.reset();
                    if (mmrec == null)
                        mmrec = LayoutManager.MainMenuLayout.getNextElementPos();
                }
            }

            //process and link main menu scenes , add gestures
            if (mainMenuScenes.Count > 1)
            {


                
                String bottomTextSuffix = " of " + mainMenuScenes.Count;
                for (int k = 0; k < mainMenuScenes.Count; k++)
                {
                    mainMenuScenes.ElementAt(k).BottomText = new UIText(this, "Page " + (k + 1) + bottomTextSuffix);
                    mainMenuScenes.ElementAt(k).BottomTextRectangle = LayoutManager.MainMenuLayout.getBottomTextRect();

                }

                UIScene pScene = mainMenuScenes.First();
                
                MAIN_MENU_ID = mainMenuScenes.First().UIID;
                for (int k = 1; k < mainMenuScenes.Count; k++)
                {

                    mainMenuScenes.ElementAt(k - 1).setNext(mainMenuScenes.ElementAt(k));
                    mainMenuScenes.ElementAt(k).setPrev(mainMenuScenes.ElementAt(k - 1));

                    mainMenuScenes.ElementAt(k).setGestureCommand("SWIPE_RIGHT_GESTURE", new SceneChangeCommand(ref this.SceneManager, mainMenuScenes.ElementAt(k - 1).UIID));
                    mainMenuScenes.ElementAt(k - 1).setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, mainMenuScenes.ElementAt(k).UIID));


                    //nextScene.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, s.UIID));
                    SceneSet.Add(mainMenuScenes.ElementAt(k - 1).UIID, mainMenuScenes.ElementAt(k - 1));
                    if (k + 1 >= mainMenuScenes.Count) SceneSet.Add(mainMenuScenes.ElementAt(k).UIID, mainMenuScenes.ElementAt(k));
                    //pS = currentCatScenes.ElementAt(k);
                }
            }
            else
            {
                //for (int k = 0; k < currentCatScenes.Count; k++)
                //{
                //    processedScenes.Add(currentCatScenes.ElementAt(k));
                //}
                if(mainMenuScenes.Count > 0)
                    SceneSet.Add(mainMenuScenes.First().UIID, mainMenuScenes.First());

            }

            for (int i = 0; i < scenesByFilename.Count; i++)
            {
                SceneSet.Add(scenesByFilename.ElementAt(i).Value.UIID, scenesByFilename.ElementAt(i).Value);
            }


            help1.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, help2.UIID));
            help1.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, MAIN_MENU_ID));
            //help1.setGestureCommand("GO_BACK_GESTURE", new SkipHelpScreenCommand(ref this.SceneManager));

            help2.setGestureCommand("SWIPE_RIGHT_GESTURE", new SceneChangeCommand(ref this.SceneManager, help1.UIID));
            help2.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, MAIN_MENU_ID));
           // help2.setGestureCommand("GO_BACK_GESTURE", new SkipHelpScreenCommand(ref this.SceneManager));

            help2.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, help3.UIID));
            help3.setGestureCommand("SWIPE_RIGHT_GESTURE", new SceneChangeCommand(ref this.SceneManager, help2.UIID));
            help3.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, MAIN_MENU_ID));
           // help3.setGestureCommand("GO_BACK_GESTURE", new SkipHelpScreenCommand(ref this.SceneManager));

            help3.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, help4.UIID));
            help4.setGestureCommand("SWIPE_RIGHT_GESTURE", new SceneChangeCommand(ref this.SceneManager, help3.UIID));
            help4.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, MAIN_MENU_ID));
            //help4.setGestureCommand("GO_BACK_GESTURE", new SkipHelpScreenCommand(ref this.SceneManager));


            helpScenes.Add(help1.UIID, help1);
            helpScenes.Add(help2.UIID, help2);
            helpScenes.Add(help3.UIID, help3);
            helpScenes.Add(help4.UIID, help4);


            Console.WriteLine(SceneSet.Count + " scenes created!!");
            
            return SceneSet;
        }

        private Dictionary<int, UIScene> createSceneCollectionFromContentNew()
        {
            // Scene x file_nums
            // Scene x categories_num
            // main menu Scene

            // 0 load images/videos                 filenames
            // 1 create UI objects for media        filesnames
            // 2 create UIPanels for media          filenames + Panel
            // 3 create Scenes for UIPanels         filenames + Scene
            // 4 create buttons that leads to media scenes      filename + Button
            // 5 create panels for buttons in STEP 4            media panel + category
            // 6 create scene for Panels in STEP 5              category_name + Scene
            // 7 create category buttons                        category name
            // 8 create Panel for STEP 6                        Main menu panel
            // 9 create MainScene for CategoryPanel             Main Scene
            Dictionary<int, UIScene> SceneSet = new Dictionary<int, UIScene>();
            List<UIScene> mediaFileScenes = new List<UIScene>();
            List<UIScene> mainmenuScenes = new List<UIScene>();
            List<UIScene> categoryScenes = new List<UIScene>();
            List<UIScene> currentCatScenes = new List<UIScene>();
            List<UIScene> processedScenes = new List<UIScene>();

            List<AbstractUI> elements = new List<AbstractUI>();
            List<AbstractUI> categoryElements = new List<AbstractUI>();

            //UIPanel p = null;
            UIScene mainmenuscene;
            UIScene mainmenuscene2 = new UIScene(this);
            UIButton prevcatButton = null;
            UIButton nextcatButton;

            BoundingRectangle mmrec = LayoutManager.MainMenuLayout.getNextElementPos();
            for (int i = 0; i < ResourceManager.CategoryModel.Count; i++)
            {
                Category currentCategory = ResourceManager.CategoryModel[i];

                if (mmrec != null)
                {
                    //Console.WriteLine("X == " + mmrec.Position.X + "  Y == " + mmrec.Position.Y);
                    //Console.WriteLine("W == " + mmrec.Width + "   H == " + mmrec.Height);

                    prevcatButton = new UIButton(mmrec.Position.X, mmrec.Position.Y, mmrec.Width, mmrec.Height, "", this, currentCategory.Name);
                    elements.Add(prevcatButton);
                }

                if (currentCategory.Filenames.Count > 0)
                {
                    BoundingRectangle cmrec = LayoutManager.CategoryMenuLayout.getNextElementPos();
                    for (int j = 0; j < currentCategory.Filenames.Count; j++)
                    {
                        if (cmrec != null)
                        {
                            UIButton catFileButton = new UIButton(cmrec.Position.X, cmrec.Position.Y, cmrec.Width, cmrec.Height, "", this, currentCategory.Filenames[j]);
                            //Console.WriteLine("CREATED AT :: " + catFileButton.Position);
                            //Console.WriteLine("DIM :: " + catFileButton.Dimension);
                            Console.WriteLine("X == " + cmrec.Position.X + "  Y == " + cmrec.Position.Y);
                            Console.WriteLine("W == " + cmrec.Width + "   H == " + cmrec.Height);

                            //CREATING ASSOCIATED MEDIA SCENE FOR THE BUTTON.
                            UIScene mediaScene = new UIScene(this, currentCategory.Filenames[j] + "_SLIDE_SCENE");
                            mediaScene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                            UISlide slide = new UISlide(currentCategory.Name + "_" + currentCategory.Filenames[j], "SLIDE", this);
                            slide.Texture = ResourceManager.getTexture(currentCategory.Name, currentCategory.Filenames[j]);
                            UIPanel mediaPanel = new UIPanel(this);
                            mediaPanel.addReceiver(slide);
                            mediaScene.IsASlide = true;
                            mediaScene.addReceiver(mediaPanel);
                            mediaScene.setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));

                            catFileButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, mediaScene.UIID);

                            processedScenes.Add(mediaScene);

                            categoryElements.Add(catFileButton);
                        }


                        cmrec = LayoutManager.CategoryMenuLayout.getNextElementPos();
                        if (j + 1 == ResourceManager.CategoryModel[i].Filenames.Count || cmrec == null)
                        {
                            UIScene s = new UIScene(this);
                            Console.WriteLine("NEW SCENE ID == " + s.UIID);
                            prevcatButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s.UIID);
                            UIPanel p = new UIPanel(this, categoryElements);
                            s.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);

                            s.addReceiver(p);
                            currentCatScenes.Add(s);
                            categoryElements = new List<AbstractUI>();
                            LayoutManager.CategoryMenuLayout.reset();
                            if (cmrec == null)
                                cmrec = LayoutManager.CategoryMenuLayout.getNextElementPos();
                        }


                    }
                    //@TODO PROCESS CATEGORY SCENES AND LINK THEM
                    //@TODO ADD THEM TO THE PROCESSED SCENES.


                    Console.WriteLine("CURRENT CAT SCENES FOUND :: " + currentCatScenes.Count);
                    for (int k = 0; k < currentCatScenes.Count; k++)
                    {
                        Console.WriteLine("CURRENT CAT SCENES :: " + currentCatScenes.ElementAt(k).UIID + "  " + currentCatScenes.ElementAt(k).Label.Text);
                    }
                    if (currentCatScenes.Count > 1)
                    {
                        UIScene pS = currentCatScenes.First();
                        for (int k = 1; k < currentCatScenes.Count; k++)
                        {
                            currentCatScenes.ElementAt(k - 1).setNext(currentCatScenes.ElementAt(k));
                            currentCatScenes.ElementAt(k).setPrev(currentCatScenes.ElementAt(k - 1));

                            //@TODO LOOK INTO THIS
                            currentCatScenes.ElementAt(k - 1).setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
                            currentCatScenes.ElementAt(k - 1).setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, currentCatScenes.ElementAt(k).UIID));
                            //nextScene.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, s.UIID));
                            processedScenes.Add(currentCatScenes.ElementAt(k - 1));
                            if (k + 1 >= currentCatScenes.Count) processedScenes.Add(currentCatScenes.ElementAt(k));
                            //pS = currentCatScenes.ElementAt(k);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < currentCatScenes.Count; k++)
                        {
                            currentCatScenes.ElementAt(k).setGestureCommand("GO_BACK_GESTURE", new SceneChangeCommand(ref this.SceneManager, -1));
                            processedScenes.Add(currentCatScenes.ElementAt(k));
                        }
                    }
                    currentCatScenes = new List<UIScene>();
                }
                


                mmrec = LayoutManager.MainMenuLayout.getNextElementPos();
                if (i + 1 == ResourceManager.CategoryModel.Count || mmrec == null)
                {
                    UIScene s = new UIScene(this);
                    elements.First().TouchCommand = new SceneChangeCommand(ref this.SceneManager, 11);
                    UIPanel p = new UIPanel(this, elements);


                    s.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);

                    s.addReceiver(p);
                    mainmenuScenes.Add(s);
                    //SceneSet.Add(s.UIID, s);
                    elements = new List<AbstractUI>();

                    LayoutManager.MainMenuLayout.reset();
                    if (mmrec == null)
                        mmrec = LayoutManager.MainMenuLayout.getNextElementPos();
                }
                //if (rec != null)
                //{
                //    Console.WriteLine("REC RECEIVED FOR ");
                  

                   
                //}
                //else
                //{
                    

                //    nextcatButton = new UIButton(rec.Position.X, rec.Position.Y, rec.Width, rec.Height, "", this, currentCategory.Name);
                //    elements.Add(nextcatButton);
                //}


            }
            //UIScene mainmenuFinal = new UIScene(this);
            //UIPanel p2 = new UIPanel(this, elements);
            //mainmenuFinal.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            //mainmenuFinal.addReceiver(p2);
           // mainmenuScenes.Add(mainmenuFinal);
            //SceneSet.Add(mainmenuscene2.UIID, mainmenuscene2);
            //p = new UIPanel(this, elements);
            //mainmenuscene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            //MAIN_MENU_ID = mainmenuscene.UIID;
            //mainmenuscene.addReceiver(p);

            //@TODO PROCCESS MAIN MENU SCENES AND LINK THEM
            //@TODO ADD THEM TO THE PRECESSED SCENES LIST

              //LayoutManager.MainMenuLayout.reset();
              //mmrec = LayoutManager.MainMenuLayout.getNextElementPos();
                
                

            if (mainmenuScenes.Count > 1)
            {
              //  BoundingRectangle bottomTextRect = new BoundingRectangle(mmrec.Position.X, this.graphics.P

               

                UIScene pScene = mainmenuScenes.First();
                MAIN_MENU_ID = mainmenuScenes.First().UIID;
                for (int k = 1; k < mainmenuScenes.Count; k++)
                {
                    
                    mainmenuScenes.ElementAt(k - 1).setNext(mainmenuScenes.ElementAt(k));
                    mainmenuScenes.ElementAt(k).setPrev(mainmenuScenes.ElementAt(k - 1));

                    mainmenuScenes.ElementAt(k).setGestureCommand("SWIPE_RIGHT_GESTURE", new SceneChangeCommand(ref this.SceneManager, mainmenuScenes.ElementAt(k - 1).UIID));
                    mainmenuScenes.ElementAt(k - 1).setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, mainmenuScenes.ElementAt(k).UIID));
                    

                    //nextScene.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, s.UIID));
                    processedScenes.Add(mainmenuScenes.ElementAt(k - 1));
                    if (k + 1 >= mainmenuScenes.Count) processedScenes.Add(mainmenuScenes.ElementAt(k));
                    //pS = currentCatScenes.ElementAt(k);
                }
            }
            else
            {
                for (int k = 0; k < currentCatScenes.Count; k++)
                {
                    processedScenes.Add(currentCatScenes.ElementAt(k));
                }
            }

            
            //for (int i = 0; i < mainmenuScenes.Count; i++)
            //{
            //    SceneSet.Add(mainmenuScenes.ElementAt(i).UIID, mainmenuScenes.ElementAt(i));
            //    Console.WriteLine("ADDING SCENE " + mainmenuScenes.ElementAt(i).UIID + "  ==  " + mainmenuScenes.ElementAt(i).Label.Text);
            //}

            for (int i = 0; i < processedScenes.Count; i++)
            {
                SceneSet.Add(processedScenes.ElementAt(i).UIID, processedScenes.ElementAt(i));
                Console.WriteLine("ADDING SCENE " + processedScenes.ElementAt(i).UIID + "  ==  " + processedScenes.ElementAt(i).Label.Text);
            }

            UIScene aa;
            SceneSet.TryGetValue(MAIN_MENU_ID, out aa);
            Console.WriteLine(" MAIN MENU SCENE GESTURE COUNT == " + aa.InterestedGestures.Count);
            
            //MAIN_MENU_ID = SceneSet.First().Key;
            Console.WriteLine("SCENE COUNT " + SceneSet.Last().Key  +  "  " + SceneSet.Count);

            Console.WriteLine(SceneSet.Count + " scenes created!!");

            return SceneSet;
        }

        private Dictionary<int, UIScene> createSceneCollectionFromContent()
        {
            // Scene x file_nums
            // Scene x categories_num
            // main menu Scene

            // 0 load images/videos                 filenames
            // 1 create UI objects for media        filesnames
            // 2 create UIPanels for media          filenames + Panel
            // 3 create Scenes for UIPanels         filenames + Scene
            // 4 create buttons that leads to media scenes      filename + Button
            // 5 create panels for buttons in STEP 4            media panel + category
            // 6 create scene for Panels in STEP 5              category_name + Scene
            // 7 create category buttons                        category name
            // 8 create Panel for STEP 6                        Main menu panel
            // 9 create MainScene for CategoryPanel             Main Scene


            Dictionary<int, UIScene> SceneSet = new Dictionary<int, UIScene>();
            List<UIScene> processedScenes = new List<UIScene>();
            Point gridSize = new Point(3, 2);
            Point elementRatio = new Point(95,90);
            Vector2 sizeRatio = new Vector2(elementRatio.X / 100.0f, elementRatio.Y / 100.0f);
            
            //get the Screen size
            Point screenDim = new Point(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            //set padding size for the screen
            Point padding = new Point((int)(screenDim.X * 0.10f), (int)(screenDim.Y * 0.10f));
            //apply padding to the screen size aka. new element space to use
            Point elementArea = new Point(screenDim.X - padding.X*2, screenDim.Y - padding.Y*2);

            Point ratio = new Point((int)(((elementArea.X)/gridSize.X) * sizeRatio.X), (int)(((elementArea.Y)/gridSize.Y) * sizeRatio.Y));

            Point spaceBetweenElements = new Point(elementArea.X - (ratio.X * gridSize.X), elementArea.Y - (ratio.Y * gridSize.Y));

            int currentX = 0, currentY = 0;
            currentX += padding.X;
            currentY += padding.Y;
            List<AbstractUI> elements = new List<AbstractUI>();
            List<UIPanel> panels = new List<UIPanel>();

            Point gridCount = new Point(0, 0);

            //creating CATEGORY MENU BUTTONS
            for (int i = 0; i < ResourceManager.CategoryModel.Count; i++)
            {
                Category currentCategory = ResourceManager.CategoryModel[i];
               

                UIButton catButton = new UIButton(currentX, currentY, ratio.X, ratio.Y, "", this, currentCategory.Name);
                


                if (gridCount.X < gridSize.X - 1)
                {
                    gridCount.X++;
                    currentX += spaceBetweenElements.X;
                    currentX += ratio.X;
                }
                else
                {
                    gridCount.X = 0;
                    currentX = 0;
                    currentX += padding.X;

                    gridCount.Y++;
                    currentY += spaceBetweenElements.Y;
                    currentY += ratio.Y;
                }

                
                List<String> filenames = currentCategory.Filenames;
                Console.WriteLine("NUMBER OF FILES FOR THE CATEGORY IS :: " + filenames.Count);
                Point SUBgridSize = new Point(2, 2);
                //method to calculate best gridsize
                if (filenames.Count > 4)
                {
                    int extra = filenames.Count - 4;
                    Boolean addX = false;
                    while (extra == 0)
                    {
                        if (addX != true)
                        {
                            SUBgridSize.Y++;
                            addX = true;
                        }
                        else
                        {
                            SUBgridSize.X++;
                            addX = false;
                        }
                        extra--;
                    }
                }


                Point SUBgridCount = new Point(0, 0);
                Point SUBelementRatio = new Point(95, 90);
                Vector2 SUBsizeRatio = new Vector2(SUBelementRatio.X / 100.0f, SUBelementRatio.Y / 100.0f);

                
                //set padding size for the screen
                Point SUBpadding = new Point((int)(screenDim.X * 0.10f), (int)(screenDim.Y * 0.10f));
                //apply padding to the screen size aka. new element space to use
                Point SUBelementArea = new Point(screenDim.X - SUBpadding.X * 2, screenDim.Y - SUBpadding.Y * 2);

                Point SUBratio = new Point((int)(((SUBelementArea.X) / SUBgridSize.X) * SUBsizeRatio.X), (int)(((SUBelementArea.Y) / SUBgridSize.Y) * SUBsizeRatio.Y));

                Point SUBspaceBetweenElements = new Point(SUBelementArea.X - (SUBratio.X * SUBgridSize.X), SUBelementArea.Y - (SUBratio.Y * SUBgridSize.Y));

                int SUBcurrentX = SUBpadding.X;
                int SUBcurrentY = SUBpadding.Y;


                UIScene sepCatScene = new UIScene(this, currentCategory.Name +  " SCENE");
                sepCatScene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                
               
                List<AbstractUI> categoryPanelElements = new List<AbstractUI>();
                ///DEMONSTRATION PURPOSES ONLY REMOVE LATER
                ///@TODO FIX find a video importing library , to have it dynamically supported like the images.
                ///
                if (filenames.Count == 0)
                {
                    UIButton cfButton = new UIButton(SUBcurrentX, SUBcurrentY, SUBratio.X, SUBratio.Y, "", this, "TEST VIDEO");

                    UIScene mScene = new UIScene(this, "TEST_VIDEO_SCENE");
                    mScene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                    UIVideo vid = new UIVideo("video", "TEST VIDEO", this);
                    UIPanel vidPanel = new UIPanel(this);
                    vidPanel.addReceiver(vid);

                    List<AbstractUI> gob2 = new List<AbstractUI>();
                    UIButton goback2 = new UIButton(0, 0, 64, 64, "", this, "Back");
                    goback2.TouchCommand = new SceneChangeCommand(ref this.SceneManager);
                    gob2.Add(goback2);
                    UIPanel meidaGOBACKPanel2 = new UIPanel(this, gob2);

                    mScene.addReceiver(vidPanel);
                    mScene.addReceiver(meidaGOBACKPanel2);

                    cfButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, mScene.UIID);

                    categoryPanelElements.Add(cfButton);
                    processedScenes.Add(mScene);

                }

                for (int j = 0; j < filenames.Count; j++)
                {
                    Console.WriteLine("GRIDCOUNT :: " + SUBgridCount);
                    Console.WriteLine("CREATING #" + (j + 1) + " CATEGORY FILE BUTTON :: " + filenames[j]);



                    UIButton catFileButton = new UIButton(SUBcurrentX, SUBcurrentY, SUBratio.X, SUBratio.Y, "", this, filenames[j]);
                    Console.WriteLine("CREATED AT :: " + catFileButton.Position);
                    Console.WriteLine("DIM :: " + catFileButton.Dimension);

                    UIScene mediaScene = new UIScene(this, filenames[j] + "_SLIDE_SCENE");
                    mediaScene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                    UISlide slide = new UISlide(currentCategory.Name + "_"+currentCategory.Filenames[j], "SLIDE", this);
                    slide.Texture = ResourceManager.getTexture(currentCategory.Name, currentCategory.Filenames[j]);
                   // slide.MediaFilename = currentCategory.Filenames[j];
                    UIPanel mediaPanel = new UIPanel(this);
                    mediaPanel.addReceiver(slide);
                    //go back button

                    List<AbstractUI> gob1 = new List<AbstractUI>();
                    UIButton goback1 = new UIButton(0, 0, 64, 64, "", this, "Back");
                    goback1.TouchCommand = new SceneChangeCommand(ref this.SceneManager);
                    gob1.Add(goback1);
                    UIPanel meidaGOBACKPanel = new UIPanel(this, gob1);

                    //go back button end;


                    mediaScene.addReceiver(mediaPanel);
                    mediaScene.addReceiver(meidaGOBACKPanel);

                    catFileButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, mediaScene.UIID);

                    categoryPanelElements.Add(catFileButton);
                    processedScenes.Add(mediaScene);


                    if (SUBgridCount.X < SUBgridSize.X - 1)
                    {
                        SUBgridCount.X++;
                        SUBcurrentX += SUBspaceBetweenElements.X;
                        SUBcurrentX += SUBratio.X;
                    }
                    else
                    {
                        SUBgridCount.X = 0;
                        SUBcurrentX = 0;
                        SUBcurrentX += SUBpadding.X;

                        SUBgridCount.Y++;
                        SUBcurrentY += SUBspaceBetweenElements.Y;
                        SUBcurrentY += SUBratio.Y;
                    }

                    
                }
                List<AbstractUI> gob = new List<AbstractUI>();
                UIButton goback = new UIButton(0, 0, 64, 64, "", this, "Back");
                goback.TouchCommand = new SceneChangeCommand(ref this.SceneManager);
                gob.Add(goback);
                UIPanel categoryGOBACKPanel = new UIPanel(this, gob);
                UIPanel categoryPanel = new UIPanel(this, categoryPanelElements, currentCategory.Name + " Panel");
                
                sepCatScene.addReceiver(categoryPanel);
                sepCatScene.addReceiver(categoryGOBACKPanel);


                catButton.TouchCommand = new SceneChangeCommand(ref this.SceneManager, sepCatScene.UIID);
                processedScenes.Add(sepCatScene);

                elements.Add(catButton);

                Console.WriteLine("\n\n");
            }
            Console.WriteLine("CATEGORY ELEMENTS NO :: " + elements.Count);
            UIPanel p = new UIPanel(this, elements);
            
            UIScene s = new UIScene(this, "MAIN_MENU_SCENE");
            int gridSizeTotal = gridSize.X * gridSize.Y;
            Console.WriteLine("GRID SIZE TOTAL ::: " + gridSizeTotal);
            Console.WriteLine("CATEGORY MODEL SIZE ::: " + ResourceManager.CategoryModel.Count);
            if (gridSizeTotal < ResourceManager.CategoryModel.Count)
            {
                Console.WriteLine("MORE CATS THAN EXPECTED");
                UIScene nextScene = new UIScene(this, "MAIN_MENU_SCENE_2");
                nextScene.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
                s.setNext(nextScene);
                nextScene.setPrev(s);
                s.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, nextScene.UIID));
                nextScene.setGestureCommand("SWIPE_LEFT_GESTURE", new SceneChangeCommand(ref this.SceneManager, s.UIID));

                processedScenes.Add(nextScene);
            }

            s.BoundingRectangle = new BoundingRectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);
            MAIN_MENU_ID = s.UIID;
            s.addReceiver(p);
            processedScenes.Add(s);

            //SceneSet.Add(s.UIID, s);
            for (int i = 0; i < processedScenes.Count; i++)
                SceneSet.Add(processedScenes.ElementAt(i).UIID, processedScenes.ElementAt(i));

            Console.WriteLine("STARTING SCENE ID == " + MAIN_MENU_ID);
            return SceneSet;
        }

        private void takeSS()
        {
            

            int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = GraphicsDevice.PresentationParameters.BackBufferHeight;

            //force a frame to be drawn (otherwise back buffer is empty) 
            Draw(new GameTime());

            //pull the picture from the buffer 
            int[] backBuffer = new int[w * h];
            GraphicsDevice.GetBackBufferData(backBuffer);

            //copy into a texture 
            Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);

            //save to disk 
            Stream stream = File.OpenWrite("SCREENSHOT.jpg");

            texture.SaveAsJpeg(stream, w, h);
            stream.Dispose();

            texture.Dispose();
        }

        private Dictionary<int, UIScene> createSceneCollection()
        {
            Dictionary<int, UIScene> SceneSet = new Dictionary<int, UIScene>();

            Point ratio = new Point((int)(graphics.PreferredBackBufferWidth * 0.40f), (int)(graphics.PreferredBackBufferHeight * 0.35f));

            UIScene s1 = new UIScene(this);
            UIScene s2 = new UIScene(this);
            UIScene s3 = new UIScene(this);
            UIScene s4 = new UIScene(this);
            UIScene s5 = new UIScene(this);

            UIVideo video = new UIVideo("Data\\video", "Video", this);
            UISlide slide = new UISlide("Data\\slide1", "Slide", this);
            UIButton b1 = new UIButton(50, 50, ratio.X, ratio.Y, "", this, "(PH)Events");
            UIButton b2 = new UIButton(50 + (int)(ratio.X * 1.3), 50, ratio.X, ratio.Y, "", this, "(PH)Notices");
            UIButton b3 = new UIButton(50 + (int)(ratio.X * 1.3), 50 + (int)(ratio.Y * 1.3), ratio.X, ratio.Y, "", this, "(PH)Opportunities");
            UIButton b4 = new UIButton(50, 50 + (int)(ratio.Y * 1.3), ratio.X, ratio.Y, "", this, "(PH)Student Works");

            UIButton b5 = new UIButton(0, 0, ratio.X, ratio.Y, "", this);
            UIButton b6 = new UIButton(0, graphics.PreferredBackBufferHeight - ratio.Y, ratio.X, ratio.Y, "", this);
            UIButton b7 = new UIButton(graphics.PreferredBackBufferWidth - ratio.X, 0, ratio.X, ratio.Y, "", this);
            UIButton b8 = new UIButton(graphics.PreferredBackBufferWidth - ratio.X, graphics.PreferredBackBufferHeight - ratio.Y, ratio.X, ratio.Y, "", this);

            b1.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s2.UIID);
            b2.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s3.UIID);
            b3.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s4.UIID);
            b4.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s5.UIID);

            b5.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s1.UIID);
            b6.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s1.UIID);
            b7.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s1.UIID);
            b8.TouchCommand = new SceneChangeCommand(ref this.SceneManager, s1.UIID);

            List<AbstractUI> uilist = new List<AbstractUI>();
            uilist.Add(b1);
            uilist.Add(b2);
            uilist.Add(b3);
            uilist.Add(b4);

            UIPanel p1 = new UIPanel(this, uilist);

            uilist.Clear();
            
            uilist.Add(b5);
            uilist.Add(slide);

            UIPanel p2 = new UIPanel(this, uilist);

            uilist.Clear();
            uilist.Add(b6);
            uilist.Add(video);

            UIPanel p3 = new UIPanel(this, uilist);

            uilist.Clear();
            uilist.Add(b7);

            UIPanel p4 = new UIPanel(this, uilist);

            uilist.Clear();
            uilist.Add(b8);

            UIPanel p5 = new UIPanel(this, uilist);

            

            //p1.addReceiver(b1);
            //p1.addReceiver(b2);
            //p1.addReceiver(b3);
            //p1.addReceiver(b4);

            //p2.addReceiver(b5);

            //p3.addReceiver(b6);

            //p4.addReceiver(b7);

            //p5.addReceiver(b8);

            s1.addReceiver(p1);
            s2.addReceiver(p2);
            s3.addReceiver(p3);
            s4.addReceiver(p4);
            s5.addReceiver(p5);

            SceneSet.Add(s1.UIID, s1);
            SceneSet.Add(s2.UIID, s2);
            SceneSet.Add(s3.UIID, s3);
            SceneSet.Add(s4.UIID, s4);
            SceneSet.Add(s5.UIID, s5);

            return SceneSet;
        }



    }
}

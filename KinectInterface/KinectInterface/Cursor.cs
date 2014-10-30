using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KinectInterface
{
    public class Cursor
    {
        private Rectangle rectangle;
        private Color currentColor;
        private Texture2D texture;

        private Texture2D insideFilled;
        private Texture2D outsideFilled;
        private Texture2D noFill;
        private Texture2D testboxbg;
        private Texture2D testboxfg;
        private Rectangle testboxbgrect;
        private Rectangle testboxfgrect;
        private float indicator_value;

        public Cursor(int w, int h)
        {
            this.rectangle = new Rectangle(0, 0, w, h);
            this.currentColor = Color.White;
            this.texture = null;

            this.indicator_value = 0.0f;
            this.testboxbgrect = new Rectangle(this.rectangle.X - 10, 0, 7, this.rectangle.Height);
            this.testboxfgrect = new Rectangle(this.rectangle.X - 10, 0, 7, (int)( this.rectangle.Height * this.indicator_value));
        }

        public void UpdatePosition(Point p)
        {
            this.rectangle.X = p.X;
            this.rectangle.Y = p.Y;
        }

        public Point GetOffSet(int x, int y, int texWidth, int texHeight)
        {
            Point p = new Point();

            p.X = (this.rectangle.Width * x) / texWidth;
            p.Y = (this.rectangle.Height * y) / texHeight;

            return p;
        }
        public float PushIndicatorValue { set { this.indicator_value = value; } }
        public Rectangle BoundingRectangle { get { return this.rectangle; } set { this.rectangle = value; } }
        public Color Color { get { return this.currentColor; } set { this.currentColor = value; } }
        public Texture2D Texture { get { return this.texture; } set { this.texture = value; this.currentColor = Color.White; } }

        public void LoadContent(GraphicsDevice gd)
        {
            //if (rm.TexturesLoaded == false)
            //{
            //    rm.LoadTextures();
            //}

            //FileStream stream = new FileStream(
            //    "C:\\Users\\Cagil\\Documents\\dissertation-kinect-interface\\KinectInterface\\KinectInterfaceContent\\Textures\\" + "hand_pointer.png", FileMode.Open);


            //Texture2D ttmp = Texture2D.FromStream(gd, stream);
            //stream.Close();
            //this.texture = ttmp;

            this.texture = ResourceManager.loadTextureOnce(gd, "hand_pointer.png");
            this.outsideFilled = ResourceManager.loadTextureOnce(gd, "hand_pointer_outside_filled.png");
            this.insideFilled = ResourceManager.loadTextureOnce(gd, "hand_pointer_half_filled.png");
            this.noFill = ResourceManager.loadTextureOnce(gd, "hand_pointer_empty.png");

            this.testboxbg = new Texture2D(gd, 1, 1);
            this.testboxbg.SetData(new[] { Color.Brown });

            this.testboxfg = new Texture2D(gd, 1, 1);
            this.testboxfg.SetData(new[] { Color.DarkBlue });
        }

        public void Update(GameTime gameTime)
        {
            this.testboxbgrect = new Rectangle(this.rectangle.X - 10, this.rectangle.Y, 7, this.rectangle.Height);
           // this.testboxfgrect = new Rectangle(this.rectangle.X - 10, this.rectangle.Y, 7, (int)(this.rectangle.Height - (this.rectangle.Height * this.indicator_value)));
            this.testboxfgrect = new Rectangle(this.rectangle.X , this.rectangle.Y, this.rectangle.Width, (int)( (this.rectangle.Height * this.indicator_value)));
        }

        public void Draw(SpriteBatch sp, GameTime time, GraphicsDeviceManager graphics)
        {
            if (texture == null) return;
            
           // sp.Draw(this.testboxbg, this.testboxbgrect, Color.Brown);
         //   sp.Draw(this.testboxfg, this.testboxfgrect, Color.Yellow);
        //    sp.Draw(this.texture, this.rectangle, this.currentColor);


            var m = Matrix.CreateOrthographicOffCenter(0,
    graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
    graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
    0, 0, 1
);

            var a = new AlphaTestEffect(graphics.GraphicsDevice)
            {
                Projection = m
            };

            var s1 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            var s2 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            sp.Begin(SpriteSortMode.Immediate, null, null, s1, null, a);
           // sp.Draw(this.texture, Vector2.Zero, Color.White); //The mask     
           
            sp.Draw(this.insideFilled, this.rectangle, Color.White);
            sp.End();

            

          //  sp.Draw(this.testboxfg, this.testboxfgrect, Color.Yellow);
           //     sp.Draw(this.texture, this.rectangle, this.currentColor);


            sp.Begin(SpriteSortMode.Immediate, null, null, s2, null, a);
       //     sp.Draw(this.testboxfg, Vector2.Zero, Color.White); //The background

            sp.Draw(this.testboxfg, this.testboxfgrect, Color.White);
            sp.End();

            sp.Begin(SpriteSortMode.Immediate, null, null, s2, null, a);
            sp.Draw(this.outsideFilled, this.rectangle, Color.White);
            sp.End();
        
        }
    }
}

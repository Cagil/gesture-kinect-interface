using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    public class UIText : AbstractUI
    {
        private Driver driver;
        private Vector2 size;
        private String text;
        private SpriteFont font;
        private Vector2 fontOrigin;
        private Vector2 fontPosition;
        private Color textColor;
        private float scale;
        private float rotation;
        private float spriteLayer;
        private SpriteEffects spriteEffects;
        //float rotation = 0.0f;
        //Vector2 spriteOrigin = new Vector2(0, 0);
        //float spriteLayer = 0.0f; // all the way in the front
       // SpriteEffects spriteEffects = new SpriteEffects();
        private BoundingRectangle textBg;
        private Texture2D bgTex;

        public Vector2 Size { get { return this.size; } }
        public String Text { get { return this.text; } set { this.text = value; this.calcSize(); } }

        public SpriteFont Font { get { return this.font; } set { this.font = value; } }
        public Vector2 Position { get { return this.fontPosition; } set { this.fontPosition = value;  } }
        public Color TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public float Scale { get { return this.scale; } set { this.scale = value; } }
        public Vector2 Origin { get { return this.fontOrigin; } }

        public float Rotation { get { return this.rotation; } set { this.rotation = value; } }
        public float Layer { get { return this.spriteLayer; } set { this.spriteLayer = value; } }
        public SpriteEffects Effect { get { return this.spriteEffects; } set { this.spriteEffects = value; } } 

        public UIText(Driver driver, String text) : base(driver)
        {
            this.driver = driver;
            this.text = text;
            this.scale = 1f;
            this.textColor = Color.White;

            this.fontOrigin = Vector2.Zero;
            this.rotation = 0.0f;
            this.Layer = 0.0f;
            this.Effect = SpriteEffects.None;
            this.textBg = null;
            
        }


        private void calcTextOrigin()
        {
            this.fontOrigin = this.Font.MeasureString(this.text) / 2;
        }

        private void calcSize()
        {
            //this.bound = new BoundingRectangle(0, 0, (int)(this.Font.MeasureString(this.Text).X), (int)(this.Font.MeasureString(this.text).Y));
            this.size = this.font.MeasureString(this.Text);
           
        }

        //public void PlaceAndAlign(BoundingRectangle container, bool resize)
        //{
        //    Console.WriteLine("Label :: " + this.text);
        //    //int spacex = (container.Position.X + container.Dimension.X) - container.Position.X;
        //   // Console.WriteLine("Spacex ::  " + spacex);
        //    if (container.Dimension.X > (int)(this.Size.X))
        //    {
        //        Console.WriteLine("Text WIdth :: " + this.Size.X);
        //        int emptyspace = container.Dimension.X - (int)(this.Size.X);
        //        Console.WriteLine("EMPTY SPACE :: " + emptyspace);
        //        this.Position = new Vector2(container.Position.X  + emptyspace / 2  + this.Origin.X, (container.Position.Y + container.Dimension.Y) - this.Origin.Y);
        //        Console.WriteLine("POS :: " + this.Position);
        //    }
        //    else
        //    {
        //        if (resize)
        //        {

        //        }
        //        int extraspace = (int)(this.Size.X) - container.Dimension.X;

        //        this.Position = new Vector2(container.Position.X - extraspace / 2  +this.Origin.X, (container.Position.Y + container.Dimension.Y) - this.Origin.Y);
        //    }

        //}

        public void PlaceAndAlign(BoundingRectangle container)
        {
            this.textBg = container;

            float xScale = (container.Dimension.X / this.size.X);
            float yScale = 1.0f;

            // Taking the smaller scaling value will result in the text always fitting in the boundaires.
            this.scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * this.scale);
            int strHeight = (int)Math.Round(size.Y * this.scale);
            
            // MIDDLE x
            this.fontPosition.X = (((container.Dimension.X - strWidth) / 2) + container.Position.X);
            //BOTTOM y
            this.fontPosition.Y = (container.Position.Y + container.Dimension.Y) - strHeight;

            // MIDDLE y
            // this.fontPosition.Y = (((container.Dimension.Y - strHeight) / 2) + container.Position.Y); 

          

            // Draw the string to the sprite batch!
           // spriteBatch.DrawString(font, strToDraw, position, Color.White, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        } // end DrawString()



        public override void LoadContent(ref ResourceManager resources, GraphicsDevice gd)
        {
            this.bgTex = new Texture2D(driver.GraphicsDevice, 1, 1);
            this.bgTex.SetData(new[] { Color.White });

            this.font = this.driver.Content.Load<SpriteFont>("Courier New");
            calcSize();
           // calcTextOrigin();
          //  calcTextBounds();
        }

        public override void Draw(ref SpriteBatch sp, GameTime gameTime)
        {
            //sp.DrawString(this.Font, this.Text, this.Position, this.TextColor,
            //    0, this.fontOrigin, this.Scale, SpriteEffects.None, 0.5f);
           // sp.DrawString(this.Font, this.Text, this.Position, this.TextColor);

            //if (this.textBg != null)
            //{
            //    sp.Draw(bgTex, textBg.XNARectangle, Color.Black);
            //}

            sp.DrawString(this.Font, this.Text, this.Position, this.TextColor,this.Rotation, this.Origin, this.Scale, this.Effect, this.Layer);

        }

        

        public override void Initialize()
        {
           // throw new NotImplementedException();
            if (this.BoundingRectangle != null)
            {
                this.PlaceAndAlign(this.BoundingRectangle);
            }
        }

        public override void Update(GameTime gameTime)
        {
           // throw new NotImplementedException();
        }

        public override void Receive(Message<AbstractUI> message)
        {
           // throw new NotImplementedException();
        }
    }
}

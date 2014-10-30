using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    public class UIButton : AbstractUI
    {
        private BoundingRectangle bigRect;
        private BoundingRectangle currentRect;

        public UIButton(int x, int y, int width, int height, String textureString, Driver driver, String label = "Button") : base(driver)
        {
            this.Position = new Point(x, y);
            this.Dimension = new Point(width, height);
            this.BoundingRectangle = new Utils.BoundingRectangle(x, y, width, height);
            this.bigRect = new BoundingRectangle(x - 5, y - 5, width + 10, height + 10);
            this.currentRect = this.BoundingRectangle;
            this.MediaFilename = textureString;

           // this.Texture = new Microsoft.Xna.Framework.Graphics.Texture2D(Driver.GraphicsDevice, width, height);
            this.Label = new UIText(driver, label);
         //   Console.WriteLine(this.UIID + " " + label);
         //   Console.WriteLine("Pos :: " +  this.Position);
         //   Console.WriteLine("Dim :: " +this.Dimension);
        }



       

        public override void LoadContent(ref ResourceManager resources, GraphicsDevice gd)
        {
           // this.Texture = Microsoft.Xna.Framework.Content.;
            this.Texture = resources.getTexture(this.MediaFilename);
            if(this.Texture == null){
                this.Texture = new Microsoft.Xna.Framework.Graphics.Texture2D(Driver.GraphicsDevice, this.Dimension.X, this.Dimension.Y);
                this.Texture = new Microsoft.Xna.Framework.Graphics.Texture2D(Driver.GraphicsDevice, 1, 1);
                this.Texture.SetData(new[] { this.CurrentColor });
            }


            this.Label.LoadContent(ref resources, gd);
          //  this.Label.PlaceAndAlign(this.BoundingRectangle);
            this.Label.PlaceAndAlign(this.BoundingRectangle);
           // this.Label.Position = new Vector2(this.Position.X + this.Label.Origin.X , (this.Position.Y + this.Dimension.Y) - this.Label.BoundingRectangle.Height);
          //  Console.WriteLine(this.Label.BoundingRectangle.Height +  "   "  +this.Label.Position);
               // texture = new Texture2D(GraphicsDevice, 1, 1);
            //texture.SetData(new[] { Color.White });
        }

        

        public override void Initialize()
        {
            //this.placeLabel();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //throw new NotImplementedException();
            if (this.State == UIState.InteractionState.FOCUSED)
            {
                this.currentRect = this.bigRect;
            }
            else
            {
                this.currentRect = this.BoundingRectangle;
            }
        }

        public override void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch sp, Microsoft.Xna.Framework.GameTime gameTime)
        {
            
            sp.Draw(this.Texture, this.currentRect.XNARectangle, this.CurrentColor);
           // sp.Draw(this.Texture, new Rectangle((int)this.Label.Position.X, (int)this.Label.Position.Y, this.Label.BoundingRectangle.Dimension.X, this.Label.BoundingRectangle.Dimension.Y), Color.Yellow);
            this.Label.Draw(ref sp, gameTime);
           // sp.Draw(this.Texture, new Rectangle((int)this.Label.Position.X, (int)this.Label.Position.Y, 5, 5), Color.Purple);
        }

        public override void Receive(Utils.Message<AbstractUI> message)
        {
            //Console.WriteLine("opening message");
            message.open(this);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}

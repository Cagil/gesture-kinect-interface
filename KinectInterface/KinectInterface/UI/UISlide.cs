using KinectInterface.Strategy;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    public class UISlide : AbstractUI
    {
        private FittingStrategy fittingStrategy;

        public FittingStrategy PlacementStrategy { get { return this.fittingStrategy; } set { this.fittingStrategy = value; } }

        public UISlide(String filename, String label, Driver driver)
            : base(driver)
        {
            this.MediaFilename = filename;
            this.Label = new UIText(driver, label);
            this.BoundingRectangle = new Utils.BoundingRectangle(0, 0, Driver.Window.ClientBounds.Width , Driver.Window.ClientBounds.Height);
            this.fittingStrategy = new FitFullscreenStrategy();
            this.Texture = null;
        }

        private void PlaceAndAlign(FittingStrategy strategy)
        {
            //float min = 1.0f;
            //float xScale = Math.Min(1.0f,(Driver.Window.ClientBounds.Width / this.Texture.Width));
            //float yScale = Math.Min(1.0f,(Driver.Window.ClientBounds.Height / this.Texture.Height));


            //// Taking the smaller scaling value will result in the text always fitting in the boundaires.
            ////float scale = Math.Min(xScale, yScale);
            ////xScale = (float)Math.Min(xScale, min);
            ////yScale = (float)Math.Min(yScale, min);
            //xScale = Math.Min(xScale, min);
            //yScale = Math.Min(yScale, min);

            //// Figure out the location to absolutely-center it in the boundaries rectangle.
            //int imgWidth = (int)Math.Round(this.Texture.Width * xScale);
            //int imgHeight = (int)Math.Round(this.Texture.Height * yScale);


            //this.BoundingRectangle = new Utils.BoundingRectangle(0, 0, imgWidth, imgHeight);
            if (Texture == null)
            {
                Console.WriteLine(this.MediaFilename + " is not found");
                
            }else
            this.BoundingRectangle = strategy.fit(new BoundingRectangle(Driver.Window.ClientBounds), this.Texture.Width, this.Texture.Height);
            // MIDDLE x
           // this.fontPosition.X = (((container.Dimension.X - strWidth) / 2) + container.Position.X);
            //BOTTOM y
          //  this.fontPosition.Y = (container.Position.Y + container.Dimension.Y) - strHeight;
            
        }

        public override void LoadContent(ref ResourceManager resources, GraphicsDevice gd)
        {

            //this.Texture = Driver.Content.Load<Texture2D>(this.MediaFilename);
            //this.Texture = resources.getTexture(this.MediaFilename);
            String[] tokenizor = new String[1];
            tokenizor[0] = "_";
            String[] mediaFileParts = this.MediaFilename.Split(tokenizor, StringSplitOptions.None);


            FileStream stream = new FileStream(resources.ContentLocation
                + mediaFileParts[0] +
                "\\" +
                mediaFileParts[1], FileMode.Open);


            this.Texture = Texture2D.FromStream(Driver.GraphicsDevice, stream);
           
            stream.Close();

            //this.Texture = resources.getTexture(this.MediaFilename);

         //   Console.WriteLine("LOADING TEXTURE FOR A SLIDE :: " + this.MediaFilename);

            PlaceAndAlign(this.fittingStrategy);
            //@TODO handle UIText
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch sp, Microsoft.Xna.Framework.GameTime gameTime)
        {
            
            if(this.Texture != null || this.BoundingRectangle != null)
                sp.Draw(this.Texture, this.BoundingRectangle.XNARectangle, Color.White);
        }

        public override void Receive(Utils.Message<AbstractUI> message)
        {
            message.open(this);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}

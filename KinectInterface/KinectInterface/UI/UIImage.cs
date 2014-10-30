using KinectInterface.Strategy;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    public class UIImage : AbstractUI
    {
        //private Texture2D texture;
        public UIImage(Driver driver)
            : base(driver)
        {

        }
        
        public override void LoadContent(ref ResourceManager resources, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            this.Texture = ResourceManager.loadTextureOnce(gd, this.MediaFilename);
            //this.BoundingRectangle = new BestFittingStrategy(true).fit(new Utils.BoundingRectangle(0, 0, this.Driver.Window.ClientBounds.Width, this.Driver.Window.ClientBounds.Height), this.Texture.Width, this.Texture.Height);
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
            if (this.Texture != null)
            {
                sp.Draw(this.Texture, this.BoundingRectangle.XNARectangle, Color.White);
            }
        }

        public override void Receive(Utils.Message<AbstractUI> message)
        {
            message.open(this);
        }

        
    }
}

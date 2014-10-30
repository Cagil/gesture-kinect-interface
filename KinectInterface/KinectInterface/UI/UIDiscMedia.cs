using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    public class UIDiscMedia : AbstractUI
    {
        private BoundingRectangle mediaContainer;
        private BoundingRectangle discContainer;
        private BoundingRectangle iconContainer;

        private UIText disc;
        private AbstractUI media;
        private AbstractUI icon;

        public AbstractUI Icon { get { return this.icon; } set { this.icon = value; } }
        public UIText Discription { get { return this.disc; } set { this.disc = value; } }
        public AbstractUI Media { get { return this.media; } set { this.media = value; } }

        public BoundingRectangle MediaContainer { get { return this.mediaContainer; } }

        public UIDiscMedia(Driver driver, BoundingRectangle container, float discRatio)
            : base(driver)
        {
            this.BoundingRectangle = container;
            this.discContainer = new BoundingRectangle(
                this.BoundingRectangle.Position.X,
                this.BoundingRectangle.Position.Y + (int)(this.BoundingRectangle.Height),
                this.BoundingRectangle.Width,
                (int)(this.BoundingRectangle.Height * 0.05f));

            int emptyHorizontalSpace = (this.BoundingRectangle.Width - this.BoundingRectangle.Height)/2;

            this.mediaContainer = new BoundingRectangle(
                this.BoundingRectangle.Position.X + emptyHorizontalSpace,
                this.BoundingRectangle.Position.Y, 
                this.BoundingRectangle.Height,
                (int)(this.BoundingRectangle.Height * 0.90f));

            //this.mediaContainer = new BoundingRectangle(
            //    this.BoundingRectangle.Position.X ,
            //    this.BoundingRectangle.Position.Y,
            //    (int)(this.BoundingRectangle.Height),
            //    (int)(this.BoundingRectangle.Height * 0.90f));

            //this.iconContainer = new BoundingRectangle(
            //    this.mediaContainer.Position.X + this.BoundingRectangle.Width / 2,
            //    this.mediaContainer.Position.Y,
            //    (int)(this.BoundingRectangle.Height * 0.90f),
            //    (int)(this.BoundingRectangle.Height * 0.90f));

            this.icon = null;
            this.disc = null;
            this.media = null;
        }

        public override void LoadContent(ref ResourceManager resources, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            if (this.disc != null)
            {
                this.disc.LoadContent(ref resources, gd);
                this.disc.PlaceAndAlign(this.discContainer);
            }

            if (this.icon != null)
            {
                this.icon.LoadContent(ref resources, gd);
                this.icon.BoundingRectangle = this.iconContainer;
            }

            if (this.media != null)
            {

                if (!(this.media is UIButton))
                {

                    this.media.BoundingRectangle = this.mediaContainer;
                    this.media.LoadContent(ref resources, gd);
                }
                else
                {
                    this.media.LoadContent(ref resources, gd);
                    this.media.BoundingRectangle = this.mediaContainer;
                }

            }
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //throw new NotImplementedException();
            if(this.media != null)
                this.media.Update(gameTime);
            if (this.icon != null)
            {
                this.icon.Update(gameTime);
            }
        }

        public override void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch sp, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.media != null)
            {
                this.media.Draw(ref sp, gameTime);
            }

            if (this.disc != null)
            {
                this.disc.Draw(ref sp, gameTime);
            }

            if (this.icon != null)
            {
                this.icon.Draw(ref sp, gameTime);
            }
        }

        public override void Receive(Utils.Message<AbstractUI> message)
        {
            message.open(this);
            if (this.icon != null)
            {
                this.icon.Receive(message);
            }

            if (this.media != null)
            {
                
                this.media.Receive(message);
            }
        }
    }
}

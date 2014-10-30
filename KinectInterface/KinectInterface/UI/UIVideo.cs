using KinectInterface.Strategy;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KinectInterface.UI
{
    public class UIVideo : AbstractUI
    {
        private VideoPlayer player;
        private Video video;
        private FittingStrategy fittingStrategy;
        private Boolean isLooping;
        private Boolean started;

        

        public Boolean IsLooping { get { return this.isLooping; } set { this.isLooping = value; } }
        public FittingStrategy PlacementStrategy { get { return this.fittingStrategy; } set { this.fittingStrategy = value; } }

        public UIVideo(String filename, String label, Driver driver)
            : base(driver)
        {
            this.MediaFilename = filename;
            this.Label = new UIText(driver, label);
            this.isLooping = false;
            this.player = new VideoPlayer();
            this.player.IsMuted = true;
        
            this.started = false;
            this.fittingStrategy = new BestFittingStrategy();
            this.BoundingRectangle = new Utils.BoundingRectangle(0, 0, Driver.Window.ClientBounds.Width, Driver.Window.ClientBounds.Height);
        }

        private void PlaceAndAlign(FittingStrategy strategy)
        {
            this.BoundingRectangle = strategy.fit(new BoundingRectangle(Driver.Window.ClientBounds), this.video.Width, this.video.Height);
        }

        public override void LoadContent(ref ResourceManager resources, GraphicsDevice gd)
        {
            //@TODO video support is not available for now. FIX! look for a library that can load videos dynamically without needing XNA COntent manager
            this.video = Driver.Content.Load<Video>(this.MediaFilename);
            //PlaceAndAlign(this.fittingStrategy);
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
            player.IsLooped = this.isLooping;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (player.State == MediaState.Stopped)
            {
                //player.IsLooped = true;
               // if(player.State == MediaState.
                if (this.started == false)
                {
                    player.Play(this.video);
                    this.started = true;
                }
                else
                {
                    if (player.IsLooped)
                        player.Play(this.video);
                }
            }
        }

        public override void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch sp, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (player.State != MediaState.Stopped)
            {
                this.Texture = player.GetTexture();
            }

            if (this.video != null)
            {
                sp.Draw(this.Texture, this.BoundingRectangle.XNARectangle, Color.White);
            }
        }

        public override void Receive(Utils.Message<AbstractUI> message)
        {
            message.open(this);
        }

        public override void Reset()
        {
            base.Reset();
           
            this.player.Stop();
            this.started = false;
        }
    }
}

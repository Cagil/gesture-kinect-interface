using KinectInterface.UI;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Messages
{
    class DrawMessage : Message<AbstractUI>
    {

        private SpriteBatch spriteBatch;
        private GameTime time;

        public DrawMessage(ref SpriteBatch sp, GameTime gameTime) : base(0){
            this.spriteBatch = sp;
            this.time = gameTime;
        }

        public override void open(AbstractUI receipent)
        {
            
            receipent.Draw(ref this.spriteBatch, this.time);
        }
    }
}

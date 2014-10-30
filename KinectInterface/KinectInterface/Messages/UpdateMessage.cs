using KinectInterface.UI;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Messages
{
    public class UpdateMessage : Message<AbstractUI>
    {
        private GameTime time;

        public UpdateMessage(GameTime gameTime) : base(0){
            this.time = gameTime;
        }

        public override void open(AbstractUI receipent)
        {
            receipent.Update(this.time);
        }

    }
}

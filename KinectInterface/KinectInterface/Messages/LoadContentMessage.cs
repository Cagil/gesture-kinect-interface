using KinectInterface.UI;
using KinectInterface.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Messages
{
    class LoadContentMessage : Message<AbstractUI>
    {
        private ResourceManager resources;
        private GraphicsDevice gd;
        public LoadContentMessage(ref ResourceManager res, GraphicsDevice gd)
            : base(0)
        {
            this.resources = res;
            this.gd = gd;
        }

        public override void open(AbstractUI receipent)
        {
            receipent.LoadContent(ref this.resources, this.gd);
        }
    }
}

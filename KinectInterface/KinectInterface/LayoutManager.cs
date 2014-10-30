using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using KinectInterface.Layout;
using KinectInterface.UI;
using KinectInterface.Layouts;

namespace KinectInterface
{
    public class LayoutManager
    {
        private Point screenSize;

        private Layout mainmenuLayout;
        private Layout categorymenuLayout;
        private Layout helpMenuLayout;

        public LayoutManager(int sx, int sy)
        {

        }

        public Layout MainMenuLayout { get { return this.mainmenuLayout; } set { this.mainmenuLayout = value; } }
        public Layout CategoryMenuLayout { get { return this.categorymenuLayout; } set { this.categorymenuLayout = value; } }
        public Layout HelpMenuLayout { get { return this.helpMenuLayout; } set { this.helpMenuLayout = value; } }
    }
}

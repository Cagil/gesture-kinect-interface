using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Commands
{
    public class SkipHelpScreenCommand : Command
    {
        private SceneManager sceneManager;

        public SkipHelpScreenCommand(ref SceneManager sm)
        {
            this.sceneManager = sm;
        }

        public void run()
        {
            if (this.sceneManager != null)
            {
                this.sceneManager.jumpToLastKnownMenuScene();
            }
        }
    }
}

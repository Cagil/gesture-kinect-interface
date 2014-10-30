using KinectInterface.UI;
using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.KinectUtils.Commands
{
    class SceneChangeCommand : Command
    {
        private SceneManager sceneManager;
        private int id;

        public SceneChangeCommand(ref SceneManager driver, int sceneId = -1 )
        {
            this.sceneManager = driver;
            this.id = sceneId;
            //@TODO assign scene to  be changed
        }

        


        public void run()
        {
            if (id == -1) { this.sceneManager.changeToPrev(); return; }
            Console.WriteLine("in scene change command " + this.id);
            this.sceneManager.jumpTo(this.id);
        }
    }
}

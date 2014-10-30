using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Commands
{
    public class FullScreenToggleCommand : Command
    {
        private Driver driver;

        public FullScreenToggleCommand(Driver driver)
        {
            this.driver = driver;
        }

        public void run()
        {
            this.driver.IsFullScreen = !this.driver.IsFullScreen;
            //this.driver.toggleFullScreen();
        }
    }
}

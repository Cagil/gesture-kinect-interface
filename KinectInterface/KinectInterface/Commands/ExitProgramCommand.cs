using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Commands
{
    public class ExitProgramCommand : Command
    {
        private Driver driver;
        public ExitProgramCommand(Driver driver)
        {
            this.driver = driver;
        }

        public void run()
        {
            this.driver.Exit();
        }
    }
}

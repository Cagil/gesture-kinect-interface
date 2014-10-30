using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Strategy
{
    public interface FittingStrategy
    {
        BoundingRectangle fit(BoundingRectangle container, int width, int height);
    }
}

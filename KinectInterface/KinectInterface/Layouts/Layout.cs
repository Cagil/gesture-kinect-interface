using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Layouts
{
    public interface Layout
    {
        BoundingRectangle getNextElementPos();
        BoundingRectangle getBottomTextRect();
        BoundingRectangle getTopTextRect();
        BoundingRectangle getHelpTextRect();
        BoundingRectangle getRightRect();
        BoundingRectangle getLeftRect();
        void reset();
    }
}

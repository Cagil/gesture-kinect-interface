using KinectInterface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Strategy
{
    public class BestFittingStrategy : FittingStrategy
    {
        Boolean centerAligned;

        public BestFittingStrategy(Boolean center = true) { this.centerAligned = center; }

        public BoundingRectangle fit(BoundingRectangle container, int width, int height)
        {
            BoundingRectangle rect;

            //taking the 1.0f as minimum to avoid if the image is larger than the screen.
            //minimum ratio needed for the screen.
            float min = 1.0f;
            float xRatio = (float)container.Dimension.X / width;
            float yRatio = (float)container.Dimension.Y / height;

            float imgDimRatio = (float)width / height;
            
            float xScale = Math.Min(min, xRatio); // smaller than 1 means image-width is smaller than the container-width
            float yScale = Math.Min(min, yRatio); // smaller than 1 means image-height is smaller than the container-height
      //      Console.WriteLine("XScale :: " + xScale);
       //     Console.WriteLine("YScale :: " + yScale);




            ////float scale = Math.Min(xScale, yScale);
            ////xScale = (float)Math.Min(xScale, min);
            ////yScale = (float)Math.Min(yScale, min);
            //xScale = Math.Min(xScale, min);
            //yScale = Math.Min(yScale, min);


           // int imgWidth = (int)Math.Round(width * xScale);
            
            int imgHeight = (int)Math.Round(height * yScale);
            //apply the actual img dimension ratio to get the true width for the slide, without disturting the image
            int imgWidth = (int)(imgHeight * imgDimRatio);

         //   Console.WriteLine("IMG RATIO == " + imgDimRatio);
         //   Console.WriteLine("imgWidth == " + imgWidth);
          //  Console.WriteLine("imgHeight == " + imgHeight);
         //   Console.WriteLine("imgWidth with ratio " + imgWidth * imgDimRatio);

            int imgX = 0;
            int imgY = 0;

            if (this.centerAligned)
            {
                //@TODO look later
                int xSpace = Math.Max(1, container.Dimension.X - imgWidth);
                int ySpace = Math.Max(1, container.Dimension.Y - imgHeight);

                imgX += xSpace / 2;
                imgY += ySpace / 2;
            }

            rect = new BoundingRectangle(imgX, imgY, imgWidth, imgHeight);

            return rect;
        }
    }
}

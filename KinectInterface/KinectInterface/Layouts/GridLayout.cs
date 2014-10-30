using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Layouts
{
    public class GridLayout : Layout
    {
        private Point gridSize;

        private Rectangle layoutSpace;
        private Rectangle usableSpace;
        private LayoutParams parameters;

        private Point elementDim;

        private int doneEleCount;
        private Point currentGridPos;
        private Point currentLoc;

        public GridLayout(int x, int y, Rectangle layoutspace, LayoutParams layoutParams = null)
        {
            this.gridSize = new Point(x, y);
            
            this.layoutSpace = layoutspace;
            this.usableSpace = new Rectangle();

            this.elementDim = new Point(0,0);

            this.doneEleCount = 0;
            this.currentGridPos = new Point(0, 0);
            this.currentLoc = new Point(0, 0);

            this.parameters = layoutParams;
            this.applyParams();
        }

        public BoundingRectangle getNextElementPos()
        {
            
            if (this.doneEleCount >= (this.gridSize.X * this.gridSize.Y)) return null;
            BoundingRectangle loc = new BoundingRectangle();


            if (this.currentGridPos.X < this.gridSize.X )
            {

            //    Console.WriteLine("POSITION == " + this.currentGridPos.X);
                this.currentLoc.X = 0;
            //    Console.WriteLine("after reset Current LOC X :: " + this.currentLoc.X);
                this.currentLoc.X = this.usableSpace.X;
            //    Console.WriteLine("after usable space Current LOC X :: " + this.currentLoc.X);
                this.currentLoc.X += (this.currentGridPos.X * this.parameters.InBetweenElementSpace.X);
            //    Console.WriteLine("after in between space Current LOC X :: " + this.currentLoc.X);
            //    Console.WriteLine("IN BETWEEN ELE SPACE :: " + this.parameters.InBetweenElementSpace);
                this.currentLoc.X += (this.currentGridPos.X * this.elementDim.X);
           //     Console.WriteLine("After ele dim Current LOC X :: " + this.currentLoc.X);
                this.currentGridPos.X++;
                //if (this.currentLoc.X < 0)
                //{
                //    Console.WriteLine("Current LOC X :: " + this.currentLoc.X);
                //}
            }
            else
            {
                
                this.currentGridPos.X = 1;
                //Console.WriteLine("ELSE POSITION == " + (this.currentGridPos.X -1));
                this.currentLoc.X = this.usableSpace.X;
                //this.currentLoc.X += this.parameters.MarginLeftTop.X;

                this.currentGridPos.Y++;
                this.currentLoc.Y += this.parameters.InBetweenElementSpace.Y;
                this.currentLoc.Y += this.elementDim.Y;
                
            }

            loc = new BoundingRectangle(this.currentLoc.X, this.currentLoc.Y, this.elementDim.X, this.elementDim.Y);

            this.doneEleCount++;
            return loc;
        }

        public void reset()
        {
            this.doneEleCount = 0;
            this.currentGridPos = new Point(0, 0);
            this.currentLoc = this.usableSpace.Location;//new Point(0, 0);
        }

        private void applyParams()
        {
            //Point gridSize = new Point(3, 2);
            //Point elementRatio = new Point(95, 90);
            //Vector2 sizeRatio = new Vector2(elementRatio.X / 100.0f, elementRatio.Y / 100.0f);

            ////get the Screen size
            //Point screenDim = new Point(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            ////set padding size for the screen
            //Point padding = new Point((int)(screenDim.X * 0.10f), (int)(screenDim.Y * 0.10f));
            ////apply padding to the screen size aka. new element space to use
            //Point elementArea = new Point(screenDim.X - padding.X * 2, screenDim.Y - padding.Y * 2);

            //Point ratio = new Point((int)(((elementArea.X) / gridSize.X) * sizeRatio.X), (int)(((elementArea.Y) / gridSize.Y) * sizeRatio.Y));

            //Point spaceBetweenElements = new Point(elementArea.X - (ratio.X * gridSize.X), elementArea.Y - (ratio.Y * gridSize.Y));

            int spaceW = 0, spaceH = 0, spaceX = 0, spaceY = 0;

            spaceW = this.layoutSpace.Width;
            spaceH = this.layoutSpace.Height;
            spaceX = this.layoutSpace.X;
            spaceY = this.layoutSpace.Y;

            int marginLeft = 0,marginRight = 0, marginTop = 0,  marginBottom = 0;

            marginLeft = (int)(this.layoutSpace.Width * this.parameters.MarginLeftTop.X);
            marginRight = (int)(this.layoutSpace.Width * this.parameters.MarginRightBottom.X);
            marginTop = (int)(this.layoutSpace.Height * this.parameters.MarginLeftTop.Y);
            marginBottom = (int)(this.layoutSpace.Height * this.parameters.MarginRightBottom.Y);

            spaceX = spaceX + marginLeft;
            spaceY = spaceY + marginTop;
            spaceW = spaceW - marginRight - spaceX;
            spaceH = spaceH - marginBottom - spaceY;

            this.usableSpace = new Rectangle(spaceX, spaceY, spaceW, spaceH);
            
            // eleBetweenX = (int)(usableW *((eleSizeX - 1) * 0.01f));
            //eleBetweenY = (int)(usableH * ((eleSizeY - 1) * 0.01f));
        
            //System.out.println("ELE_BETWEEN_X :: " + eleBetweenX);
            //System.out.println("ELE_BETWEEN_Y :: " + eleBetweenY);
        
            //int leftusableW = usableW - (eleSizeX - 1) * eleBetweenX;
            //int leftusableH = usableH - (eleSizeY - 1) * eleBetweenY;
        
            //eleWidth = (int)(leftusableW / (eleSizeX));
            //eleHeight = (int)(leftusableH / (eleSizeY));
        
            //System.out.println("ELE_WIDTH :: "  +  eleWidth);
            //System.out.println("ELE_HEIGHT :: " + eleHeight);

            this.parameters.InBetweenElementSpace = new Point(
                (int)(usableSpace.Width * ((this.gridSize.X - 1) * 0.01f)),
                (int)(usableSpace.Height * ((this.gridSize.Y - 1) * 0.02f))
                );

            Point leftoverSpace = new Point(
                this.usableSpace.Width - ((gridSize.X - 1) * this.parameters.InBetweenElementSpace.X),
                this.usableSpace.Height - ((gridSize.Y - 1) * this.parameters.InBetweenElementSpace.Y)
                );

            this.elementDim = new Point(
                (int)(leftoverSpace.X / gridSize.X),
                (int)(leftoverSpace.Y / gridSize.Y)
                );

            //this.elementDim = new Point(
            //    (int)(((this.usableSpace.Width) / this.gridSize.X) * this.parameters.ElementRatio.X),
            //    (int)(((this.usableSpace.Height ) / this.gridSize.Y) * this.parameters.ElementRatio.Y));

            int divx = this.gridSize.X - 1; divx = Math.Max(1, divx);
            int divy = this.gridSize.Y - 1; divy = Math.Max(1, divy);



            //this.parameters.InBetweenElementSpace = new Point(
            //    (int)((this.usableSpace.Width - spaceX) * (1.0f - this.parameters.ElementRatio.X) / (1.0f / divx)),
            //    (int)((this.usableSpace.Height - spaceY) * (1.0f - this.parameters.ElementRatio.Y) / (1.0f / divy))
            //);

            //int betweenspacex = this.elementDim.X * this.gridSize.X;
            //if (gridSize.X > 1)
            //{
            //    betweenspacex = betweenspacex / (gridSize.X - 1);
            //    this.parameters.InBetweenElementSpace = new Point(betweenspacex, this.parameters.InBetweenElementSpace.Y);
            //}


            //this.parameters.InBetweenElementSpace = new Point(
            //    (int)(((this.usableSpace.Width )  / this.gridSize.X) * (1.0f - this.parameters.ElementRatio.X)),
            //    (int)(((this.usableSpace.Height - this.usableSpace.Y) / this.gridSize.Y) * (1.0f - this.parameters.ElementRatio.Y)));

            //Console.WriteLine("LAYOUT PARAMS");
            //Console.WriteLine(usableSpace);
            //Console.WriteLine(this.usableSpace.Width - this.usableSpace.X);
            //Console.WriteLine(this.elementDim);
            //Console.WriteLine(this.parameters.InBetweenElementSpace);
            //Console.WriteLine((this.elementDim.X * this.gridSize.X) + (this.parameters.InBetweenElementSpace.X * (this.gridSize.X - 1)));

            //int eleBtwnSpaceX = 0;
            //int eleBtwnSpaceY = 0;

            //int eSpaceGridSizeX = this.gridSize.X - 1;
            //int eSpaceGridSizeY = this.gridSize.Y - 1;

            //if(eSpaceGridSizeX > 0)
            //    eleBtwnSpaceX = (this.usableSpace.Width - (this.elementDim.X * this.gridSize.X)) / eSpaceGridSizeX;
            //if(eSpaceGridSizeY > 0)
            //    eleBtwnSpaceY = (this.usableSpace.Height - (this.elementDim.Y * this.gridSize.Y)) / eSpaceGridSizeY;

            //this.parameters.InBetweenElementSpace = new Point(eleBtwnSpaceX, eleBtwnSpaceY);

            this.currentLoc = new Point(this.usableSpace.X, this.usableSpace.Y);
        }


        public BoundingRectangle getBottomTextRect()
        {
            return new BoundingRectangle(this.usableSpace.X, (this.usableSpace.Y + this.usableSpace.Height), this.usableSpace.Width, this.layoutSpace.Height - (this.usableSpace.Y + this.usableSpace.Height));
        }

        public BoundingRectangle getTopTextRect()
        {
           // throw new NotImplementedException();

            return null;
        }

        public BoundingRectangle getHelpTextRect()
        {
            int helpWidth = (int)(this.usableSpace.X * 0.95f);
            int helpHeight = (int)(this.usableSpace.Y * 1.00f);

            return new BoundingRectangle(0, 0, helpWidth, helpHeight);
        }


        public BoundingRectangle getRightRect()
        {
            throw new NotImplementedException();
        }

        public BoundingRectangle getLeftRect()
        {
            throw new NotImplementedException();
        }
    }
}

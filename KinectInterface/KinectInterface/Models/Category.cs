using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Models
{
    public class Category
    {
        private String name;
        private List<String> associatedFilenames;

       // private String location;

        public String Name { get { return this.name; } set { this.name = value; } }
        public List<String> Filenames { get { return this.associatedFilenames; } set { this.associatedFilenames = value; } }

        public Category()
        {
            this.name = null;
            this.associatedFilenames = new List<String>();
        }

        public Category(String catName)
        {
            this.name = catName;
            this.associatedFilenames = new List<String>();
        }



    }
}

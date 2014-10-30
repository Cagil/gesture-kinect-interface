using KinectInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface
{
    public class ParsedData
    {
        private List<Category> categories;

        //private List<String[]> categoriesFilenames;

        public List<Category> Categories { get { return this.categories; } set { this.categories = value; } }
       // public List<String[]> Filenames { get { return this.categoriesFilenames; } set { this.categoriesFilenames = value; } }

        public ParsedData()
        {
            this.categories = new List<Category>();
           // this.categoriesFilenames = new List<String[]>();
        }

       // public ParsedData(List<String> cats, List<String[]> catFiles){
        public ParsedData(List<Category> cats){
            this.categories = cats;

        //    this.categoriesFilenames = catFiles;
        }
        
    }
}

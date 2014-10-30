using KinectInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface
{
    public class Parser
    {
        private Dictionary<String, String> settings;
        private String contentLocation;
        private String settingsFileLoc;

        public String GetParam(String key)
        {
            String r = null;
            this.settings.TryGetValue(key, out r);

            return r;
        }
       
        public Parser(String loc = null) {
            this.settings = new Dictionary<string, string>();
            String a = System.IO.Directory.GetCurrentDirectory();
            this.settingsFileLoc =a + @"\Settings.txt";

            IEnumerable<String> settingsText = System.IO.File.ReadLines(this.settingsFileLoc);

            for (int i = 0; i < settingsText.Count<String>(); i++)
            {
                String currentLine = settingsText.ElementAt(i);
                if (currentLine.Contains('#') || currentLine.Length == 0 || !currentLine.Contains('='))
                {
                    continue;
                }
                else
                {
                    
                    String[] parts = currentLine.Split(new char[] { '=' });
                  //  Console.WriteLine("PART 1 = " + parts[0].Trim());
                 //   Console.WriteLine("PART 2 = " + parts[1].Trim());
                    this.settings.Add(parts[0].Trim(), parts[1].Trim());
                }
            }

            //Console.WriteLine("CURRENT DIRECTORY === " + a);

            this.contentLocation = null;
            this.settings.TryGetValue("content_folder_loc", out this.contentLocation);

            

            if (this.contentLocation == null)
            {
                Console.WriteLine("CANNOT FIND CONTENT FOLDER LOCC");
            }
            else
            {
                Console.WriteLine("CURRENT DATA LOC = " + this.contentLocation);
            }

            //if (loc == null) findContentLocation(); else this.contentLocation = loc;
        }

        private void findContentLocation()
        {
            

          //  this.contentLocation = @"C:\\Users\\Cagil\\Documents\\dissertation-kinect-interface\\KinectInterface\\KinectInterfaceContent\\Data\\";
        }

        //public void parseContent(out List<String> categories,out  List<String[]> catFilenames)
        //{
        //   categories = new List<String>();
        //   catFilenames = new List<String[]>();

        //    String[] dirsInfo = System.IO.Directory.GetDirectories(@contentLocation);
        //    String[] dirs = new String[dirsInfo.Length];
        //    List<String> files = new List<String>();
        //    //number of categories and their names
        //    for (int i = 0; i < dirsInfo.Length; i++)
        //    {
        //        dirs[i] = dirsInfo[i].Remove(0, contentLocation.Length);
        //        categories.Add(dirs[i]);
        //        //   Console.WriteLine(dirs[i]);

        //        //file names for each directory
        //        String[] fles = System.IO.Directory.GetFiles(dirsInfo[i]);
        //        for (int j = 0; j < fles.Length; j++)
        //        {
        //            fles[j] = fles[j].Remove(0, contentLocation.Length + dirs[i].Length + 1);
        //            //  Console.WriteLine("FILE :: " + fles[j]);
        //        }
        //        catFilenames.Add(fles);
        //    }

        //    //return new ParsedData(categories, catFilenames);
        //}

        public ParsedData parseContent()
        {
          //  List<String> categories = new List<String>();
           // List<String[]> catFilenames = new List<String[]>();
            List<Category> categories = new List<Category>();
            ParsedData data = new ParsedData();
            Category tempCat;

            String[] dirsInfo = System.IO.Directory.GetDirectories(@contentLocation);
            String[] dirs = new String[dirsInfo.Length];
         //   List<String> files = new List<String>();
            //number of categories and their names
            for (int i = 0; i < dirsInfo.Length; i++)
            {
                dirs[i] = dirsInfo[i].Remove(0, contentLocation.Length);
                //categories.Add(dirs[i]);
                //   Console.WriteLine(dirs[i]);
                tempCat = new Category();
                tempCat.Name = dirs[i];
                //file names for each directory
                String[] fles = System.IO.Directory.GetFiles(dirsInfo[i]);
                for (int j = 0; j < fles.Length; j++)
                {
                    if(fles[j].Contains(".wmv")) continue;
                    //fles[j] = fles[j].Remove(0, contentLocation.Length + dirs[i].Length + 1);
                    //  Console.WriteLine("FILE :: " + fles[j]);
                    tempCat.Filenames.Add(fles[j].Remove(0, contentLocation.Length + dirs[i].Length + 1));
                }
                
                
                //catFilenames.Add(fles);
                categories.Add(tempCat);
            }

            data.Categories = categories;

            return data;
            //return new ParsedData(categories, catFilenames);
        }
    }
}

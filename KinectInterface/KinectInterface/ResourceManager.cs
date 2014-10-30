using KinectInterface.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KinectInterface
{
    public class ResourceManager
    {
        private Boolean isDataLoaded;
        private Boolean isTexturesLoaded;
        //private List<String> categories;
        //private List<String[]> categoryFilenames;
        private List<Category> catModelCol;
        private Dictionary<String, Texture2D> textures;

        private Texture2D cursorTexture;
      //  public static Dictionary<String, Texture2D> models = new Dictionary<string,Texture2D>();

        private String contentLocation;
        private String dataFolderName;
        private String textureFolderName;
        private GraphicsDevice gDevice;
       // private Dictionary<String, Video> videos;

        public Boolean DataLoaded { get { return this.isDataLoaded; } }
        public Boolean TexturesLoaded { get { return this.isTexturesLoaded; } }

        public String DataFolderName { get { return this.dataFolderName; } set { this.dataFolderName = value; } }
        public String TextureFolderName { get { return this.textureFolderName; } set { this.textureFolderName = value; } }
        public String ContentLocation { get { return this.contentLocation; } }
        public List<Category> CategoryModel { get { return this.catModelCol; } set { this.catModelCol = value; } }
        public Texture2D CursorTexture { get { return this.cursorTexture; } }

        

        public ResourceManager(GraphicsDevice graphicsDevice, String location)
        {
            this.gDevice = graphicsDevice;

            this.contentLocation = location;

            this.catModelCol = new List<Category>();
            this.textures = new Dictionary<string,Texture2D>();

            this.isDataLoaded = false;
            this.isTexturesLoaded = false;
            
           // videos = new Dictionary<string,Video>();
        }

        public void LoadContent()
        {

            for (int i = 0; i < this.catModelCol.Count; i++)
            {
                Category currCat = this.catModelCol.ElementAt(i);
                for (int j = 0; j < currCat.Filenames.Count; j++)
                {
                    String file = currCat.Filenames.ElementAt(j);
                    if (file.Contains(".jpg") || file.Contains(".png"))
                    {
                        Console.WriteLine("LOADING MEDIA FILES :: " + file); 
                        FileStream stream = new FileStream(this.contentLocation + this.catModelCol.ElementAt(i).Name + "\\" + file, FileMode.Open);


                        Texture2D ttmp = Texture2D.FromStream(gDevice, stream);
                        this.textures.Add(currCat.Name + "_" + file, ttmp);
                        stream.Flush();
                        stream.Close();
                        
                        //stream = null;
                      //  models.Add(currCat.Name + "_" + file, ttmp);
                    
                    }
                }

            }
           
        }

        public static Texture2D loadTextureOnce(GraphicsDevice gd, String filename)
        {
            if (filename == "DEBUGbg")
            {
                Texture2D t = new Texture2D(gd, 1, 1);
                t.SetData(new[] { Color.Black });
                return t;
            }

            FileStream stream = new FileStream(
               "C:\\Users\\Cagil\\Documents\\dissertation-kinect-interface\\KinectInterface\\KinectInterfaceContent\\Textures\\" + filename, FileMode.Open);


            Texture2D ttmp = Texture2D.FromStream(gd, stream);
            stream.Close();

            return ttmp;
        }

        public void LoadTextures()
        {
            FileStream stream = new FileStream(
                "C:\\Users\\Cagil\\Documents\\dissertation-kinect-interface\\KinectInterface\\KinectInterfaceContent\\Textures\\"+ "hand.png", FileMode.Open);


            Texture2D ttmp = Texture2D.FromStream(this.gDevice, stream);
            stream.Close();
            this.cursorTexture = ttmp;
            this.isTexturesLoaded = true;
        }

        private Texture2D loadTexture(String catName , String filename)
        {
            
            //Console.WriteLine("LOADING MEDIA FILES :: " + file);
            FileStream stream = new FileStream(this.contentLocation  + catName + "\\" + filename, FileMode.Open);


            Texture2D ttmp = Texture2D.FromStream(gDevice, stream);
            this.textures.Add(catName + "_" + filename, ttmp);
            stream.Close();

            return ttmp;
        }
        

        public Texture2D getTexture(String categoryname, String filename)
        {
            Texture2D found = null;
            this.textures.TryGetValue(categoryname + "_" + filename, out found);

            if (found == null)
            {
                found = loadTexture(categoryname, filename);
            }

            return found;

        }

        public Texture2D getTexture(String filename)
        {
            if (filename.Length == 0) return null;
            Texture2D found = null;
            this.textures.TryGetValue(filename, out found);

            if (found == null)
            {
                //Console.WriteLine("FILENAME == " + filename);
                String[] tokenizor = new String[1];
                tokenizor[0] = "_";
                String[] mediaFileParts = filename.Split(tokenizor, StringSplitOptions.None);

                found = loadTexture(mediaFileParts[0], mediaFileParts[1]);

            }

            return found;
        }
    }
}

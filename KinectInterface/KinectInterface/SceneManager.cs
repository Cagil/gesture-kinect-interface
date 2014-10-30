using KinectInterface.Messages;
using KinectInterface.UI;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface
{
    public class SceneManager : Receiver<AbstractUI>
    {
        private IMediator hub;

        private int mainSceneId;

        private UIScene currentScene;
        private UIScene currentSlide;

        private int currSlideIndex;
        private Boolean autoPilot;

        private int slideChangeTime;
        private int slideChangeTimer;

        private int maxQueue;
        private Stack<UIScene> SceneQueue;

        private Dictionary<int, UIScene> HelpSceneCollection;
        private Dictionary<int, UIScene> SceneCollection;
        private Dictionary<int, UIScene> SlideCollection;

        private int lastknowmenuid;

        private bool setup_;

        public Dictionary<int, UIScene> HelpScenes { get { return this.HelpSceneCollection; } set { this.HelpSceneCollection = value; } }
        public UIScene CurrentScene { get { if (this.autoPilot) return this.currentSlide; else return this.currentScene; } }
        public Dictionary<int, UIScene> Scenes { set { this.SceneCollection = value; this.setUpSlidesFromCollection(); } }
        public IMediator Hub { set { this.hub = value; } }
        public Boolean AutoPilot { get { return this.autoPilot; } set { this.autoPilot = value; } }
        public int MainMenuSceneId { get { return this.mainSceneId; } set { this.mainSceneId = value; if(setup_ == false) this.StartingSceneId = value; } }
        public int SlideCycleTimeLimit { get { return this.slideChangeTimer; }
            set
            {
                if (value > 5000 && value < 10000)
                {
                    this.slideChangeTimer = value;
                }
                else
                {
                    this.slideChangeTimer = 6000;
                }
            } 
        }

        public int StartingSceneId {
            set
            {
                if (this.setup_ == false)
                {
                    if (this.setUpStartingScene_(value))
                    {
                        this.setup_ = true;
                    }
                }
            }
        }

        public SceneManager()
        {
            this.maxQueue = 10;
            this.currentScene = null;
            this.currentSlide = null;
            this.SceneQueue = new Stack<UIScene>(maxQueue);
            this.SceneCollection = new Dictionary<int, UIScene>();
            this.SlideCollection = new Dictionary<int, UIScene>();
            this.HelpSceneCollection = new Dictionary<int, UIScene>();
            this.currSlideIndex = 0;
            this.lastknowmenuid = 0;
            this.slideChangeTime = 0;
            this.slideChangeTimer = 7500;
            this.setup_ = false;
           
        }

        public SceneManager(Dictionary<int, UIScene> allScenes)
        {
            this.maxQueue = 10;
            this.currentScene = null;
            this.currentSlide = null;
            this.SceneQueue = new Stack<UIScene>(maxQueue);
            this.SceneCollection = allScenes;
            this.SlideCollection = new Dictionary<int, UIScene>();
            this.HelpSceneCollection = new Dictionary<int, UIScene>();
            this.setUpSlidesFromCollection();
            this.currSlideIndex = 0;

            this.slideChangeTime = 0;
            this.slideChangeTimer = 7500;
            this.setup_ = false;


        }

        public void Update(GameTime gameTime)
        {
            if (this.autoPilot)
            {
                if (this.currentSlide != null)
                    this.updateSlideCycleTimer(gameTime);
                else
                    this.autoPilot =  !this.autoPilot;
            }

            this.CurrentScene.isAutoPilotOn = this.autoPilot;
            this.CurrentScene.Update(gameTime);
        }

        public void toggleAutoPilot()
        {
            

            this.autoPilot = !this.autoPilot;
        }

        

        public void Receive(Message<AbstractUI> message)
        {
            if (this.autoPilot) return;
            this.currentScene.Receive(message);
        }

        public void ReceiveRecognizedGesture(String name)
        {
            if (this.autoPilot) return;
            Console.WriteLine("SCENE MANAGER RECEIVED RECOGNIZED GESTURE - NAME = " + name);
            this.currentScene.onGestureNoticed(name);
        }

        //public void checkOnPush(int x, int y)
        //{
        //    this.CurrentScene.onTouch(new Microsoft.Xna.Framework.Vector2(x, y));
        //}

        //public void checkOnMove(int x, int y)
        //{
        //    if (!this.CurrentScene.onFocus(new Microsoft.Xna.Framework.Vector2(x, y)))
        //    {
        //        this.hub.resetPushTimer();
        //    }
        //}

        private bool setUpStartingScene_(int id){
            UIScene found = null;
            this.SceneCollection.TryGetValue(id, out found);

            if (found != null)
            {              
                this.currentScene = found;
                this.hub.sendInterestedGestureList(this.currentScene.InterestedGestures);
                return true;
            }
            else
            {
                Console.WriteLine("UISCENE ID-" + id +" IS NOT FOUND");
                return false;
            }
        }

        public void jumpTo(int sceneId)
        {
            if (this.autoPilot) return;
            UIScene found = null;
            this.SceneCollection.TryGetValue(sceneId, out found);

            if (found != null)
            {
                if (this.currentScene != null)
                {
                    this.resetCurrentScene();

                    if (this.canAddToStack(found))
                    {
                        this.SceneQueue.Push(this.currentScene);
                        this.lastknowmenuid = this.currentScene.UIID;
                    }

                    //Console.WriteLine("PREV SCENES, STACK LIST");
                    //for (int i = 0; i < this.SceneQueue.Count; i++)
                    //{
                    //    Console.WriteLine(this.SceneQueue.ElementAt(i).Label.Text + " :::  " + this.SceneQueue.ElementAt(i).UIID);
                    //}
                }
                this.lastknowmenuid = found.UIID;
                this.changeScene(found);
                return;
            }
            else
            {
                Console.WriteLine("UISCENE ID-" + sceneId + " IS NOT FOUND");
            }

            this.HelpSceneCollection.TryGetValue(sceneId, out found);
            if (found != null)
            {
                if (this.currentScene != null)
                {
                    this.resetCurrentScene();
                }

                this.changeScene(found);
            }
        }

        public void jumpToLastKnownMenuScene()
        {
            if (this.autoPilot != true)
            {
                UIScene lastmenuscene = null;
                this.SceneCollection.TryGetValue(this.lastknowmenuid, out lastmenuscene);
                //if (lastmenuscene != null)
                if (lastmenuscene != null)
                {
                    changeScene(lastmenuscene);
                }
                else
                {
                    UIScene mainmenuscene = null;
                    this.SceneCollection.TryGetValue(this.MainMenuSceneId, out mainmenuscene);
                    if(mainmenuscene != null)
                        changeScene(mainmenuscene);
                }
            }
        }

        private void updateSlideCycleTimer(GameTime gameTime)
        {
            if (this.autoPilot)
            {
                this.slideChangeTime += gameTime.ElapsedGameTime.Milliseconds;
                if (this.slideChangeTime >= this.slideChangeTimer)
                {
                    this.cycleSlides();
                    this.slideChangeTime = 0;
                }
            }
        }

        private void cycleSlides()
        {
            if (this.autoPilot)
            {
                if (this.SlideCollection.Count > 0)
                {
                    this.currentSlide = this.SlideCollection.ElementAt(this.currSlideIndex).Value;

                    if (this.currSlideIndex + 1 < this.SlideCollection.Count)
                    {
                        this.currSlideIndex++;
                    }
                    else
                    {
                        this.currSlideIndex = 0;
                    }
                }

            }
        }

        private void changeScene(UIScene newScene)
        {
            this.currentScene = newScene;
            this.hub.sendInterestedGestureList(this.currentScene.InterestedGestures);       
        }

        public void changeToPrev()
        {
            if (this.autoPilot) return;
            if (this.SceneQueue.Count > 0)
            {
                UIScene prev = null;
                prev = this.SceneQueue.Pop();
                Console.WriteLine(" " + prev.UIID + " is chosen to go back to");

                this.changeScene(prev);
            }

            
        }

        private bool canAddToStack(UIScene newScene)
        {
            bool result = true;
            if (newScene.hasNext())
            {
                if (newScene.getNext().UIID == this.currentScene.UIID)
                {
                    result = false;
                }
            }

            if(newScene.hasPrev())
            {
                if (newScene.getPrev().UIID == this.currentScene.UIID)
                {
                    result = false;
                }
            }

            return result;
        }

        private void resetCurrentScene()
        {
            UIScene prevScene = null;
            int prevSceneId = this.currentScene.UIID;
            this.SceneCollection.TryGetValue(prevSceneId, out prevScene);
            if (prevScene == null)
            {
                this.HelpSceneCollection.TryGetValue(prevSceneId, out prevScene);
            }
            if(prevScene != null)
                prevScene.Receive(new ResetMessage());
        }

        //public void addSlide(UIScene obj)
        //{
            
        //}

        private void setUpSlidesFromCollection()
        {
           

            for (int i = 0; i < this.SceneCollection.Count; i++)
            {
                if (this.SceneCollection.ElementAt(i).Value.IsASlide)
                {
                    this.SlideCollection.Add(this.SceneCollection.ElementAt(i).Value.UIID, this.SceneCollection.ElementAt(i).Value);
                }
            }

            if(this.SlideCollection.Count > 0)
                this.currentSlide = this.SlideCollection.ElementAt(0).Value;
        }



    }
}

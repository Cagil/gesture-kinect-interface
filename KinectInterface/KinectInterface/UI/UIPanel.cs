using KinectInterface.Messages;
using KinectInterface.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.UI
{
    class UIPanel : AbstractUI, Sender<AbstractUI>
    {
        private List<Receiver<AbstractUI>> elements;

        public UIPanel(Driver driver, String label = "Panel") : base(driver)
        {
            this.elements = new List<Receiver<AbstractUI>>();
            this.BoundingRectangle = new BoundingRectangle(0, 0, 0, 0); //(0, 0, driver.Window.ClientBounds.Width, driver.Window.ClientBounds.Height);
            this.Label = new UIText(driver, label);
        }

        public UIPanel(Driver driver, List<AbstractUI> uis, String label = "Panel") : base(driver)
        {
            
            this.elements = new List<Receiver<AbstractUI>>();
            this.BoundingRectangle = BoundingRectangle.CreateFromGroup(uis);
            this.Label = new UIText(driver, label);

            this.registerUiList(uis);
        }

        private void registerUiList(List<AbstractUI> uis){
            foreach(AbstractUI ui in uis)
                this.addReceiver(ui);
        }

        public override void LoadContent(ref ResourceManager resources, GraphicsDevice gd)
        {
           // this.Texture = new Texture2D(gd, 1, 1);
           // this.Texture.SetData(new[] { this.CurrentColor });
            this.Label.LoadContent(ref resources, gd);
            LoadContentMessage message = new LoadContentMessage(ref resources, gd);
            broadcast(message);
        }

        public override void Initialize()
        {
            InitializeMessage message = new InitializeMessage();
            broadcast(message);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            UpdateMessage message = new UpdateMessage(gameTime);
            broadcast(message);
        }

        public override void Draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch sp, Microsoft.Xna.Framework.GameTime gameTime)
        {
            DrawMessage message = new DrawMessage(ref sp, gameTime);
          
            broadcast(message);
        }

        public override void Receive(Utils.Message<AbstractUI> message)
        {
            message.open(this);
            if (this.State != UIState.InteractionState.IDLE)
            {
                broadcast(message);
            }
           
        }

        public void addReceiver(Receiver<AbstractUI> newReceiver)
        {
            this.elements.Add(newReceiver);
        }

        public void removeReceiver(Receiver<AbstractUI> receiver)
        {
            this.elements.Remove(receiver);
        }

        public void broadcast(Message<AbstractUI> message)
        {
            for (int i = 0; i < this.elements.Count; i++)
            {
                this.elements.ElementAt(i).Receive(message);
            }
        }

        public override void Reset()
        {
            base.Reset();

            broadcast(new ResetMessage());
        }

    }
}

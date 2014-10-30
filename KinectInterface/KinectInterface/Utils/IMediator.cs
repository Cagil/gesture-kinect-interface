using KinectInterface.Gestures;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectInterface.Utils
{
    public interface IMediator
    {

        void sendInterestedGestureList(List<String> gestureNames);
        void sendNoticedGesture(String name);
        void sendAutoPilotStartRequest();
        void sendAutoPilotStopRequest();
     //   void sendInputOnMove(int x, int y);
     //   void sendInputOnPush(int x, int y);
     //   void resetPushTimer();
        Dictionary<JointType, Joint> getSkeleton();
        Point get2DCursorPosition();
        bool IsKinectReady();
        bool IsPersonDetected();
        

        //void update(GameTime gameTime);
        void registerInputManager(ref InputManager im);
        void registerSceneManager(ref SceneManager sm);
        void registerKinectManager(ref KinectManager km);
        void registerGesturePool(ref GestureFactory gf);
    }
}

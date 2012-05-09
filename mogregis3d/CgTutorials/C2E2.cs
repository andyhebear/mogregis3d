using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.CgTutorials
{
    /// <summary>
    /// Mogre-based example using a Cg
    /// vertex and a Cg fragment programs from Chapter 2 of "The Cg Tutorial"
    /// (Addison-Wesley, ISBN 0321194969). 
    /// </summary>
    class C2E2Tutorial : Mogre.Demo.ExampleApplication.Example
    {
        static void DrawStar(ManualObject obj, float x, float y, int starPoints, float R, float r)
        {
            int i;
            float piOverStarPoints = 3.14159f / starPoints,
                   angle = 0.0f;

            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_FAN;
            obj.Begin("CgTutorials/C2E2_green_Material", operation);
            obj.Position(x, y, 0.0f);  /* Center of star */
            /* Emit exterior vertices for star's points. */
            for (i = 0; i < starPoints; i++)
            {
                obj.Position(x + R * Math.Cos(angle), y + R * Math.Sin(angle), 0.0f);
                angle += piOverStarPoints;
                obj.Position(x + r * Math.Cos(angle), y + r * Math.Sin(angle), 0.0f);
                angle += piOverStarPoints;
            }
            /* End by repeating first exterior vertex of star. */
            angle = 0;
            obj.Position(x + R * Math.Cos(angle), y + R * Math.Sin(angle), 0.0f);
            obj.End();
        }

        public override void CreateScene()
        {
            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial02Node");

            ManualObject manualObj1 = sceneMgr.CreateManualObject("Tutorial02Object1");
            ManualObject manualObj2 = sceneMgr.CreateManualObject("Tutorial02Object2");
            ManualObject manualObj3 = sceneMgr.CreateManualObject("Tutorial02Object3");
            ManualObject manualObj4 = sceneMgr.CreateManualObject("Tutorial02Object4");
            ManualObject manualObj5 = sceneMgr.CreateManualObject("Tutorial02Object5");
            ManualObject manualObj6 = sceneMgr.CreateManualObject("Tutorial02Object6");
            
            /*                                  star    outer   inner  */
            /*                     x      y     Points  radius  radius */
            /*                  =====  =====  ======  ======  ====== */
            DrawStar(manualObj1, -0.1f, 0.0f, 5, 0.5f, 0.2f);
            DrawStar(manualObj2, -0.84f, 0.1f, 5, 0.3f, 0.12f);
            DrawStar(manualObj3, 0.92f, -0.5f, 5, 0.25f, 0.11f);
            DrawStar(manualObj4, 0.3f, 0.97f, 5, 0.3f, 0.1f);
            DrawStar(manualObj5, 0.94f, 0.3f, 5, 0.5f, 0.2f);
            DrawStar(manualObj6, -0.97f, -0.8f, 5, 0.6f, 0.2f);
           
            node1.AttachObject(manualObj1);
            node1.AttachObject(manualObj2);
            node1.AttachObject(manualObj3);
            node1.AttachObject(manualObj4);
            node1.AttachObject(manualObj5);
            node1.AttachObject(manualObj6);
        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            // Position it at 30 in Z direction
            camera.Position = new Vector3(0, 0, 5);
            // Look at our triangle
            camera.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
            camera.NearClipDistance = 1;
        }

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Chapter2", "FileSystem", "General");
        }

        public override void CreateViewports()
        {
            base.CreateViewports();
            viewport.BackgroundColour = new ColourValue(0.1f, 0.3f, 0.6f, 0.0f);  /* Blue background */
        }

    }
}
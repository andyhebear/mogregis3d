using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.CgTutorials
{
    /// <summary>
    /// Mogre-based example using a Cg
    /// vertex and a Cg fragment programs from Chapter 3 of "The Cg Tutorial"
    /// Addison-Wesley, ISBN 0321194969). 
    /// </summary>
    class C3E3Tutorial : Mogre.Demo.ExampleApplication.Example
    {
        static ManualObject DrawTriangle(ManualObject obj)
        {
            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_LIST;

            obj.Begin("CgTutorials/C3E3_Material", operation);
            obj.Position(0.0f, -0.8f, 0.0f);
            obj.TextureCoord(0, 0); 

            obj.Position(0.8f, 0.8f, 0.0f);
            obj.TextureCoord(1, 0);

            obj.Position(-0.8f, 0.8f, 0.0f);
            obj.TextureCoord(0.5f, 1);

            obj.End();

            return obj;
        }

        public override void CreateScene()
        {
            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial05Node1");

            ManualObject manualObj1 = sceneMgr.CreateManualObject("Tutorial05Object1");
            node1.AttachObject(DrawTriangle(manualObj1));
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
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Chapter3", "FileSystem", "General");
        }

        public override void CreateViewports()
        {
            base.CreateViewports();
            viewport.BackgroundColour = new ColourValue(0.1f, 0.3f, 0.6f, 0.0f);  /* Blue background */
        }

    }
}
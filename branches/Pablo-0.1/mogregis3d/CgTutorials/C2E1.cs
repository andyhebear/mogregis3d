using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.CgTutorials
{
    /// <summary>
    /// Mogre-based very simple vertex program example
    /// using Cg program from Chapter 2 of "The Cg Tutorial" (Addison-Wesley,
    /// ISBN 0321194969).
    /// </summary>
    class C2E1Tutorial : Mogre.Demo.ExampleApplication.Example
    {
        ManualObject theObj;

        public ManualObject CreateNode(string name, SceneManager sceneMgr, bool useScreenPos)
        {
            ManualObject manualObj = sceneMgr.CreateManualObject(name);
            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_LIST;

            if (useScreenPos)
                manualObj.Begin("CgTutorials/C2E1v_green_Material", operation);
            else
                manualObj.Begin("CgTutorials/C2E1v_green_Material_2", operation);

            manualObj.Position(-0.5f, 0.0f, 0.0f);
            manualObj.Position(0.5f, 0.0f, 0.0f);
            manualObj.Position(0.0f, 0.5f, 0.0f);

            manualObj.End();

            return manualObj;
        }

        public override void CreateScene()
        {
            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial01Node");
            theObj = CreateNode("Tutorial01Object", base.sceneMgr, true);
            node1.AttachObject(theObj);
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
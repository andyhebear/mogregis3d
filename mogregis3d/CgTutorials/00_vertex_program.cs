using System;
using System.Collections.Generic;
using System.Text;

using Mogre.Helpers;

namespace Mogre.Demo.CgTutorials
{
    /// <summary>
    /// Ogre-based very simple example using ManualObject.
    /// </summary>
    class T00VertexProgram : Mogre.Demo.ExampleApplication.Example
    {
        public ManualObject CreateNode(string name, SceneManager sceneMgr)
        {
            ManualObject manualObj = sceneMgr.CreateManualObject(name);
            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_LIST;
            MaterialPtr material = MaterialManager.Singleton.Create("Test/Tutorial00",
                                                                    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            material.GetTechnique(0).GetPass(0).VertexColourTracking =
                           (int)TrackVertexColourEnum.TVC_AMBIENT;
            
            manualObj.Begin("Test/Tutorial00", operation);

            manualObj.Position(-5.0f, 0.0f, 0.0f);
            manualObj.Position(5.0f, 0.0f, 0.0f);
            manualObj.Position(0.0f, 5.0f, 0.0f);

            manualObj.End();

            return manualObj;
        }

        public override void CreateScene()
        {
            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial00Node");
            node1.AttachObject(CreateNode("Tutorial00Object", base.sceneMgr));
        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            // Position it at 30 in Z direction
            camera.Position = new Vector3(0, 0, 20);
            // Look at our triangle
            camera.LookAt(new Vector3(0.0f, 2.5f, 0.0f));
            camera.NearClipDistance = 1;
        }
    }
}
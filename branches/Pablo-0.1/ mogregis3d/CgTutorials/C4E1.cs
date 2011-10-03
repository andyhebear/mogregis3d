using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.CgTutorials
{
    /// <summary>
    /// Mogre-based very simple vertex program example
    /// using Cg program from Chapter 4 of "The Cg Tutorial" (Addison-Wesley,
    /// ISBN 0321194969).
    /// </summary>
    class C4E1Tutorial : Mogre.Demo.ExampleApplication.Example
    {
        AnimationState animState = null;

        bool FrameStarted(FrameEvent evt)
        {
            animState.AddTime(evt.timeSinceLastFrame);
            return true;
        }

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();
            Root.Singleton.FrameStarted += FrameStarted;
        }

        public override void CreateScene()
        {
            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial08Node");
            Entity entity = base.sceneMgr.CreateEntity("Tutorial08Entity", SceneManager.PrefabType.PT_SPHERE);
            entity.SetMaterialName("CgTutorials/C4E1_Material");
            node1.AttachObject(entity);

            // Make sure the camera track this node
            camera.SetAutoTracking(true, node1);

            // Create the camera node & attach camera
            SceneNode camNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            camNode.AttachObject(camera);

            // set up spline animation of node
            Animation anim = sceneMgr.CreateAnimation("CameraTrack", 10F);

            // Spline it for nice curves

            anim.SetInterpolationMode(Animation.InterpolationMode.IM_SPLINE);
            // Create a track to animate the camera's node
            NodeAnimationTrack track = anim.CreateNodeTrack(0, camNode);

            // Setup keyframes
            TransformKeyFrame key = track.CreateNodeKeyFrame(0F); // startposition
            key = track.CreateNodeKeyFrame(2.5F);
            key.Translate = new Vector3(500F, 500F, -1000F);
            key = track.CreateNodeKeyFrame(5F);
            key.Translate = new Vector3(-1500F, 1000F, -600F);
            key = track.CreateNodeKeyFrame(7.5F);
            key.Translate = new Vector3(0F, 100F, 0F);
            key = track.CreateNodeKeyFrame(10F);
            key.Translate = new Vector3(0F, 0F, 0F);

            // Create a new animation state to track this
            animState = sceneMgr.CreateAnimationState("CameraTrack");
            animState.Enabled = true;

            camera.PolygonMode = PolygonMode.PM_WIREFRAME;
        }

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Chapter4", "FileSystem", "General");
        }

        public override void CreateViewports()
        {
            base.CreateViewports();
            viewport.BackgroundColour = new ColourValue(0.1f, 0.3f, 0.6f, 0.0f);  /* Blue background */
        }

    }
}
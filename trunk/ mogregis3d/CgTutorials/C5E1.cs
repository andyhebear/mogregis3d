using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.CgTutorials
{
    /// <summary>
    /// Mogre-based per-vertex lighting example
    /// using Cg program from Chapter 5 of "The Cg Tutorial" (Addison-Wesley,
    /// ISBN 0321194969).
    /// </summary>
    class C5E1Tutorial : Mogre.Demo.ExampleApplication.Example
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
            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial09Node");

            // Create a point light
            Light l = base.sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // Add light to the scene node
            SceneNode lightNode = base.sceneMgr.RootSceneNode.CreateChildSceneNode();
            lightNode.CreateChildSceneNode(new Vector3(10, 10, 10)).AttachObject(l);

            MeshPtr pMesh = MeshManager.Singleton.Load("knot.mesh",
                                         ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                                         HardwareBuffer.Usage.HBU_DYNAMIC_WRITE_ONLY,
                                         HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY,
                                         true,
                                         true);//can still read it
            //ushort src, dest;
            //if (!pMesh.SuggestTangentVectorBuildParams(VertexElementSemantic.VES_TANGENT, out src, out dest))
            //    pMesh.BuildTangentVectors(VertexElementSemantic.VES_TANGENT, src, dest);

            //create entity
            Entity entity = sceneMgr.CreateEntity("Tutorial09Entity", "knot.mesh");
            entity.SetMaterialName("CgTutorials/C5E1_Material");
            //attach to main node
            node1.AttachObject(entity);

            // set up spline animation of node
            Animation anim = sceneMgr.CreateAnimation("LightTrack", 10F);

            // Spline it for nice curves

            anim.SetInterpolationMode(Animation.InterpolationMode.IM_SPLINE);
            // Create a track to animate the camera's node
            NodeAnimationTrack track = anim.CreateNodeTrack(0, lightNode);

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
            animState = sceneMgr.CreateAnimationState("LightTrack");
            animState.Enabled = true;


        }

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Chapter5", "FileSystem", "General");
        }

        public override void CreateViewports()
        {
            base.CreateViewports();
            viewport.BackgroundColour = new ColourValue(0.1f, 0.3f, 0.6f, 0.0f);  /* Blue background */
        }

    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.CgTutorials
{
    /// <summary>
    /// A small example about how to render to texture and using it 
    /// </summary>
    class RenderToTextureTutorial : Mogre.Demo.ExampleApplication.Example
    {
        ManualObject manualObj1;
        ManualObject manualObj2;
        SceneNode node1;
        SceneNode node2;
        const uint constantColor = 123;
        RenderTarget rttTex;
        float time = 0.0f;
        bool isBottom = false;

        bool FrameStarted(FrameEvent evt)
        {
            time -= evt.timeSinceLastFrame;
            if (time <= 0.0f)
            {
                Viewport v = rttTex.GetViewport(0);
                if (isBottom)
                {
                    v.SetDimensions(0.0f, 0.5f, 1.0f, 0.5f);
                    isBottom = false;
                }
                else
                {
                    v.SetDimensions(0, 0, 1.0f, 0.5f);
                    isBottom = true;
                }
                rttTex.Update();
                time = 2.0f;
            }
            return true;
        }

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();
            Root.Singleton.FrameStarted += FrameStarted;
        }

        static ManualObject DrawTriangle1(ManualObject obj)
        {
            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_LIST;

            obj.Begin("CgTutorials/RenderToTexture_Material2", operation);

            obj.Position(0.8f, -0.8f, 0.0f);
            obj.TextureCoord(1, 1);

            obj.Position(0.8f, 0.8f, 0.0f);
            obj.TextureCoord(1, 0);

            obj.Position(-0.8f, -0.8f, 0.0f);
            obj.TextureCoord(0.0f, 1);
            obj.End();

            return obj;
        }

        static ManualObject DrawTriangle2(ManualObject obj)
        {
            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_STRIP;

            obj.Begin("CgTutorials/RenderToTexture_Material", operation);
            obj.Position(-0.8f, 0.8f, 0.0f);
            obj.TextureCoord(0, 0);

            obj.Position(-0.8f, -0.8f, 0.0f);
            obj.TextureCoord(0.0f, 1);

            obj.Position(0.8f, 0.8f, 0.0f);
            obj.TextureCoord(1, 0);

            obj.Position(0.8f, -0.8f, 0.0f);
            obj.TextureCoord(1, 1);

            obj.End();

            return obj;
        }

        public override void CreateScene()
        {
            TexturePtr mTexture = TextureManager.Singleton.CreateManual("RenderArea",
                                      ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_2D,
                                      512, 512, 0, PixelFormat.PF_R8G8B8, (int)TextureUsage.TU_RENDERTARGET);
            rttTex = mTexture.GetBuffer().GetRenderTarget();
            rttTex.IsAutoUpdated = false;
            {
                // Create the camera
                Camera camera2 = sceneMgr.CreateCamera("PlayerCam2");

                camera2.Position = new Vector3(0, 0, 3);
                camera2.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
                camera2.NearClipDistance = 1;

                Viewport v = rttTex.AddViewport(camera2);

                MaterialPtr mat = MaterialManager.Singleton.GetByName("CgTutorials/RenderToTexture_Material");
                mat.GetTechnique(0).GetPass(0).GetTextureUnitState(0).SetTextureName("RenderArea");
                v.BackgroundColour = new ColourValue(0.0f, 0.3f, 0.2f, 0.0f);
                //v.SetClearEveryFrame(false);
                //v.OverlaysEnabled = false;
                rttTex.PreRenderTargetUpdate += new RenderTargetListener.PreRenderTargetUpdateHandler(RenderArea_PreRenderTargetUpdate);
                rttTex.PostRenderTargetUpdate += new RenderTargetListener.PostRenderTargetUpdateHandler(RenderArea_PostRenderTargetUpdate);
            }

            node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("TutorialRender2TexNode1");
            node2 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("TutorialRender2TexNode2");

            manualObj1 = sceneMgr.CreateManualObject("TutorialRender2TexObject1");
            manualObj2 = sceneMgr.CreateManualObject("TutorialRender2TexObject2");

            node1.AttachObject(DrawTriangle1(manualObj1));
            node2.AttachObject(DrawTriangle2(manualObj2));

        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            camera.Position = new Vector3(0, 0, 2);
            camera.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
            camera.NearClipDistance = 1;
        }

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Other", "FileSystem", "General");
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Chapter3", "FileSystem", "General");
        }

        public override void CreateViewports()
        {
            base.CreateViewports();
            viewport.BackgroundColour = new ColourValue(0.1f, 0.3f, 0.6f, 0.0f);  /* Blue background */
        }

        Random ran = new Random();
        void RenderArea_PreRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            // Random color
            Vector4 color = new Vector4((float)ran.NextDouble(), (float)ran.NextDouble(), (float)ran.NextDouble(), 1.0f);
            manualObj1.GetSection(0).SetCustomParameter(constantColor, color);
            manualObj1.Visible = true;
            manualObj2.Visible = false;
        }

        void RenderArea_PostRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            manualObj1.Visible = false;
            manualObj2.Visible = true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using MogreFramework;

using osgGISProjects;

using System.Windows.Forms;
using System.Drawing;

namespace Mogre.Demo.CameraTrack
{
    class Program
    {
        static void Main(string[] args)
        //static void Main()
        {
            try
            {
                MogreGisApplication app = new MogreGisApplication();
                //new SceneCreator(app);
                app.Go();
            }
            catch (System.Runtime.InteropServices.SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    ExampleApplication.Example.ShowOgreException();
                else
                    throw;
            }
        }
    }
#if TODO_DANI
    class MogreGisApplication : OgreWindow
    {
//#if TODO_DANI
        const float TRANSLATE = 200;
        const float ROTATE = 0.2f;
        bool mRotating = false;
        Vector3 mTranslation = Vector3.ZERO;
        Point mLastPosition;

        protected override void CreateInputHandler()
        {
            this.Root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);

            this.KeyDown += new KeyEventHandler(KeyDownHandler);
            this.KeyUp += new KeyEventHandler(KeyUpHandler);

            this.MouseDown += new MouseEventHandler(MouseDownHandler);
            this.MouseUp += new MouseEventHandler(MouseUpHandler);
            this.MouseMove += new MouseEventHandler(MouseMoveHandler);
        }

        bool FrameStarted(FrameEvent evt)
        {
            Camera.Position += Camera.Orientation * mTranslation * evt.timeSinceLastFrame;

            return true;
        }

        void KeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    mTranslation.z = -TRANSLATE;
                    break;

                case Keys.Down:
                case Keys.S:
                    mTranslation.z = TRANSLATE;
                    break;

                case Keys.Left:
                case Keys.A:
                    mTranslation.x = -TRANSLATE;
                    break;

                case Keys.Right:
                case Keys.D:
                    mTranslation.x = TRANSLATE;
                    break;

                case Keys.PageUp:
                case Keys.Q:
                    mTranslation.y = TRANSLATE;
                    break;

                case Keys.PageDown:
                case Keys.E:
                    mTranslation.y = -TRANSLATE;
                    break;
            }
        }

        void KeyUpHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                case Keys.Down:
                case Keys.S:
                    mTranslation.z = 0;
                    break;

                case Keys.Left:
                case Keys.A:
                case Keys.Right:
                case Keys.D:
                    mTranslation.x = 0;
                    break;

                case Keys.PageUp:
                case Keys.Q:
                case Keys.PageDown:
                case Keys.E:
                    mTranslation.y = 0;
                    break;
            }
        }

        void MouseDownHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Hide();
                mRotating = true;
                mLastPosition = Cursor.Position;
            }
        }

        void MouseUpHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Show();
                mRotating = false;
            }
        }

        void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (mRotating)
            {
                float x = mLastPosition.X - Cursor.Position.X;
                float y = mLastPosition.Y - Cursor.Position.Y;

                Camera.Yaw(new Degree(x * ROTATE));
                Camera.Pitch(new Degree(y * ROTATE));

                mLastPosition = Cursor.Position;
            }
        }
//#endif
    }

    class SceneCreator
    {
        public SceneCreator(OgreWindow win)
        {
            win.SceneCreating += new OgreWindow.SceneEventHandler(SceneCreating);
        }

        void SceneCreating(OgreWindow win)
        {
            win.Camera.Position = new Vector3(0, 200, 400);

            RenderProject project = new RenderProject();
            Project prj = XmlSerializer.loadProject("Test1.xml");
            project.render3d(prj, win.SceneManager);
        }
    }
#endif
//#if TODO_DANI
    class MogreGisApplication : Mogre.Demo.ExampleApplication.Example
    {
#if TODO_DANI
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
#endif

        // Scene creation
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.2F, 0.2F, 0.2F);

            // Create a skydome
            sceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            // Create a light
            //Light l = sceneMgr.CreateLight("MainLight");

            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            //l.Position = new Vector3(20F, 80F, 50F);

            // Define a floor plane mesh
            /*Plane p;
            p.normal = Vector3.UNIT_Y;
            p.d = 200;*/

            Plane p = new Plane(Vector3.UNIT_Y, -2);

            MeshManager.Singleton.CreatePlane("FloorPlane", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, p, 200000F, 20000F, 20, 20, true, 1, 50F, 50F, Vector3.UNIT_Z);

            Entity ent;
            // Create an entity (the floor)
            ent = sceneMgr.CreateEntity("floor", "FloorPlane");
            //ent.SetMaterialName("Examples/RustySteel");
            ent.SetMaterialName("Examples/TextureEffect2");

            // Attach to child of root node, better for culling (otherwise bounds are the combination of the 2)
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
/*
            // Add a head, give it it's own node
            SceneNode headNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            ent = sceneMgr.CreateEntity("cube", "cube.mesh");
            ent.SetMaterialName("Examples/Chrome");

            headNode.AttachObject(ent);
*/
            //TODO Dani.
            // Aqui hacemos las pruebas de los filtros.

            RenderProject project = new RenderProject();
            Project prj = XmlSerializer.loadProject("Test1.xml");
            project.render3d(prj, sceneMgr);

            camera.Position = new Vector3(-6500, 4000, 0);
            
#if TODO_DANI
            // Make sure the camera track this node
            camera.SetAutoTracking(true, headNode);

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
#endif
            // Put in a bit of fog for the hell of it        
            sceneMgr.SetFog(FogMode.FOG_EXP, ColourValue.White, 0.00005F);
        }
    }
//#endif
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace osgGISProjects
{
    public class MogreApp : Mogre.Demo.ExampleApplication.Example
    {
        //private Mogre.Root root;
        //private Mogre.RenderWindow window;
        public MogreApp()
        {
            this.Setup();
        }

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./MogreResources", "FileSystem", "General");
        }

        public MogreApp(Vector3 position, Vector3 lookAt, Vector4 backgroundcolor)
        {
            this.Setup();
            if (position != null)
            {
                camera.Position = position;
            }
            if (lookAt != null)
            {
                camera.LookAt(lookAt);
            }
            if (backgroundcolor != null)
            {
                viewport.BackgroundColour = new ColourValue(backgroundcolor.x, backgroundcolor.y, backgroundcolor.z, backgroundcolor.w);
            }
        }

        public override void CreateScene()
        {
            //NOP
        }
        
        public SceneManager SceneManager
        {
            get { return sceneMgr; }
        }

        public Root getRoot()
        {
            return this.root;
        }
    }
}

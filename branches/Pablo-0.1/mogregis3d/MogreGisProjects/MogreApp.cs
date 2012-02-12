using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace osgGISProjects
{
    public class MogreApp : Mogre.Demo.ExampleApplication.Example
    {
        private static List<osgGISProjects.Project.MogreLocation> locations;
        //private Mogre.Root root;
        //private Mogre.RenderWindow window;
        public MogreApp()
        {
            this.Setup();
        }

        public static void setLocations(List<osgGISProjects.Project.MogreLocation> l)
        {
            locations = l;
        }

        public override void SetupResources()
        {
            base.SetupResources();
            if (locations != null)
            {
                foreach (osgGISProjects.Project.MogreLocation l in locations)
                {
                    ResourceGroupManager.Singleton.AddResourceLocation(l.Name, l.Type, l.Group);
                }
            }
        }

        public MogreApp(Vector3 position, Vector3 lookAt, Vector4 backgroundcolor, int viewDistance,String backgroundMaterial)
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
            this.camera.NearClipDistance = 0.005f;
            this.camera.FarClipDistance = viewDistance;

            if (backgroundMaterial != null)
            {
                this.SceneManager.SetSkyDome(true, backgroundMaterial, 0, 15);
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

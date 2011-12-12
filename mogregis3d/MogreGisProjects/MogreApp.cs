using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace osgGISProjects
{
    public class MogreApp : Mogre.TutorialFramework.BaseApplication
    {
        //private Mogre.Root root;
        //private Mogre.RenderWindow window;

        public MogreApp()
        {
            this.Setup();
        }

        protected override void CreateScene()
        {
            mCamera.Position = new Vector3(-300, 500, 0);
            mCamera.LookAt(0, 0, 0);
        }

        public SceneManager SceneManager
        {
            get { return mSceneMgr; }
        }

        public Root getRoot()
        {
            return this.mRoot;
        }
    }
}

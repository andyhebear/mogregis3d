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
    class C3E4Tutorial : Mogre.Demo.ExampleApplication.Example
    {
        /* Apply an inefficient but simple-to-implement subdivision scheme for a triangle. */
        static void TriangleDivide(ManualObject obj, int depth,
                                   float[] a, float[] b, float[] c,
                                   float[] ca, float[] cb, float[] cc)
        {
            if (depth == 0)
            {
                obj.Position(c[0], c[1], 0);
                obj.Colour(cc[0], cc[1], cc[2]);
                obj.Position(b[0], b[1], 0);
                obj.Colour(cb[0], cb[1], cb[2]);
                obj.Position(a[0], a[1], 0);
                obj.Colour(ca[0], ca[1], ca[2]);
            }
            else
            {
                float[] d = new float[2] { (a[0] + b[0]) / 2, (a[1] + b[1]) / 2 };
                float[] e = new float[2] { (b[0] + c[0]) / 2, (b[1] + c[1]) / 2 };
                float[] f = new float[2] { (c[0] + a[0]) / 2, (c[1] + a[1]) / 2 };
                float[] cd = new float[3] { (ca[0] + cb[0]) / 2, (ca[1] + cb[1]) / 2, (ca[2] + cb[2]) / 2 };
                float[] ce = new float[3] { (cb[0] + cc[0]) / 2, (cb[1] + cc[1]) / 2, (cb[2] + cc[2]) / 2 };
                float[] cf = new float[3] { (cc[0] + ca[0]) / 2, (cc[1] + ca[1]) / 2, (cc[2] + ca[2]) / 2 };

                depth -= 1;
                TriangleDivide(obj, depth, a, d, f, ca, cd, cf);
                TriangleDivide(obj, depth, d, b, e, cd, cb, ce);
                TriangleDivide(obj, depth, f, e, c, cf, ce, cc);
                TriangleDivide(obj, depth, d, e, f, cd, ce, cf);
            }
        }

        /// <summary>
        /// Large vertex displacements such as are possible with C3E4v_twist
        /// require a high degree of tessellation.  This routine draws a
        /// triangle recursively subdivided to provide sufficient tessellation. */
        /// </summary>
        static void DrawSubDividedTriangle(ManualObject obj, int subdivisions)
        {
            float[] a = new float[2] { -0.8f, 0.8f };
            float[] b = new float[2] { 0.8f, 0.8f };
            float[] c = new float[2] { 0.0f, -0.8f };

            float[] ca = new float[3] { 0, 0, 1 };
            float[] cb = new float[3] { 0, 0, 1 };
            float[] cc = new float[3] { 0.7f, 0.7f, 1 };

            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_LIST;
            obj.Begin("CgTutorials/C3E4_Material", operation);
            TriangleDivide(obj, subdivisions, a, b, c, ca, cb, cc);
            obj.End();
        }


        public override void CreateScene()
        {
            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial06Node1");

            ManualObject manualObj1 = sceneMgr.CreateManualObject("Tutorial06Object1");
            DrawSubDividedTriangle(manualObj1, 5);
            node1.AttachObject(manualObj1);

            Material material = (Material)MaterialManager.Singleton.GetByName("CgTutorials/C3E4_Material").Target;
            material.SetParameter("twisting", "0.0");
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
            viewport.BackgroundColour = new ColourValue(1.0f, 1.0f, 1.0f, 0.0f);  /* White background */
        }

    }
}
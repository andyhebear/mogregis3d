using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Point = System.Drawing.PointF;
using Scenic.Render;
using Scenic;

namespace Mogre.Demo.Primitives
{
    public class MogreRendererCallback : IRendererCallback
    {
        SceneManager sceneManager;
        public MogreRendererCallback(SceneManager sceneMngr)
        {
            sceneManager = sceneMngr;
        }

        NumberFormatInfo nfi = new CultureInfo("en-us").NumberFormat;
        protected List<Point> pointsList = new List<Point>();
        protected ManualObject rendererObj;
        protected SceneNode node1;
        protected int count = 0;
        protected System.Drawing.Drawing2D.Matrix transform;

        public int AddVertex(float x, float y)
        {
            Console.WriteLine("AddVertex: (" + x.ToString(nfi) + "; " + y.ToString(nfi) + ")");
            Point p = TransformVertex(x, y);
            rendererObj.Position(new Vector3(p.X, p.Y, 0));
            pointsList.Add(p);
            return pointsList.Count - 1;
        }

        public int AddVertex(float x, float y, ScenicColor color)
        {
            return AddVertex(x, y);
        }

        public int AddVertex(float x, float y, ScenicColor color, float tu1, float tv1)
        {
            int vertex =  AddVertex(x, y);
            rendererObj.Colour(color.red, color.green, color.blue, color.alpha);
            return vertex;

        }

        public int AddVertex(float x, float y, ScenicColor color, float tu1, float tv1, float tu2, float tv2)
        {
            return AddVertex(x, y);
        }

        public void AddTriangle(int a, int b, int c)
        {
            Console.WriteLine("AddTriangle: (" + a.ToString(nfi) + "; " + b.ToString(nfi) + "; " + c.ToString(nfi) + ")");
            Point va = pointsList[a];
            Point vb = pointsList[b];
            Point vc = pointsList[c];
            if ((va.X - vb.X) * (vb.Y - vc.Y) - (va.Y - vb.Y) * (vb.X - vc.X) > 0)
                rendererObj.Triangle((ushort)a, (ushort)b, (ushort)c);
            else
                rendererObj.Triangle((ushort)c, (ushort)b, (ushort)a);
        }

        public void SetDefaultTex2(float tu2, float tv2)
        {
            Console.WriteLine("SetDefaultTex2: (" + tu2.ToString(nfi) + "; " + tv2.ToString(nfi) + ")");
        }


        public void BeginBlock()
        {
            Console.WriteLine("BeginBlock");
            pointsList = new List<Point>();
            if (rendererObj == null)
            {
                rendererObj = sceneManager.CreateManualObject("Element" + count);
                count++;
            }
            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_LIST;

            MaterialPtr material = MaterialManager.Singleton.CreateOrRetrieve("Test/ColourLines3d",
                                                                    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME).first;
            material.GetTechnique(0).GetPass(0).VertexColourTracking =
                           (int)TrackVertexColourEnum.TVC_AMBIENT;
            rendererObj.Begin("Test/ColourLines3d", operation);
        }

        public void EndBlock()
        {
            Console.WriteLine("EndBlock");
            if (node1 == null)
                node1 = sceneManager.RootSceneNode.CreateChildSceneNode("Lines3DNode_1");
            ManualObject.ManualObjectSection section = rendererObj.End();
            node1.AttachObject(rendererObj);
            rendererObj = null;
        }

        public void draw()
        {
            Console.WriteLine("draw");
        }

        public void empty()
        {
            Console.WriteLine("empty");
        }

        public System.Drawing.Drawing2D.Matrix Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        protected Point TransformVertex(float x, float y)
        {
            Point p = new Point(x, y);

            if (Transform != null)
            {
                Point[] v = new Point[] { p };
                Transform.TransformPoints(v);
                p = v[0];
            }

            return p;
        }
    }
}

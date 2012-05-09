using System;
using System.Collections.Generic;

using Mogre.Utils.GluTesselator;

namespace Mogre.Demo.PolygonExample
{
    public class Area
    {
        public Area(int f, int l)
        {
            first = f;
            last = l;
        }

        public int first;
        public int last;
    }

    public class Polygon
    {
        static readonly double[][] test1 = new double[][] { new double[] { 0, 0, 0 },
                                                          new double[] { 0, 16, 0 }, 
                                                          new double[] { 16, 16, 0 }, 
                                                          new double[] { 16, 0, 0 } };

        static readonly double[][] test2 = new double[][] { new double[] { 0, 0, 0 },
                                                          new double[] { 0, 6, 0 }, 
                                                          new double[] { 2, 2, 0 }, 
                                                          new double[] { 6, 6, 0 }, 
                                                          new double[] { 6, 0, 0 } };

        static readonly double[][] test3 = new double[][] { new double[] { 0, 0, 0 },
                                                          new double[] { 6, 0, 0 }, 
                                                          new double[] { 6, 6, 0 }, 
                                                          new double[] { 2, 2, 0 }, 
                                                          new double[] { 0, 6, 0 } };

        // define concave quad data (vertices only)
        //  0    2
        //  \ \/ /
        //   \3 /
        //    \/
        //    1
        static readonly double[][] quad1 = new double[][] { new double[] { -1, 3, 0 },
                                                            new double[] { 0, 0, 0 },
                                                            new double[] { 1, 3, 0 }, 
                                                            new double[] { 0, 2, 0 } };

        // define concave quad with a hole
        //  0--------3
        //  | 4----7 |
        //  | |    | |
        //  | 5----6 |
        //  1--------2
        static readonly double[][] quad2 = new double[][] { new double[] {-2,3,0},
                                                            new double[] {-2,0,0},
                                                            new double[] {2,0,0},
                                                            new double[] { 2,3,0},
                                                            new double[] {-1,2,0},
                                                            new double[] {-1,1,0},
                                                            new double[] {1,1,0},
                                                            new double[] { 1,2,0} };
        static readonly Area[] quad2Holes = new Area[] { new Area(0, 3), new Area(4, 7) };

        public void AddPoint(float x, float y, float z)
        {
            pointsList.Add(new Vector3(x, y, z));
        }

        public void AddPoint(Vector3 point)
        {
            pointsList.Add(point);
        }

        public Vector3 GetPoint(ushort index)
        {
            return pointsList[index];
        }

        public ushort NumPoints
        {
            get { return (ushort)pointsList.Count; }
        }

        public void UpdatePoint(ushort index, Vector3 newPoint)
        {
            pointsList[index] = newPoint;
        }

        public ManualObject CreateNode(string name, SceneManager sceneMgr, bool isClosed)
        {
            if (polygon == null)
            {
                polygon = sceneMgr.CreateManualObject(name);
                MaterialPtr material = MaterialManager.Singleton.Create("Test/ColourPolygon",
                         ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                material.GetTechnique(0).GetPass(0).VertexColourTracking =
                               (int)TrackVertexColourEnum.TVC_AMBIENT;
                Tesselate(quad1);
            }
            return polygon;
        }

        private void Tesselate(double[][] data)
        {
            MogreTessellationCallbacks callback = new MogreTessellationCallbacks(polygon);

            GLUtessellatorImpl Glu = (GLUtessellatorImpl)GLUtessellatorImpl.gluNewTess();
            Glu.gluTessCallback(GLU.GLU_TESS_VERTEX, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_BEGIN, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_END, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_ERROR, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_COMBINE, callback);
            Glu.gluTessBeginPolygon(null);
            Glu.gluTessBeginContour();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                Glu.gluTessVertex(data[i], 0, new Vector3((float)data[i][0], (float)data[i][1], (float)data[i][2]));
            }
            Glu.gluTessEndContour();
            Glu.gluTessNormal(0, 0, 1);
            Glu.gluTessEndPolygon();
        }

        private void TesselateComplex(double[][] data, Area[] holes)
        {
            MogreTessellationCallbacks callback = new MogreTessellationCallbacks(polygon);

            GLUtessellatorImpl Glu = (GLUtessellatorImpl)GLUtessellatorImpl.gluNewTess();
            Glu.gluTessCallback(GLU.GLU_TESS_VERTEX, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_BEGIN, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_END, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_ERROR, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_COMBINE, callback);
            Glu.gluTessBeginPolygon(null);
            for (int area = 0; area < holes.Length; area++)
            {
                Glu.gluTessBeginContour();
                for (int i = holes[area].first; i <= holes[area].last; i++)
                {
                    Glu.gluTessVertex(data[i], 0, new Vector3((float)data[i][0], (float)data[i][1], (float)data[i][2]));
                }
                Glu.gluTessEndContour();
            }
            Glu.gluTessEndPolygon();
        }

        protected List<Vector3> pointsList = new List<Vector3>();
        protected ManualObject polygon;
    }

    public class MogreTessellationCallbacks : GLUtessellatorCallback
    {
        ManualObject manualObj;

        public MogreTessellationCallbacks(ManualObject mo)
        {
            manualObj = mo;
        }

        #region GLUtessellatorCallback Members

        public void begin(int type)
        {
            string typeName;
            switch (type)
            {
                case GL.GL_LINE_LOOP:
                    typeName = "GL_LINE_LOOP";
                    break;
                case GL.GL_TRIANGLE_FAN:
                    typeName = "GL_TRIANGLE_FAN";
                    manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_FAN);
                    break;
                case GL.GL_TRIANGLE_STRIP:
                    typeName = "GL_TRIANGLE_STRIP";
                    manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_STRIP);
                    break;
                case GL.GL_TRIANGLES:
                    typeName = "GL_TRIANGLES";
                    manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
                    break;
                default:
                    typeName = "Unknown";
                    break;
            }
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod() + "Primitive type = " + typeName);
        }

        public void beginData(int type, object polygonData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void edgeFlag(bool boundaryEdge)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void edgeFlagData(bool boundaryEdge, object polygonData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void vertex(object vertexData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod() + " vertex=" + vertexData);
            manualObj.Position((Vector3)vertexData);
        }

        public void vertexData(object vertexData, object polygonData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void end()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            manualObj.End();
        }

        public void endData(object polygonData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void combine(double[] coords, object[] data, float[] weight, object[] outData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void combineData(double[] coords, object[] data, float[] weight, object[] outData, object polygonData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void error(int errnum)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void errorData(int errnum, object polygonData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        #endregion
    }

    class PolygonExample1 : Mogre.Demo.ExampleApplication.Example
    {

        public override void CreateScene()
        {
            Polygon polygon = new Polygon();

            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Polygon1Node");
            node1.AttachObject(polygon.CreateNode("Polygon1", base.sceneMgr, true));
        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            // Position it at 30 in Z direction
            camera.Position = new Vector3(0, 0, 30);
            // Look at our line
            camera.LookAt(new Vector3(5, 5, 5));
            camera.NearClipDistance = 1;
        }

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();

            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStartedHandler);
        }

        bool FrameStartedHandler(FrameEvent evt)
        {
            //if(ExampleFrameListener.frameStarted(evt) == false)
            //return false;
            return true;
        }
    }
}
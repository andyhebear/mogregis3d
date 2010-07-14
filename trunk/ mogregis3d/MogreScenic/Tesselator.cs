/* 
Scenic Graphics Library
Copyright (C) 2007 Jouni Tulkki

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USApackage scenic;*/
using System;
using System.Collections.Generic;
using Path = scenic.path.Path;
using PathWalker = scenic.path.PathWalker;
using Mogre.Utils.GluTesselator;
using Mogre;
namespace scenic
{

    public class Tesselator
    {
        virtual public Path Path
        {
            set
            {
                this.path = value;
            }

        }

        private class Walker : PathWalker
        {
            private Tesselator tesselator;

            public Tesselator InternalTesselator
            {
                get
                {
                    return tesselator;
                }

            }
            public Walker(Tesselator tess)
            {
                this.tesselator = tess;
            }

            public virtual void beginSubPath(bool isClosed)
            {
                InternalTesselator.beginContour();
            }

            public virtual void endSubPath()
            {
                InternalTesselator.endContour();
            }

            public virtual void lineTo(float x, float y)
            {
                InternalTesselator.vertex(x, y);
            }
        }

        private class Triangle
        {
            public Triangle(Tesselator tess)
            {
                this.tesselator = tess;
            }

            public Tesselator InternalTesselator
            {
                get
                {
                    return tesselator;
                }

            }
            private Tesselator tesselator;
            internal int vertex1;
            internal bool edge1;
            internal int vertex2;
            internal bool edge2;
            internal int vertex3;
            internal bool edge3;
        }

        public List<System.Drawing.PointF> vertices = new List<System.Drawing.PointF>();
        private List<Triangle> triangles = new List<Triangle>();
        private bool cacheTriangles = true;
        private Path path;
        private DrawContext context;
        private GLUtessellatorImpl Glu;

        public Tesselator(Path path)
        {
            this.path = path;
        }

        public virtual void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            if (Glu == null)
            {
                begin(path.Convex, context.context);
                path.walk(new Walker(this), transform, context.pathError);
                end();
            }
            draw(context.context);
        }

        private void begin(bool isConvex, int contextId)
        {
            MogreTessellationCallbacks callback = new MogreTessellationCallbacks(null);

            Glu = (GLUtessellatorImpl)GLUtessellatorImpl.gluNewTess();
            Glu.gluTessCallback(GLU.GLU_TESS_VERTEX, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_BEGIN, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_END, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_ERROR, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_COMBINE, callback);
            Glu.gluTessBeginPolygon(null);
        }

        private void end()
        {
            Glu.gluTessNormal(0, 0, 1);
            Glu.gluTessEndPolygon();
        }

        private void beginContour()
        {
            Glu.gluTessBeginContour();
        }

        private void endContour()
        {
            Glu.gluTessEndContour();
        }

        private void vertex(double x, double y)
        {
            Glu.gluTessVertex(new double[] { x, y, 0 }, 0, new Mogre.Vector3((float)x, (float)y, (float)0));
        }

        private void draw(int contextId) { }

        // Called from native code
        private void addVertex(double x, double y)
        {
            if (cacheTriangles)
                vertices.Add(new System.Drawing.PointF((float)x, (float)y));
            else
                context.renderer.tessVertex(context.context, x, y);
        }
        // Called from native code
        private void addTriangle(int vertex1, bool edge1, int vertex2, bool edge2, int vertex3, bool edge3)
        {
            if (cacheTriangles)
            {
                Triangle t = new Triangle(this);

                t.vertex1 = vertex1;
                t.edge1 = edge1;
                t.vertex2 = vertex2;
                t.edge2 = edge2;
                t.vertex3 = vertex3;
                t.edge3 = edge3;
                triangles.Add(t);
            }
            else
            {
                context.renderer.tessTriangle(context.context, vertex1, edge1, vertex2, edge2, vertex3, edge3);
            }
        }
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
                    //manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_FAN);
                    break;
                case GL.GL_TRIANGLE_STRIP:
                    typeName = "GL_TRIANGLE_STRIP";
                    //manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_STRIP);
                    break;
                case GL.GL_TRIANGLES:
                    typeName = "GL_TRIANGLES";
                    //manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
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
            //manualObj.Position((Vector3)vertexData);
        }

        public void vertexData(object vertexData, object polygonData)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void end()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            //manualObj.End();
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


}
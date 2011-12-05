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
using Path = Scenic.path.Path;
using PathWalker = Scenic.path.PathWalker;
using Mogre.Utils.GluTesselator;
using Mogre;
namespace Scenic
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
                //InternalTesselator.beginContour();
            }

            public virtual void endSubPath()
            {
                //InternalTesselator.endContour();
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


        public Tesselator(Path path)
        {
            this.path = path;
        }

        public virtual void draw(DrawContext cntxt, System.Drawing.Drawing2D.Matrix transform)
        {
            Console.WriteLine("Tesselator >>> begin draw");
            context = cntxt;

            begin(path.Convex, context.context);
            path.walk(new Walker(this), transform, context.pathError);
            end(context.context);

            draw(context.context);
            Console.WriteLine("Tesselator >>> end draw");
        }

        private void begin(bool isConvex, int contextId)
        {
            context.renderer.tessBegin(contextId);

        }

        private void end(int contextId)
        {
            context.renderer.tessEnd(contextId);
            //cntxt.lineRenderer.end();
        }

        private void beginContour(int contextId)
        {
            context.renderer.tessBeginContour(contextId);
        }

        private void endContour(int contextId)
        {
            context.renderer.tessBeginContour(contextId);

        }

        private void vertex(double x, double y)
        {
            context.renderer.tessVertex(0, x, y);
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


}
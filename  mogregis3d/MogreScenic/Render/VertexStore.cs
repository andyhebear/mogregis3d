using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace scenic.Render
{
    public class VertexStore : IRendererCallback
    {
        NumberFormatInfo nfi = new CultureInfo("en-us").NumberFormat;
        private System.Drawing.Drawing2D.Matrix transform = null;

        public int AddVertex(float x, float y, ScenicColor color)
        {
            Console.WriteLine("AddVertex: (" + x.ToString(nfi) + "; " + y.ToString(nfi) + ")");
            return -1;
        }

        public int AddVertex(float x, float y, ScenicColor color, float tu1, float tv1)
        {
            Console.WriteLine("AddVertex: (" + x.ToString(nfi) + "; " + y.ToString(nfi) + ")");
            return -1;
        }

        public int AddVertex(float x, float y, ScenicColor color, float tu1, float tv1, float tu2, float tv2)
        {
            Console.WriteLine("AddVertex: (" + x.ToString(nfi) + "; " + y.ToString(nfi) + ")");
            return -1;
        }

        public void AddTriangle(int a, int b, int c)
        {
            Console.WriteLine("AddTriangle: (" + a.ToString(nfi) + "; " + b.ToString(nfi) + "; " + c.ToString(nfi) + ")");
        }

        public void SetDefaultTex2(float tu2, float tv2)
        {
            Console.WriteLine("SetDefaultTex2: (" + tu2.ToString(nfi) + "; " + tv2.ToString(nfi) + ")");
        }

        public void BeginBlock()
        {
            Console.WriteLine("BeginBlock");
        }

        public void EndBlock()
        {
            Console.WriteLine("EndBlock");
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
            set { transform = value;}
        }

    }
}

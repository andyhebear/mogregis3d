using System;
using System.Collections.Generic;
using System.Text;

namespace scenic.Render
{
    public interface IRendererCallback
    {
        int AddVertex(float x, float y, ScenicColor color);

        int AddVertex(float x, float y, ScenicColor color, float tu1, float tv1);

        int AddVertex(float x, float y, ScenicColor color, float tu1, float tv1, float tu2, float tv2);

        void AddTriangle(int a, int b, int c);

        void SetDefaultTex2(float tu2, float tv2);

        void BeginBlock();

        void EndBlock();

        void draw();

        System.Drawing.Drawing2D.Matrix Transform { get; set;}
    }
}

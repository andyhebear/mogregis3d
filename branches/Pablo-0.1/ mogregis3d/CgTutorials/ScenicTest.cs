using System;
using System.Collections.Generic;
using System.Text;
using Point = System.Drawing.PointF;

//using Mogre.Helpers;

using Mogre;
using Scenic;

namespace Mogre.Demo.Primitives
{
    class ScenicTestExample1 : Mogre.Demo.ExampleApplication.Example
    {
        public override void CreateScene()
        {
            Vector4 green = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            Vector4 red = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            float[] dashPattern2 = new float[] { 2, 2 };

            Graphics g = new Graphics(new MogreRendererCallback(base.sceneMgr));

#if TESTLINE
            TestLine(g);
#endif

#if !TESTREC
            // A big red rectangle 
            g.Rectangle(0, 10, 10, 10);
            g.StrokeColor = new ScenicColor(1.0f, 0.0f, 0.0f, 1.0f);
            g.LineWidth = 0.2f;
            g.LineJoin = LineJoinStyle.ROUND_JOIN;
            g.EndCap = LineCapStyle.BUTT_CAP;
            g.Stroke();
#endif

#if !TESTFILLEDREC
            // A filled green rectangle 
            g.Rectangle(0, 0, 5, 5);
            g.StrokeColor = new ScenicColor(0.0f, 1.0f, 0.0f, 1.0f);
            g.FillColor = new ScenicColor(0.0f, 1.0f, 0.0f, 1.0f);
            g.LineWidth = 0.01f;
            g.LineJoin = LineJoinStyle.BEVEL_JOIN;
            //g.Fill();
            g.Stroke();
#endif
        }

        protected void TestLine(Graphics g)
        {
            TestLineColor(g);
            TestLineCap(g);
            TestLineJoin(g);
            TestLineWith(g);
        }

        protected void TestLineColor(Graphics g)
        {
            Random rand = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                g.MoveTo(0, i * 2);
                g.LineTo(8, i * 2);
                g.StrokeColor = new ScenicColor((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
                g.LineWidth = 0.1f;
                g.LineJoin = LineJoinStyle.ROUND_JOIN;
                g.EndCap = LineCapStyle.BUTT_CAP;
                g.Stroke();
            }
        }

        protected void TestLineCap(Graphics g)
        {
            for (int i = 0; i < 3; i++)
            {
                g.MoveTo(10, i * 4);
                g.LineTo(18, i * 4);
                g.StrokeColor = new ScenicColor(0.2f, 1.0f, 0.8f, 1.0f);
                g.LineWidth = 0.1f;
                g.LineJoin = LineJoinStyle.ROUND_JOIN;
                if (i == 0)
                    g.EndCap = LineCapStyle.BUTT_CAP;
                else if (i == 1)
                    g.EndCap = LineCapStyle.ROUND_CAP;
                else
                    g.EndCap = LineCapStyle.SQUARE_CAP;
                g.Stroke();
            }
        }

        protected void TestLineJoin(Graphics g)
        {
            for (int i = 0; i < 3; i++)
            {
                g.MoveTo(20, i * 4);
                g.LineTo(24, i * 4+2);
                g.LineTo(28, i * 4);
                g.StrokeColor = new ScenicColor(0.8f, 1.0f, 0.2f, 1.0f);
                g.LineWidth = 0.1f;
                g.EndCap = LineCapStyle.BUTT_CAP;

                if (i == 0)
                    g.LineJoin = LineJoinStyle.BEVEL_JOIN;
                else if (i == 1)
                    g.LineJoin = LineJoinStyle.MITER_JOIN;
                else
                    g.LineJoin = LineJoinStyle.ROUND_JOIN;
                g.Stroke();
            }
        }

        protected void TestLineWith(Graphics g)
        {
            Random rand = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                g.MoveTo(30, i * 2);
                g.LineTo(38, i * 2);
                g.StrokeColor = new ScenicColor(0.5f, 0.5f, 1.0f, 1.0f);
                g.LineWidth = (i+0.1f)/10.0f;
                g.LineJoin = LineJoinStyle.ROUND_JOIN;
                g.EndCap = LineCapStyle.BUTT_CAP;
                g.Stroke();
            }
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

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/SharpMap", "FileSystem", "General");
        }
    }
}
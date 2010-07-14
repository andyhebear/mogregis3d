using System;
using System.Collections.Generic;
using Color = System.Drawing.Color;
using System.Text;

using PointF = System.Drawing.PointF;

namespace scenic.Render
{
    public class LineRenderer
    {
        private Context context;

        private bool isClosed;
        private PointF firstp;
        private PointF firstd;
        private PointF prevp;
        private PointF prevd;
        private int count;
        private ScenicColor color;
        private float width;
        private float minu;
        private float maxu;
        private float miterLimit;
        private bool closeStroke;
        private IRendererCallback rendererCallback;
        private float dashLength;
        private float dashPhase;

        public void begin(Context context, bool isClosed)
        {
            this.context = context;
            this.isClosed = isClosed;
            miterLimit = context.miterLimit;
            //float det = Math.Sqrt(Math.Abs(context.transform.m00 * context.transform.m11 -
            //              context.transform.m01 * context.transform.m10));
            float det = (float)Math.Sqrt(Math.Abs(context.transform.Elements[0] * context.transform.Elements[3] -
                                            context.transform.Elements[1] * context.transform.Elements[2]));
            float a = det * context.lineWidth / 2.0f;

            width = context.lineWidth / 2.0f * (a + 0.1f) / a;
            minu = 0.25f;
            maxu = 0.25f + (a + 0.5f) / 2.0f;
            rendererCallback = context.vertexRenderer;
            rendererCallback.Transform = context.transform;

            count = 0;
            color = context.color;
            closeStroke = false;

            //if (lineEdgeTexture == 0)
                createEdgeTexture();
            //	if(lineDashTexture == 0)
            createDashTexture();
            dashPhase = context.lineDashPhase;
        }

        public void end()
        {
            if (isClosed && count > 1)
            {
                // TODO: better comparison
                if (Math.Abs(firstp.X - prevp.X) > 0.001 || Math.Abs(firstp.Y - prevp.Y) > 0.001)
                    addPoint(firstp);
                drawJoin(firstp, prevd, firstd);
            }
            else if (!isClosed && count > 1)
            {
                drawCap(prevp, prevd);
            }
        }

        public void addPoint(PointF p)
        {
            count++;

            if (count == 1)
            {
                firstp = p;
                prevp = p;
            }
            else
            {
                float m;
                float length;
                PointF n = new PointF(p.X - prevp.X, p.Y - prevp.Y);


                length = (float)Math.Sqrt(n.X * n.X + n.Y * n.Y);
                if (length == 0.0)
                    return;

                m = width / length;
                PointF d = new PointF(-n.Y * m, n.X * m);

                rendererCallback.SetDefaultTex2(dashPhase, 0.5f);
                if (count > 2)
                    drawJoin(prevp, prevd, d);
                drawLine(prevp, p, d, length / context.lineWidth / dashLength);
                dashPhase += length / context.lineWidth / dashLength;
                dashPhase -= (float)Math.Floor(dashPhase);
                rendererCallback.SetDefaultTex2(dashPhase, 0.5f);
                if (count == 2 && !isClosed)
                {
                    PointF d2 = new PointF(-d.X, -d.Y);
                    drawCap(prevp, d2);
                }
                prevp = p;
                prevd = d;
                if (count == 2)
                    firstd = d;
            }
        }

        //private void draw();

        private void createEdgeTexture()
        {
#if PENDING
            lineEdgeTexture = new Texture(2, 1, FMT_A8R8G8B8, 0);
            uint[] dest = new uint[2];

            dest[0] = 0x00000000;
            dest[1] = 0xffffffff;

            lineEdgeTexture.write(0, 0, 0, 2, 1, dest, 0, FMT_A8R8G8B8);
#endif
        }

        private void createDashTexture()
        {
             if (context.lineDashLengths.Count == 0)
            {
                dashLength = 1;
                return;
            }

#if PENDING
            lineDashTexture = new Texture(2048, 1, FMT_A8R8G8B8, 0, 0);
#endif
            uint[] dest = new uint[2048];
            int i, j = 0, k;
            float phase = 0;
            bool state = true;
            float l = 0;

            for (i = 0; i < context.lineDashLengths.Count; i++)
                l += context.lineDashLengths[i];
            dashLength = l;

            for (i = 0; i < context.lineDashLengths.Count; i++)
            {
                phase += context.lineDashLengths[i];
                k = (int)(phase / l * 2048);
                while (j < k)
                    dest[j++] = state ? 0xffffffff : 0x0;
                state = !state;
            }
            while (j < 2048)
                dest[j++] = 0xffffffff;

#if PENDING
            lineDashTexture.write(0, 0, 0, 2048, 1, dest, 0, FMT_A8R8G8B8);
#endif
        }

        private void drawLine(PointF p1, PointF p2, PointF d, float length)
        {
            rendererCallback.BeginBlock();
            rendererCallback.AddVertex(p1.X - d.X, p1.Y - d.Y, color, minu, 0.0f);
            rendererCallback.AddVertex(p1.X, p1.Y, color, maxu, 0.0f);
            rendererCallback.AddVertex(p1.X + d.X, p1.Y + d.Y, color, minu, 0.0f);

            rendererCallback.SetDefaultTex2(dashPhase + length, 0.5f);

            rendererCallback.AddVertex(p2.X - d.X, p2.Y - d.Y, color, minu, 0.0f);
            rendererCallback.AddVertex(p2.X, p2.Y, color, maxu, 0.0f);
            rendererCallback.AddVertex(p2.X + d.X, p2.Y + d.Y, color, minu, 0.0f);

            rendererCallback.AddTriangle(0, 3, 1);
            rendererCallback.AddTriangle(1, 3, 4);
            rendererCallback.AddTriangle(1, 4, 2);
            rendererCallback.AddTriangle(2, 4, 5);

            rendererCallback.EndBlock();
        }

        private void drawCap(PointF p, PointF d)
        {
            if (context.lineCap == LineCapStyle.SQUARE_CAP)
                drawSquareCap(p, d);
            else if (context.lineCap == LineCapStyle.ROUND_CAP)
                drawRoundCap(p, d);
        }

        private void drawSquareCap(PointF p, PointF d)
        {
            float maxw;

            PointF ds = new PointF(d.X, d.Y);
            //ds.X = d.X * context.transform.m00 + d.Y * context.transform.m01;
            //ds.Y = d.X * context.transform.m10 + d.Y * context.transform.m11;
            context.transform.TransformPoints(new PointF[] { ds });

            maxw = (float)Math.Sqrt(ds.X * ds.X + ds.Y * ds.Y) / 2.0f;

            PointF n = new PointF(d.Y, -d.X);

            rendererCallback.BeginBlock();

            rendererCallback.AddVertex(p.X, p.Y, color, maxw, 0.0f);
            rendererCallback.AddVertex(p.X - d.X, p.Y - d.Y, color, 0.0f, 0.0f);
            rendererCallback.AddVertex(p.X - d.X + n.X, p.Y - d.Y + n.Y, color, 0.0f, 0.0f);
            rendererCallback.AddVertex(p.X + d.X + n.X, p.Y + d.Y + n.Y, color, 0.0f, 0.0f);
            rendererCallback.AddVertex(p.X + d.X, p.Y + d.Y, color, 0.0f, 0.0f);

            rendererCallback.AddTriangle(0, 1, 2);
            rendererCallback.AddTriangle(0, 2, 3);
            rendererCallback.AddTriangle(0, 3, 4);

            rendererCallback.EndBlock();
        }

        private void drawRoundCap(PointF p, PointF d)
        {
            double startAngle, stopAngle;
            double dlength = Math.Sqrt(d.X * d.X + d.Y * d.Y);
            float maxw;

            PointF ds = new PointF(d.X, d.Y);
            //ds.X = d.X * context.transform.m00 + d.Y * context.transform.m01;
            //ds.Y = d.X * context.transform.m10 + d.Y * context.transform.m11;
            context.transform.TransformPoints(new PointF[] { ds });

            maxw = (float)Math.Sqrt(ds.X * ds.X + ds.Y * ds.Y) / 2.0f;

            startAngle = Math.Atan2(-d.Y, -d.X);
            stopAngle = startAngle + Math.PI;

            rendererCallback.BeginBlock();

            rendererCallback.AddVertex(p.X, p.Y, color, maxw, 0.0f);
            rendererCallback.AddVertex(p.X - d.X, p.Y - d.Y, color, 0.0f, 0.0f);

            int nstep = (int)(Math.PI / (Math.PI * 2.0) * 50.0);
            int index = 1;

            for (int i = 0; i <= nstep; i++)
            {
                double angle = startAngle + (stopAngle - startAngle) * i / nstep;
                double x = Math.Cos(angle) * dlength;
                double y = Math.Sin(angle) * dlength;
                int a = rendererCallback.AddVertex((float)(p.X + x), (float)(p.Y + y), color, 0.0f, 0.0f);

                rendererCallback.AddTriangle(0, index, a);
                index = a;
            }

            rendererCallback.EndBlock();
        }

        private void drawJoin(PointF p, PointF d1, PointF d2)
        {
            if (context.lineJoin == LineJoinStyle.BEVEL_JOIN)
                drawBevelJoin(p, d1, d2);
            else if (context.lineJoin == LineJoinStyle.MITER_JOIN)
                drawMiterJoin(p, d1, d2);
            else if (context.lineJoin == LineJoinStyle.ROUND_JOIN)
                drawRoundJoin(p, d1, d2);
        }

        private void drawBevelJoin(PointF p, PointF d1, PointF d2)
        {
            rendererCallback.BeginBlock();
            rendererCallback.AddVertex(p.X, p.Y, color, maxu, 0.0f);
            if (d1.X * d2.Y - d1.Y * d2.X > 0.0)
            {
                rendererCallback.AddVertex(p.X - d1.X, p.Y - d1.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X - d2.X, p.Y - d2.Y, color, minu, 0.0f);
            }
            else
            {
                rendererCallback.AddVertex(p.X + d1.X, p.Y + d1.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X + d2.X, p.Y + d2.Y, color, minu, 0.0f);
            }
            rendererCallback.AddTriangle(2, 1, 0);
            rendererCallback.EndBlock();
        }

        private void drawMiterJoin(PointF p, PointF d1, PointF d2)
        {
            PointF a = new PointF(d1.X + d2.X, d1.Y + d2.Y);

            float l = (float)Math.Sqrt(a.X * a.X + a.Y * a.Y);
            float w = width;
            float m2 = 2.0f * w / l;

            if (m2 > miterLimit)
            {
                drawBevelJoin(p, d1, d2);
                return;
            }

            a.X = a.X / l * m2 * w;
            a.Y = a.Y / l * m2 * w;

            rendererCallback.BeginBlock();
            rendererCallback.AddVertex(p.X, p.Y, color, maxu, 0.0f);
            if (d1.X * d2.Y - d1.Y * d2.X > 0.0)
            {
                rendererCallback.AddVertex(p.X - a.X, p.Y - a.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X - d1.X, p.Y - d1.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X - d2.X, p.Y - d2.Y, color, minu, 0.0f);
            }
            else
            {
                rendererCallback.AddVertex(p.X + a.X, p.Y + a.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X + d1.X, p.Y + d1.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X + d2.X, p.Y + d2.Y, color, minu, 0.0f);

            }
            rendererCallback.AddTriangle(2, 1, 0); //0, 1, 2
            rendererCallback.AddTriangle(0, 1, 3); //0, 1, 3
            rendererCallback.EndBlock();
        }

        private void drawRoundJoin(PointF p, PointF d1, PointF d2)
        {
            double startAngle, stopAngle;
            double dlength = Math.Sqrt(d1.X * d1.X + d1.Y * d1.Y);

            rendererCallback.BeginBlock();

            rendererCallback.AddVertex(p.X, p.Y, color, maxu, 0.0f);
            if (d1.X * d2.Y - d1.Y * d2.X > 0.0)
            {
                startAngle = Math.Atan2(-d1.Y, -d1.X);
                stopAngle = Math.Atan2(-d2.Y, -d2.X);
                rendererCallback.AddVertex(p.X - d1.X, p.Y - d1.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X - d2.X, p.Y - d2.Y, color, minu, 0.0f);
            }
            else
            {
                startAngle = Math.Atan2(d1.Y, d1.X);
                stopAngle = Math.Atan2(d2.Y, d2.X);
                rendererCallback.AddVertex(p.X + d1.X, p.Y + d1.Y, color, minu, 0.0f);
                rendererCallback.AddVertex(p.X + d2.X, p.Y + d2.Y, color, minu, 0.0f);
            }

            if (stopAngle > startAngle + Math.PI)
                stopAngle -= Math.PI * 2.0;
            else if (stopAngle < startAngle - Math.PI)
                stopAngle += Math.PI * 2.0;

            int nstep = (int)(Math.Abs(stopAngle - startAngle) / (Math.PI * 2.0) * 50.0);
            int index = 1;

            //	printf("%lf %lf %d\n", startAngle / Math.PI * 180.0, stopAngle / Math.PI * 180.0, nstep);
            for (int i = 1; i < nstep; i++)
            {
                double angle = startAngle + (stopAngle - startAngle) * i / nstep;
                double x = Math.Cos(angle) * dlength;
                double y = Math.Sin(angle) * dlength;
                int a = rendererCallback.AddVertex((float)(p.X + x), (float)(p.Y + y), color, minu, 0.0f);

                rendererCallback.AddTriangle(0, index, a);
                index = a;
            }
            rendererCallback.AddTriangle(0, index, 2);

            rendererCallback.EndBlock();
        }

    }
}

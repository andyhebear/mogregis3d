using System;
using System.Collections.Generic;
using System.Text;
using Affine2 = System.Drawing.Drawing2D.Matrix;
using Color = System.Drawing.Color;
namespace scenic.Render
{
    public class Context
    {
#if PENDING
        private Surface originalRenderTarget;
        public FilterKernel filterKernel;
        public Canvas canvas;

        public VertexStore glyphVs;

        public PolygonRenderer polygonRenderer;
        public Texture aaPolygonTexture;
        public ClipArea clip;

        public Primitives primitives;
        public ScenePrimitives scenePrimitives;

        public Context(Canvas canvas);
        public Context(Image image);
#endif
        public ScenicColor color;
        public List<float> lineDashLengths = new List<float>();
        public Affine2 transform = new Affine2();
        private bool insideScene = false;

        public Image image;
        public Device device;
        public int width;
        public int height;

        public bool polygonAntialias = false;
        public double filterWidth;
        public double filterHeight;

        public float lineWidth;
        public LineCapStyle lineCap;
        public LineJoinStyle lineJoin;
        public float miterLimit;
        public float lineDashPhase;
        public LineRenderer lineRenderer;
        public IRendererCallback vertexRenderer;

        public bool beginScene()
        {
            //TODO 
            return true;
        }

        public void endScene()
        {
            //TODO
        }

        public void freeDeviceResources()
        {
#if PENDING
            if (aaPolygonTexture)
                aaPolygonTexture.release();
#endif
        }

#if PENDING
        public void setPrimitives(Primitives p)
        {
            if (primitives == p)
                return;

            if (primitives)
                primitives.setContext(NULL);

            primitives = p;
            if (primitives)
                primitives.setContext(this);
        }

        public void setRenderTarget(Surface target)
        {
            SurfaceInfo info = target.getInfo();
            device.setRenderTarget(target);
            initializeViewport(info.width, info.height);
        }

        public void initializeViewport(int width, int height)
        {
            device.initializeViewport(width, height);
        }
#endif
    }
}

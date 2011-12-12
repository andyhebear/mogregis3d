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

using Mogre;
using Mogre.Utils.GluTesselator;

using Filter = Scenic.filter.Filter;
//using Renderer = scenic.jni.Renderer;
namespace Scenic
{
    public class Device
    {
    }

    public class Renderer
    {
        public const int SURFACE_TYPE_COLOR = 0;
        public const int SURFACE_TYPE_ALPHA = 1;
        public const int SURFACE_TYPE_ALPHA2X = 2;
        public const int SURFACE_TYPE_ALPHA4X = 3;

        List<Render.Context> contexts = new List<Scenic.Render.Context>();
        static Render.LineRenderer lineRenderer = new Scenic.Render.LineRenderer();
        private GLUtessellatorImpl Glu;

        public Renderer(Render.IRendererCallback callbacks)
        {
            Scenic.Render.Context render = new Scenic.Render.Context();
            render.lineRenderer = lineRenderer;
            render.vertexRenderer = callbacks;
            contexts.Add(render);
        }

        // Text 
        public void beginText(int context)
        {
            throw new NotImplementedException();
        }

        public void endText(int context)
        {
            throw new NotImplementedException();
        }

        public void setTextTexture(int context, int image)
        {
            throw new NotImplementedException();
        }

        public void drawGlyph(int context, float tex_x, float tex_y,
                        float tex_width, float tex_height, int screen_x, int screen_y,
                        int screen_widtht, int screen_height)
        {
            throw new NotImplementedException();
        }


        // Tesselerator 
        public void tessBegin(int context)
        {
            Render.Context cntxt = getContext(context);
            cntxt.vertexRenderer.BeginBlock();
            GLUtessellatorCallback callback = new MogreTessellationCallbacks(null);

            Glu = (GLUtessellatorImpl)GLUtessellatorImpl.gluNewTess();
            Glu.gluTessCallback(GLU.GLU_TESS_VERTEX, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_BEGIN, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_END, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_ERROR, callback);
            Glu.gluTessCallback(GLU.GLU_TESS_COMBINE, callback);
            Glu.gluTessBeginPolygon(null);
        }

        public void tessEnd(int context)
        {
            Glu.gluTessNormal(0, 0, 1);
            Glu.gluTessEndPolygon();

            Render.Context cntxt = getContext(context);
            cntxt.vertexRenderer.EndBlock();
        }

        public void tessBeginContour(int context)
        {
            Glu.gluTessBeginContour();
        }

        public void tessEndContour(int context)
        {
            Glu.gluTessEndContour();
        }

        public void tessVertex(int context, double x, double y)
        {
            Glu.gluTessVertex(new double[] { x, y, 0 }, 0, new Mogre.Vector3((float)x, (float)y, (float)0));
        }

        public void tessTriangle(int context, int vertex1, bool edge1, int vertex2, bool edge2,
                        int vertex3, bool edge3)
        {
            throw new NotImplementedException();
        }


        // Line 
        public void polylineSetStyle(int context, float width, int cap, int joint, float miterLimit,
                        float[] lineDashLengths, double lineDashPhase)
        {
            throw new NotImplementedException();
        }

        public void polylineBegin(int contextId, bool closed)
        {
            Render.Context context = getContext(contextId);

            context.lineRenderer = lineRenderer;
            context.lineRenderer.begin(context, closed);
        }

        public void polylineEnd(int contextId)
        {
            Render.Context context = getContext(contextId);
            context.lineRenderer.end();
        }

        public void polylinePoint(int contextId, float x, float y)
        {
            Render.Context context = getContext(contextId);
            System.Drawing.PointF p = new System.Drawing.PointF(x, y);
            context.lineRenderer.addPoint(p);
        }

        // Primitives 
        public void beginPrimitives(int context)
        {
            throw new NotImplementedException();
        }

        public void setAttribute(int context, int index, int size, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void setAttribute(int context, int index, int size, float[] data)
        {
            throw new NotImplementedException();
        }

        public void setFragmentParameter(int context, int index, float[] parameter)
        {
            throw new NotImplementedException();
        }

        public void setTexture(int context, int index, int image)
        {
            throw new NotImplementedException();
        }

        public void drawIndices(int context, int type, int[] indices)
        {
            throw new NotImplementedException();
        }

        public void endPrimitives(int context)
        {
            throw new NotImplementedException();
        }


        // Surfaces 
        public int beginClip(int context, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public bool beginClip(int context, ref System.Drawing.Rectangle bounds)
        {
            return beginClip(context, bounds.X, bounds.Y, bounds.Width, bounds.Height) != 0;
        }

        public int beginSurface(int contextId, int x, int y, int width, int height, int type)
        {
            throw new NotImplementedException();

#if PENDING
                Render.Context context = getContext(contextId);
                ClipArea clip = new ClipArea();

                clip.parent = context.clip;
                clip.x = max(context.clip.x, x);
                clip.y = max(context.clip.y, y);
                clip.width = min(context.clip.x + context.clip.width, x + width) - clip.x;
                clip.height = min(context.clip.y + context.clip.height, y + height) - clip.y;
                clip.type = type;
                clip.texture = 0;
                clip.offsetX = clip.x;
                clip.offsetY = clip.y;
                clip.linearColorSpace = clip.parent.linearColorSpace;

                if (clip.width < 1 || clip.height < 1)
                {
                    delete clip;

                    return 0;
                }

                context.clip = clip;
                context.setPrimitives(0);

                Texture oldTexture = textureCache.getTexture(clip.width, clip.height);
                int w = 0, h = 0;

                if (oldTexture && !oldTexture.contentsLost() &&
                    oldTexture.getWidth() >= clip.width &&
                    oldTexture.getHeight() >= clip.height)
                {
                    //ENTER(Java_scenic_jni_Renderer_beginSurface.1);
                    clip.texture = oldTexture;
                    oldTexture = 0;
                    clip.textureWidth = clip.texture.getWidth();
                    clip.textureHeight = clip.texture.getHeight();
                }
                if (oldTexture)
                {
                    w = oldTexture.getWidth();
                    h = oldTexture.getHeight();
                    oldTexture.release();
                    oldTexture = 0;
                }
                if (!clip.texture)
                {
                    //ENTER(Java_scenic_jni_Renderer_beginSurface.2);
                    w = max(clip.width, w + 100);
                    h = max(clip.height, h + 100);

                    // Some graphics cards (GeForce 2 - 4) do not support small render targets
                    if (w < 16)
                        w = 16;
                    if (h < 16)
                        h = 16;

                    Console.WriteLine("Create temporary surface (width=%d, height=%d)\n", w, h);
                    clip.texture = new Texture(w, h, FMT_A8R8G8B8, USAGE_RENDER_TARGET);
                    if (!clip.texture)
                    {
                        Console.WriteLine("Device::createTexture failed\n");

                        return 0;
                    }
                    clip.textureWidth = w;
                    clip.textureHeight = h;
                }
                {
                    //ENTER(Java_scenic_jni_Renderer_beginSurface.3);
                    clip.surface = clip.texture.getSurface(0);
                    context.setRenderTarget(clip.surface);
                    RGBAFloat color;

                    color.red = color.green = color.blue = color.alpha = 0.0;

                    context.device.clear(color, NULL);
                }

                return 1;
#endif
            return 0;
        }

        public int beginSurface(int context, ref System.Drawing.Rectangle bounds, int type)
        {
            return beginSurface(context, bounds.X, bounds.Y, bounds.Width, bounds.Height, type);
        }

        public void drawSurface(int contextId)
        {
            Render.Context context = getContext(contextId);


#if PENDING
	        ENTER(Java_scenic_jni_Renderer_drawSurface);

	        Context *context = getContext(contextId);
	        int diffuse = context->color.getRGBA32();
	        ClipArea *surface;
	        VertexStore vs(VSF_TEX1, context, 0);

	        context->setPrimitives(0);

	        surface = context->clip;
	        context->clip = context->clip->parent;
	        context->setRenderTarget(context->clip->surface);
	        surface->surface->release();
	        surface->surface = NULL;

	        for(int y = 0; y < 2; y++)
	        {
		        for(int x = 0; x < 2; x++)
		        {
			        double lx = surface->x + x * surface->width;
			        double ly = surface->y + y * surface->height;
			        double tx1 = (float)x / surface->textureWidth * surface->width;
			        double ty1 = (float)y / surface->textureHeight * surface->height;

			        vs.addVertex(lx, ly, diffuse, tx1, ty1);
		        }
	        }

	        vs.addTriangle(0, 1, 2);
	        vs.addTriangle(3, 2, 1);

	        context->device->setTexture(0, surface->texture);
	        context->device->setSamplerStates(0, FILTER_POINT, ADDRESS_CLAMP);
	        context->device->setTextureStageModulation(0, surface->getModulation());
	        context->device->setBlendFunc(BLEND_ONE, BLEND_ONE_MINUS_SRC_ALPHA);
	        if(surface->linearColorSpace && !context->clip->linearColorSpace)
		        context->device->setState(STATE_SRGBWRITEENABLE, BOOLEAN_TRUE);

	        vs.draw();

	        context->device->setTexture(0, NULL);
	        context->device->setState(STATE_SRGBWRITEENABLE, BOOLEAN_FALSE);

	        textureCache.addTexture(surface->texture);
	        delete surface;
#endif
        }

        public void drawSurfaceAndClip(int context)
        {
            throw new NotImplementedException();
        }

        public void discardSurface(int context)
        {
            throw new NotImplementedException();
        }

        public Render.Context getContext(int contextid)
        {
            return contexts[contextid];
        }

        public void color(int contextId, ScenicColor pcolor)
        {
            Render.Context context = getContext(contextId);
            context.color = pcolor;
        }


        public void setTransform(int contextId, System.Drawing.Drawing2D.Matrix affine)
        {
#if PENDING
	        ENTER(Java_scenic_jni_Renderer_setTransform);
	        Affine2 t;

	        t.m00 = m00;
	        t.m01 = m01;
	        t.m10 = m10;
	        t.m11 = m11;
	        t.dx = dx;
	        t.dy = dy;
#endif
            Scenic.Render.Context context = getContext(contextId);
            context.transform = affine;
        }
        public void polylineSetStyle(int contextId, float width, LineCapStyle cap, LineJoinStyle join,
                                        float miterLimit, float[] lineDashLengths, float lineDashPhase)
        {
            Scenic.Render.Context context = getContext(contextId);

            context.lineWidth = width;
            context.lineCap = cap;
            context.lineJoin = join;
            context.miterLimit = miterLimit;
            context.lineDashLengths = new List<float>();
            if (lineDashLengths != null)
            {
                for (int i = 0; i < lineDashLengths.GetLongLength(0); i++)
                    context.lineDashLengths.Add(lineDashLengths[i]);
            }
            context.lineDashPhase = lineDashPhase;
        }
    }

    public class DrawContext : System.ICloneable
    {
        public Device device;
        public SceneParent root;
        public System.Object renderingInstance;
        public int context;
        public int width;
        public int height;
        public Renderer renderer;
        //UPGRADE_NOTE: The initialization of  'surfaceType' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005_3"'
        public int surfaceType = Renderer.SURFACE_TYPE_COLOR;
        public Filter AAFilter;
        public double pathError = 1.0 / 10.0;

        public DrawContext(Render.IRendererCallback callbacks)
        {
            renderer = new Renderer(callbacks);
        }

        public DrawContext(DrawContext context, int surfaceType)
        {
            this.device = context.device;
            this.root = context.root;
            this.renderingInstance = context.renderingInstance;
            this.context = context.context;
            this.width = context.width;
            this.height = context.height;
            this.renderer = context.renderer;
            this.surfaceType = surfaceType;
            this.AAFilter = context.AAFilter;
            this.pathError = context.pathError;
        }

        //UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1224_3"'
        public virtual System.Object Clone()
        {
            try
            {
                return (DrawContext)base.MemberwiseClone();
            }
            //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100_3"'
            catch (System.Exception e)
            {
                Console.WriteLine("Error: " + e);
                return null;
            }
        }

        public virtual bool usesCustomAAFilter()
        {
            return AAFilter != null && AAFilter != SceneSettings.DefaultAAFilter;
        }
    }


    public class MogreTessellationCallbacks : GLUtessellatorCallback
    {
        public ManualObject manualObj;

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


}
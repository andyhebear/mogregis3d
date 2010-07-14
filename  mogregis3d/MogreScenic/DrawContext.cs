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

using Filter = scenic.filter.Filter;
//using Renderer = scenic.jni.Renderer;
namespace scenic
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

        List<Render.Context> contexts = new List<scenic.Render.Context>();
        static Render.LineRenderer lineRenderer = new scenic.Render.LineRenderer();

        public Renderer(Render.IRendererCallback callbacks)
        {
            scenic.Render.Context render = new scenic.Render.Context();
            render.lineRenderer = lineRenderer;
            render.vertexRenderer = callbacks;
            contexts.Add(render);
        }

        public void tessVertex(int contextId, double x, double y)
        {
        }

        public void tessTriangle(int contextId, int vertex1, bool edge1, int vertex2, bool edge2, int vertex3, bool edge3)
        {
        }

        Render.Context getContext(int contextid)
        {
            return contexts[contextid];
        }

        public void polylineBegin(int contextId, bool closed)
        {
            Render.Context context = getContext(contextId);

            context.lineRenderer = lineRenderer;
            context.lineRenderer.begin(context, closed ? true : false);
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

        public int beginSurface(int contextId, ref System.Drawing.Rectangle rec, int type)
        {

            Render.Context context = getContext(contextId);
#if PENDING
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

        public void color(int contextId, ScenicColor pcolor)
        {
            Render.Context context = getContext(contextId);
            context.color = pcolor;
        }

        public void drawSurface(int contextId)
        {
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
            scenic.Render.Context context = getContext(contextId);
            context.transform = affine;
        }
        public void polylineSetStyle(int contextId, float width, LineCapStyle cap, LineJoinStyle join,
                                        float miterLimit, float[] lineDashLengths, float lineDashPhase)
        {
            scenic.Render.Context context = getContext(contextId);

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
}
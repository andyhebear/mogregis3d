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
//using Renderer = scenic.jni.Renderer;
namespace Scenic
{

    /// <summary> 
    /// This is the base class for shapes. Shapes are scene nodes that 
    /// specify an area. The area of the shape is drawn with a color. 
    /// The default color is white (red=1, green=1, blue=1, alpha=1).
    /// </summary>
    public abstract class SceneShape : SceneLeaf
    {

        /// <summary> Gets/Sets the shape color.</summary>
        virtual public ScenicColor Color
        {
            get
            {
                return color;
            }

            set
            {
                this.color = value;
                changed();
            }

        }

        private static readonly ScenicColor white = new ScenicColor(1.0f, 1.0f, 1.0f, 1.0f);

        private ScenicColor color = white;

        public SceneShape()
            : base()
        {
        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle visibleArea)
        {
            int surfaceType = 0; // TODO PENDING getSurfaceType(getDrawType(context, transform, color));

            if (surfaceType == Renderer.SURFACE_TYPE_COLOR || (surfaceType == context.surfaceType && color.White))
            {
                draw(context, transform, color);
            }
            else
            {
                System.Drawing.Rectangle bounds = getBounds(context, transform);

                if (bounds.Width <= 0 || bounds.Height <= 0)
                    return;
                System.Drawing.Rectangle tempAux = addSafetyMargin(bounds);
                if (context.renderer.beginSurface(context.context, ref tempAux, surfaceType) != 0)
                    return;
                //draw(context, transform, white);
                draw(context, transform, color);
                context.renderer.color(context.context, color);
                context.renderer.drawSurface(context.context);
            }
        }

        internal abstract void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color);

        internal override void prepareDraw(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
        }

        internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            return getDrawType(context, transform, color);
        }

        internal virtual int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
			return DRAW_SIMPLE;
        }

        internal abstract override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform);
    }
}
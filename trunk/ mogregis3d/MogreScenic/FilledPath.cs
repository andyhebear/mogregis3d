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
using Filter = scenic.filter.Filter;
using Path = scenic.path.Path;
namespace scenic
{

    /// <summary> FilledPath class fills the area inside the given path.</summary>
    public class FilledPath : SceneShape
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the path to be filled.
        /// 
        /// </summary>
        /// <returns> the path to be filled.
        /// </returns>
        /// <summary> Sets the path to be filled.
        /// 
        /// </summary>
        /// <param name="path">the path to be filled.
        /// </param>
        virtual public Path Path
        {
            get
            {
                return path;
            }

            set
            {
                this.path = value;
                changed();
                if (tesselator != null)
                    tesselator.Path = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the Fill rule.</summary>
        /// <summary> Sets the Fill rule used when filling the path. The Fill
        /// rule must be one of the constants defined in the FillRule
        /// interface.
        /// 
        /// </summary>
        /// <param name="rule">the Fill rule. 
        /// </param>
        virtual public FillRule FillRule
        {
            get
            {
                return fillRule;
            }

            set
            {
                fillRule = value;
                changed();
            }

        }
        private Path path;
        private FillRule fillRule;
        private Tesselator tesselator;

        /// <summary> Constructs a filled path using the given path.
        /// 
        /// </summary>
        /// <param name="path">the path to be filled.
        /// </param>
        public FilledPath(Path path)
        {
            this.path = path;
        }

        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
			if (tesselator == null)
				tesselator = new Tesselator(path);
			
			context.renderer.setTransform(context.context, transform);
			context.renderer.color(context.context, color);
			//PENDING context.renderer.setPolygonAntialiasingFilter(context.context, context.AAFilter);
			tesselator.draw(context, transform);
        }

        internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
#if PENDING
			return (context.AAFilter != null)?DRAW_SURFACE4X:DRAW_SIMPLE;
#endif
            return 0;
        }

        internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            return 0; //PENDING (context.AAFilter != null && Color.White) ? DRAW_SURFACE4X : DRAW_SIMPLE;
        }

        internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            System.Drawing.Rectangle r = path.getBounds(transform);
            Filter filter = context.AAFilter;

            // Compensate for the antialiasing filter
            if (filter != null)
            {
                r = addMargin(r, (int)System.Math.Ceiling(filter.Width), (int)System.Math.Ceiling(filter.Height));
            }
            return r;
        }

        protected internal override void hide()
        {
            base.hide();
        }
    }
}
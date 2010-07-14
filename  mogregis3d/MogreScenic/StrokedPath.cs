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
using Path = scenic.path.Path;
using PathWalker = scenic.path.PathWalker;
namespace scenic
{

    /// <summary> This shape strokes a path using the given parameters.</summary>
    public class StrokedPath : SceneShape
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the path to be stroked.</summary>
        /// <summary> Sets the path to be stroked.</summary>
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
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the line width. The line width is defined in logical units.</summary>
        /// <summary> Sets the line width. The line width is defined in logical units.</summary>
        virtual public float LineWidth
        {
            get
            {
                return lineWidth;
            }

            set
            {
                this.lineWidth = value;
                changed();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the line cap style.</summary>
        /// <summary> Sets the line cap style. The line cap style must be one of the 
        /// constants in the LineCapStyle interface.
        /// </summary>
        virtual public LineCapStyle EndCap
        {
            get
            {
                return endCap;
            }

            set
            {
                this.endCap = value;
                changed();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the line join style.</summary>
        /// <summary> Sets the line join style. The line join style must be one of the
        /// constants in the LineJoinStyle interface.
        /// </summary>
        virtual public LineJoinStyle LineJoin
        {
            get
            {
                return lineJoin;
            }

            set
            {
                this.lineJoin = value;
                changed();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the miter limit.</summary>
        /// <summary> Sets the miter limit. The miter limit defines the maximum length
        /// of the spike when using miter joins. If the spike is longer then the
        /// miter limit a bevel join is used instead.
        /// </summary>
        virtual public float MiterLimit
        {
            get
            {
                return miterLimit;
            }

            set
            {
                miterLimit = value;
                changed();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the line dash lengths.</summary>
        /// <summary> Sets the line dash pattern. The dash pattern is defined
        /// using an array which contains the lengths of consecutive
        /// visible and non-visible portions of the dash pattern. The 
        /// lenths are given in logical units. 
        /// 
        /// </summary>
        /// <param name="lineDashLengths">the line dash pattern.
        /// </param>
        virtual public float[] DashArray
        {
            get
            {
                return dashArray;
            }

            set
            {
                this.dashArray = value;
                changed();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the line dash phase.</summary>
        /// <summary> Sets the phase of the line dash pattern. The phase defines the
        /// starting position of the line dash pattern.
        /// 
        /// </summary>
        /// <param name="lineDashPhase">the line dash phase.
        /// </param>
        virtual public float DashPhase
        {
            get
            {
                return dashPhase;
            }

            set
            {
                this.dashPhase = value;
                changed();
            }

        }
        private Path path;
        private float lineWidth = 1.0f;
        private LineCapStyle endCap = LineCapStyle.BUTT_CAP;
        private LineJoinStyle lineJoin = LineJoinStyle.BEVEL_JOIN;
        private float miterLimit = 10.0f;
        private float[] dashArray;
        private float dashPhase;

        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Walker' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019_3"'
        private class Walker : PathWalker
        {
            private void InitBlock(StrokedPath enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private StrokedPath enclosingInstance;
            public StrokedPath Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            private DrawContext context;

            public Walker(StrokedPath enclosingInstance, DrawContext context)
            {
                InitBlock(enclosingInstance);
                this.context = context;
            }

            public virtual void beginSubPath(bool isClosed)
            {
                context.renderer.polylineBegin(context.context, isClosed);
            }

            public virtual void endSubPath()
            {
                context.renderer.polylineEnd(context.context);
            }

            public virtual void lineTo(float x, float y)
            {
                context.renderer.polylinePoint(context.context, x, y);
            }
        }

        /// <summary> Creates a StrokedPath object with the given path.
        /// 
        /// </summary>
        /// <param name="path">the path to be stroked.
        /// </param>
        public StrokedPath(Path path)
        {
            this.path = path;
        }

        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
			context.renderer.color(context.context, color);
			context.renderer.setTransform(context.context, transform);
			context.renderer.polylineSetStyle(context.context, (float) lineWidth, endCap, lineJoin, (float) miterLimit, dashArray, dashPhase);
			path.walk(new Walker(this, context), transform, context.pathError);
        }

        internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
#if PENDING
			if (color.alpha != 1.0f)
				return SceneShape.DRAW_SURFACE;
			return SceneShape.DRAW_SIMPLE;
#endif
            return 0;
        }

        internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            System.Drawing.Rectangle r = path.getBounds(transform);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
            System.Drawing.Rectangle tempAux = new System.Drawing.Rectangle(0, 0, (int)System.Math.Ceiling(lineWidth / 2.0 * 4.0), (int)System.Math.Ceiling(lineWidth / 2.0 * 4.0));
            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            System.Drawing.Rectangle wr = Util.transform(transform, ref tempAux);

            r.X -= wr.Width;
            r.Y -= wr.Width;
            r.Width += wr.Width * 2;
            r.Height += wr.Height * 2;

            return r;
        }
    }
}
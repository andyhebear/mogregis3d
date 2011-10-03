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
using BoxKernel = Scenic.filter.BoxKernel;
using Filter = Scenic.filter.Filter;
namespace Scenic
{

    /// <summary> Scene settings class is used to apply rendering settings to child nodes. 
    /// in the scene graph. Currently experimental, does not work.
    /// </summary>
    public class SceneSettings : SceneContainer
    {
        /// <summary> This filter represents the default antialiasing filter. When
        /// this filter is used, the library uses the default antialiasing algorithms
        /// of the underlying implementation.
        /// </summary>
        public static Filter DefaultAAFilter
        {
            get
            {
                return defaultAAFilter;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the antialiasing filter that is used to antialias polygons and text.
        /// 
        /// </summary>
        /// <returns> the antialiasing filter
        /// </returns>
        /// <summary> Sets the antialiasing filter that is used to antialias polygons and text.
        /// Settings the filter to null disables antialiasing.
        /// 
        /// </summary>
        /// <param name="antialiasingFilter">the antialiasing filter
        /// </param>
        virtual public Filter AntialiasingFilter
        {
            get
            {
                return antialiasingFilter;
            }

            set
            {
                this.antialiasingFilter = value;
            }

        }
        //UPGRADE_NOTE: Final was removed from the declaration of 'defaultAAFilter '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003_3"'
        //UPGRADE_NOTE: The initialization of  'defaultAAFilter' was moved to static method 'scenic.SceneSettings'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005_3"'
        private static readonly Filter defaultAAFilter;

        private Filter antialiasingFilter;

        /// <summary> Constructs a new SceneSettings object.</summary>
        public SceneSettings()
        {
        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle visibleArea)
        {
            DrawContext newContext = (DrawContext)context.Clone();

            newContext.AAFilter = antialiasingFilter;
            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            base.draw(newContext, transform, ref visibleArea);
        }

        internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            DrawContext newContext = (DrawContext)context.Clone();

            newContext.AAFilter = antialiasingFilter;
            return base.getDrawType(newContext, transform);
        }

        internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            DrawContext newContext = (DrawContext)context.Clone();

            newContext.AAFilter = antialiasingFilter;
            return base.getBounds(newContext, transform);
        }
        static SceneSettings()
        {
            defaultAAFilter = new Filter(BoxKernel.Kernel, 1.0, 1.0);
        }
    }
}
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
namespace scenic
{

    /// <summary> <p>This scene node clips its children inside a Clip area. The Clip
    /// area is defined by a scene. The clipping is performed by modulating
    /// the child scene with the Clip scene. The Clip scene is usually a 
    /// shape, but it can be any scene.</p>
    /// 
    /// <p>SceneClip can be used to paint a shape with a brush by setting
    /// the shape as the Clip shape and adding the brush as a child.</p>
    /// 
    /// </summary>
    public class SceneClip : SceneContainer
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the Clip scene.
        /// 
        /// </summary>
        /// <returns> the Clip scene.
        /// </returns>
        /// <summary> Sets the Clip scene.
        /// 
        /// </summary>
        /// <param name="Clip">the Clip scene.
        /// </param>
        virtual public SceneNode Clip
        {
            get
            {
                return clip;
            }

            set
            {
#if PENDING
				if (isVisible())
					this.clip.removeVisibleParent(this);
				this.clip = value;
				if (isVisible())
					this.clip.addVisibleParent(this);
				changed();
#endif
            }

        }
        private SceneNode clip;

        /// <summary> Construct an SceneClip object with the given Clip scene.
        /// 
        /// </summary>
        /// <param name="Clip">the Clip scene.
        /// </param>
        public SceneClip(SceneNode clip)
        {
#if PENDING
			this.clip = clip;
			if (isVisible())
				clip.addVisibleParent(this);
#endif
        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle visibleArea)
        {
#if PENDING
			System.Drawing.Rectangle bounds = getBounds(context, transform);
			int clipDrawType = clip.getDrawType(context, transform);
			
			if (bounds.Width <= 0 || bounds.Height <= 0)
				return ;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			bounds = addSafetyMargin(ref bounds);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			if (context.renderer.beginSurface(context.context, ref bounds, Renderer.SURFACE_TYPE_COLOR) == 0)
				return ;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			base.draw(context, transform, ref bounds);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			if (context.renderer.beginSurface(context.context, ref bounds, getSurfaceType(clipDrawType)) == 0)
				return ;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			clip.draw(new DrawContext(context, clipDrawType), transform, ref bounds);
			context.renderer.color(context.context, 1.0f, 1.0f, 1.0f, 1.0f);
			context.renderer.drawSurfaceAndClip(context.context);
#endif
        }

        protected internal override void show()
        {
#if PENDING
			base.show();
			if (clip != null)
				clip.addVisibleParent(this);
#endif
        }

        protected internal override void hide()
        {
#if PENDING
			base.hide();
			if (clip != null)
				clip.removeVisibleParent(this);
#endif

        }

        internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            System.Drawing.Rectangle result = base.getBounds(context, transform);

            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            System.Drawing.Rectangle tempAux = clip.getBounds(context, transform);
            Util.common(ref result, ref tempAux);
            return result;
        }
    }
}
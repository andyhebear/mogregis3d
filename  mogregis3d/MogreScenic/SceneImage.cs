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
namespace Scenic
{

    /// <summary> This class is used to draw an image.</summary>
    public class SceneImage : SceneLeaf, ImageObserver
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the image.</summary>
        /// <summary> Sets the image.</summary>
        virtual public Image Image
        {
            get
            {
                return image;
            }

            set
            {
#if PENDING
				if (value != null)
					value.deleteObserver(this);
				this.image = (Image) value;
				if (isVisible() && value != null)
					value.addObserver(this);
				changed();
#endif
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the source Rectangle. The source Rectangle specifies
        /// the part of the image that is displayed. If the Rectangle is
        /// null, the entire image is displayed.
        /// </summary>
        /// <summary> Sets the source Rectangle. The source Rectangle specifies
        /// the part of the image that is displayed. If the Rectangle is
        /// null, the entire image is displayed.
        /// </summary>
        virtual public System.Drawing.RectangleF SourceRect
        {
            get
            {
                return sourceRect;
            }

            set
            {
                this.sourceRect = value;
                changed();
            }

        }
        private Image image;
        private System.Drawing.RectangleF sourceRect;

        /// <summary> Creates a SceneImage object with the given image.
        /// 
        /// </summary>
        /// <param name="image">the image.
        /// </param>
        public SceneImage(Image image)
        {
            Image = image;
        }

        /// <summary> Creates a SceneImage object with the given image.
        /// 
        /// </summary>
        /// <param name="image">the image.
        /// </param>
        /// <param name="sourceRect">the part of the image that is displayed.
        /// </param>
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public SceneImage(Image image, ref System.Drawing.RectangleF sourceRect)
        {
            Image = image;
            this.sourceRect = sourceRect;
        }

        protected internal override void show()
        {
#if PENDING
			base.show();
			if (image != null)
			{
				image.addObserver(this);
			}
#endif
        }

        protected internal override void hide()
        {
#if PENDING
			base.hide();
			if (image != null)
			{
				image.deleteObserver(this);
			}
#endif
        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle visibleArea)
        {
#if PENDING
			if (image == null || image.getWidth() == 0 || image.getHeight() == 0)
				return ;
			
			context.renderer.color(context.context, new Color(1.0f, 1.0f, 1.0f, 1.0f));
			context.renderer.setTransform(context.context, transform);
			if (!sourceRect.IsEmpty)
				context.renderer.drawImage(context.context, image.Id, (double) sourceRect.X, (double) sourceRect.Y, (double) sourceRect.Width, (double) sourceRect.Height);
			else
				context.renderer.drawImage(context.context, image.Id, 0, 0, image.getWidth(), image.getHeight());
#endif
        }

        internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            if (!sourceRect.IsEmpty)
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
                System.Drawing.Rectangle tempAux = new System.Drawing.Rectangle(0, 0, (int)sourceRect.Width, (int)sourceRect.Height);
                //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
                return Util.transform(transform, ref tempAux);
            }
            else
            {
                System.Drawing.Rectangle tempAux2 = new System.Drawing.Rectangle(0, 0, image.getWidth(), image.getHeight());
                //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
                return Util.transform(transform, ref tempAux2);
            }
        }

        public virtual void update(Image image)
        {
#if PENDING
            System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Empty;
            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            changed(ref tempAux);
#endif
        }
    }
}
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
//using AWTShapePath = scenic.path.AWTShapePath;
using Path = scenic.path.Path;
namespace scenic
{

    /// <summary> This shape draws text. The text is specified using glyph codes.
    /// The positions of the glyphs are given as an array that
    /// contains the x and y coordinates of each glyph.
    /// </summary>
    public class TextShape : SceneShape
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
        /// <summary> Gets the font.</summary>
        /// <summary> Sets the font.</summary>
        virtual public System.Drawing.Font Font
        {
            get
            {
                return font;
            }

            set
            {
                this.font = value;
                changed();
            }

        }
        /// <summary> Gets the glyph codes.</summary>
        virtual public int[] GlyphCodes
        {
            get
            {
                return glyphCodes;
            }

        }
        /// <summary> Gets the glyph positions.</summary>
        virtual public float[] Positions
        {
            get
            {
                return positions;
            }

        }
        private System.Drawing.Font font;
        private int[] glyphCodes;
        private float[] positions;
        private FilledPath[] glyphPolygons;

        private const double polygonSizeLimit = 100.0;
        private const double polygonFlatness = 1.0 / 10.0;

        /// <summary> Constructs a TextShape object with the given parameters.
        /// 
        /// </summary>
        /// <param name="font">the font.
        /// </param>
        /// <param name="glyphCodes">an array of glyph codes.
        /// </param>
        /// <param name="positions">an array containing the x and y coordinates of each glyph.
        /// </param>
        public TextShape(System.Drawing.Font font, int[] glyphCodes, float[] positions)
        {
            this.font = font;
            this.glyphCodes = glyphCodes;
            this.positions = positions;
        }

        /// <summary> Sets the glyph codes and positions.
        /// 
        /// </summary>
        /// <param name="glyphCodes">an array of glyph codes.
        /// </param>
        /// <param name="positions">an array containing the x and y coordinates of each glyph.
        /// </param>
        public virtual void setGlyphs(int[] glyphCodes, float[] positions)
        {
            this.glyphCodes = glyphCodes;
            this.positions = positions;
            glyphPolygons = null;
            changed();
        }

        internal override void changed()
        {
            glyphPolygons = null;
            base.changed();
        }

        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
            if (font == null)
                return;

            if (usePolygon(context, transform))
                drawUsingPolygon(context, transform, color);
            else
                drawUsingTexture(context, transform, color);
        }

        private void drawUsingPolygon(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
#if PENDING
			if (glyphPolygons == null)
			{
				glyphPolygons = new FilledPath[glyphCodes.Length];
				for (int i = 0; i < glyphCodes.Length; i++)
				{
					System.Drawing.Drawing2D.Matrix temp_Matrix;
					temp_Matrix = new System.Drawing.Drawing2D.Matrix();
					temp_Matrix.Translate((float) positions[i * 2], (float) positions[i * 2 + 1]);
					System.Drawing.Drawing2D.Matrix at = temp_Matrix;
					//UPGRADE_TODO: Interface 'java.awt.Shape' was converted to 'System.Drawing.Drawing2D.GraphicsPath' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
					System.Drawing.Drawing2D.GraphicsPath shape = TextRenderer.getGlyphOutline(font, at, glyphCodes[i]);
					Path path = new AWTShapePath(shape);
					
					glyphPolygons[i] = new FilledPath(path);
				}
			}
			
			System.Drawing.Drawing2D.Matrix temp = new System.Drawing.Drawing2D.Matrix();
			
			for (int i = 0; i < glyphPolygons.Length; i++)
			{
				System.Drawing.Drawing2D.Matrix temp_Matrix2;
				temp_Matrix2 = new System.Drawing.Drawing2D.Matrix();
				temp_Matrix2.Translate((float) positions[i * 2], (float) positions[i * 2 + 1]);
				System.Drawing.Drawing2D.Matrix at = temp_Matrix2;
				
				//UPGRADE_ISSUE: Method 'java.awt.geom.AffineTransform.setTransform' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtgeomAffineTransformsetTransform_javaawtgeomAffineTransform_3"'
				temp.setTransform(transform);
				//UPGRADE_TODO: Method 'java.awt.geom.AffineTransform.concatenate' was converted to 'System.Drawing.Drawing2D.Matrix.Multiply' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtgeomAffineTransformconcatenate_javaawtgeomAffineTransform_3"'
				temp.Multiply(at, System.Drawing.Drawing2D.MatrixOrder.Append);
				glyphPolygons[i].draw(context, temp, color);
			}
#endif
        }

        private void drawUsingTexture(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
#if PENDING
            System.Drawing.Drawing2D.Matrix at = Util.linearTransform(transform);

            GlyphCache.CachedFont drawFont = new GlyphCache.CachedFont(font, at);
            GlyphCache cache = Device.GlyphCache;
            Image img = (Image)cache.Image;
            double texm = 1.0 / img.getWidth();

            context.renderer.color(context.context, color);
            context.renderer.beginText(context.context);
            context.renderer.setTextTexture(context.context, img.Id);

            for (int i = 0; i < glyphCodes.Length; i++)
            {
                GlyphImage g = cache.getGlyph(drawFont, glyphCodes[i]);

                if (g == null)
                {
                    context.renderer.endText(context.context);
                    g = cache.renderGlyph(drawFont, glyphCodes[i]);
                    context.renderer.beginText(context.context);
                    context.renderer.setTextTexture(context.context, img.Id);
                }
                if (g != null && !g.cache.IsEmpty && g.width > 0 && g.height > 0)
                {
                    System.Drawing.Rectangle r = g.cache;
                    double px = positions[i * 2];
                    double py = positions[i * 2 + 1];
                    double tpx = (float)transform.Elements.GetValue(0) * px + (float)transform.Elements.GetValue(2) * py + (System.Single)transform.OffsetX;
                    double tpy = (float)transform.Elements.GetValue(1) * px + (float)transform.Elements.GetValue(3) * py + (System.Single)transform.OffsetY;
                    double ix = System.Math.Floor(tpx + 0.5001) - g.x;
                    double iy = System.Math.Floor(tpy + 0.5001) - g.y;

                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
                    context.renderer.drawGlyph(context.context, (float)(r.X * texm), (float)(r.Y * texm), (float)(r.Width * texm), (float)(r.Height * texm), (int)ix, (int)iy, g.width, g.height);
                }
            }
            context.renderer.endText(context.context);
#endif
        }

        private bool isStraight(System.Drawing.Drawing2D.Matrix tm)
        {
            return ((float)tm.Elements.GetValue(2) == 0 && (float)tm.Elements.GetValue(1) == 0) || ((float)tm.Elements.GetValue(0) == 0 && (float)tm.Elements.GetValue(3) == 0);
        }

        private bool usePolygon(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            transform.Elements.GetValue(0);
            transform.Elements.GetValue(3);
            transform.Elements.GetValue(2);
            return context.usesCustomAAFilter() || !isStraight(transform) || System.Math.Sqrt((float)transform.Elements.GetValue(1)) * (int)font.Size > polygonSizeLimit;
        }

        internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ScenicColor color)
        {
#if PENDING
            if (usePolygon(context, transform))
                return DRAW_SURFACE4X;
            else
                return DRAW_SIMPLE;
#endif
            return 0;
        }

        internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
#if PENDING
            System.Drawing.Rectangle r = System.Drawing.Rectangle.Empty;

            //UPGRADE_TODO: Class 'java.awt.font.FontRenderContext' was converted to 'System.Windows.Forms.Control' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_3"'
            //UPGRADE_ISSUE: Constructor 'java.awt.font.FontRenderContext.FontRenderContext' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontFontRenderContextFontRenderContext_javaawtgeomAffineTransform_boolean_boolean_3"'
            System.Drawing.Text.TextRenderingHint frc = new FontRenderContext(transform, false, true);
            for (int i = 0; i < glyphCodes.Length; i++)
            {

                //UPGRADE_ISSUE: Class 'java.awt.font.GlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtfontGlyphVector_3"'
                //UPGRADE_TODO: Method 'java.awt.Font.createGlyphVector' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095_3"'
                GlyphVector gv = font.createGlyphVector(frc, new int[] { glyphCodes[i] });
                System.Drawing.Rectangle gr = gv.getPixelBounds(frc, positions[i * 2], positions[i * 2 + 1]);

                if (r.IsEmpty)
                    r = gr;
                else
                    r = System.Drawing.Rectangle.Union(r, gr);
            }
            if (r.IsEmpty)
                return new System.Drawing.Rectangle();

            Filter filter = context.AAFilter;

            // Compensate for the antialiasing filter
            if (filter != null)
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
                //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
                r = addMargin(ref r, (int)System.Math.Ceiling(filter.Width), (int)System.Math.Ceiling(filter.Height));
            }
            return r;
#endif
            return new System.Drawing.Rectangle();
        }
    }
}
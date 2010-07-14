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
namespace scenic
{


    class GlyphCache
    {
        virtual public Image Image
        {
            get
            {
#if PENDING
                return cacheImage.getCacheImage();
#endif
                return new Image();
            }

        }
        public class CachedFont
        {
            public System.Drawing.Font font;
            public System.Drawing.Drawing2D.Matrix transform;

            public CachedFont(System.Drawing.Font font, System.Drawing.Drawing2D.Matrix transform)
            {
                this.font = font;
                this.transform = transform;
            }

            public override int GetHashCode()
            {
                return font.GetHashCode() ^ transform.GetHashCode();
            }

            public override bool Equals(System.Object o)
            {
                CachedFont f = (CachedFont)o;
                return font.Equals(f.font) && transform.Equals(f.transform);
            }
        }

        //Map<CachedFont, GlyphImage[]>
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
        private Dictionary<CachedFont, GlyphImage[]> fontMap = new Dictionary<CachedFont, GlyphImage[]>();
        private CacheImage cacheImage;

        public GlyphCache()
        {
#if PENDING
            cacheImage = new CacheImage(new Image(1024, 1024, Format.AL8));
#endif
        }

        public virtual GlyphImage[] getGlyphs(CachedFont font)
        {
#if PENDING
            GlyphImage[] r = (GlyphImage[])fontMap.get_Renamed(font);

            if (r == null)
            {
                //UPGRADE_ISSUE: Method 'java.awt.Font.getNumGlyphs' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtFontgetNumGlyphs_3"'
                r = new GlyphImage[font.font.getNumGlyphs()];
                fontMap.put(font, r);
            }

            return r;
#endif
            return new GlyphImage[0];
        }

        public virtual GlyphImage getGlyph(CachedFont font, int glyphCode)
        {
            GlyphImage g = getGlyphs(font)[glyphCode];

            //		System.out.println(g != null);

            if (g == null || g.cache.IsEmpty || g.cache.Width < 0)
            {
                return null;
            }

            return g;
        }

        public virtual GlyphImage renderGlyph(CachedFont font, int glyphCode)
        {
#if PENDING
            GlyphImage g = TextRenderer.renderGlyph(font.font, font.transform, glyphCode);

            g.cache = cacheImage.addImage(g);
            g.data = null;
            getGlyphs(font)[glyphCode] = g;

            return g;
#endif
            return new GlyphImage();
        }
    }
}
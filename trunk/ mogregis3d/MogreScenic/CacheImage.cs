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
namespace scenic
{
	
	
	class CacheImage
	{
#if PENDING
		private Image cacheImage;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
		private LinkedList < Rectangle > glyphList = new LinkedList < Rectangle >();
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
		private LinkedList < Rectangle > oldGlyphList = new LinkedList < Rectangle >();
		private int top;
		private int rowHeight;
		private int rowX;
		private int width;
		private int height;
		
		public CacheImage(Image image)
		{
			cacheImage = image;
			width = image.getWidth();
			height = image.getHeight();
		}
		
		public virtual Image getCacheImage()
		{
			return cacheImage;
		}
		
		public virtual System.Drawing.Rectangle addImage(ImageInfo image)
		{
			int w = image.width;
			int h = image.height;
			
			if (cacheImage.Format != image.format)
				return System.Drawing.Rectangle.Empty;
			
			if (rowX + w > width)
			{
				top += rowHeight;
				rowHeight = 0;
				rowX = 0;
			}
			if (top + h > height)
			{
				if (h > height)
					return System.Drawing.Rectangle.Empty;
				top = 0;
				rowX = 0;
				rowHeight = h;
				System.Collections.IEnumerator itr = oldGlyphList.listIterator();
				
				//UPGRADE_TODO: Method 'java.util.ListIterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilListIteratorhasNext_3"'
				while (itr.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.ListIterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilListIteratornext_3"'
					((System.Drawing.Rectangle) itr.Current).Width = - 1;
				}
				oldGlyphList = glyphList;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
				glyphList = new LinkedList < Rectangle >();
			}
			if (h > rowHeight)
				rowHeight = h;
			
			System.Collections.IEnumerator itr2 = oldGlyphList.listIterator();
			//UPGRADE_TODO: Method 'java.util.ListIterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilListIteratorhasNext_3"'
			while (itr2.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.ListIterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilListIteratornext_3"'
				System.Drawing.Rectangle r = (System.Drawing.Rectangle) itr2.Current;
				
				if (r.Y >= top + rowHeight)
					break;
				r.Width = - 1;
				//UPGRADE_ISSUE: Method 'java.util.ListIterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilListIteratorremove_3"'
				itr2.remove();
			}
			
			System.Drawing.Rectangle r2 = new System.Drawing.Rectangle(rowX, top, w, h);
			
			rowX += w;
			glyphList.addLast(r2);
			cacheImage.write(r2.X, r2.Y, r2.Width, r2.Height, image.data, image.format);
			
			return r2;
		}
#endif
	}
}
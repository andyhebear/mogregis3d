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
	
	
	class GlyphImage:ImageInfo
	{
		public int x;
		public int y;
		public System.Drawing.Rectangle cache;
		
		public override System.String ToString()
		{
			System.String s = new System.Text.StringBuilder().ToString();
			
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int i = data[y * width + x];
					if (i < 0)
						i += 256;
					if (i < 100)
						s += " ";
					if (i < 10)
						s += " ";
					s += (i + " ");
				}
				s += "\n";
			}
			
			return s;
		}
	}
}
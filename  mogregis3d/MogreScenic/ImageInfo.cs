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
	
	/// <summary> This class contains the information of an image. This class
	/// is meant as an utility class for storing image information. 
	/// ImageInfo objects can not be directly displayed. 
	/// To display images use the Image class.
	/// </summary>
	public class ImageInfo
	{
		public int width;
		public int height;
		public int format;
		public sbyte[] data;
	}
}
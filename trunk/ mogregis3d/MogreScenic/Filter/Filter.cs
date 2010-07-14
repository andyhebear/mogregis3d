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
namespace scenic.filter
{
	
	/// <summary> A 2d filter. The filter consists of the filter kernel, and the width
	/// and height settings.
	/// </summary>
	public class Filter
	{
		virtual public Kernel Kernel
		{
			get
			{
				return kernel;
			}
			
		}
		virtual public double Width
		{
			get
			{
				return width;
			}
			
		}
		virtual public double Height
		{
			get
			{
				return height;
			}
			
		}
		private Kernel kernel;
		private double width;
		private double height;
		
		public Filter(Kernel kernel, double width, double height)
		{
			this.kernel = kernel;
			this.width = width;
			this.height = height;
		}
	}
}
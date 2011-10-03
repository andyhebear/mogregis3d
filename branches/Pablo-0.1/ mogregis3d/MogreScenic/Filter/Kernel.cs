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
//using Format = scenic.Format;
using Image = Scenic.Image;
namespace Scenic.filter
{
	
	/// <summary> Abstract base class for 2d filter kernels.</summary>
	public abstract class Kernel
	{
		// Used from native code
		private int id;
		/// <summary> Returns the value of the filter kernel at the given point. The filter
		/// kernel is defined in the range [-0.5 0.5] for both x and y axis.
		/// Values outside that range are ignored. Integral over the defined range 
		/// should be 1.
		/// 
		/// </summary>
		/// <param name="x">x coordinate, in the range [-0.5 0.5]
		/// </param>
		/// <param name="y">y coordinate, in the range [-0.5 0.5]
		/// </param>
		/// <returns> value of the filter kernel at point (x, y)
		/// </returns>
		public abstract double getValue(double x, double y);
		
		public virtual Image createImage()
		{
#if PENDING
			Image image = new Image(32, 32, Format.ABGR_16F);
			float[] data = new float[image.getWidth() * image.getHeight()];
			double max = 0.0;
			
			for (int y = 0; y < image.getHeight(); y++)
			{
				for (int x = 0; x < image.getWidth(); x++)
				{
					double x2 = (x + 0.5) / image.getWidth() - 0.5;
					double y2 = (y + 0.5) / image.getHeight() - 0.5;
					double a = getValue(x2, y2);
					
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
					data[x + y * image.getWidth()] = (float) a;
					max = System.Math.Max(max, a);
				}
			}
			for (int y = 0; y < image.getHeight(); y++)
				for (int x = 0; x < image.getWidth(); x++)
					data[x + y * image.getWidth()] = (float) (data[x + y * image.getWidth()] / max);
			
			image.write(0, 0, image.getWidth(), image.getHeight(), data, Format.AL_32F);
			return image;
#endif 
            return new Image();
		}
	}
}
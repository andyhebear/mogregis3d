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
	
	/// <summary> This class defines a color that consists of red, green, blue and 
	/// alpha components. Each component is a single precision floating point number
	/// which ranges from 0 to 1. By default, alpha component 1 means fully 
	/// opaque and 0 fully transparent. Scenic uses premultiplied alpha
	/// ie. the red, green and blue channels are not multiplied with the alpha
	/// value when the color is displayed. The range of the color values is
	/// not limited. Currently, however, values outside 0 - 1 are not supported by the
	/// renderer.
	/// </summary>
	public class ScenicColor
	{
		virtual internal int A8R8G8B8
		{
			get
			{
				return ((int) (clamp(alpha) * 255.0f) << 24) | ((int) (clamp(red) * 255.0f) << 16) | ((int) (clamp(green) * 255.0f) << 8) | (int) (clamp(blue) * 255.0f);
			}
			
		}
		virtual internal bool White
		{
			get
			{
				return red == 1.0 && green == 1.0 && blue == 1.0 && alpha == 1.0;
			}
			
		}
		public float red;
		public float green;
		public float blue;
		public float alpha;
		
		/// <summary> Default constructor. All components are set to 0.
		/// 
		/// </summary>
		public ScenicColor()
		{
		}
		
		/// <summary> Creates a grey with the given luminance. Red, green and blue channels are
		/// set to the luminance value. Alpha is set to 1.
		/// 
		/// </summary>
		/// <param name="luminance">the luminance value.
		/// </param>
		public ScenicColor(float luminance)
		{
			red = luminance;
			green = luminance;
			blue = luminance;
			alpha = 1.0f;
		}
		
		/// <summary> Creates a color with the given red, green and blue values.
		/// Alpha is set to 1.
		/// </summary>
		public ScenicColor(float red, float green, float blue)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
			this.alpha = 1.0f;
		}
		
		/// <summary> Creates a color with the given red, green, blue and alpha
		/// values.
		/// </summary>
		public ScenicColor(float red, float green, float blue, float alpha)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
			this.alpha = alpha;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public ScenicColor(ref System.Drawing.Color color)
		{
			this.red = (int) color.R / 255.0f;
			this.green = (int) color.G / 255.0f;
			this.blue = (int) color.B / 255.0f;
			this.alpha = (int) color.A / 255.0f;
		}
		
		/// <summary> Creates a color using 8-bit component values in the range 0-255.</summary>
		public static ScenicColor byteColor(int red, int green, int blue)
		{
			return new ScenicColor(red / 255.0f, green / 255.0f, blue / 255.0f);
		}
		
		/// <summary> Creates a color using 8-bit component values in the range 0-255.</summary>
		public static ScenicColor byteColor(int red, int green, int blue, int alpha)
		{
			return new ScenicColor(red / 255.0f, green / 255.0f, blue / 255.0f, alpha / 255.0f);
		}
		
		/// <summary> Clamps the given value to the range [0, 1].
		/// 
		/// </summary>
		/// <param name="a">the value to be clamped.
		/// </param>
		/// <returns> the clamped value.
		/// </returns>
		public static float clamp(float a)
		{
			if (a < 0.0f)
				return 0.0f;
			else if (a > 1.0f)
				return 1.0f;
			return a;
		}
		
		/// <summary> Returns a color multiplied with the given value. Red, green and
		/// blue channels are multiplied with the given value. Alpha channel
		/// is left unchanged.
		/// 
		/// </summary>
		/// <param name="m">the value used for multiplication.
		/// </param>
		/// <returns> the multiplied color.
		/// </returns>
		public virtual ScenicColor multiply(float m)
		{
			return new ScenicColor(red * m, green * m, blue * m, alpha);
		}
		
		/// <summary> Returns a color multiplied by the alpha channel. Red, green and blue
		/// channels are multiplied with the alpha channel. The alpha channel
		/// is left unchanged.
		/// 
		/// </summary>
		/// <returns> the color multiplied by alpha.
		/// </returns>
		public virtual ScenicColor multiplyByAlpha()
		{
			return new ScenicColor(red * alpha, green * alpha, blue * alpha, alpha);
		}
		
		public override System.String ToString()
		{
			return "(" + red + " " + green + " " + blue + " " + alpha + ")";
		}
		
		/// <summary> Interpolates between the given colors. The v parameter should
		/// be in the range [0, 1].
		/// 
		/// </summary>
		/// <param name="a">the first color to be interpolated.
		/// </param>
		/// <param name="b">the second color to be interpolated.
		/// </param>
		/// <param name="v">selects the position between the colors.
		/// </param>
		/// <returns> the interpolated color.
		/// </returns>
		public static ScenicColor interpolate(ScenicColor a, ScenicColor b, float v)
		{
			float v2 = 1.0f - v;
			
			return new ScenicColor(a.red * v2 + b.red * v, a.green * v2 + b.green * v, a.blue * v2 + b.blue * v, a.alpha * v2 + b.alpha * v);
		}
	}
}
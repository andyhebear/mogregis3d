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
	
	/// <summary> This class contains utility methods.</summary>
	public class Util
	{
		/// <summary> Combines the given Rectangle with the given point. The given 
		/// Rectangle is modified so that it includes the given point.
		/// 
		/// </summary>
		/// <param name="a">the Rectangle.
		/// </param>
		/// <param name="x">the x coordinate of the point
		/// </param>
		/// <param name="y">the y coodrinate of the point
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public static void  combine(ref System.Drawing.Rectangle a, int x, int y)
		{
			if (a.Width == 0)
			{
				a.X = x;
				a.Y = y;
				a.Width = 1;
				a.Height = 1;
				return ;
			}
			int oldx = a.X;
			int oldy = a.Y;
			
			a.X = System.Math.Min(a.X, x);
			a.Y = System.Math.Min(a.Y, y);
			a.Width = System.Math.Max(oldx + a.Width, x + 1) - a.X;
			a.Height = System.Math.Max(oldy + a.Height, y + 1) - a.Y;
		}
		
		/// <summary> Combines the two rectangles. The Rectangle a is modified
		/// so that it contains the are defined by Rectangle b.
		/// 
		/// </summary>
		/// <param name="a">the first Rectangle.
		/// </param>
		/// <param name="b">the second Rectangle.
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public static void  combine(ref System.Drawing.Rectangle a, ref System.Drawing.Rectangle b)
		{
			if (a.Width == 0)
			{
				a.X = b.X;
				a.Y = b.Y;
				a.Width = b.Width;
				a.Height = b.Height;
				return ;
			}
			int oldx = a.X;
			int oldy = a.Y;
			
			a.X = System.Math.Min(a.X, b.X);
			a.Y = System.Math.Min(a.Y, b.Y);
			a.Width = System.Math.Max(oldx + a.Width, b.X + b.Width) - a.X;
			a.Height = System.Math.Max(oldy + a.Height, b.Y + b.Height) - a.Y;
		}
		
		/// <summary> Helper method which modifies the Rectangle so that it contains
		/// the given point when using the given transform.
		/// 
		/// </summary>
		/// <param name="tm">the transform.
		/// </param>
		/// <param name="r">the Rectangle.
		/// </param>
		/// <param name="xp">the x coordinate of the point.
		/// </param>
		/// <param name="yp">the y coordinate of the point.
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public static void  addPointToBounds(System.Drawing.Drawing2D.Matrix tm, ref System.Drawing.Rectangle r, double xp, double yp)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
			int xs = (int) System.Math.Floor((float) tm.Elements.GetValue(0) * xp + (float) tm.Elements.GetValue(2) * yp + (System.Single) tm.OffsetX);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
			int ys = (int) System.Math.Floor((float) tm.Elements.GetValue(1) * xp + (float) tm.Elements.GetValue(3) * yp + (System.Single) tm.OffsetY);
			
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			Util.combine(ref r, xs, ys);
		}
		
		/// <summary> Calculates the common area shared by the rectangles. 
		/// The Rectangle a is modified so that it contains the 
		/// common area of the two rectangles. The common area
		/// is the largest Rectangle that is inside the two
		/// rectangles.
		/// 
		/// </summary>
		/// <param name="a">the first Rectangle.
		/// </param>
		/// <param name="b">the second Rectangle.
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public static void  common(ref System.Drawing.Rectangle a, ref System.Drawing.Rectangle b)
		{
			int oldx = a.X;
			int oldy = a.Y;
			
			a.X = System.Math.Max(a.X, b.X);
			a.Y = System.Math.Max(a.Y, b.Y);
			a.Width = System.Math.Min(oldx + a.Width, b.X + b.Width) - a.X;
			a.Height = System.Math.Min(oldy + a.Height, b.Y + b.Height) - a.Y;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public static bool hasCommonArea(ref System.Drawing.Rectangle a, ref System.Drawing.Rectangle b)
		{
			return b.X < a.X + a.Width && b.Y < a.Y + a.Height && a.X < b.X + b.Width && a.Y < b.Y + b.Height;
		}
		
		/// <summary> Returns a power of two that is larger or equal to the given value.
		/// Power of two is a value of the form 2^n, where n is any integer >= 0.
		/// So, for example, for the value 30 this method would return 32.
		/// 
		/// </summary>
		/// <param name="size">the value.
		/// </param>
		/// <returns> the power of two.
		/// </returns>
		public static int toPowerOfTwo(int size)
		{
			int s = 1;
			
			if (size == 0)
				return 0;
			while (s < size)
				s *= 2;
			return s;
		}
		
		/// <summary> Returns a Rectangle that contains the area of the given Rectangle transformed
		/// by the given transform.
		/// 
		/// </summary>
		/// <param name="a">the transform.
		/// </param>
		/// <param name="b">the Rectangle.
		/// </param>
		/// <returns> the combination.
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public static System.Drawing.Rectangle transform(System.Drawing.Drawing2D.Matrix a, ref System.Drawing.Rectangle b)
		{
			System.Drawing.Rectangle r = new System.Drawing.Rectangle();
			double m00 = (float) a.Elements.GetValue(0);
			double m01 = (float) a.Elements.GetValue(2);
			double m10 = (float) a.Elements.GetValue(1);
			double m11 = (float) a.Elements.GetValue(3);
			double dx = (System.Single) a.OffsetX;
			double dy = (System.Single) a.OffsetY;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
			r.X = (int) System.Math.Floor(m00 * ((m00 > 0)?b.X:(b.X + b.Width)) + m01 * ((m01 > 0)?b.Y:(b.Y + b.Height)) + dx);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
			r.Y = (int) System.Math.Floor(m10 * ((m10 > 0)?b.X:(b.X + b.Width)) + m11 * ((m11 > 0)?b.Y:(b.Y + b.Height)) + dy);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
			r.Width = (int) System.Math.Ceiling(m00 * ((m00 < 0)?b.X:(b.X + b.Width)) + m01 * ((m01 < 0)?b.Y:(b.Y + b.Height)) + dx) - r.X;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
			r.Height = (int) System.Math.Ceiling(m10 * ((m10 < 0)?b.X:(b.X + b.Width)) + m11 * ((m11 < 0)?b.Y:(b.Y + b.Height)) + dy) - r.Y;
			
			return r;
		}
		
		public static System.Drawing.Drawing2D.Matrix multiply(System.Drawing.Drawing2D.Matrix a, System.Drawing.Drawing2D.Matrix b)
		{
			//UPGRADE_TODO: Method 'java.awt.geom.AffineTransform.clone' was converted to 'System.Drawing.Drawing2D.Matrix.Clone' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtgeomAffineTransformclone_3"'
			System.Drawing.Drawing2D.Matrix tm = (System.Drawing.Drawing2D.Matrix) a.Clone();
			
			//UPGRADE_TODO: Method 'java.awt.geom.AffineTransform.concatenate' was converted to 'System.Drawing.Drawing2D.Matrix.Multiply' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtgeomAffineTransformconcatenate_javaawtgeomAffineTransform_3"'
			tm.Multiply(b, System.Drawing.Drawing2D.MatrixOrder.Append);
			
			return tm;
		}
		
		public static System.Drawing.Drawing2D.Matrix linearTransform(System.Drawing.Drawing2D.Matrix a)
		{
			return new System.Drawing.Drawing2D.Matrix((System.Single) a.Elements.GetValue(0), (System.Single) a.Elements.GetValue(1), (System.Single) a.Elements.GetValue(2), (System.Single) a.Elements.GetValue(3), (System.Single) 0.0, (System.Single) 0.0);
		}
	}
}
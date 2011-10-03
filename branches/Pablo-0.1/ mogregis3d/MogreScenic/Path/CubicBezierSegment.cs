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
using Util = Scenic.Util;
namespace Scenic.path
{
	
	/// <summary> This class defines a cubic Bezier curve. This class does not 
	/// define the start point of the curve. The start point is the 
	/// end point of the previous segment.
	/// </summary>
	public class CubicBezierSegment:CurvedSegment
	{
		/// <summary> Gets the x coordinate of second control point.</summary>
		virtual public double X1
		{
			get
			{
				return x1;
			}
			
		}
		/// <summary> Gets the y coordinate of second control point.</summary>
		virtual public double Y1
		{
			get
			{
				return y1;
			}
			
		}
		/// <summary> Gets the x coordinate of third control point.</summary>
		virtual public double X2
		{
			get
			{
				return x2;
			}
			
		}
		/// <summary> Gets the y coordinate of third control point.</summary>
		virtual public double Y2
		{
			get
			{
				return y2;
			}
			
		}
		/// <summary> Gets the x coordinate of fourth control point.</summary>
		virtual public double X3
		{
			get
			{
				return x3;
			}
			
		}
		/// <summary> Gets the y coordinate of fourth control point.</summary>
		virtual public double Y3
		{
			get
			{
				return y3;
			}
			
		}
		private double x1, y1;
		private double x2, y2;
		private double x3, y3;
		
		/// <summary> Creates a cubic bezier segment with the given parameters. The
		/// first control point is not defined here, it is defined by the end
		/// point of the previous segment.
		/// 
		/// </summary>
		/// <param name="x1">x coordinate of second control point.
		/// </param>
		/// <param name="y1">y coordinate of second control point.
		/// </param>
		/// <param name="x2">x coordinate of third control point.
		/// </param>
		/// <param name="y2">y coordinate of third control point.
		/// </param>
		/// <param name="x3">x coordinate of fourth control point.
		/// </param>
		/// <param name="y3">y coordinate of fourth control point.
		/// </param>
		public CubicBezierSegment(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
			this.x3 = x3;
			this.y3 = y3;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public override System.Drawing.PointF calcPoint(double t, ref System.Drawing.PointF position)
		{
			double it = 1.0 - t;
			double a1 = it * it * it;
			double a2 = 3.0 * t * it * it;
			double a3 = 3.0 * t * t * it;
			double a4 = t * t * t;
			System.Drawing.PointF point = new System.Drawing.PointF();
			
			point.X = (float) (a1 * (double) position.X + a2 * x1 + a3 * x2 + a4 * x3);
			point.Y = (float) (a1 * (double) position.Y + a2 * y1 + a3 * y2 + a4 * y3);
			
			return point;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public override void  getBounds(System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle r, ref System.Drawing.PointF position)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			Util.addPointToBounds(transform, ref r, x1, y1);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			Util.addPointToBounds(transform, ref r, x2, y2);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			Util.addPointToBounds(transform, ref r, x3, y3);
			position.X = (float) x3;
			position.Y = (float) y3;
		}
	}
}
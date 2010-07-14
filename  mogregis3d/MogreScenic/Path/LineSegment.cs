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
using Util = scenic.Util;
namespace scenic.path
{
	
	/// <summary> This class defines a straight line segment. This class does not 
	/// define the start point of the line. The start point is the 
	/// end point of the previous segment.
	/// </summary>
	public class LineSegment : Segment
	{
		/// <summary> Gets the x coordinate of the end point.</summary>
		virtual public double X
		{
			get
			{
				return x;
			}
			
		}
		/// <summary> Gets the y coordinate of the end point.</summary>
		virtual public double Y
		{
			get
			{
				return y;
			}
			
		}
		private float x;
        private float y;
		
		/// <summary> Creates a straight line segment with the given parameters.
		/// 
		/// </summary>
		/// <param name="x">the x coordinate of the end point.
		/// </param>
		/// <param name="y">the y coordinate of the end point.
		/// </param>
        public LineSegment(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public virtual void  walk(PathWalker walker, ref System.Drawing.PointF position, System.Drawing.Drawing2D.Matrix errorMatrix, double error)
		{
			walker.lineTo(x, y);
			position.X = (float) x;
			position.Y = (float) y;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public virtual void  getBounds(System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle r, ref System.Drawing.PointF position)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			Util.addPointToBounds(transform, ref r, x, y);
			
			position.X = (float) x;
			position.Y = (float) y;
		}
	}
}
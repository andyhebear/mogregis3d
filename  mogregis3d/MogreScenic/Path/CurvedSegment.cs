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
namespace Scenic.path
{
	
	/// <summary> This is the base class for all curved segments.</summary>
	public abstract class CurvedSegment : Segment
	{
		
		protected internal CurvedSegment()
		{
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public virtual void  walk(PathWalker walker, ref System.Drawing.PointF position, System.Drawing.Drawing2D.Matrix errorMatrix, double error)
		{
			System.Drawing.PointF p1 = position;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			System.Drawing.PointF p2 = calcPoint(1.0, ref position);
			
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			iterate(walker, ref position, errorMatrix, error, 0.0, ref p1, 1.0, ref p2);
			position.X = (float) p2.X;
			position.Y = (float) p2.Y;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		private void  iterate(PathWalker walker, ref System.Drawing.PointF position, System.Drawing.Drawing2D.Matrix errorMatrix, double error, double t1, ref System.Drawing.PointF p1, double t2, ref System.Drawing.PointF p2)
		{
			double tc = (t1 + t2) / 2.0;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			System.Drawing.PointF pc = calcPoint(tc, ref position);
			double ex, ey, tex, tey;
			
			ex = (double) pc.X - ((double) p1.X + (double) p2.X) / 2.0;
			ey = (double) pc.Y - ((double) p1.Y + (double) p2.Y) / 2.0;
			tex = (float) errorMatrix.Elements.GetValue(0) * ex + (float) errorMatrix.Elements.GetValue(2) * ey;
			tey = (float) errorMatrix.Elements.GetValue(1) * ex + (float) errorMatrix.Elements.GetValue(3) * ey;
			if (tex * tex + tey * tey >= error * error)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
				iterate(walker, ref position, errorMatrix, error, t1, ref p1, tc, ref pc);
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
				iterate(walker, ref position, errorMatrix, error, tc, ref pc, t2, ref p2);
			}
			else
			{
				walker.lineTo(p2.X, p2.Y);
			}
		}
		
		/// <summary> Calculates the position of the segment at the given t value. 
		/// 
		/// </summary>
		/// <param name="t">the t value (must be in the range [0, 1]).
		/// </param>
		/// <param name="position">the previous position.
		/// </param>
		/// <returns> the position.
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public abstract System.Drawing.PointF calcPoint(double t, ref System.Drawing.PointF position);
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		public abstract void  getBounds(System.Drawing.Drawing2D.Matrix param1, ref System.Drawing.Rectangle param2, ref System.Drawing.PointF param3);
	}
}
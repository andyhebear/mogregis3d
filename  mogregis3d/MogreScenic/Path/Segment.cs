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
namespace scenic.path
{
	
	/// <summary> This is the base interface for different kinds of segments.</summary>
	public interface Segment
	{
		/// <summary> Walks the given walker through this segment.
		/// 
		/// </summary>
		/// <param name="walker">the walker.
		/// </param>
		/// <param name="position">the start position.
		/// </param>
		/// <param name="error">the error matrix.
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		void  walk(PathWalker walker, ref System.Drawing.PointF position, System.Drawing.Drawing2D.Matrix errorMatrix, double error);
		/// <summary> Gets the bounds of this segment using the given transform.
		/// 
		/// </summary>
		/// <param name="transform">the transform
		/// </param>
		/// <param name="r">the bounds.
		/// </param>
		/// <param name="position">the start position.
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		void  getBounds(System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle r, ref System.Drawing.PointF position);
	}
}
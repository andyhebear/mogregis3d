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
	
	/// <summary> This interface is used when walking a path. Each subpath
	/// begins with a call to beginSubPath-method, which is followed with
	/// calls to LineTo-method. The endSubPath-method
	/// is called at the end of each subpath.
	/// </summary>
	public interface PathWalker
	{
		/// <summary> Begins a new subpath.
		/// 
		/// </summary>
		/// <param name="isClosed">defines is the subpath closed.
		/// </param>
		void  beginSubPath(bool isClosed);
		/// <summary> Ends the subpath.</summary>
		void  endSubPath();
		/// <summary> Adds a new point to the subpath.
		/// 
		/// </summary>
		/// <param name="x">x coordinate of the point.
		/// </param>
		/// <param name="y">y coordinate of the point.
		/// </param>
		void  lineTo(float x, float y);
	}
}
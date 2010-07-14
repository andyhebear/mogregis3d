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
	
	/// <summary> This scene node transforms its children using a 2-dimensional 
	/// affine transformation.
	/// </summary>
	public class SceneTransform:SceneContainer
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1199_3"'
		/// <summary> Gets the transformation.</summary>
		/// <summary> Sets the transformation.</summary>
		virtual public System.Drawing.Drawing2D.Matrix Transform
		{
			get
			{
				return transform;
			}
			
			set
			{
				this.transform = value;
#if PENDING
				changed();
#endif
			}
			
		}
		private System.Drawing.Drawing2D.Matrix transform;
		
		/// <summary> Creates a SceneTransform object with the given transform.
		/// 
		/// </summary>
		/// <param name="transform">the transform.
		/// </param>
		public SceneTransform(System.Drawing.Drawing2D.Matrix transform)
		{
			this.transform = transform;
		}
		
		/// <summary> Creates a SceneTransform object with the given transform and 
		/// scene node.
		/// 
		/// </summary>
		/// <param name="transform">the transform.
		/// </param>
		/// <param name="node">the scene node
		/// </param>
		public SceneTransform(System.Drawing.Drawing2D.Matrix transform, SceneNode node)
		{
			this.transform = transform;
			add(node);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		internal override void  draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle visibleArea)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			base.draw(context, Util.multiply(transform, this.transform), ref visibleArea);
		}
		
		internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
		{
			return base.getBounds(context, Util.multiply(transform, this.transform));
		}
		
		internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
		{
			return base.getDrawType(context, Util.multiply(transform, this.transform));
		}
	}
}
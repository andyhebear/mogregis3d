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
using System.Collections.Generic;
namespace scenic
{

    /// <summary> This class is a base class for scene nodes that contain other
    /// scene nodes. The child nodes are painted starting from the first
    /// child.
    /// </summary>
    public class SceneContainer : SceneNode, SceneParent
    {
        private List<SceneNode> children = new List<SceneNode>();

        /// <summary> Constructs an empty SceneContainer.</summary>
        public SceneContainer()
        {
        }

        /// <summary> Adds a child node to the given index in the child list.
        /// 
        /// </summary>
        /// <param name="index">the index in the child list.
        /// </param>
        /// <param name="node">the child node.
        /// </param>
        public virtual void add(int index, SceneNode node)
        {
			if (node == null)
				throw new System.ArgumentException();
			children.Add( node);
        }

        /// <summary> Adds a child node at the end of the child list.
        /// 
        /// </summary>
        /// <param name="node">the child node.
        /// </param>
        public virtual void add(SceneNode node)
        {
			if (node == null)
				throw new System.ArgumentException();
			children.Add(node);
        }

        /// <summary> Removes all child nodes from this container.</summary>
        public virtual void clear()
        {
#if PENDING
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
			Iterator < SceneNode > itr = children.iterator();
			
			if (isVisible())
			{
				while (itr.hasNext())
				{
					itr.next().removeVisibleParent(this);
				}
			}
			children.Clear();
			System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Empty;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			changed(ref tempAux);
#endif
        }

        public virtual void remove(SceneNode node)
        {
#if PENDING
			children.remove(node);
			if (isVisible())
				node.removeVisibleParent(this);
			System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Empty;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			changed(ref tempAux);
#endif
        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        internal override void draw(DrawContext context, System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle visibleArea)
        {
			foreach (SceneNode node in children)
                node.draw(context, transform, ref visibleArea);
        }

        internal override void prepareDraw(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
            foreach (SceneNode node in children)
                node.prepareDraw(context, transform);
        }

        internal override System.Drawing.Rectangle getBounds(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
#if PENDING
			System.Drawing.Rectangle result = new System.Drawing.Rectangle();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
			Iterator < SceneNode > itr = children.iterator();
			
			while (itr.hasNext())
			{
				SceneNode node = itr.next();
				System.Drawing.Rectangle nodeRect = node.getBounds(context, transform);
				
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
				Util.combine(ref result, ref nodeRect);
			}
			
			return result;
#endif
            return new System.Drawing.Rectangle();
        }

        internal override int getDrawType(DrawContext context, System.Drawing.Drawing2D.Matrix transform)
        {
#if PENDING
			if (children.Count == 1)
				return ((SceneNode) children.get_Renamed(0)).getDrawType(context, transform);
			return SceneNode.DRAW_SIMPLE;
#endif
            return 0;
        }

        protected internal override void show()
        {
#if PENDING
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
			Iterator < SceneNode > itr = children.iterator();
			
			while (itr.hasNext())
				itr.next().addVisibleParent(this);
#endif
        }

        protected internal override void hide()
        {
#if PENDING
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
            Iterator<SceneNode> itr = children.iterator();

            while (itr.hasNext())
                itr.next().removeVisibleParent(this);
#endif

        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public virtual void childChanged(ref System.Drawing.Rectangle area)
        {
#if PENDING
            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            changed(ref area);
#endif

        }
    }
}
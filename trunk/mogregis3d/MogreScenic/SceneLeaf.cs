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
using Rectangle = System.Drawing.Rectangle;
namespace Scenic
{

    /// <summary> 
    /// This is the base class for different leaf nodes. Leaf nodes are
    /// nodes that do not contain other nodes.
    /// </summary>
    abstract public class SceneLeaf : SceneNode
    {

        internal virtual void changed()
        {
        }

        static public Rectangle addSafetyMargin(Rectangle bounds)
        {
            return new Rectangle(bounds.X - safetyMargin, bounds.Y - safetyMargin,
                    bounds.Width + safetyMargin * 2, bounds.Height + safetyMargin * 2);
        }

        static public Rectangle addMargin(Rectangle bounds, int hmargin, int vmargin)
        {
            return new Rectangle(bounds.X - hmargin, bounds.Y - vmargin,
                    bounds.Width + hmargin * 2, bounds.Height + vmargin * 2);
        }
    }
}
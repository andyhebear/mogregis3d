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

    /// <summary> 
    /// This class defines a path that can be used for stroking and filling.
    /// A path consists of one or more subpaths. Each subpath consists
    /// of one or more connected segments. There are different kinds 
    /// of segments: straight lines, Bezier curves and elliptical arcs.
    /// 
    /// Path, subpaths and segments are constant. Their values cannot
    /// be changed after they are created.
    /// </summary>
    public class GenericPath : Path
    {
        /// <summary> Gets the subpaths.</summary>
        virtual public SubPath[] SubPaths
        {
            get
            {
                return (SubPath[])subPaths.Clone();
            }
        }

        virtual public bool Convex
        {
            get
            {
                if (subPaths.Length == 1)
                    return subPaths[0].Convex;
                return false;
            }

        }

        private SubPath[] subPaths;

        /// <summary> 
        /// Creates a path that has the given subpaths.
        /// </summary>
        /// <param name="subPaths">the subpaths.
        /// </param>
        public GenericPath(SubPath[] subPaths)
        {
            this.subPaths = subPaths;
        }

        /// <summary> 
        /// Walks the given walker through the path.
        /// </summary>
        /// <param name="walker">the walker.
        /// </param>
        /// <param name="error">the error matrix.
        /// </param>
        public virtual void walk(PathWalker walker, System.Drawing.Drawing2D.Matrix errorMatrix, double error)
        {
            for (int i = 0; i < subPaths.Length; i++)
                subPaths[i].walk(walker, errorMatrix, error);
        }

        /// <summary> 
        /// Calculates the bounds of this path using the given transform.
        /// </summary>
        /// <param name="transform">the transform.
        /// </param>
        /// <returns> the bounds.
        /// </returns>
        public virtual System.Drawing.Rectangle getBounds(System.Drawing.Drawing2D.Matrix transform)
        {
            System.Drawing.Rectangle r = new System.Drawing.Rectangle();

            for (int i = 0; i < subPaths.Length; i++)
            {
                //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
                subPaths[i].getBounds(transform, ref r);
            }

            return r;
        }
    }
}
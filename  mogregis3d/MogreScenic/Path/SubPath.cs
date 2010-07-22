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

    /// <summary> A subpath defines a continuous path which consists of
    /// different kinds of segments. If a subpath is closed its
    /// end point is connected to the start point with a straight
    /// line.
    /// </summary>
    public class SubPath
    {
        /// <summary> Gets the x coordinate of the start point.</summary>
        virtual public double StartX
        {
            get
            {
                return startX;
            }

        }
        /// <summary> Gets the y coordinate of the start point.</summary>
        virtual public double StartY
        {
            get
            {
                return startY;
            }

        }
        /// <summary> Gets is the subpath closed.</summary>
        virtual public bool Closed
        {
            get
            {
                return closed;
            }

        }
        /// <summary> Gets the segments.</summary>
        virtual public Segment[] Segments
        {
            get
            {
                return (Segment[])segments.Clone();
            }

        }
        virtual public bool Convex
        {
            get
            {
                return false;
            }

        }
        private float startX;
        private float startY;
        private Segment[] segments;
        private bool closed;

        /// <summary> Creates a new subpath with the given start point and segments.
        /// 
        /// </summary>
        /// <param name="startX">x coordinate of the start point.
        /// </param>
        /// <param name="startY">y coordinate of the start point.
        /// </param>
        /// <param name="segments">the segments.
        /// </param>
        /// <param name="closed">defines is the subpath closed.
        /// </param>
        public SubPath(float startX, float startY, Segment[] segments, bool closed)
        {
            this.startX = startX;
            this.startY = startY;
            this.segments = segments;
            this.closed = closed;
        }

        /// <summary> Walks the given walker through this subpath.
        /// 
        /// </summary>
        /// <param name="walker">the walker.
        /// </param>
        /// <param name="error">the error matrix.
        /// </param>
        public virtual void walk(PathWalker walker, System.Drawing.Drawing2D.Matrix errorMatrix, double error)
        {
            System.Drawing.PointF position = new System.Drawing.PointF((float)startX, (float)startY);

            walker.beginSubPath(closed);
            walker.lineTo(startX, startY);
            for (int i = 0; i < segments.Length; i++)
            {
                //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
                segments[i].walk(walker, ref position, errorMatrix, error);
            }
            walker.endSubPath();
        }

        /// <summary> Gets the bounds of this subpath using the given transform.
        /// 
        /// </summary>
        /// <param name="transform">the transform
        /// </param>
        /// <param name="r">the bounds.
        /// </param>
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public virtual void getBounds(System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle r)
        {
            System.Drawing.PointF position = new System.Drawing.PointF((float)startX, (float)startY);

            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            Util.addPointToBounds(transform, ref r, startX, startY);
            for (int i = 0; i < segments.Length; i++)
            {
                //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
                segments[i].getBounds(transform, ref r, ref position);
            }
        }
    }
}
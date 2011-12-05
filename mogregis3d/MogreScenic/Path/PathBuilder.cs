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
namespace Scenic.path
{

    /// <summary> This class offers a convenient interface for creating paths.</summary>
    public class PathBuilder
    {
        /// <summary> Get the subpaths as an array.
        /// 
        /// </summary>
        /// <returns> the subpaths
        /// </returns>
        virtual public SubPath[] SubPaths
        {
            get
            {
                endSubPath(false);
                SubPath[] result = new SubPath[subPaths.Count];
                subPaths.CopyTo(result);
                return result;
            }

        }
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
        private List<SubPath> subPaths;
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
        private List<Segment> segments;
        private float startX;
        private float startY;
        internal bool hasStartPoint = false;

        /// <summary> Creates a new PathBuilder object.</summary>
        public PathBuilder()
        {
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
            subPaths = new List<SubPath>();
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
            segments = new List<Segment>();
        }

        /// <summary> Begins a new subpath from the given position.</summary>
        public virtual void moveTo(float x, float y)
        {
            if (hasStartPoint)
                endSubPath(false);
            startX = x;
            startY = y;
            hasStartPoint = true;
        }

        /// <summary> Draws a straight line to the given position. If not start point
        /// has been set, the function sets the start point.
        /// </summary>
        public virtual void lineTo(float x, float y)
        {
            if (hasStartPoint)
                segments.Add(new LineSegment(x, y));
            else
                moveTo(x, y);
        }

        /// <summary> Draws a quadratic Bezier-curve using the given
        /// control points. The first control point is the current
        /// position.
        /// 
        /// </summary>
        /// <param name="p1">the second control point.
        /// </param>
        /// <param name="p2">the third control point.
        /// </param>
        public virtual void curveTo(System.Drawing.PointF p1, System.Drawing.PointF p2)
        {
            addSegment(new QuadraticBezierSegment((double)p1.X, (double)p1.Y, (double)p2.X, (double)p2.Y));
        }

        /// <summary> Draws a cubic Bezier-curve using the given
        /// control points. The first control point is the current
        /// position.
        /// 
        /// </summary>
        /// <param name="p1">the second control point.
        /// </param>
        /// <param name="p2">the third control point.
        /// </param>
        /// <param name="p3">the fourth control point.
        /// </param>
        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public virtual void curveTo(ref System.Drawing.PointF p1, ref System.Drawing.PointF p2, ref System.Drawing.PointF p3)
        {
            addSegment(new CubicBezierSegment((double)p1.X, (double)p1.Y, (double)p2.X, (double)p2.Y, (double)p3.X, (double)p3.Y));
        }

        /// <summary> Draws an elliptic Arc starting from the current position.
        /// 
        /// </summary>
        /// <param name="xRadius">the x radius of the ellipse.
        /// </param>
        /// <param name="yRadius">the y radius of the ellipse.
        /// </param>
        /// <param name="rotation">the rotation of the ellipse in degrees.
        /// </param>
        /// <param name="startAngle">the start angle of the Arc in degrees.
        /// </param>
        /// <param name="stopAngle">the stop angle of the Arc in degrees.
        /// </param>
        public virtual void arcTo(double xRadius, double yRadius, double rotation, double startAngle, double stopAngle)
        {
            addSegment(new ArcSegment(xRadius, yRadius, rotation, startAngle, stopAngle));
        }

        /// <summary> Draws an elliptic Arc starting from the given position.
        /// 
        /// </summary>
        /// <param name="x">the x-coordinate of the starting point of the Arc.
        /// </param>
        /// <param name="y">the y-coordinate of the starting point of the Arc.
        /// </param>
        /// <param name="xRadius">the x radius of the ellipse.
        /// </param>
        /// <param name="yRadius">the y radius of the ellipse.
        /// </param>
        /// <param name="rotation">the rotation of the ellipse in degrees.
        /// </param>
        /// <param name="startAngle">the start angle of the Arc in degrees.
        /// </param>
        /// <param name="stopAngle">the stop angle of the Arc in degrees.
        /// </param>
        public virtual void arc(float x, float y, double xRadius, double yRadius, double rotation, double startAngle, double stopAngle)
        {
            ArcSegment s = new ArcSegment(xRadius, yRadius, rotation, startAngle, stopAngle);
            System.Drawing.PointF p = s.calcRelativePoint(0.0);
            lineTo(x + p.X, y + p.Y);
            addSegment(s);
        }

        /// <summary> Draws a Rectangle.
        /// 
        /// </summary>
        /// <param name="x">the left side of the Rectangle.
        /// </param>
        /// <param name="y">the top of the Rectangle.
        /// </param>
        /// <param name="width">the width of the Rectangle.
        /// </param>
        /// <param name="height">the height of the Rectangle.
        /// </param>
        public virtual void rectangle(float x, float y, float width, float height)
        {
            moveTo(x, y);
            lineTo(x + width, y);
            lineTo(x + width, y + height);
            lineTo(x, y + height);
            close();
        }

        /// <summary> Draws a Rectangle with rounded edges. 
        /// 
        /// </summary>
        /// <param name="x">the left side of the Rectangle.
        /// </param>
        /// <param name="y">the top of the Rectangle.
        /// </param>
        /// <param name="width">the width of the Rectangle.
        /// </param>
        /// <param name="height">the height of the Rectangle.
        /// </param>
        /// <param name="radius">the radius of the rounded edges.
        /// </param>
        public virtual void roundedRectangle(float x, float y, float width, float height, float radius)
        {
            if (radius * 2.0 > System.Math.Min(width, height))
                radius = (float)System.Math.Min(width, height) / 2.0f;

            moveTo(x + radius, y);
            lineTo(x + width - radius, y);
            arcTo(radius, radius, 0, -90, 0);
            lineTo(x + width, y + height - radius);
            arcTo(radius, radius, 0, 0, 90);
            lineTo(x + radius, y + height);
            arcTo(radius, radius, 0, 90, 180);
            lineTo(x, y + radius);
            arcTo(radius, radius, 0, 180, 270);
            close();
        }

        /// <summary> Adds a segment to the current subpath.
        /// 
        /// </summary>
        /// <param name="s">the segment to be added.
        /// </param>
        public virtual void addSegment(Segment s)
        {
            if (!hasStartPoint)
                throw new System.SystemException();
            segments.Add(s);
        }

        /// <summary> Closes the current subpath.</summary>
        public virtual void close()
        {
            endSubPath(true);
        }

        private void endSubPath(bool closed)
        {
            if (segments.Count > 0)
            {
                Segment[] tmp = new Segment[segments.Count];
                segments.CopyTo(tmp);
                SubPath subPath = new SubPath(startX, startY, tmp, closed);
                subPaths.Add(subPath);
                segments.Clear();
            }
            hasStartPoint = false;
        }

        /// <summary> Creates a Path object that contains the created path.
        /// 
        /// </summary>
        /// <returns> the Path object.
        /// </returns>
        public virtual Path createPath()
        {
            endSubPath(false);

            SubPath[] tmp = new SubPath[subPaths.Count];
            subPaths.CopyTo(tmp);
            return new GenericPath(tmp);
        }
    }
}
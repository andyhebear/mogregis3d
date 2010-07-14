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

    /// <summary> This class defines an elliptical Arc. This class does not 
    /// define the start point of the Arc. The start point is the 
    /// end point of the previous segment.
    /// </summary>
    public class ArcSegment : CurvedSegment
    {
        /// <summary> Gets the x radius of the ellipse.</summary>
        virtual public double XRadius
        {
            get
            {
                return xRadius;
            }

        }
        /// <summary> Gets the y radius of the ellipse.</summary>
        virtual public double YRadius
        {
            get
            {
                return yRadius;
            }

        }
        /// <summary> Gets the rotation of the ellipse in degrees.</summary>
        virtual public double Rotation
        {
            get
            {
                return rotation;
            }

        }
        /// <summary> Gets the start angle of the Arc in degrees.</summary>
        virtual public double StartAngle
        {
            get
            {
                return startAngle;
            }

        }
        /// <summary> Gets the stop angle of the Arc in degrees.</summary>
        virtual public double StopAngle
        {
            get
            {
                return stopAngle;
            }

        }
        private double xRadius;
        private double yRadius;
        private double rotation;
        private double startAngle;
        private double stopAngle;

        /// <summary> Creates an elliptic Arc segment with the given parameters.
        /// 
        /// </summary>
        /// <param name="xRadius">x radius of the ellipse.
        /// </param>
        /// <param name="yRadius">y radius of the ellipse.
        /// </param>
        /// <param name="rotation">rotation of the ellipse in degrees.
        /// </param>
        /// <param name="startAngle">start angle of the Arc in degrees.
        /// </param>
        /// <param name="stopAngle">stop angle of the Arc in degrees.
        /// </param>
        public ArcSegment(double xRadius, double yRadius, double rotation, double startAngle, double stopAngle)
        {
            this.xRadius = xRadius;
            this.yRadius = yRadius;
            this.rotation = rotation;
            this.startAngle = startAngle;
            this.stopAngle = stopAngle;
        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public override System.Drawing.PointF calcPoint(double t, ref System.Drawing.PointF position)
        {
            System.Drawing.PointF p = new System.Drawing.PointF();
            double angle = SupportClass.DegreesToRadians(startAngle + (stopAngle - startAngle) * t);
            double x, y;
            double sinr = System.Math.Sin(SupportClass.DegreesToRadians(rotation));
            double cosr = System.Math.Cos(SupportClass.DegreesToRadians(rotation));

            x = (System.Math.Cos(angle) - System.Math.Cos(SupportClass.DegreesToRadians(startAngle))) * xRadius;
            y = (System.Math.Sin(angle) - System.Math.Sin(SupportClass.DegreesToRadians(startAngle))) * yRadius;

            p.X = (float)((double)position.X + cosr * x - sinr * y);
            p.Y = (float)((double)position.Y + sinr * x + cosr * y);

            return p;
        }

        /// <summary> Calculates the point along the Arc, when the center of the ellipse
        /// is thought to be at (0, 0). 
        /// 
        /// </summary>
        /// <param name="t">the place along the Arc (must be inside the rangle [0, 1]).
        /// </param>
        /// <returns> the point.
        /// </returns>
        public virtual System.Drawing.PointF calcRelativePoint(double t)
        {
            System.Drawing.PointF p = new System.Drawing.PointF();
            double angle = SupportClass.DegreesToRadians(startAngle + (stopAngle - startAngle) * t);
            double x, y;
            double sinr = System.Math.Sin(SupportClass.DegreesToRadians(rotation));
            double cosr = System.Math.Cos(SupportClass.DegreesToRadians(rotation));

            x = System.Math.Cos(angle) * xRadius;
            y = System.Math.Sin(angle) * yRadius;

            p.X = (float)(cosr * x - sinr * y);
            p.Y = (float)(sinr * x + cosr * y);

            return p;
        }

        //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
        public override void getBounds(System.Drawing.Drawing2D.Matrix transform, ref System.Drawing.Rectangle r, ref System.Drawing.PointF position)
        {
            System.Drawing.Rectangle a = new System.Drawing.Rectangle();
            double x, y;

            x = (double)position.X - System.Math.Cos(SupportClass.DegreesToRadians(startAngle)) * xRadius;
            y = (double)position.Y - System.Math.Sin(SupportClass.DegreesToRadians(startAngle)) * yRadius;

            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
            a.X = (int)(x - xRadius);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
            a.Y = (int)(y - yRadius);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
            a.Width = (int)System.Math.Ceiling(x + xRadius) - a.X;
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042_3"'
            a.Height = (int)System.Math.Ceiling(y + yRadius) - a.Y;

            System.Drawing.Drawing2D.Matrix temp_Matrix;
            temp_Matrix = new System.Drawing.Drawing2D.Matrix();
            temp_Matrix.Rotate((float)(rotation * (180 / System.Math.PI)));
            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            a = Util.transform(Util.multiply(transform, temp_Matrix), ref a);

            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            Util.combine(ref r, ref a);

            //UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
            System.Drawing.PointF endPoint = calcPoint(1.0, ref position);

            position.X = (float)endPoint.X;
            position.Y = (float)endPoint.Y;
        }
    }
}
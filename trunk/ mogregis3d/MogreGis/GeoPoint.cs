using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sharp3D.Math.Core;

namespace MogreGis
{

    /**
     * A 2D or 3D georeferenced point in space.
     */
    public class GeoPoint : SharpMap.Geometries.Point
    {
        public GeoPoint() : base() { }
        public GeoPoint(double[] point) : base(point) { }
        public GeoPoint(double x, double y) : base(x, y) { }

#if TODO

        /**
         * Constructs a new invalid point.
         */
        public GeoPoint() { }

        /**
         * Copy constructor.
         */
        public GeoPoint(GeoPoint to_copy)
        {
            dim = to_copy.dim;
            spatial_ref = to_copy.spatial_ref;
            point = to_copy.point;
        }


        /**
         * Creates a new 2D georeferenced point.
         * @param input
         *      2D point data
         * @param srs
         *      Spatial reference system.
         */
        public GeoPoint(Vector2D input, SpatialReference srs)
        {
            point.X = input.X;
            point.Y = input.Y;
            spatial_ref = srs;
            dim = 2;
        }


        /**
         * Create a new 3D georeferenced point.
         * @param input
         *      3D point data
         * @param srs
         *      Spatial reference system.
         */
        public GeoPoint(Vector3D input, SpatialReference srs)
        {
            point = input;
            spatial_ref = srs;
            dim = 3;
        }


        /**
         * Creates a new 2D georeferenced point.
         * @param x, y
         *      2D point data
         * @param srs
         *      Spatial reference system.
         */
        public GeoPoint(double x, double y, SpatialReference srs)
        {
            point.X = x;
            point.Y = y;
            spatial_ref = srs;
            dim = 2;
        }


        /**
         * Create a new 3D georeferenced point.
         * @param x, y, z
         *      3D point data
         * @param srs
         *      Spatial reference system.
         */
        public GeoPoint(double x, double y, double z, SpatialReference srs)
        {
            point.X = x;
            point.Y = y;
            point.Z = z;
            spatial_ref = srs;
            dim = 2;
        }

        public double X
        {
            get { return point.X; }
        }

        public double Y
        {
            get { return point.Y; }
        }

        public double Z
        {
            get { return point.Z; }
        }

        /**
                 * Returns true if the point contains valid data.
                 */
        public bool isValid()
        {
            return dim > 0 && spatial_ref != null;
        }


        /**
         * Returns the dimensionality of the point (2 or 3)
         */
        public uint getDim()
        {
            return dim;
        }


        /**
         * Sets the dimensionality of the point (2 or 3)
         */
        public void setDim(uint _dim)
        {
            dim = _dim > 0 && _dim <= 3 ? _dim : dim;
        }


        /**
         * Returns the spatial reference system relative to which the point
         * data is expressed.
         */
        public SpatialReference getSRS()
        {
            return spatial_ref;
        }


        /**
         * Gets a copy of the point that is transformed out of its SRS'
         * reference frame, if applicable.
         */
        public GeoPoint getAbsolute()
        {
            return new GeoPoint(
                 this.point * getSRS().getInverseReferenceFrame(),
                getSRS().cloneWithNewReferenceFrame(Matrix3D.Identity));
        }

        /**
         * Gets a readable representation of the point
         */
        public override string ToString()
        {
            string str = "INVALID";
            if (isValid() && getDim() == 2)
                str = point.X + ", " + point.Y;
            else if (isValid() && getDim() == 3)
                str = point.X + ", " + point.Y + ", " + point.Z;

            return str;
        }

        /**
         * Returns true if this point is mathematically equivalent to another.
         */
        public static bool operator ==(GeoPoint lhs, GeoPoint rhs)
        {
            return
                lhs.isValid() && rhs.isValid() &&
                SpatialReference.equivalent(lhs.getSRS(), rhs.getSRS()) &&
                lhs.getDim() == rhs.getDim() &&
                lhs.point.X == rhs.point.X &&
                (lhs.getDim() < 2 || lhs.point.Y == rhs.point.Y) &&
                (lhs.getDim() < 3 || lhs.point.Z == rhs.point.Z);
        }
        public static bool operator !=(GeoPoint lhs, GeoPoint rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is GeoPoint))
                return false;
            return this == obj;
        }

        public override int GetHashCode()
        {
            return point.GetHashCode();
        }

        /**
         * Creates an invalid point.
         */
        public static GeoPoint invalid()
        {
            return new GeoPoint();
        }




        private uint dim = 0;
      
        private Vector3D point = new Vector3D();
#endif
        private SpatialReference spatial_ref = null;
        internal void setSpatialReference(SpatialReference sr)
        {
            spatial_ref = sr;
        }

    }

    public class GeoPointList : List<GeoPoint>
    {

        public GeoPointList() { }
        public GeoPointList(int cap) : base(cap) { }

        public bool intersects(GeoExtent ex)
        {
#if TODO
            if (ex.isInfinite())
                return true;

            //TODO: srs transform

            if (ex.isPoint()) // do a point-in-polygon test
            {
                GeoPoint point = ex.getSouthwest();
                bool result = false;
                GeoPointList polygon = this;
                for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
                {
                    if ((((polygon[i].Y <= point.Y) && (point.Y < polygon[j].Y)) ||
                         ((polygon[j].Y <= point.Y) && (point.Y < polygon[i].Y))) &&
                        (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                    {
                        result = !result;
                    }
                }
                return result;
            }
            else // check for all points within extent -- not actually correct //TODO
            {
                GeoExtent e = new GeoExtent();
                foreach (GeoPoint i in this)
                {
                    e.expandToInclude(i);
                }
                return e.intersects(ex);
            }
#endif
            throw new NotImplementedException();
        }

        /**
         * Returns true is the point list is "closed", i.e. the last point
         * is equal to the first (in 2D)
         */
        public bool isClosed()
        {
            return this.Count >= 2 && this.First() == this.Last();
        }
    }

    public class GeoPartVisitor
    {
        public GeoPartVisitor() { }
        public virtual bool visitPart(GeoPointList part) { return true; }
    }

    //TODO Esto no se podría cambiar por un Action
    public class GeoPointVisitor
    {
        public GeoPointVisitor() { }
        public virtual GeoPoint visitPoint(GeoPoint point) { return null; }
    }
}

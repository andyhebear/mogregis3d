using System;
using System.Collections.Generic;
using System.Linq;

using Sharp3D.Math.Core;

namespace MogreGis
{
    #region GEOPOINT
    /**
     * A 2D or 3D georeferenced point in space.
     */
    public class GeoPoint : SharpMap.Geometries.Point3D
    {
        #region CONSTRUCTORES
        /**
         * Constructs a new invalid point.
         */
        public GeoPoint()
        {
            SpatialReference = null;
            dim = 0;
        }

        /**
         * Copyc constructor
         */
        [System.Obsolete]
        public GeoPoint(GeoPoint p)
        {
            throw new NotImplementedException();    
        }

        //public static explicit operator GeoPoint(SharpMap.Geometries.Point p)
        //{
        //    if (p is GeoPoint)
        //        return (GeoPoint)p;
        //    else if (p is SharpMap.Geometries.Point3D)
        //        return new GeoPoint(p.X, p.Y, (p as SharpMap.Geometries.Point3D).Z, (SpatialReference)p.SpatialReference);
        //    return new GeoPoint(p.X, p.Y, (SpatialReference)p.SpatialReference);
        //}

        /**
         * Creates a new 2D georeferenced point.
         * @param input
         *      2D point data
         * @param srs
         *      Spatial reference system.
         */
        public GeoPoint(Vector2D input, SpatialReference srs)
            : base(input.X, input.Y, 0)
        {
            this.SpatialReference = srs;
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
            : base(input.X, input.Y, input.Z)
        {
            this.SpatialReference = srs;
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
            : base(x, y, 0)
        {
            this.SpatialReference = srs;
            dim = 2;
        }

        /**
         * Create  a new 3D georeferenced point.
         * @param x, y, z
         *      3D point data
         * @param srs
         *      Spatial reference system.
         */
        public GeoPoint(double x, double y, double z, SpatialReference srs)
            : base(x, y, z)
        {
            this.SpatialReference = srs;
            dim = 3;
        }
        #endregion

        #region METODOS
        /**
         * Returns true if the point contains valid data.
         */
        public bool isValid()
        {
            return dim > 0 && this.SpatialReference != null;
        }

        /**
         * Returns the dimensionality of the point (2 or 3).
         */
        public int getDim()
        {
            return dim;
        }

        /**
         * Sets the dimesionality of the point (2 or 3).
         */
        public void setDim(int value)
        {
            dim = value > 0 && dim <= 3 ? value : dim;
        }

        /**
         * Returns the spatial reference system relative to which the point
         * data is expressed.
         */
        public SpatialReference getSRS()
        {
            return (SpatialReference)this.SpatialReference;
        }

        /**
         * Gets a copy of the point that is transformed out of its SRS
         * reference frame, if applicable.
         */
        public GeoPoint getAbsolute()
        {
            throw new NotImplementedException();
            //return new GeoPoint ((this) * getSRS().getInverseReferenceFrame(),getSRS().cloneWithNewReferenceFrame(Mogre.Matrix4.IDENTITY));
        }

        /**
         * Gets a readable representation of the point
         */
        public override string ToString()
        {
            string str;
            if (isValid() && getDim() == 2)
            {
                str = X + "," + Y;
            }
            else if (isValid() && getDim() == 3)
            {
                str = X + "," + Y + "," + Z;
            }
            else
                str = "INVALID";
            return str;
        }

        /**
         * Returns true if this point is mathematically equivalent to another.
         */
        public static bool operator ==(GeoPoint rhs, GeoPoint rhs2)
        {
            return rhs2.isValid() && rhs.isValid() &&
                rhs2.getSRS().Equals(rhs.getSRS()) && rhs2.getDim() == rhs.getDim() &&
                rhs2.X == rhs.X &&
                (rhs2.getDim() < 2 || rhs2.Y == rhs.Y) && (rhs2.getDim() < 3 || rhs2.Z == rhs.Z);
        }

        /**
         * Returns true if this point is mathematically diferent to another.
         */
        public static bool operator !=(GeoPoint rhs, GeoPoint rhs2)
        {
            return !(rhs == rhs2);
        }

        /**
         * Creates an invalid point.
         */
        public static GeoPoint invalid()
        {
            return new GeoPoint();
        }


        public void setSpatialReference(SpatialReference sr)
        {
            this.SpatialReference = sr;
        }

        public void set(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        #endregion



        #region ATRIBUTOS
        private int dim;
        //private SpatialReference spatialReference;
        #endregion
    }
    #endregion

    #region GEOPOINTLIST
    public class GeoPointList : List<GeoPoint>
    {
        #region CONSTRUCTORES
        public GeoPointList() { }
        public GeoPointList(int cap) : base(cap) { }
        #endregion

        #region METODOS
        public bool intersects(GeoExtent ex)
        {
            if (ex.isInfinite())
            {
                return true;
            }
            if (ex.isPoint())
            {
                GeoPoint point = ex.sw;
                bool result = false;
                GeoPointList polygon = this;
                for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
                {
                    if ((((polygon[i].Y <= point.Y) && (point.Y < polygon[j].Y)) ||
                        ((polygon[j].Y <= point.Y) && (point.Y < polygon[i].Y))) &&
                        (point.X < (polygon[j].X - polygon[i].X) *
                        (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                    {
                        result = !result;
                    }
                }
                return result;
            }
            else //check for all points within extent -- not actually correct //TODO
            {
                GeoExtent e;
                e = new GeoExtent(this[0], this[0]);
                foreach (GeoPoint geoPoint in this)
                {

                    if (geoPoint == this[0])
                    {
                        //NOP
                    }
                    else
                    {
                        e.expandToInclude(geoPoint);
                    }
                }
                return e.intersects(ex);
            }
        }

        /**
         * Returns true is the point list is "closed", i.e. the last point
         * is equal to the first (in 2D).
         */
        public bool isClosed()
        {
            return Count >= 2 && this[0].Equals(this[Count - 1]);
            //return Count >= 2 && front() == back();
        }
        #endregion
    }
    #endregion

    #region GEOPARTVISITOR
    public class GeoPartVisitor
    {
        public GeoPartVisitor() { }
        public bool visitPart(GeoPointList part) { return true; }
    }
    #endregion

    #region GEOPOINTVISITOR
    public class GeoPointVisitor
    {
        public GeoPointVisitor() { }
        public bool visitPoint(GeoPoint point) { return true; }
    }
    #endregion
}
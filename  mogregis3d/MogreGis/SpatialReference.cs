using System;
using System.Collections.Generic;


using Sharp3D.Math.Core;
using ProjNet.CoordinateSystems;
//using SharpMap.CoordinateSystems;

namespace MogreGis
{
    /**
     * Mathematical description that associates a set of 2D or 3D coordinates with a
     * real-world location on the earth (or other planet). A.K.A "SRS".
     *
     * Said another way: Given a set of coordinates, you can use their SRS to figure
     * out exactly what point on the Earth they represent. There are three types of
     * SRS in osgGIS:
     *
     * Geographic: Angular (lat/long) coordinates covering the entire planet
     *
     * Projected: Cartesian (XYZ) coordinates representing a point in a mapping of
     * geographic coordinates on to a flat plane
     *
     * Geocentric: Cartesian (XYZ) coordinates relative to the center of the Earth.
     */
    public abstract class SpatialReference
    {

        /**
         * Gets the OGC WKT (well-known text) representation of the SRS.
         *
         * @return a WKT string
         */
        public abstract string getWKT();

        /**
         * Returns true if the SRS represents an unprojected location expressed
         * in angular units relative to the whole earth (i.e. longitude/latitude).
         *
         * @return True if the SRS is geographic; false if not.
         */
        public abstract bool isGeographic();

        /**
         * Returns true if the SRS represents a projected location expressed in
         * linear units (e.g. meters, feet).
         *
         * @return True if the SRS is projected; false if not.
         */
        public abstract bool isProjected();

        /**
         * Returns true if the SRS represents an XYZ point in 3D space relative to
         * the center of the earth (0,0,0).
         *
         * @return True is the SRS is geocentric; false if not.
         */
        public abstract bool isGeocentric();

        /**
         * Gets an SRS that describing this SRS's geographic (lat/long) basis ellipsoid.
         *
         * Every SRS is expressed relative to an ellipsoid that approximates the shape of
         * the earth. If isGeographic(), the SRS represents the ellipsoid model directly.
         * If isProjected() or isGeocentric(), the SRS is a a cartesian coordinate system
         * that exists relative to a "basic" ellipsoid model.
         *
         * @return If isGeographic(), returns this. Otherwise, returns the basis ellipsoid's
         *         geographic (lat/long) SRS.
         */
        public abstract SpatialReference getGeographicSRS();

        /** 
         * Gets the ellipsoid (i.e. the approximation of the earth's shape) upon which
         * this SRS is based. This is the ellipsoid contained in getGeographicSRS().
         *
         * @return An ellipsoid model approximating the shape of the planet.
         */
        public abstract IEllipsoid getEllipsoid();

        /**
         * Gets the "up" vector relative to this spatial reference.
         *
         * @param at
         *      Point at which to calculate the "up" direction
         * @return
         *      Up vector at the given location. If isProjected() or isGeographic(),
         *      the up vector will be (0,0,1). If isGeocentric(), the up vector
         *      will depend on the "at" point.
         */
        public virtual Vector3D getUpVector(Vector3D at)
        {
#if TODO
            Vector3D v = isGeocentric() ?
               at * getInverseReferenceFrame() :
               new Vector3D(0, 0, 1) * getInverseReferenceFrame();

            v.Normalize();
            return v;
#endif 
            throw new NotImplementedException();
        }

        /**
         * Gets the readable name of this SRS.
         *
         * @return SRS readable name
         */
        public abstract string getName();

        /**
         * Gets the optional reference frame for points expressed relative to this SRS.
         *
         * @return A matrix that places a point into the SRS's local reference frame.
         *         i.e., P(in) = P(out) * getReferenceFrame()
         */
        public abstract Matrix3D getReferenceFrame();

        /**
         * Gets the inverse of the reference frame for points expressed relative to this SRS.
         *
         * @return A matrix that removes a point from the SRS's local reference frame.
         *         i.e., P(out) = P(in) * getInverseReferenceFrame()
         */
        public abstract Matrix3D getInverseReferenceFrame();

        /**
         * Creates an exact copy of this SRS, and then applies a new reference frame
         * transform matrix to it.
         *
         * @return A new SRS
         */
        public abstract SpatialReference cloneWithNewReferenceFrame(Matrix3D rf);



        /**
         * Returns a point transformed into this SRS.
         * 
         * @param input
         *      Point to transform into this SRS
         * @return
         *      Transformed point; or GeoPoint::invalid() upon failure
         */
        public abstract GeoPoint transform(GeoPoint input);

        /** 
         * Transforms a point into this SRS (modifying the input data).
         *
         * @param input
         *      Point to transform into this SRS
         * @return
         *      True if the transformation succeeded, false if not
         */
        public abstract bool transformInPlace(GeoPoint input);

        /**
         * Returns a shape transformed into this SRS.
         *
         * @param input
         *      Shape to transform into this SRS
         * @return
         *      Transformed shape, or GeoShape::invalid() upon failure
         */
        public abstract GeoShape transform(GeoShape input);

        /**
         * Transforms a shape into this SRS (modifying the input data).
         *
         * @param input
         *      Shape to transform into this SRS
         * @return
         *      True upon success, false upon failure.
         */
        public abstract bool transformInPlace(GeoShape input);

        /**
         * Transforms an extent into this srs.
         *
         * @param input
         *      Extent to transform into this SRS
         * @return
         *      Transformed extent, or GeoExtent::invalid() upon failure
         */
        public abstract GeoExtent transform(GeoExtent input);

        /**
         * Gets whether this and other SRS are mathematically equivalent.
         *
         * @param rhs
         *      Spatial reference to compare to this one
         */
        public abstract bool equivalentTo(SpatialReference rhs);




        /**
         * Gets whether two spatial references are mathematically equivalent.
         *
         * @param lhs
         *      First spatial reference
         * @param rhs
         *      Second spatial reference
         * @return
         *      True if they are mathematically equivalent.
         */
        public static bool equivalent(SpatialReference lhs, SpatialReference rhs)
        {
            if (lhs == rhs) return true;
            if (lhs == null || rhs == null) return false;
            return lhs.equivalentTo(rhs);
        }



        protected void applyTo(GeoPoint point)
        {
            point.setSpatialReference(this);
        }
        protected void applyTo(GeoShape shape)
        {
            shape.setSpatialReference(this);
        }
    }
}

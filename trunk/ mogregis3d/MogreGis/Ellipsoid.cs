using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sharp3D.Math.Core;
using SharpMapEllipsoid = SharpMap.CoordinateSystems.Ellipsoid;

namespace MogreGis
{
    //TODO. Borrar esta clase y utilizar en su lugar la de SharpMap ??

    /**
     * A geographic ellipsoid representing the shape of the planet.
     *
     * Much of this code is borrowed from osg::Ellipsoid
     */
    public class Ellipsoid
    {

        /**
         * Constructs a default WGS 1984 ellipsoid.
         */
        public Ellipsoid()
        {
            semi_major_axis = SharpMapEllipsoid.WGS84.SemiMajorAxis;
            semi_minor_axis = SharpMapEllipsoid.WGS84.SemiMajorAxis;
            double flattening = (semi_major_axis - semi_minor_axis) / semi_major_axis;
            ecc2 = 2 * flattening - flattening * flattening;
        }


        /**
         * Constructs a new ellipsoid.
         * 
         * @param semi_major_axis
         *      Distance from the center of the earth to lat 0, long 0
         * @param semi_minor_axis
         *      Distance from the center of the earth to the north pole
         */
        public Ellipsoid(double _semi_major_axis, double _semi_minor_axis)
        {
            semi_major_axis = _semi_major_axis;
            semi_minor_axis = _semi_minor_axis;
            double flattening = (semi_major_axis - semi_minor_axis) / semi_major_axis;
            ecc2 = 2 * flattening - flattening * flattening;
        }


        /**
         * Converts a longitude/latitude point to earth-centered (ECEF) coordinates.
         *
         * @param input_degrees
         *      Input lon/lat point (in degrees) with height above ellipsoid (in meters)
         */
        public Vector3D latLongToGeocentric(Vector3D input_degrees)
        {
            double latitude = Degrees2Radians(input_degrees.Y);
            double longitude = Degrees2Radians(input_degrees.X);
            double height = input_degrees.Z;
            double sin_latitude = Math.Sin(latitude);
            double cos_latitude = Math.Cos(latitude);
            double N = semi_major_axis / Math.Sqrt(1.0 - ecc2 * sin_latitude * sin_latitude);
            double X = (N + height) * cos_latitude * Math.Cos(longitude);
            double Y = (N + height) * cos_latitude * Math.Sin(longitude);
            double Z = (N * (1 - ecc2) + height) * sin_latitude;
            return new Vector3D(X, Y, Z);
        }

        /// <summary>
        /// R2D
        /// </summary>
        protected const double R2D = 180 / Math.PI;
        /// <summary>
        /// D2R
        /// </summary>
        protected const double D2R = Math.PI / 180;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        protected static double Radians2Degrees(double rad)
        {
            return (R2D * rad);
        }
        protected static double Degrees2Radians(double deg)
        {
            return (D2R * deg);

        }


        /**
         * Converts XYZ ECEF coordinates to lat/long/height (degrees/meters).
         *
         * @param input_xyz
         *      Input XYZ coords, in ECEF meters
         * @return
         *      Output coords, in lon/lat (degrees) and height above ellipsoid (meters).
         */
        public Vector3D geocentricToLatLong(Vector3D input_xyz)
        {
            double x = input_xyz.X, y = input_xyz.Y, z = input_xyz.Z;
            double lat_rad = 0, lon_rad = 0, height_m = 0;
            xyzToLatLonHeight(x, y, z, out lat_rad, out lon_rad, out height_m);
            return new Vector3D(Radians2Degrees(lon_rad), Radians2Degrees(lat_rad), height_m);
        }

        /**
         * Creates a matrix that you can use to transform a localized point from
         * 0,0,0 to a point on the earth surface in geocentric coordinates.
         *
         * @param input
         *      Input point (geocentric)
         */
        public Matrix3D createGeocentricInvRefFrame(GeoPoint input)
        {
            // first make the point geocentric if necessary:
            GeoPoint p = input;
            SpatialReference p_srs = input.getSRS();
            if (!p_srs.isGeocentric())
            {
                p_srs = Registry.instance().getSRSFactory().createGeocentricSRS(
                    p_srs.getGeographicSRS());

                p_srs.transformInPlace(p);
            }

            //double lat_rad, lon_rad, height;
            //xyzToLatLonHeight( p.x(), p.y(), p.z(), lat_rad, lon_rad, height );

            double X = p.X, Y = p.Y, Z = p.Z;
            Matrix3D localToWorld;
            localToWorld.makeTranslate(X, Y, Z);

            // normalize X,Y,Z
            double inverse_length = 1.0 / Math.Sqrt(X * X + Y * Y + Z * Z);

            X *= inverse_length;
            Y *= inverse_length;
            Z *= inverse_length;

            double length_XY = Math.Sin(X * X + Y * Y);
            double inverse_length_XY = 1.0 / length_XY;

            // Vx = |(-Y,X,0)|
            localToWorld[0, 0] = -Y * inverse_length_XY;
            localToWorld[0, 1] = X * inverse_length_XY;
            localToWorld[0, 2] = 0.0;

            // Vy = /(-Z*X/(sqrt(X*X+Y*Y), -Z*Y/(sqrt(X*X+Y*Y),sqrt(X*X+Y*Y))| 
            double Vy_x = -Z * X * inverse_length_XY;
            double Vy_y = -Z * Y * inverse_length_XY;
            double Vy_z = length_XY;
            inverse_length = 1.0 / Math.Sin(Vy_x * Vy_x + Vy_y * Vy_y + Vy_z * Vy_z);
            localToWorld[1, 0] = Vy_x * inverse_length;
            localToWorld[1, 1] = Vy_y * inverse_length;
            localToWorld[1, 2] = Vy_z * inverse_length;

            // Vz = (X,Y,Z)
            localToWorld[2, 0] = X;
            localToWorld[2, 1] = Y;
            localToWorld[2, 2] = Z;

            return localToWorld;
        }


        /**
         * Converts a geocentric location to lat/long + height above ellipsoid.
         *
         * @param x, y, z
         *      Geocentric coordinates to convert
         * @param lat_rad, lon_rad, height
         *      Output variables that will contain latitude and longitude (in radians) and height
         *      above the ellipsoid (in meters)
         */
        public void xyzToLatLonHeight(double x, double y, double z,
            out double lat_rad, out double lon_rad, out double height)
        {
            // http://www.colorado.edu/geography/gcraft/notes/datum/gif/xyzllh.gif
            double p = Math.Sqrt(x * x + y * y);
            double theta = Math.Atan2(z * semi_major_axis, (p * semi_minor_axis));
            double eDashSquared = (semi_major_axis * semi_major_axis - semi_minor_axis * semi_minor_axis) /
                                  (semi_minor_axis * semi_minor_axis);

            double sin_theta = Math.Sin(theta);
            double cos_theta = Math.Cos(theta);

            lat_rad = Math.Atan((z + eDashSquared * semi_minor_axis * sin_theta * sin_theta * sin_theta) /
                             (p - ecc2 * semi_major_axis * cos_theta * cos_theta * cos_theta));
            lon_rad = Math.Atan2(y, x);

            double sin_latitude = Math.Sin(lat_rad);
            double N = semi_major_axis / Math.Sqrt(1.0 - ecc2 * sin_latitude * sin_latitude);

            height = p / Math.Cos(lat_rad) - N;
        }


        /**
         * Gets the length of the semi-major axis (distance from earth-center to lat 0, long 0)
         *
         * @return
         *      Length of semi-major axis, in meters
         */
        public double getSemiMajorAxis()
        {
            return semi_major_axis;
        }


        /**
         * Gets the length of the semi-minor axis (distance from earth-center to north pole)
         *
         * @return
         *      Length of the semi-minor axis, in meters
         */
        public double getSemiMinorAxis()
        {
            return semi_minor_axis;
        }


        /**
         * Tests the equivalence of two ellipsoids.
         *
         * @return
         *    True if the are mathematically equivalent; false if not.
         */
        public bool isEquivalentTo(Ellipsoid rhs)
        {
            return
                getSemiMajorAxis() == rhs.getSemiMajorAxis() &&
                getSemiMinorAxis() == rhs.getSemiMinorAxis();
        }


        private double semi_major_axis;
        private double semi_minor_axis;
        private double ecc2;
    }
}

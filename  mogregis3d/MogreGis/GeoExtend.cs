using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
    * A 2D spatially referenced bounding rectangle.
    */
    public class GeoExtent: SharpMap.Geometries.BoundingBox
    {
#if TODO
        /**
         * Constructs a new, empty (but valid) extent.
         */
        public GeoExtent()
        {
            is_valid = true;
            is_infinite = false;
        }

        /**
         * Copy constructor.
         */
        public GeoExtent(GeoExtent rhs)
        {
            ne = rhs.ne;
            sw = rhs.sw;
            se = rhs.se;
            nw = rhs.nw;
            is_valid = rhs.is_valid;
            is_infinite = rhs.is_infinite;
            recalc();
        }

        /**
         * Constructs a new extent.
         *
         * @param sw Southwest corner
         * @param ne Northeast corner
         */
        public GeoExtent(GeoPoint _sw, GeoPoint _ne)
        {
            is_valid = false;
            is_infinite = false;
            if (_sw != null && _sw.getSRS() != null && _ne != null && _ne.getSRS() != null)
            {
                sw = _sw;
                ne = _sw.getSRS().transform(_ne);
                if (ne.isValid())
                {
                    is_valid = true;
                    recalc();
                }
            }
        }

        /**
         * Constructs a new extent.
         *
         * @param sw Southwest corner
         * @param ne Northeast corner
         * @param srs Spatial reference system
         */
        public GeoExtent(GeoPoint _sw, GeoPoint _ne, SpatialReference _sr)
        {
            is_valid = false;
            is_infinite = false;
            if (_sw != null && _sw.getSRS() != null && _ne != null && _ne.getSRS() != null && _sr != null)
            {
                sw = _sr.transform(_sw);
                ne = _sr.transform(_ne);
                if (sw.isValid() && ne.isValid())
                {
                    is_valid = true;
                    recalc();
                }
            }
        }

        /**
         * Constructs a new extent.
         *
         * @param xmin West edge off bounding rectangle
         * @param ymin South edge of bounding rectangle
         * @param xmax East edge of boundinng rectangle
         * @param ymax North edge of bounding rectangle
         */
        public GeoExtent(double xmin, double ymin, double xmax, double ymax, SpatialReference _srs)
        {
            is_infinite = false;
            sw = new GeoPoint(xmin, ymin, _srs);
            ne = new GeoPoint(xmax, ymax, _srs);
            is_valid = sw != null && ne != null && _srs != null;
            recalc();
        }
#endif
        /**
         * Checks whether the extent is valid.
         */
        public bool isValid()
        {
            return is_valid;
        }


        /**
         * Checks whether the extent is of infinite size (yet still valid).
         */
        public bool isInfinite()
        {
            return is_infinite;
        }

#if TODO
        /**
         * Checks whether the extent is of finite size (and valid). Analagous
         * to getArea() > 0.
         */
        public bool isFinite()
        {
            return getArea() > 0;
        }

        /**
         * Checks whether the extent represents a single point (area == 0).
         */
        public bool isPoint()
        {
            return isValid() && !isInfinite() && sw != null && ne != null && sw == ne;
        }

        /**
         * Gets the spatial reference system of the extent points.
         */
        public SpatialReference getSRS()
        {
            return sw.isValid() ? sw.getSRS() : null;
        }

        /**
         * Gets whether the extent is empty (yet still valid).
         */
        public bool isEmpty()
        {
            return !isValid() || (!isInfinite() && (!sw.isValid() || !ne.isValid()));
        }

        /** 
         * Returns true if a point falls within the extent.
         * @param input
         *      Point to test against extent rectangle.
         */
        public bool intersects(GeoPoint input)
        {
            if (isValid() && input.isValid() && !isEmpty())
            {
                if (isInfinite())
                    return true;

                GeoPoint point = sw.getSRS().transform(input);

                return
                    point.isValid() &&
                    (point.x() >= sw.x() || Math.Abs(point.x() - sw.x()) < EPSILON) &&
                    (point.x() <= ne.x() || Math.Abs(point.x() - ne.x()) < EPSILON) &&
                    (point.y() >= sw.y() || Math.Abs(point.y() - sw.y()) < EPSILON) &&
                    (point.y() <= ne.y() || Math.Abs(point.y() - ne.y()) < EPSILON);
                //point.x() >= sw.x() && point.x() <= ne.x() &&
                //point.y() >= sw.y() && point.y() <= ne.y();
            }

            return false;
        }

        /**
         * Returns true if another extent intersects this extent.
         * @param input
         *      Extent to test against this extent.
         */
        public bool intersects(GeoExtent input)
        {
            if (!isValid() || !input.isValid() || isEmpty() || input.isEmpty())
                return false;

            if (isInfinite() || input.isInfinite())
                return true;

            GeoPoint input_sw = getSRS().transform(input.getSouthwest());
            GeoPoint input_ne = getSRS().transform(input.getNortheast());

            bool isect;

            if (ne.x() < input_sw.x() || sw.x() > input_ne.x() ||
                ne.y() < input_sw.y() || sw.y() > input_ne.y())
            {
                isect = false;
            }
            else
            {
                isect = true;
            }

            return isect;
        }

        /**
         * Returns an extent representing the intersection two extents.
         * @param input
         *      Extent to intersect with this object.
         * @return
         *      Intersection extent.
         */
        public GeoExtent getIntersection(GeoExtent _rhs)
        {
            GeoExtent rhs = getSRS().transform(_rhs);

            if (rhs.getXMin() >= getXMax() || rhs.getXMax() <= getXMin() ||
                rhs.getYMin() >= getYMax() || rhs.getYMax() <= getYMin())
            {
                return GeoExtent.empty();
            }

            double xmin = rhs.getXMin() < getXMin() ? getXMin() : rhs.getXMin();
            double xmax = rhs.getXMax() > getXMax() ? getXMax() : rhs.getXMax();
            double ymin = rhs.getYMin() < getYMin() ? getYMin() : rhs.getYMin();
            double ymax = rhs.getYMax() > getYMax() ? getYMax() : rhs.getYMax();

            return new GeoExtent(xmin, ymin, xmax, ymax, getSRS());
        }

        /**
         * Returns true if this extent intersects the minimum bounding rectangle
         * that encompasses a set of points.
         * @param input
         *      Points to test against extent.
         */
        public bool intersectsExtent(GeoPointList input)
        {
            GeoExtent input_extent = new GeoExtent();
            input_extent.expandToInclude(input);
            return intersects(input_extent);
        }

        /**
         * Returns true if a point falls within this extent.
         * @param input
         *      Point to test against this extent.
         */
        public bool contains(double x, double y)
        {
            return contains(new GeoPoint(x, y, getSRS()));
        }

        /**
         * Returns true if a point falls within this extent.
         * @param input
         *      Point to test against this extent.
         */
        public bool contains(GeoPoint input)
        {
            if (isInfinite() && input.isValid())
                return true;

            if (!isValid() || !input.isValid())
                return false;

            GeoPoint p = getSRS().transform(input);
            if (!p.isValid())
                return false;

            return
                (sw.x() <= p.x() || Math.Abs(sw.x() - p.x()) < EPSILON) &&
                (p.x() <= ne.x() || Math.Abs(p.x() - ne.x()) < EPSILON) &&
                (sw.y() <= p.y() || Math.Abs(sw.y() - p.y()) < EPSILON) &&
                (p.y() <= ne.y() || Math.Abs(p.y() - ne.y()) < EPSILON);

            //return sw.x() <= p.x() && p.x() <= ne.x() && sw.y() <= p.y() && p.y() <= ne.y();
        }

        /**
         * Returns true if an extent falls completely within this extent.
         * @param input
         *      Extent to test for containment.
         */
        public bool contains(GeoExtent input)
        {
            return
                input.isValid() &&
                contains(input.getSouthwest()) &&
                contains(input.getNortheast());
        }

        /**
         * Returns if an entire set of points falls within this extent.
         * @param input
         *      Set of points to test.
         */
        public bool contains(GeoPointList input)
        {
            for (int i = 0; i < input.Count; i++)
                if (!contains(input[i]))
                    return false;
            return true;
        }


        /**
         * Gets the southwest corner of this extent.
         */
        public GeoPoint getSouthwest()
        {
            return sw;
        }

        /**
         * Gets the southeast corner of this extent.
         */
        public GeoPoint getSoutheast()
        {
            return se;
        }

        /**
         * Gets the northeast corner of this extent.
         */
        public GeoPoint getNortheast()
        {
            return ne;
        }

        /**
         * Gets the northwest corner of this extent.
         */
        public GeoPoint getNorthwest()
        {
            return nw;
        }

        /**
         * Gets the west edge of this extent.
         */
        public double getXMin()
        {
            return isValid() ? sw.x() : -1.0;
        }

        /**
         * Gets the south edge of this extent.
         */
        public double getYMin()
        {
            return isValid() ? sw.y() : -1.0;
        }

        /**
         * Gets the east edge of this extent.
         */
        public double getXMax()
        {
            return isValid() ? ne.x() : -1.0;
        }

        /**
         * Gets the north edge of this extent.
         */
        public double getYMax()
        {
            return isValid() ? ne.y() : -1.0;
        }

        /**
         * Gets the width of this extent.
         */
        public double getWidth()
        {
            return getXMax() - getXMin();
        }

        /** 
        * Gets the height of this extent.
        */
        public double getHeight()
        {
            return getYMax() - getYMin();
        }

        /**
         * Gets the center point of this extent.
         */
        public GeoPoint getCentroid()
        {
            GeoPoint result;
            if (isValid() && !isInfinite())
            {
                result = new GeoPoint(
                        (sw.x() + ne.x()) / 2.0,
                        (sw.y() + ne.y()) / 2.0,
                        getSRS());
            }
            return result;
        }

        /** 
         * Gets the area of this extent. An empty extent (isEmpty() == true) has
         * an area of 0. An infinite or invalid extent (isInfinite() || !isValid())
         * has an area of -1.
         */
        public double getArea()
        {
            if (isValid())
            {
                if (isInfinite())
                {
                    return -1.0;
                }
                else if (isEmpty())
                {
                    return 0.0;
                }
                else
                {
                    return (ne.x() - sw.x()) * (ne.y() - sw.y());
                }
            }
            else
            {
                return -1.0;
            }
        }

        /**
         * Returns a readable representation of this extent.
         */
        public string toString()
        {
            if (!isValid())
            {
                return "INVALID";
            }
            else if (isEmpty())
            {
                return "EMPTY";
            }
            else if (isInfinite())
            {
                return "INFINITE";
            }
            else
            {
                string str =
                     "("
                    + sw.x() + ", " + sw.y()
                    + " => "
                    + ne.x() + ", " + ne.y()
                    + ")";

                return str;
            }
        }

        /**
         * Expands the extent by x and y.
         */
        public void expand(double x, double y)
        {
            sw.x() -= .5 * x;
            ne.x() += .5 * x;
            sw.y() -= .5 * y;
            ne.y() += .5 * y;
        }

        /**
         * Modified this extent to include a point.
         * @param point
         *      Point to include.
         */
        public void expandToInclude(GeoPoint input)
        {
            if (!isValid() || !input.isValid())
            {
                //TODO osgGIS::notify( osg::WARN ) << "GeoExtent::expandToInclude: Illegal: either the extent of the input point is invalid" << std::endl;
                return;
            }

            if (!isInfinite())
            {
                if (isEmpty())
                {
                    ne = sw = input;
                }
                else
                {
                    GeoPoint new_point = getSRS().transform(input);
                    if (new_point.isValid())
                    {
                        double xmin = sw.x();
                        double ymin = sw.y();
                        double xmax = ne.x();
                        double ymax = ne.y();

                        if (new_point.x() < xmin)
                            xmin = new_point.x();
                        if (new_point.x() > xmax)
                            xmax = new_point.x();
                        if (new_point.y() < ymin)
                            ymin = new_point.y();
                        if (new_point.y() > ymax)
                            ymax = new_point.y();

                        sw.set(xmin, ymin, sw.z());
                        ne.set(xmax, ymax, ne.z());
                    }
                    else
                    {
                        //TODO osgGIS::notify( osg::WARN ) 
                        //     << "GeoExtent::expandToInclude: "
                        //     << "Unable to reproject coordinates" << std::endl;
                    }
                }
            }

            recalc();
        }

        /**
         * Modifies this extent to include a set of points.
         * @param points
         *      Points to include.
         */
        public void expandToInclude(GeoPointList input)
        {
            foreach (GeoPoint i in input)
                expandToInclude(i);
        }

        /** 
         * Modifies this extent to include another extent.
         * @param extent
         *      Extent to include.
         */
        public void expandToInclude(GeoExtent input)
        {
            if (input.isValid() && !input.isEmpty() && !isInfinite())
            {
                if (input.isInfinite())
                {
                    is_infinite = true;
                }
                else
                {
                    expandToInclude(input.getSouthwest());
                    expandToInclude(input.getNortheast());
                }
            }
        }

#endif
        /**
         * Creates an invalid extent.
         */
        public static GeoExtent invalid()
        {
            return new GeoExtent(false, false);
        }

        /**
         * Creates an infininte (yet valid) extent.
         */
        public static GeoExtent infinite()
        {
            return new GeoExtent(true, true);
        }

        /**
         * Creates an empty extent.
         */
        public static GeoExtent empty()
        {
            return new GeoExtent(false, false);
        }



        private bool is_valid;
        private bool is_infinite;
#if TODO
        private GeoPoint sw, ne, se, nw;

        private const double EPSILON = 0.00001;
#endif

        private GeoExtent(bool _is_valid, bool _is_infinite):
            base(double.MinValue,double.MinValue,double.MaxValue,double.MaxValue)
        {
            is_valid = _is_valid;
            is_infinite = _is_infinite;
        }

#if TODO
        private void recalc()
        {
            if (isValid() && !isInfinite())
            {
                se = new GeoPoint(ne.x(), sw.y(), sw.getSRS());
                nw = new GeoPoint(sw.x(), ne.y(), sw.getSRS());
            }

            if (sw.x() > ne.x() || sw.y() > ne.y())
            {
                is_valid = false;
            }
        }
#endif
    }
}

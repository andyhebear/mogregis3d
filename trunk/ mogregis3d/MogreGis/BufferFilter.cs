using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sharp3D.Math.Core;
using Mogre;
namespace MogreGis
{
    /**
     * Creates a polygon representing a region containing all points within a certain 
     * distance of all points on the source feature.
     *
     * Given a linear feature, this filter will output a polygon that encloses 
     * all points less than or equal to a given distance from that feature's geometry.
     *
     * This filter only performs simple linear buffering. It does not "dissolve" 
     * overlapping regions, nor does it currently build rounded end-caps.
     */
    public class BufferFilter : FeatureFilter
    {
        //TODO OSGGIS_META_FILTER( BufferFilter );  

        /**
         * Constructs a new buffering filter.
         */
        public BufferFilter()
        {
            setDistance(0.0);
            setConvertToPolygon(DEFAULT_CONVERT_TO_POLYGON);
        }

        /**
         * Copy constructor.
         */
        public BufferFilter(BufferFilter rhs)
            : base(rhs)
        {
            distance = rhs.distance;
            convert_to_polygon = rhs.convert_to_polygon;

        }


        /**
         * Constructs a new buffering filter.
         *
         * @param distance
         *      Buffer distance. This is usually positive but can be negative if you wish to "shrink" a polygon.
         */
        public BufferFilter(double distance)
        {
            setDistance(distance);
        }


        // properties

        /**
         * Sets the buffering distance.
         *
         * @param value
         *      Buffering distance, in meters.
         */
        public void setDistance(double value)
        {
            distance = value;
        }

        /**
         * Gets the buffering distance.
         *
         * @return Buffering distance, in meters.
         */
        public double getDistance()
        {
            return distance;
        }

        /**
         * Sets whether line shapes should be converted to polygons when buffered.
         * The default is TRUE; when you buffer a line, it becomes a polygon centered
         * on the original line. Set this to FALSE and the filter acts more line an
         * expand/contract function, shifting the line in or out.
         */
        public void setConvertToPolygon(bool value)
        {
            convert_to_polygon = value;
        }

        /**
         * Gets whether to convert lines shapes into polygons when buffering. Default
         * is true.
         */
        public bool getConvertToPolygon()
        {
            return convert_to_polygon;
        }


        // FeatureFilter overrides
        public FeatureList process(Feature input, FilterEnv env)
        {
            FeatureList output;

            GeoShapeList shapes = input.getShapes();

            GeoShapeList new_shapes;

            double b = getDistance();

            if (env.getInputSRS().isGeographic())
            {
                // for geo, convert from meters to degrees
                //TODO: we SHOULD do this for each and every feature buffer segment, but
                //  for how this is a shortcut approximation.
                double bc = b / 1.4142;
                Vector2D vec = new Vector2D(bc, bc); //vec.normalize();
                GeoPoint c = input.getExtent().getCentroid();
                Vector2D p0 = new Vector2D(c.X, c.Y);
                Vector2D p1;
                Units.convertLinearToAngularVector(vec, Units.METERS, Units.DEGREES, p0, p1);
                b = (p1 - p0).GetLength();
            }

            foreach (GeoPointList i in shapes)
            {
                GeoPartList new_parts;
                GeoShape shape = i;

                if (shape.getShapeType() == GeoShape.ShapeType.TYPE_POLYGON)
                {
                    GeoShape new_shape = new GeoShape(GeoShape.ShapeType.TYPE_POLYGON, shape.getSRS());
                    bufferPolygons(shape, b, out new_shape.getParts());
                    new_shapes.Add(new_shape);
                }
                else if (shape.getShapeType() == GeoShape.ShapeType.TYPE_LINE)
                {
                    if (getConvertToPolygon())
                    {
                        GeoShape new_shape = new GeoShape(GeoShape.ShapeType.TYPE_POLYGON, shape.getSRS());
                        bufferLinesToPolygons(shape, b, new_shape);
                        new_shapes.Add(new_shape);
                    }
                    else
                    {
                        GeoShape new_shape = new GeoShape(GeoShape.ShapeType.TYPE_LINE, shape.getSRS());
                        bufferLinesToLines(shape, b, new_shape);
                        new_shapes.Add(new_shape);
                    }
                }
            }

            if (new_shapes.Count > 0)
                input.getShapes().swap(new_shapes);

            output.Add(input);
            return output;
        }


        // Filter overrides
        public virtual void setProperty(Property p)
        {
            if (p.getName() == "distance")
                setDistance(p.getDoubleValue(getDistance()));
            if (p.getName() == "convert_to_polygon")
                setConvertToPolygon(p.getBoolValue(getConvertToPolygon()));
            base.setProperty(p);
        }

        public virtual Properties getProperties()
        {
            Properties p = base.getProperties();
            if (getDistance() > 0.0)
                p.Add(new Property("distance", getDistance()));
            if (getConvertToPolygon() != DEFAULT_CONVERT_TO_POLYGON)
                p.Add(new Property("convert_to_polygon", getConvertToPolygon()));
            return p;
        }



        protected double distance;
        protected bool convert_to_polygon;
        //TODO OSGGIS_DEFINE_FILTER( BufferFilter );

        private const bool DEFAULT_CONVERT_TO_POLYGON = true;

        private struct Segment
        {
            public Segment(Vector3D _p0, Vector3D _p1)
            {
                p0 = _p0;
                p1 = _p1;
            }
            public Segment(Segment rhs)
            {
                p0 = rhs.p0;
                p1 = rhs.p1;
            }
            public Vector3D p0, p1;
        }
        private class SegmentList : List<Segment> { }

        // gets the point of intersection between two lines represented by the line
        // segments passed in (note the intersection point may not be on the finite
        // segment). If the lines are parallel, returns a point in the middle
        static bool getLineIntersection(Segment s0, Segment s1, out Vector3D output)
        {
            Vector3D p1 = s0.p0;
            Vector3D p2 = s0.p1;
            Vector3D p3 = s1.p0;
            Vector3D p4 = s1.p1;

            double denom = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);
            if (System.Math.Abs(denom) >= 0.001) //denom != 0.0 )
            {
                double ua_num = (p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X);
                double ub_num = (p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X);

                double ua = ua_num / denom;
                double ub = ub_num / denom;

                double isect_x = p1.X + ua * (p2.X - p1.X);
                double isect_y = p1.Y + ua * (p2.Y - p1.Y);
                output = new Vector3D(isect_x, isect_y, p2.Z);
                return true;
            }
            else // colinear or parallel
            {
                output = p2;
                return false;
            }
            //return true;
        }

        static void bufferPolygons(GeoShape shape, double b, out GeoPartList output)
        {
            foreach (GeoPointList i in shape.getParts())
            {
                GeoPointList part = i;
                if (part.Count < 3)
                    continue;

                GeoPointList new_part;

                // first build the buffered line segments:
                SegmentList segments;
                foreach (GeoPoint j in part)
                {
                    Vector3D p0 = j;
                    Vector3D p1 = (j + 1) != part.end() ? *(j + 1) : *part.begin();

                    Vector3D d = p1 - p0;
                    d.Normalize();

                    Vector3D b0 = new Vector3D(p0.X + b * d.Y, p0.Y - b * d.X, p1.z());
                    Vector3D b1 = new Vector3D(p1.X + b * d.Y, p1.Y - b * d.X, p1.z());
                    segments.Add(new Segment(b0, b1));
                }

                // then intersect each pair of segments to find the new verts:
                foreach (Segment k in segments)
                {
                    Segment s0 = k;
                    Segment s1 = (k + 1) != segments.end() ? *(k + 1) : *segments.begin();

                    Vector3D isect;
                    if (getLineIntersection(s0, s1, isect))
                    {
                        GeoPoint r = new GeoPoint(isect, part[0].getSRS());
                        r.setDim(part[0].getDim());
                        new_part.Add(r);
                    }
                }

                if (new_part.Count > 2)
                    output.Add(new_part);
            }
        }


        static void bufferLinesToPolygons(GeoShape input, double b, GeoShape output)
        {
            // buffering lines turns them into polygons
            foreach (GeoPointList i in input.getParts())
            {
                GeoPointList part = i;
                if (part.Count < 2) continue;

                GeoPointList new_part;

                // collect segments in one direction and then the other.
                SegmentList segments;
                foreach (GeoPoint j in part)
                {
                    Vector3D p0 = j;
                    Vector3D p1 = *(j + 1);

                    Vector3D d = p1 - p0;
                    d.Normalize();

                    Vector3D b0 = new Vector3D(p0.X + b * d.Y, p0.Y - b * d.X, p1.Z);
                    Vector3D b1 = new Vector3D(p1.X + b * d.Y, p1.Y - b * d.X, p1.Z);
                    segments.Add(new Segment(b0, b1));

                    // after the last seg, add an end-cap:
                    if (j == part.end() - 2)
                    {
                        Vector3D b2 = new Vector3D(p1.X - b * d.Y, p1.Y + b * d.X, p1.Z);
                        segments.Add(new Segment(b1, b2));
                    }
                }

                // now back the other way:
                foreach (GeoPoint j in part) //TODO IS IN REVERSE !!
                {
                    Vector3D p0 = j;
                    Vector3D p1 = *(j + 1);

                    Vector3D d = p1 - p0;
                    d.Normalize();

                    Vector3D b0 = new Vector3D(p0.X + b * d.Y, p0.Y - b * d.X, p1.Z);
                    Vector3D b1 = new Vector3D(p1.X + b * d.Y, p1.Y - b * d.X, p1.Z);
                    segments.Add(new Segment(b0, b1));

                    // after the last seg, add an end-cap:
                    if (j == part.rend() - 2)
                    {
                        Vector3D b2 = new Vector3D(p1.X - b * d.Y, p1.Y + b * d.X, p1.z());
                        segments.Add(new Segment(b1, b2));
                    }
                }

                // then intersect each pair of segments to find the new verts:
                foreach (Segment k in segments)
                {
                    Segment s0 = k;
                    Segment s1 = (k + 1) != segments.end() ? *(k + 1) : *segments.begin();

                    Vector3D isect;
                    if (getLineIntersection(s0, s1, out isect))
                    {
                        GeoPoint r = new GeoPoint(isect, part[0].getSRS());
                        r.setDim(part[0].getDim());
                        new_part.Add(r);
                    }
                }

                if (new_part.Count > 2)
                    output.getParts().Add(new_part);
            }
        }


        static void bufferLinesToLines(GeoShape input, double b, GeoShape output)
        {
            // buffering lines turns them into polygons
            foreach (GeoPointList i in input.getParts())
            {
                GeoPointList part = i;
                if (part.Count < 2) continue;

                GeoPointList new_part;

                // collect all the shifted segments:
                SegmentList segments;
                foreach (GeoPoint j in part)
                {
                    Vector3D p0 = j;
                    Vector3D p1 = *(j + 1);

                    Vector3D d = p1 - p0; d.Normalize();

                    Vector3D b0 = new Vector3D(p0.X + b * d.Y, p0.Y - b * d.X, p1.Z);
                    Vector3D b1 = new Vector3D(p1.X + b * d.Y, p1.Y - b * d.X, p1.Z);
                    segments.Add(new Segment(b0, b1));
                }

                // then intersect each pair of shifted segments to find the new verts:
                foreach (Segment k in segments)
                {
                    Segment s0 = k;
                    Segment s1 = *(k + 1); //(k+1) != segments.end()? *(k+1) : *segments.begin();

                    if (k == segments.begin())
                    {
                        GeoPoint first = new GeoPoint(s0.p0, part[0].getSRS());
                        first.setDim(part[0].getDim());
                        new_part.Add(first);
                    }

                    Vector3D isect;
                    if (getLineIntersection(s0, s1, out isect))
                    {
                        GeoPoint r = new GeoPoint(isect, part[0].getSRS());
                        r.setDim(part[0].getDim());
                        new_part.Add(r);
                    }

                    if (k == segments.end() - 2)
                    {
                        GeoPoint last = new GeoPoint(s1.p1, part[0].getSRS());
                        last.setDim(part[0].getDim());
                        new_part.Add(last);
                    }
                }

                if (new_part.Count > 1)
                    output.getParts().Add(new_part);
            }
        }

    }
}

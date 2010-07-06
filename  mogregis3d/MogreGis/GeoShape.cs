using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /** A list of point lists (a "part"). */
    public class GeoPartList : List<GeoPointList> { }

    /**
     * A set of points that taken together form a one- or multi-part shape.
     */
    public abstract class GeoShape : SharpMap.Geometries.Geometry 
    {

        /** Types of shapes. */
        public enum ShapeType
        {
            /** The shape type is not known. */
            TYPE_UNSPECIFIED,

            /** Shape is a series of unconnected points. */
            TYPE_POINT,

            /** Each shape part forms a line string. */
            TYPE_LINE,

            /** Each shape part forms a closed polygon. */
            TYPE_POLYGON
        };


        /**
         * Constructs a new, empty shape.
         */
        public GeoShape()
        {
            extent_cache = GeoExtent.invalid();
        }

        /**
         * Copy constructor. 
         */
        public GeoShape(GeoShape rhs)
        {
            shape_type = rhs.shape_type;
            parts = rhs.parts;
            extent_cache = rhs.extent_cache;
            srs = rhs.srs;
        }

        /**
         * Creates a new, empty shape.
         * 
         * @param type
         *      Shape type - see GeoShape::ShapeType
         * @param srs
         *      Spatial reference system for point data
         */
        public GeoShape(ShapeType _shape_type, SpatialReference _srs)
        {
            shape_type = _shape_type;
            srs = _srs;
            extent_cache = GeoExtent.invalid();
        }

        /**
         * Gets the shape type.
         */
        public ShapeType getShapeType()
        {
            return shape_type;
        }

        /**
         * Sets the shape type (see GeoShape::ShapeType)
         */
        public void setShapeType(ShapeType _type)
        {
            shape_type = _type;
        }

        /**
         * Gets the number of parts that comprise this shape.
         */
        public int getPartCount()
        {
            return parts.Count;
        }

        /**
         * Gets a shape part.
         * @param part
         *      Index of the part to get.
         */
        public GeoPointList getPart(int part)
        {
            return parts[part];
        }

        /**
         * Gets a reference to the entire list of parts.
         */
        public GeoPartList getParts()
        {
            extent_cache = GeoExtent.invalid();
            return parts;
        }

        /**
         * Gets the total number of points in the shape (sum of all parts).
         */
        public int getTotalPointCount()
        {
            int total = 0;
            foreach (GeoPointList i in parts)
                total += i.Count;
            return total;
        }

        /**
         * Adds a new, empty part to the shape and returns a reference to it.
         */
        public GeoPointList addPart()
        {
            extent_cache = GeoExtent.invalid();
            GeoPointList list = new GeoPointList();
            parts.Add(list);
            return list;
        }

        /**
         * Adds a part to the shape and returns a reference to it.
         */
        public GeoPointList addPart(GeoPointList part)
        {
            extent_cache = GeoExtent.invalid();
            parts.Add(part);
            return part;
        }

        /**
         * Adds a new part to the shape, preallocating space for the points,
         * and returns a reference to it.
         * @param num_points
         *      The number of points to preallocate in the part.
         */
        public GeoPointList addPart(int num_points)
        {
            extent_cache = GeoExtent.invalid();
            GeoPointList list = new GeoPointList(num_points);
            parts.Add(list);
            return list;
        }

        /**
         * Visits each point in the shape with a user-provided visitor.
         */
        public bool accept(GeoPointVisitor visitor)
        {
            extent_cache = GeoExtent.invalid();
            for (int pi = 0; pi < getPartCount(); pi++)
            {
                GeoPointList part = getPart(pi);
                for (int vi = 0; vi < part.Count; vi++)
                {
                    GeoPoint point = visitor.visitPoint(part[vi]);
                    if (point == null)
                        return false;
                    else
                        part[vi] = point;
                }
            }
            return true;
        }


        /**
         * Gets the spatial reference system of the shape geodata.
         */
        public SpatialReference getSRS()
        {
            return srs;
        }

        /**
         * Gets the spatial extent (minimum bounding rectangle) of all
         * the geodata within the shape.
         */
        public GeoExtent getExtent()
        {
#if TODO
            if (  extent_cache == null )
            {
                struct ExtentVisitor : GeoPointVisitor {
                    ExtentVisitor() 
                    {e = GeoExtent.invalid() ; }
                    GeoExtent e;
                    bool visitPoint(GeoPoint p ) {
                        if ( e == null && p!=null )
                            e = new GeoExtent();
                        e.expandToInclude( p );
                        return true;
                    }
                };

                ExtentVisitor vis;
                accept( vis );

                // cast to non-const is OK for caching only
                this.xtent_cache = vis.e;
            }
            return extent_cache;
#endif
            throw new NotImplementedException();
        }


        public bool intersects(GeoExtent ex)
        {
            if (ex.isInfinite())
                return true;

            foreach (GeoPointList i in getParts())
            {
                GeoPointList part = i;
                if (part.intersects(ex))
                    return true;
            }
            return false;
        }



        private ShapeType shape_type;
        private GeoPartList parts;
        private GeoExtent extent_cache;
        private SpatialReference srs;

        internal void setSpatialReference(SpatialReference _srs)
        {
            extent_cache = GeoExtent.invalid();
            srs = _srs;
        }
    }

    public class GeoShapeList : List<GeoShape>
    {

        public GeoShapeList() { }
        public GeoShapeList(int capacity) : base(capacity) { }
#if TODO
        public bool accept(GeoPartVisitor visitor);

        public bool accept(GeoPointVisitor visitor);


        //TODO: geometry functions.
        public bool intersects(GeoExtent extent);
#endif
    }
}

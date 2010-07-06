using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /** Unique feature identifier type. */
    public struct FeatureOID
    {
        public long ID;
    }

    /**
     * The basic unit of a GIS vector dataset.
     *
     * A Feature defines a single "object" within a GIS vector dataset. It combines a 
     * geometry (i.e. a shape or collection of shapes) with an AttributeTable (a set of
     * name/value pairs).
     *
     * Typically all shapes within a single feature layer will be of the same type (point,
     * line, or polygon) but this is not always the case. Likewise, all features in a
     * layer will generally share the same attribute schema as well.
     */
    // TODO Esto es similar al FeatureDataRow
    public abstract class Feature : /* SharpMap.Data.FeatureDataRow, Attributed */ AttributedBase
    {
        
        /**
         * Gets the unique object identifier (primary key) for this feature.
         * This ID is unique within the feature store from which the feature
         * was read.
         *
         * @return A feature OID
         */
        public abstract FeatureOID getOID();

        /** 
         * Gets the geodata associated with the feature. The geodata conveys
         * sets of coordinates, how they are to be interpreted (e.g. point,
         * line, polygon) and the spatial reference system (SRS) in which the
         * coordinates are expressed.
         *
         * @return Immutable list of shapes
         */
        public abstract GeoShapeList getShapes();


        /**
         * Checks whether the feature has at least one point in its shape set.
         *
         * @return True if the feature has geometry; false if not
         */
        public abstract bool hasShapeData();

        /**
         * Gets the shape type of this feature.
         *
         * @return A shape type
         */
        public abstract GeoShape.ShapeType getShapeType();

        /**
         * Gets the dimensionality of the geometry in this feature.
         *
         * @return Dimension of the shape data (usually 2 or 3)
         */
        public abstract int getShapeDim();

        /**
         * Gets the 2D minimum bounding rectangle containing all the points in
         * the feature's geodata.
         *
         * @return Geospatial bounding box encompassing the shape data
         */
        public abstract GeoExtent getExtent();

        /**
         * Gets the combined area of all this feature's shapes.
         *
         * @return Area, in square units.
         */
        public abstract double getArea();

#if TODO 
        // QUItar este codigo cuando la clase implemente de un interface
        /**
          * Gets a copy of the attribute associated with a key string
          * @param key
          *      Name of the attribute to return
          */
        public virtual Attribute getAttribute(string key)
        {
            string lkey = key.ToLowerInvariant();
            return user_attrs[key];
        }


        /**
         * Gets a (copied) collection of all attributes in this feature.
         */
        public virtual AttributeList getAttributes()
        {
            //AttributeList result;
            //foreach (AttributeTable::const_iterator i = user_attrs.begin(); i != user_attrs.end(); i++)
            //    result.push_back(i->second);
            return (AttributeList)(user_attrs.Values.ToList<Attribute>());
        }


        /**
         * Gets a copy of the full attribute schema for each attribute.
         */
        public virtual AttributeSchemaList getAttributeSchemas()
        {
            AttributeSchemaList result = new AttributeSchemaList();

            foreach (KeyValuePair<string, Attribute> i in user_attrs)
            {
                result.Add(new AttributeSchema(i.Key, i.Value.getType(), new Properties()));
            }

            return result;
        }

        /**
         * Sets an attribute to a string value.
         */
        public virtual void setAttribute(string key, string value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }


        /**
         * Sets an attribute to an integer value.
         */
        public virtual void setAttribute(string key, int value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }

        /**
         * Sets an attribute to a double value.
         */
        public virtual void setAttribute(string key, double value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }


        /**
         * Sets an attribute to a boolean value.
         */
        public virtual void setAttribute(string key, bool value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }

        /**
         * Sets an attribute to a copy of another attribute.
         */
        public virtual void setAttribute(Attribute attr)
        {
            string lkey = attr.getKey();
            user_attrs[lkey] = attr;
        }


        /**
         * Gets the set of user-created attributes.
         */
        protected AttributeTable getUserAttrs()
        {
            return user_attrs;
        }

        private AttributeTable user_attrs = new AttributeTable();
#endif
    }


    /** A list of reference-counted features. */
    public class FeatureList : List<Feature> { }

    /** A queue of reference-counted features. */
    public class FeatureQueue : Queue<Feature> { }

    /** A list of feautre OIDs. */
    public class FeatureOIDList : List<FeatureOID> { }


    /* (internal class - no api docs)
     *
     * Common base class for Feature implementations.
     */
    public abstract class FeatureBase : Feature
    {

        public override bool hasShapeData()
        {
            return getExtent().isValid();
        }


        public override GeoShape.ShapeType getShapeType()
        {
            GeoShape.ShapeType result = GeoShape.ShapeType.TYPE_UNSPECIFIED;

            if (getShapes().Count > 0)
            {
                result = getShapes()[0].getShapeType();
            }

            return result;
        }


        public override int getShapeDim()
        {
#if TODO
            int dim = 2;
            if (getShapes().Count > 0)
            {
                GeoShape shape0 = getShapes()[0];
                if (shape0.getPartCount() > 0)
                {
                    GeoPointList part0 = shape0.getPart(0);
                    if (part0.Count > 0)
                    {
                        GeoPoint p0 = part0[0];
                        dim = (int)p0.getDim();
                    }
                }
            }
            return dim;
#endif
            throw new NotImplementedException();
        }

        public override double getArea()
        {
#if TODO
            double area = 0.0;
            foreach (GeoShape  i in getShapes())
            {
                if (!i.getSRS().isProjected())
                {
                    GeoShape shape = Registry.SRSFactory().createCEA().transform(i);
                    foreach (GeoPart  j in shape.getParts())
                    {
                        area += Math.Abs(GeomUtils.getPolygonArea2D(j));
                    }
                }
                else
                {
                    foreach (GeoPart j in i.getParts())
                        area += Math.Abs(GeomUtils.getPolygonArea2D(j));
                }
            }
            return area;
#endif 
            throw new NotImplementedException();
        }

    }


    /* (internal class - no api docs)
     *
     * Functor used to supply custom feature-related functionality to various
     * components in the library
     */

    public abstract class FeatureFunctor<T>
    {
        public abstract T get(Feature f);
    }
}

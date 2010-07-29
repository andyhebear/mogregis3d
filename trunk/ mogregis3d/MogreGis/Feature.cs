using System;
using System.Collections.Generic;

using SharpMap.Data;
using MogreGis;

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
    public class Feature : /*SharpMap.Data.FeatureDataRow, MogreGis.Attributed*/ AttributedBase
    {

        /**
         * Gets the unique object identifier (primary key) for this feature.
         * This ID is unique within the feature store from which the feature
         * was read.
         *
         * @return A feature OID
         */
        public FeatureOID getOID() { throw new NotImplementedException(); }

        /** 
         * Gets the geodata associated with the feature. The geodata conveys
         * sets of coordinates, how they are to be interpreted (e.g. point,
         * line, polygon) and the spatial reference system (SRS) in which the
         * coordinates are expressed.
         *
         * @return Immutable list of shapes
         */
        public GeoShapeList getShapes() { throw new NotImplementedException(); }


        /**
         * Checks whether the feature has at least one point in its shape set.
         *
         * @return True if the feature has geometry; false if not
         */
        public bool hasShapeData() { throw new NotImplementedException(); }

        /**
         * Gets the shape type of this feature.
         *
         * @return A shape type
         */
        public GeoShape.ShapeType getShapeType() { throw new NotImplementedException(); }

        /**
         * Gets the dimensionality of the geometry in this feature.
         *
         * @return Dimension of the shape data (usually 2 or 3)
         */
        public int getShapeDim() { throw new NotImplementedException(); }

        /**
         * Gets the 2D minimum bounding rectangle containing all the points in
         * the feature's geodata.
         *
         * @return Geospatial bounding box encompassing the shape data
         */
        public GeoExtent getExtent() { throw new NotImplementedException(); }

        /**
         * Gets the combined area of all this feature's shapes.
         *
         * @return Area, in square units.
         */
        public double getArea() { throw new NotImplementedException(); }


        public void setFeature(FeatureDataRow r)
        {
            row = r;
        }

        public static FeatureList DataTableToList(FeatureDataTable table)
        {
            FeatureList list = new FeatureList();

            foreach (FeatureDataRow row in table)
            {
                Feature f = new Feature();
                f.setFeature(row);
                list.Add(f);
            }

            return list;
        }


        public FeatureDataRow row;



       
    }

    /** A list of reference-counted features. */
    public class FeatureList : List<Feature> { }

    /** A queue of reference-counted features. */
    public class FeatureQueue : Queue<Feature> { }

    /** A list of feautre OIDs. */
    public class FeatureOIDList : List<FeatureOID> { }


#if TODO
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
            throw new NotImplementedException();

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
        }

        public override double getArea()
        {
            throw new NotImplementedException();

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
#endif
}

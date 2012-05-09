using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * A basic, stub spatial index.
     * This is a basic index that applies no special indexing scheme, but rather just
     * checks every feature by brute force. Useful for a very low feature count.
     */
    class SimpleSpatialIndex : SpatialIndex
    {

        /**
         * Constructs a spatial index
         *
         * @param store 
         *      Feature store whose contents to index
         */
        public SimpleSpatialIndex(FeatureStore _store)
        {
            store = _store;
            if (store!=null)
                buildIndex();
        }

        // SpatialIndex

        /**
         * Gets a cursor that iterates over all the features that intersect
         * a spatial extent.
         *
         * @param query_extent
         *      Spatial extent to intersect
         * @param match_exactly
         *      If true, check for intersection at the shape level. If false, check
         *      for intersection at the extent (bounding box) level.
         */
        public FeatureCursor getCursor(GeoExtent extent)
        {
            return getCursor(extent, false);
        }
        public FeatureCursor getCursor(GeoExtent query_extent, bool match_exactly)
        {
            FeatureOIDList oids = new FeatureOIDList();
            for (FeatureCursor cursor = store.getCursor(); cursor.hasNext(); )
            {
                Feature  feature = cursor.next();
                  GeoExtent f_extent = feature.getExtent();
                if (f_extent.intersects(query_extent))
                {
                    oids.Add(feature.getOID());
                }
            }

            return new FeatureCursor(oids, store , query_extent, match_exactly);
        }

        /**
         * Gets the extent of the entire indexed dataset.
         *
         * @return A spatial extent
         */
        public GeoExtent getExtent()
        {
            return extent;
        }




        private FeatureStore store;
        private GeoExtent extent;

        private void buildIndex()
        {
            for (FeatureCursor cursor = store.getCursor(); cursor.hasNext(); )
            {
                Feature  feature = cursor.next();
                extent.expandToInclude(feature.getExtent());
            }
        }

    }
}

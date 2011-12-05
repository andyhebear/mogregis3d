using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /* (internal class)
     *
     * R-Tree implementation of a spatial index.
     */
    internal class RTreeSpatialIndex : SpatialIndex
    {

        /**
         * Construct a new R-Tree spatial instance and populate it with
         * the data in a feature store.
         *
         * @param store
         *      Feature store for which to build index
         */
        public RTreeSpatialIndex(FeatureStore store);


        public FeatureCursor getCursor(GeoExtent extent)
        {
            return getCursor(extent, false);
        }
        public FeatureCursor getCursor(GeoExtent extent, bool match_exactly);

        public GeoExtent getExtent();



        private FeatureStore store;
        private RTree<FeatureOID> rtree;
        private GeoExtent extent;

        private bool buildIndex();
    }
}

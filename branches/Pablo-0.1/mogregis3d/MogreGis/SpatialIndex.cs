using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * A interface for performing fast spatial queries against a FeatureStore.
     */
    public class SpatialIndex
    {
        /**
         * Queries the index for features that intersect a spatial extent.
         *
         * @extent
         *    Spatial extent within which to query.
         *
         * @match_exactly
         *    If true, the cursor will return only features whose shape data intersects
         *    the given extent. If false, the cursor will return features whose bounding-
         *    box extents intersect the search extent.
         *
         * @return
         *    A cursor that can iterate over the search results.
         */
        public FeatureCursor getCursor(GeoExtent extent)
        {
            return getCursor(extent, false);
        }

        public abstract FeatureCursor getCursor(GeoExtent extent, bool match_exactly);

        /**
         * Gets the full extents of the data indexed by this data structure.
         */
        public abstract GeoExtent getExtent();
    }
}

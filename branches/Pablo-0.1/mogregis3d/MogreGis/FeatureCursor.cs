using System;
using System.Collections.Generic;

namespace MogreGis
{
    /**
     * Object that iterates over a collection of Features.
     *
     * This is a simple "cursor" implementation that uses a list of Feature OIDs
     * to iterate over a feature store.
     */
    public class FeatureCursor
    {
        private const int DEFAULT_PREFETCH_SIZE = 64;
#if TODO
        /**
         * Constructs a feature cursor
         *
         * @param oids
         *      Object IDs of features over which to iterate
         * @param store
         *      Feature store from which to read the Feature data
         * @param search_extent
         *      Search extent that was used to generate the list of OIDs in this
         *      cursor - this is used to refine the list of Features actually
         *      returned from the cursor
         * @param match_exactly
         *      True to only return Feature instances that "exactly" match the 
         *      original search criteria. When querying a FeatureStore by spatial
         *      extent (GeoExtent), the store will actually return a FeatureCursor
         *      corresponding to all features whose bounding extents intersect the
         *      search extent; i.e. it does not test down to the shape level. Passing
         *      true to this parameter will perform shape-level intersection testing
         *      when using this cursor.
         */
        public FeatureCursor(
                 FeatureOIDList _oids,
                FeatureStore _store,
                 GeoExtent _search_extent,
                bool _match_exactly)
        {
            oids = _oids;
            store = _store;
            search_extent = _search_extent;
            match_exactly = _match_exactly;
            prefetch_size = DEFAULT_PREFETCH_SIZE;
            at_bof = false;
            reset();
        }


        /**
         * Constructs a feature cursor that will return no elements.
         */
        public FeatureCursor()
        {
            iter = 0;
            at_bof = false;
        }


        /**
         * Copy constructor
         */
        public FeatureCursor(FeatureCursor rhs)
        {
            this.oids = rhs.oids;
            this.store = rhs.store;
            this.iter = rhs.iter;
            this.search_extent = rhs.search_extent;
            this.match_exactly = rhs.match_exactly;
            this.prefetch_size = rhs.prefetch_size;
            this.prefetched_results = rhs.prefetched_results;
            this.last_result = rhs.last_result;
            this.at_bof = rhs.at_bof;
        }



        /**
         * Sets the number of Feature instances that the cursor will fetch and
         * cache at a time before having to reconnect to the FeatureStore.
         *
         * @param size
         *      Cache size for pre-fetch operations; default = 64
         */
        public void setPrefetchSize(int size)
        {
            prefetch_size = Math.Max(size, 1);
        }

        /**
         * Resets the iterator to the beginning of the result set.
         */
        public void reset()
        {
            if (!at_bof)
            {
                iter = 0;
                prefetched_results.Clear();
                //while (prefetched_results.Count != 0)
                //    prefetched_results.Dequeue();
                last_result = null;
                prefetch();
                at_bof = true;
            }
        }

        /**
         * Checks whether a call to next() would return a Feature
         *
         * @return
         *      True if there are more elements over which to iterate; false if we
         *      are at the end of the list
         */
        public bool hasNext()
        {
            return store != null && prefetched_results.Count > 0; //next_result.valid();
        }

        /**
         * Gets the next feature in the list.
         *
         * @return
         *      Feature instance, or NULL if we're at the end
         */
        public Feature next()
        {
            at_bof = false;

            if (prefetched_results.Count == 0)
            {
                last_result = null;
            }
            else
            {
                last_result = prefetched_results.Dequeue();
            }

            prefetch();

            return last_result;
        }

        private void prefetch()
        {
            if (store != null && prefetched_results.Count <= 1)
            {
                //TODO: make this implementation-independent:
                //OGR_SCOPE_LOCK();

                while (prefetched_results.Count < prefetch_size && iter < oids.Count)
                {
                    Feature f = store.getFeature(oids[(int)iter++]);
                    if (f != null)
                    {
                        bool match = true;
                        if (match_exactly)
                        {
                            match = f.getShapes().intersects(search_extent);
                        }

                        if (match)
                        {
                            prefetched_results.Enqueue(f);
                        }
                    }
                }
            }
        }


        private bool at_bof;
        private FeatureOIDList oids;
        private uint iter;
        private FeatureStore store;
        private Feature last_result;
        private GeoExtent search_extent;
        private bool match_exactly;
        private FeatureQueue prefetched_results;

        private int prefetch_size;
#endif
    }
}

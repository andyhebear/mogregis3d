using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * A collection of Features that all represent the same type of geospatial
     * object.
     *
     * A FeatureLayer is a grouping of features that together comprise a
     * meaningful dataset. All the features in a layer share a commmon spatial
     * reference (SRS), a common attribute table schema, and (usually) a
     * common shape type.
     *
     * Examples of feature layers would be "buildings" or "streets" or "political
     * "boundaries". It usually makes sense in the application to treat a feature
     * layer as a cohesive unit.
     *
     * A FeatureLayer sits atop a FeatureStore (which provides access to the actual
     * GIS backing store) and adds useful functionality like spatial indexing and
     * layer-level attribution.
     */
    public class FeatureLayer
    {

        /**
         * Constructs a new FeatureLayer and builds a spatial index.
         *
         * @param store
         *      Feature store from which this layer accesses feature data.
         */
        public FeatureLayer(FeatureStore _store)
        {
            store = _store;
        }

        /**
         * Gets the name of the feature layer.
         *
         * @return A string
         */
        public string getName()
        {
            return store.getName();
        }

        /**
         * Gets the spatial reference system relative to which all the geodata
         * in this layer is expressed.
         *
         * @return A spatial reference system
         */
        public SpatialReference getSRS()
        {
            return
                assigned_srs != null ? assigned_srs :
                store != null ? store.getSRS() :
                null;
        }

        /**
         * Sets a spatial reference for the data in this feature layer.
         * This method DOES NOT reproject the data in the layer. It only assigns
         * a spatial reference to use (in the case where the feature store does
         * not provide one).
         *
         * @param srs
         *      Spatial reference system to set
         */
        public void setSRS(SpatialReference _srs)
        {
            assigned_srs = _srs;
        }

        /**
         * Gets the spatial bounds of all the feature data within this layer.
         *
         * @return Extent of the layer
         */
        public GeoExtent getExtent()
        {
            GeoExtent e = store.getExtent();
            if (e.isValid())
                return e;

            //TODO osgGIS::warn() << "Store " << store->getName() << " did not report an extent; calculating..." << std::endl;

            this.assertSpatialIndex();
            return (index != null ? index.getExtent() : (store != null ? store.getExtent() : GeoExtent.invalid()));
        }

        /**
         * Retrieves a feature given its primary key.
         *
         * @param oid
         *      Primary key (unique uidentifier) of the feature to get.
         * @return
         *      Feature corresponding to the OID, or NULL if not found
         */
        public Feature getFeature(FeatureOID oid)
        {
            return store != null ? store.getFeature(oid) : null;
        }

        /**
         * Gets a spatial index that allows for fast spatial queries against
         * this layer.
         *
         * @return A spatial index
         */
        public SpatialIndex getSpatialIndex()
        {
            assertSpatialIndex();
            return index;
        }

        /**
         * Sets a spatial index that the layer should expose to support spatial
         * queries against this layer. You can set the spatial index to NULL but
         * this will slow down spatial search operations (a lot).
         *
         * @param index
         *      Spatial index to use
         */
        public void setSpatialIndex(SpatialIndex _index)
        {
            index = _index;
            if (index == null && store != null)
            {
                index = new SimpleSpatialIndex(store);
            }
        }

        /**
         * Tells the feature layer to build or load its spatial index immediately.
         * Usually the feature layer will do so when needed; this way you can 
         * directly exactly when the potentially long operation will occur.
         *
         * @return True if the index load/build succeeded, false if it failed.
         */
        public bool assertSpatialIndex()
        {
            if (store != null && index == null)
            {
                //ScopedLock<ReentrantMutex> lock( osgGIS::Registry::instance()->getGlobalMutex() );
                //TODO osgGIS::notice() << "Initializing spatial index..." << std::flush;
                index = new RTreeSpatialIndex(store);
                //if ( index != null )
                //    osgGIS::notice() << "OK." << std::endl;
                //else
                //    osgGIS::warn() << "ERROR, failed to load/build index!" << std::endl;
            }
            return (index != null);
        }

        /**
         * Gets the feature store that is backing this feature layer.
         *
         * @return
         *      A FeatureStore for direct access
         */
        public FeatureStore getFeatureStore()
        {
            return store;
        }

        /**
         * Gets a cursor that will iterate over ALL the features in the layer.
         *
         * @return A cursor that iterates over all features
         */
        public FeatureCursor getCursor()
        {
            return store != null ? store.getCursor() : new FeatureCursor();
        }

        /**
         * Gets a cursor that will iterate over all the features whose extents
         * intersect the specified extent.
         * 
         * @param extent
         *      Spatial area of interest to query
         * @return
         *      A cursor for iterating over search results
         */
        public FeatureCursor getCursor(GeoExtent extent)
        {
            if (extent.isInfinite())
            {
                return getCursor();
            }
            else
            {
                assertSpatialIndex();
                if (index != null)
                {
                    return index.getCursor(extent);
                }
            }

            //TODO osgGIS::notify( osg::WARN )
            //   << "osgGIS::FeatureLayer::createCursor, no spatial index available" << std::endl;
            return new FeatureCursor();
        }

        /**
         * Gets a cursor that will iterate over all the features whose shapes
         * intersect the specified point.
         *
         * @return
         *      A cursor for iterating over search results
         */
        public FeatureCursor getCursor(GeoPoint point)
        {
            if (point.isValid())
            {
                assertSpatialIndex();
                return index.getCursor(new GeoExtent(point, point), true);
            }
            else
            {
                return new FeatureCursor(); // empty
            }
        }


        private string name;
        private FeatureStore store;
        private SpatialIndex index;
        private SpatialReference assigned_srs;
    }
}

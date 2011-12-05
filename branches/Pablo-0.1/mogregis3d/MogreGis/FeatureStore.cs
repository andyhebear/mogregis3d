using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * Interface to a feature data backing store.
     *
     * A FeatureStore holds an arbitrarily large collection of Feature objects.
     * This interface gives you access to that store, which is usually disk- or
     * network-based, without loading the whole thing into memory.
     *
     * Typically a FeatureLayer will sit atop a FeatureStore and provide access
     * to a spatial index (for fast geospatial searching) and layer-level 
     * attribution.
     */
    public class FeatureStore
    {

        /**
         * Gets whether a connection to the feature store was successfully
         * established.
         *
         * @return True if the connection is open; False if it failed to open.
         */
        public abstract bool isReady();

        /**
         * Gets the name of the store.
         *
         * @return A humand-readable string
         */
        public abstract string getName();

        /**
         * Gets the spatial reference system (SRS) of the geodata in the
         * store. This may be NULL is no SRS is specified.
         *
         * @return A spatial reference system
         */
        public abstract SpatialReference getSRS();

        /**
         * Gets the total number of features in the backing store.
         *
         * @return An integer count, or -1 if it cannot be determined
         */
        public abstract int getFeatureCount();

        /**
         * Gets the bounding rectangle containing all the geodata
         * in the feature store.
         *
         * @return A spatial extent containing all features
         */
        public abstract GeoExtent getExtent();

        /**
         * Gets an individual feature from the store.
         * 
         * @param oid
         *      Primary key (unique identifier) of the feature to get
         * @return
         *      Feature with the specified OID, or NULL if it does not exist
         */
        public abstract Feature getFeature(FeatureOID oid);

        /**
         * Creates a cursor that will iterator over ALL the features in
         * the backing store, in no particular order.
         *
         * @return A cursor for iterating over search results
         */
        public abstract FeatureCursor getCursor();

        /**
         * Writes a feature to the feature store. The store must have been opened
         * for writing.
         *
         * @param feature
         *      Feature to insert. It must have the exact schema of the store.
         * @return
         *      True if inserting succeeded, false if it failed.
         */
        public abstract bool insertFeature(Feature feature);

        /**
         * Creates and returns a new, empty feature instance. This method does NOT
         * insert the new feature into the store (see insertFeature). The called is
         * responsible for deallocating the returned object.
         */
        public abstract Feature createFeature();

        /**
         * Returns true if the feature store supports random access to
         * feature data (i.e. whether you can call getFeature(oid)).
         */
        public abstract bool supportsRandomRead();

        /**
         * Gets the schema of each attribute in the feature store. User-defined
         * attributes added to individual features are not included.
         *
         * @return
         *      Attribute schema table
         */
        public abstract AttributeSchemaTable getAttributeSchemas();

        /**
         * Gets the modification timestamp of the feature store source data.
         *
         * @return
         *      Raw modification timestamp
         */
        public abstract DateTime getModTime();
    }
}

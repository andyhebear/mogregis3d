using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * Interface for classes that create new FeatureStore connections.
     */
    public class FeatureStoreFactory
    {

        /**
         * Connects to an existing feature store and returns a handle.
         *
         * @param uri
         *      URI of feature store to which to connect
         * @return
         *      Feature store reference. Caller is responsible for deleting
         *      the return object. Call isReady() on the result to determine 
         *      whether the connection succeeded.
         */
        public abstract FeatureStore connectToFeatureStore(string uri);

        /**
         * Creates a new feature store and returns a handle.
         *
         * @param uri
         *      Location at which to create the feature store.
         * @param shape_type
         *      Type of shapes in this feature store.
         * @param schema
         *      Attribute schema for features in this store.
         *
         * @return
         *      Connection to the new feature store. The caller is responsible
         *      for deleting the return object.
         */
        public abstract FeatureStore createFeatureStore(string uri,
                                                        GeoShape.ShapeType type,
                                                        AttributeSchemaList schema,
                                                        int dimensionality,
                                                        SpatialReference srs,
                                                        Properties props);
    }
}

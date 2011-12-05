using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * A feature layer wrapped as a Resource that filters can access
     * during filter-graph compilation.
     */
    public class FeatureLayerResource : Resource
    {
        //TODO  OSGGIS_META_RESOURCE(FeatureLayerResource);


        /**
         * Construct a new empty feature layer resource.
         */
        public FeatureLayerResource()
        {
            init();
        }

        /**
         * Constructs a new feature layer resource that wraps a named feature layer.
         *
         * @param name
         *      Name of feature layer to wrap
         */
        public FeatureLayerResource(string _name)
            : base(_name)
        {
            init();
        }

        /**
         * Gets the feature layer associated with this resource.
         *
         * @return The feature layer, or NULL if a connection cannot be made
         */
        public FeatureLayer getFeatureLayer()
        {
            //TODO ScopedLock<ReentrantMutex> sl( getMutex() );

            if (feature_layer == null)
            {
                feature_layer = Registry.instance().createFeatureLayer(getAbsoluteURI());
            }

            return feature_layer;
        }

        public virtual void setProperty(Property prop)
        {
            base.setProperty(prop);
        }
        public virtual Properties getProperties()
        {
            Properties props = base.getProperties();
            return props;
        }



        private void init()
        {
            //NOP
        }
        private FeatureLayer feature_layer;
    }

    public class FeatureLayerResourceVec : List<FeatureLayerResource> { }
}

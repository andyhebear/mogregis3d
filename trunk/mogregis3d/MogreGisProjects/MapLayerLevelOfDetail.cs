using System;
using System.Collections.Generic;
#if TODO
using MogreGis;

namespace osgGISProjects
{
    /* (internal)
     * A single level of detail for a MapLayer.
     */
    public class MapLayerLevelOfDetail
    {

        public MapLayerLevelOfDetail(
            FeatureLayer _layer,
            FilterGraph _graph,
            Properties _env_props,
            ResourcePackager _packager,
            float _min_range,
            float _max_range,
            bool _replace_previous,
            uint _depth,
            Object _user_data)
        {
            layer = _layer;
            graph = _graph;
            env_props = _env_props;
            packager = _packager;
            min_range = _min_range;
            max_range = _max_range;
            replace_previous = _replace_previous;
            depth = _depth;
            user_data = _user_data;
        }


        public FeatureLayer getFeatureLayer()
        {
            return layer;
        }

        public FilterGraph getFilterGraph()
        {
            return graph;
        }

        public float getMinRange()
        {
            return min_range;
        }

        public float getMaxRange()
        {
            return max_range;
        }

        public bool getReplacePrevious()
        {
            return replace_previous;
        }
        public uint getDepth()
        {
            return depth;
        }

        public ResourcePackager getResourcePackager()
        {
            return packager;
        }

        public Object getUserData()
        {
            return user_data;
        }
        public Properties getEnvProperties()
        {
            return env_props;
        }


        private FeatureLayer layer;
        private FilterGraph graph;
        private Properties env_props;
        private ResourcePackager packager;
        private float min_range, max_range;
        private bool replace_previous;
        private uint depth;
        private Object user_data;
    }

    public class MapLayerLevelsOfDetail : List<MapLayerLevelOfDetail> { }
}
#endif
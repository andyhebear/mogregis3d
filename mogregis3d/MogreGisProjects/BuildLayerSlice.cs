using System;
using System.Collections.Generic;

#if TODO_DANI
using MogreGis;
#endif

namespace osgGISProjects
{
    public class BuildLayerSlice
    {

        public BuildLayerSlice()
        {
            min_range = 0.0f;
            max_range = float.MaxValue;
            max_tex_size = 0;

            sub_slices = new BuildLayerSliceList();
        }

#if TODO_DANI
        public FilterGraph getFilterGraph()
        {
            return graph;
        }

        public void setFilterGraph(FilterGraph _graph)
        {
            graph = _graph;
        }
#endif

        public Source getSource()
        {
            return source;
        }

        public void setSource(Source _source)
        {
            source = _source;
        }

        public float getMinRange()
        {
            return min_range;
        }

        public void setMinRange(float value)
        {
            min_range = value;
        }

        public float getMaxRange()
        {
            return max_range;
        }

        public void setMaxRange(float value)
        {
            max_range = value;
            if (max_range < 0) max_range = float.MaxValue;
        }


        public void setMaxTextureSize(uint value)
        {
            max_tex_size = value;
        }

        public uint getMaxTextureSize()
        {
            return max_tex_size;
        }

        public BuildLayerSliceList getSubSlices()
        {
            return sub_slices;
        }

#if TODO_DANI
        public Properties getProperties()
        {
            return props;
        }
#endif


        private Source source;
        private BuildLayerSliceList sub_slices;
        private float min_range;
        private float max_range;
        private uint max_tex_size;

#if TODO_DANI
        private FilterGraph graph;
        private Properties props;
#endif
    }

    public class BuildLayerSliceList : List<BuildLayerSlice> { };
}
//#endif
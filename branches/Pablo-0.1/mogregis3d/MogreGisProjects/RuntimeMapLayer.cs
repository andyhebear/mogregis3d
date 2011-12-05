using System;
using System.Collections.Generic;


namespace osgGISProjects
{
    public class RuntimeMapLayer
    {

      #if TODO
        public RuntimeMapLayer() { }

        public BuildLayer getBuildLayer()
        {
            return build_layer;
        }

        public void setBuildLayer(BuildLayer value)
        {
            build_layer = value;
        }

        public bool getSearchable()
        {
            return searchable;
        }

        public void setSearchable(bool value)
        {
            searchable = value;
        }

        public BuildLayer getSearchLayer()
        {
            return search_layer != null ? search_layer : getBuildLayer();
        }

        public void setSearchLayer(BuildLayer value)
        {
            search_layer = value;
        }

        public bool getVisible()
        {
            return visible;
        }

        public void setVisible(bool value)
        {
            visible = value;
        }


        private bool searchable = false;
        private bool visible = true;
        private BuildLayer build_layer;
        private BuildLayer search_layer;
#endif
    }

    public class RuntimeMapLayerList : List<RuntimeMapLayer> { } ;
}
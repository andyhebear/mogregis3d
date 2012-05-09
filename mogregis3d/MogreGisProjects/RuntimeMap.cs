using System;
using System.Collections.Generic;


namespace osgGISProjects
{
    public class RuntimeMap
    {

        /**
         * Constructs a new, empty map.
         */
        public RuntimeMap() { }

        /**
         * Sets the URI at the base of relative paths used by this map.
         */
        public void setBaseURI(string base_uri) { throw new NotImplementedException(); }

        /**
         * Gets the name of this map, unique within the current build.
         */
        public string getName()
        {
            return name;
        }

        /**
         * Sets the name of this map.
         */
        public void setName(string value)
        {
            name = value;
        }


        /**
         * Sets the terrain underlying this map.
         */
        public void setTerrain(Terrain value)
        {
            terrain = value;
        }

        /**
         * Gets the terrain underlying this map.
         */
        public Terrain getTerrain()
        {
            return terrain;
        }


        /**
         * Gets the list of layers comprising this map.
         */
        public RuntimeMapLayerList getMapLayers()
        {
            return map_layers;
        }


        private string name;

        private Terrain terrain;

        private RuntimeMapLayerList map_layers = new RuntimeMapLayerList();

    }

    public class RuntimeMapList : List<RuntimeMap> { } ;
}

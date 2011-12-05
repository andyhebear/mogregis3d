using System;
using System.Collections.Generic;
//#if TODO
namespace osgGISProjects
{
    /**
     * A build target is a unit of operation in the build process, from
     * the "user's" perspective. 
     */
    public class BuildTarget
    {

        public BuildTarget() 
        {
            layers = new BuildLayerList();
        }

        public BuildTarget(string _name)
        {
            setName(_name);
        }

        public string getName()
        {
            return name;
        }

        public void setName(string _name)
        {
            name = _name;
        }

        public Terrain getTerrain()
        {
            return terrain;
        }

        public void setTerrain(Terrain value)
        {
            terrain = value;
        }

        public void addLayer(BuildLayer _layer)
        {
            layers.Add(_layer);
        }

        public BuildLayerList getLayers()
        {
            return layers;
        }


        private string name;
        private BuildLayerList layers;
        private Terrain terrain;
    }

    public class BuildTargetList : List<BuildTarget> { };
}

//#endif
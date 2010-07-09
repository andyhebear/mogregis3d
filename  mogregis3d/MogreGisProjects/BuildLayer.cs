using System;
using System.Collections.Generic;

//#if TODO
namespace osgGISProjects
{
    public class BuildLayer
    {

        public enum LayerType
        {
            TYPE_SIMPLE,
            TYPE_GRIDDED,
            TYPE_CORRELATED,
            TYPE_QUADTREE
        };


        public BuildLayer() 
        {
            slices = new BuildLayerSliceList();
        }

        public BuildLayer(string _name)
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

        public void setBaseURI(string value)
        {
            base_uri = value;
        }

        public Source getSource()
        {
            return source;
        }

        public void setSource(Source _source)
        {
            source = _source;
        }

        public Terrain getTerrain()
        {
            return terrain;
        }

        public void setTerrain(Terrain _terrain)
        {
            terrain = _terrain;
        }

        public void setTargetPath(string value)
        {
            target_path = value;
        }

        public string getTargetPath()
        {
            return target_path;
        }

#if TODO_DANI
        public string getAbsoluteTargetPath()
        {
            return PathUtils.getAbsPath(base_uri, target_path);
        }
#endif

        public BuildLayerSliceList getSlices()
        {
            return slices;
        }

        public void setType(LayerType value)
        {
            type = value;
        }

        public LayerType getType()
        {
            return type;
        }

#if TODO_DANI
        public Properties getProperties()
        {
            return properties;
        }

        public int getPropertyAsInt(string name);

        public string getPropertyAsString(string name);

        public double getPropertyAsDouble(string name);

        // user properties that will get installed into each FilterEnv.
        public Properties getEnvProperties()
        {
            return env_properties;
        }
#endif


        private string name;
        private string base_uri;
        private BuildLayerSliceList slices;
        private Source source;
        private Terrain terrain;
        private string target_path;
        private LayerType type = LayerType.TYPE_SIMPLE;
#if TODO_DANI
        private Properties properties;
        private Properties env_properties;
#endif
    }

    public class BuildLayerList : List<BuildLayer> { } ;
}
//#endif
using System;
using System.IO;

using MogreGis;

namespace osgGISProjects
{
    public class Project
    {

        public Project()
        {
            //NOP
            sources = new SourceList();

            layers = new BuildLayerList();
            targets = new BuildTargetList();
            terrains = new TerrainList();

            graphs = new FilterGraphList();
        }

        public void setSourceURI(string value)
        {
            source_uri = value;
        }

        public string getSourceURI()
        {
            return source_uri;
        }

        public string getBaseURI()
        {
#if TODO_DANI
            return osgDB.getFilePath(source_uri);
#endif
            return Path.GetFullPath(source_uri);
        }

        public string getName()
        {
            return name;
        }

        public void setName(string value)
        {
            name = value;
        }

        public string getWorkingDirectory()
        {
            return !string.IsNullOrEmpty(work_dir) ? work_dir : ("work_" + getName());
        }

        public string getAbsoluteWorkingDirectory()
        {
            return Path.Combine(getBaseURI(), getWorkingDirectory());
        }

        public void setWorkingDirectory(string value)
        {
            work_dir = value;
        }

        public void merge(Project src)
        {
#if TODO_DANI
            getScripts().AddRange(src.getScripts());
            getFilterGraphs().AddRange(src.getFilterGraphs());
            getSources().AddRange(src.getSources());

            getTerrains().AddRange(src.getTerrains());

            getLayers().AddRange(src.getLayers());
            getTargets().AddRange(src.getTargets());
            getResources().AddRange(src.getResources());
#endif
        }

        public FilterGraphList getFilterGraphs()
        {
            return graphs;
        }

        public FilterGraph getFilterGraph(string key)
        {
            foreach (FilterGraph i in graphs)
            {
                if (i.getName() == key)
                    return i;
            }
            return null;
        }

        public SourceList getSources()
        {
            return sources;
        }


        public Source getSource(string key)
        {
            foreach (Source i in sources)
            {
                if (i.getName() == key)
                    return i;
            }
            return null;
        }
#if TODO
        public bool testSources()
        {
            foreach (Source i in getSources())
            {
                Source source = i;

                //osgGIS.notify(osg.NOTICE) << "Source: \"" << source.getName() << "\":" << std.endl;

                if (source.isIntermediate())
                {
                    // osgGIS.notify(osg.WARN) << "Source is intermediate; skipping." << std.endl;
                    continue;
                }

                object store = null;

                if (source.getType() == Source.SourceType.TYPE_FEATURE)
                {
                    store = Registry.instance().getFeatureStoreFactory().connectToFeatureStore(source.getAbsoluteURI());
                }
                else if (source.getType() == Source.SourceType.TYPE_RASTER)
                {
                    store = Registry.instance().getRasterStoreFactory().connectToRasterStore(source.getAbsoluteURI());
                }

                if (store == null)
                {
                    //osgGIS.notify(osg.WARN) << "*** FAILED TO CONNECT TO " << source.getAbsoluteURI() << std.endl;
                }

                //osgGIS.notify(osg.NOTICE) << std.endl;
            }

            return true;
        }

#if TODO_DANI
        public TerrainList getTerrains()
        {
            return terrains;
        }

        public Terrain getTerrain(string key)
        {
            foreach (Terrain i in terrains)
            {
                if (i.getName() == key)
                    return i;
            }
            return null;
        }
#endif


        public BuildLayerList getLayers()
        {
            return layers;
        }

        public BuildLayer getLayer(string key)
        {
            foreach (BuildLayer i in layers)
            {
                if (i.getName() == key)
                    return i;
            }
            return null;
        }


        public BuildTargetList getTargets()
        {
            return targets;
        }

        public BuildTarget getTarget(string key)
        {
            foreach (BuildTarget i in targets)
            {
                if (i.getName() == key)
                    return i;
            }
            return null;
        }


        public RuntimeMapList getMaps()
        {
            return maps;
        }

        public RuntimeMap getMap(string key)
        {
            foreach (RuntimeMap i in maps)
            {
                if (i.getName() == key)
                    return i;
            }
            return null;
        }


        public ResourceList getResources()
        {
            return resources;
        }

        public ScriptList getScripts()
        {
            return scripts;
        }

#endif

        public BuildLayerList getLayers()
        {
            return layers;
        }

        public BuildTargetList getTargets()
        {
            return targets;
        }

        public TerrainList getTerrains()
        {
            return terrains;
        }

        public Terrain getTerrain(string key)
        {
            foreach (Terrain i in terrains)
            {
                if (i.getName().Equals(key))
                    return i;
            }
            return null;
        }

        protected string source_uri;
        protected string name;
        protected string work_dir;

        protected SourceList sources;

        protected BuildLayerList layers;
        protected BuildTargetList targets;
        protected TerrainList terrains;

        protected FilterGraphList graphs;

#if TODO
        protected ResourceList resources;//CAMBIAR
        protected RuntimeMapList maps;
        protected ScriptList scripts;
#endif    
    }
}

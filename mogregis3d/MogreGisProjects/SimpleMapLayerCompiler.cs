using System;
#if TODO
using MogreGis;

namespace osgGISProjects
{
    /**
     * Compiles simple in-memory scene graphs from MapLayer definitions.
     */
    public class SimpleMapLayerCompiler : MapLayerCompiler
    {

        /**
         * Constructs a new compiler.
         *
         * @param map_layer
         *      Layer that we are going to compile.
         *
         * @param session
         *      Session under which to compiler the map layer
         */
        public SimpleMapLayerCompiler(MapLayer map_layer)
            : this(map_layer, null)
        { }

        public SimpleMapLayerCompiler(MapLayer map_layer, Session session) :
            base(map_layer, session) { }

        // MapLayerCompiler interface

        public virtual Profile createProfile()
        {
            return new Profile();
        }

        public virtual CellCursor createCellCursor(Profile profile)
        {
            //TODO
            return null;
        }



        protected virtual uint queueTasks(Profile profile, TaskManager task_man)
        {
            uint level = 0;
            foreach (MapLayerLevelOfDetail i in map_layer.getLevels())
            {
                MapLayerLevelOfDetail level_def = i;

                string s = level.ToString();

                FilterEnv cell_env = getSession().createFilterEnv();
                cell_env.setExtent(map_layer.getAreaOfInterest()); //GeoExtent.infinite() );
                cell_env.setTerrainNode(getTerrainNode());
                cell_env.setTerrainSRS(getTerrainSRS());
                foreach (Property prop in level_def.getEnvProperties())
                    cell_env.setProperty(prop);

                Task task = new CellCompiler(s,
                                            s,
                                            level_def.getFeatureLayer(),
                                            level_def.getFilterGraph(),
                                            level_def.getMinRange(),
                                            level_def.getMaxRange(),
                                            cell_env,
                                            null, null, null);

                task_man.queueTask(task);
            }
            return level;
        }

        protected virtual void buildIndex(Profile profile, osg.Group scene_graph)
        {
            //scene_graph = new osg.Group();
            if (lod.valid())
            {
                scene_graph.addChild(lod.get());
            }
        }

        protected virtual void processCompletedTask(CellCompiler task)
        {
            if (task.getResult().isOK() && task.getResultNode())
            {
                if (!lod.valid())
                {
                    lod = new osg.LOD();
                }

                uint key = uint.Parse(task.getName());
                MapLayerLevelOfDetail def = getLodForKey(key, getMapLayer());
                if (def != null)
                {
                    lod.addChild(task.getResultNode(), def.getMinRange(), def.getMaxRange());
                }
            }
        }



        static MapLayerLevelOfDetail getLodForKey(uint key, MapLayer map_layer)
        {
            uint level = 0;
            foreach (MapLayerLevelOfDetail i in map_layer.getLevels())
            {
                if (key == level)
                    return i;
            }
            return null;
        }

        private osg.LOD lod;
    }
}
#endif
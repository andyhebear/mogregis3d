using System;
using System.Collections.Generic;


using Mogre;

namespace MogreGis
{
    /**
     * Contains the context under which a filter processes data.
     *
     * Even though filters are meant to operate as independent units, they do need
     * access to a shared context that governs the environment in which they work. This
     * class provides that shared context.
     *
     * A FilterEnv contains information that applies across the entire FilterGraph, such as
     * a spatial extent and spatial reference systems. It also provides access to shared data
     * (like a reference terrain) and session-level services like the ResourceLibrary (for
     * accessing external resources) and a scripting engine (for running user code).
     *
     * There is exactly one FilterEnv per compiler. By definition, that means it exists in one
     * thread. References to objects above the scope of the FilterEnv (like the Session, terrain
     * node, etc.) are not necessarily thread-safe however. You can access to Session Mutex
     * to account for this if necessary.
     */
    public class FilterEnv
    {

        public FilterEnv(SceneManager sceneMgr, string name)
        {
            this.sceneMgr = sceneMgr;
            this.name = name;
            session = new Session();
           
        }

        /**
         * Constructs a new, default filter environment.
         *
         * @param session
         *      Session in which this filter environment exists.
         */
        private FilterEnv(Session _session)
        {
            session = _session;
            feature_extent = GeoExtent.infinite();
            cell_extent = GeoExtent.infinite();
            resource_cache = new ResourceCache();
            report = new Report();
        }


        /**
         * Copy constructor.
         */
        public FilterEnv(FilterEnv rhs)
        {
            session = rhs.session;
            feature_extent = rhs.feature_extent;
            cell_extent = rhs.cell_extent;
            in_srs = rhs.in_srs;
            out_srs = rhs.out_srs;
            terrain_node = rhs.terrain_node;
            terrain_srs = rhs.terrain_srs;
#if TODO
            terrain_read_cb = rhs.terrain_read_cb.get();
#endif
            script_engine = rhs.script_engine;
            properties = rhs.properties;
            optimizer_hints = rhs.optimizer_hints;
            resource_cache = rhs.resource_cache;
            report = rhs.report;
        }

        /**
         * Gets an exact copy of this instance.
         *
         * @return Exact copy of the FilterEnv
         */
        public FilterEnv clone()
        {
            FilterEnv a = new FilterEnv(this);
            return a;
        }

        /**
         * Gets a copy of the instance, copying output data to the input slots.
         * Specifically advance() will make a copy and move the source's output
         * spatial reference to be the new object's input spatial reference.
         *
         * @return A new FilterEnv
         */
        public FilterEnv advance()
        {
            FilterEnv a = clone();
            a.setInputSRS(getOutputSRS());
            a.setOutputSRS(getOutputSRS());
            return a;
        }

        /**
         * Sets the spatial bounds that a filter should consider relevant under
         * this environment.
         *
         * @param extent Working spatial extent
         */
        public void setExtent(GeoExtent extent)
        {
            feature_extent = extent;
            if (cell_extent.isInfinite())
                cell_extent = extent;
        }

        /**
         * Gets the spatial bounds that a filter should consider relevant.
         *
         * @return Working spatial extent
         */
        public GeoExtent getExtent()
        {
            return feature_extent;
        }

        /**
         * Gets the *first* spatial bounds that were set for this environment.
         *
         * @return the original working extent
         */
        public GeoExtent getCellExtent()
        {
            return cell_extent;
        }

        /**
         * Sets the spatial reference system in which the filter's input geodata
         * is expressed.
         *
         * @param SRS of input data
         */
        public void setInputSRS(SpatialReference srs)
        {
            in_srs = srs;
        }

        /**
         * Gets the spatial reference system in which the filter's input geodata
         * is expressed.
         *
         * @return SRS of input data
         */
        public SpatialReference getInputSRS()
        {
            return in_srs;
        }

        /**
         * Sets the spatial reference system in which the filter's output geodata
         * is expressed. The filter itself would call this to inform the next filter
         * that it had changed the spatial reference of the data stream. 
         *
         * For example, if you implement a transformation filter that reprojects the
         * incoming data, you must call setOutputSRS() so that the next filter knows
         * about the transformation.
         *
         * @param srs
         *      SRS of output data
         */
        public void setOutputSRS(SpatialReference srs)
        {
            out_srs = srs;
        }

        /**
         * Gets the spatial reference system in which the filter's output geodata
         * is expressed.
         *
         * @return SRS of output data
         */
        public SpatialReference getOutputSRS()
        {
            return out_srs;
        }

        /**
         * Sets a terrain scene graph that the filter should consider relevant to 
         * its operation. This only applies to filters that need to know about a
         * reference terrain (e.g., for conforming data to a terrain).
         *
         * TODO: move this up to the Session level?
         *
         * @param node
         *      Root node of the reference terrain scene graph
         */
        public void setTerrainNode(Node node)
        {
            terrain_node = node;
        }

        /**
         * Gets a terrain scene graph that the filter should consider relevant.
         * The filter would call this method to fetch a terrain if it needs one to
         * do its processing.
         *
         * @return Root node of the reference terrain scene graph
         */
        public Node getTerrainNode()
        {
            return terrain_node;
        }

        /**
         * Gets the spatial reference system of the terrain.
         *
         * @return SRS of the reference terrain
         */
        public SpatialReference getTerrainSRS()
        {
            return terrain_srs;
        }

        /**
         * Sets the spatial reference system of the terrain.
         *
         * @param srs
         *      SRS of the reference terrain
         */
        public void setTerrainSRS(SpatialReference srs)
        {
            terrain_srs = srs;
        }

#if TODO
        /**
         * Sets the read callback that terrain intersectors can share.
         *
         * @param cb
         *      Read callback for caching and paged-lod traversal
         */
        public void setTerrainReadCallback(SmartReadCallback cb)
        {
            terrain_read_cb = cb;
        }

        /** 
         * Gets the read callback that terrain intersections share.
         *
         * @return Read callback for caching and paged-lod traversal
         */
        public SmartReadCallback getTerrainReadCallback()
        {
            return terrain_read_cb;
        }
#endif

        /**
         * Gets the compiler session under which this filterenv exists.
         *
         * @return The filter environment's parent Session
         */
        public Session getSession()
        {
            return session;
        }

        /**
         * Sets the script engine
         *
         * @param engine A scripting engine
         */
        public void setScriptEngine(ScriptEngine engine)
        {
            script_engine = engine;
        }

        /**
         * Gets the scripting engine. Shortcut for getSession().getScriptEngine()
         *
         * @return A scripting engine
         */
        public ScriptEngine getScriptEngine()
        {
#if TODO_PH
            if (script_engine == null && session != null)
                script_engine = session.createScriptEngine();
#endif
            return script_engine;

            throw new NotImplementedException();
        }

        /**
         * Gets the optimizer hints - a filter can use this to control the general
         * optimizer that runs at the end (if applicable)
         *
         * @param A set of optimizer hints for this compilation
         */
        public OptimizerHints getOptimizerHints()
        {
            return optimizer_hints;
        }

        /**
         * Gets the local resource instantiation cache for this filter env.
         *
         * @return The local resource cache.
         */
        public ResourceCache getResourceCache()
        {
            return resource_cache;
        }

        /**
         * Gets the report object contained in this environment, returning NULL if
         * it does not exist.
         */
        public Report getReport()
        {
            return report;
        }

        public virtual void setProperty(Property prop)
        {
            properties.set(prop);
            //properties.push_back( prop );
        }

        public virtual Properties getProperties()
        {
            return properties;
        }

        public virtual Property getProperty(string name)
        {
            return properties.get(name);
        }


        private GeoExtent cell_extent;
        private GeoExtent feature_extent;
        private SpatialReference in_srs;
        private SpatialReference out_srs;
        private SpatialReference terrain_srs;
        private Node terrain_node;
#if TODO
        private SmartReadCallback terrain_read_cb;
#endif
        private ScriptEngine script_engine;
        private Session session;
        private Report report;
        private ResourceCache resource_cache;
        private Properties properties;
        private OptimizerHints optimizer_hints;

        public string getName()
        {
            return this.name;
        }

        public SceneManager getSceneMgr()
        {
            return this.sceneMgr;
        }

        private string name;
        private SceneManager sceneMgr;

    }
}

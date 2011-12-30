﻿#if !TODO_PH_MIRAR_RESOURCEFACTORIMAP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Mogre;

namespace MogreGis
{
    /**
     * The Registry is a package-wide singleton that stores factory objects
     * and global settings for the osgGIS library.
     *
     * All the get*() methods return a default implementation that is
     * automatically installed in the Registry. You only need to set a factory
     * implementation if you are replacing the stock factory with your own
     * custom implementation.
     */
    public class Registry
    {
        /**
         * Es un constructor statico que añade al MathTransformFilter, el BuildGeomFilter y el AreaFilter.
         * Esta comentado porque de momento no esta implementado ninguno de esos filtros, y hay que mirar
         * si no se deberia hacer como con los resources, que fueran ellos los que se fueran añadiendo
         * ellos mismos.
         */
        static Registry()
        {
#if TODO_PH
            Registry.instance().addFilterType(MathTransformFilter.getStaticFilterType(), MathTransformFilter.getFilterFactory());
            Registry.instance().addFilterType(BuildGeomFilter.getStaticFilterType(), BuildGeomFilter.getFilterFactory());
            Registry.instance().addFilterType(AreaFilter.getStaticFilterType(), AreaFilter.getFilterFactory());
#endif
            //Registry.instance().LoadAndRegistryEngine("MogreGis.IronPythonScriptEngine", "../../../PythonScriptEngine/bin/Debug/PythonScriptEngine.dll");
            
            Registry.instance().addFilterType(TransformFilter.getStaticFilterType(), TransformFilter.getFilterFactory() );
            Registry.instance().addFilterType(BuildGeomFilter.getStaticFilterType(), BuildGeomFilter.getFilterFactory());
            Registry.instance().addFilterType(AreaFilter.getStaticFilterType(), AreaFilter.getFilterFactory());
            Registry.instance().addFilterType(AttributeFilter.getStaticFilterType(), AttributeFilter.getFilterFactory());

        }

        /**
         * Gets the singleton registry instance.
         */
        public static Registry instance()
        {
            if (singleton == null)
            {
                singleton = new Registry();
            }
            return singleton;
        }


        /**
         * Gets the active SRS facotry. This is a convenience function that
         * simply calls Registry.instance().getSRSFactory().
         */
        public static SpatialReferenceFactory SRSFactory()
        {
            return Registry.instance().getSRSFactory();
        }
#if TODO_PH
        /**
         * Creates a feature layer by connecting to a feature store.
         *
         * @param uri
         *      URI of the feature store to use as the underlying data
         *      source for this layer.
         * @return
         *      A new feature layer. Caller is responsible for deleting
         *      the return object.
         */
        public FeatureLayer createFeatureLayer(string uri)
        {
            FeatureLayer result = null;
            if (getFeatureStoreFactory() != null)
            {
                FeatureStore store = getFeatureStoreFactory().connectToFeatureStore(uri);

                if (store != null && store.isReady())
                {
                    result = new FeatureLayer(store);

#if TODO
                    // if the store doesn't provide a spatial reference, try to load one from
                    // a PRJ file:
                    if (result != null && result.getSRS() == null)
                    {
                        if (osgDB.fileExists(uri)) // make sure it's a file:
                        {
                            string prj_file = osgDB.getNameLessExtension(uri) + ".prj";
                            fstream istream;
                            istream.open(prj_file.c_str());
                            if (istream.is_open())
                            {
                                istream.seekg(0, std.ios.end);
                                int length = istream.tellg();
                                istream.seekg(0, std.ios.beg);
                                char[] buf = new char[length];
                                istream.read(buf, length);
                                istream.close();
                                string prj = buf;
                                SpatialReference prj_srs =
                                 Registry.instance().getSRSFactory().createSRSfromWKT(prj);
                                result.setSRS(prj_srs);
                            }
                        }
                    }
#endif
                }
            }
            return result;
        }

#endif
        /**
         * Gets an interface for creating spatial reference systems.
         *
         * @return A spatial reference factory object
         */
        public SpatialReferenceFactory getSRSFactory()
        {
            return spatial_ref_factory;
        }

        /** 
         * Sets the interface for creating spatial reference systems.
         * You can call this to replace the default implememtation with
         * another one.
         *
         * @param factory
         *      New spatial reference factory implementation.
         */
        public void setSRSFactory(SpatialReferenceFactory _factory)
        {
            spatial_ref_factory = _factory;
        }

#if TODO   
        /**
         * Gets the interface for creating feature store connections.
         *
         * @return A feature store factory interface
         */
        public FeatureStoreFactory getFeatureStoreFactory()
        {
            return feature_store_factory;
        }

        /** 
         * Sets the interface for creating feature store connections. You
         * can call this to replace the default implementation with a
         * custom one.
         *
         * @param factory
         *      New feature store factory implementation.
         */
        public void setFeatureStoreFactory(FeatureStoreFactory factory)
        {
            feature_store_factory = factory;
        }

        /**
         * Gets the interface for creating raster store connections.
         *
         * @return A raster store factory interface
         */
        public RasterStoreFactory getRasterStoreFactory()
        {
            return raster_store_factory;
        }

        /**
         * Sets the interface for creating raster store connections. You
         * can call this to replace the default implementation with a
         * custom one.
         *
         * @param factory
         *      New raster store factory implementation.
         */
        public void setRasterStoreFactory(RasterStoreFactory factory)
        {
            raster_store_factory = factory;
        }
#endif

        /**
         * Creates a new interface for evaluating scripts.
         *
         * @return A scripting engine. Caller is responsible for deleting
         *         the return object.
         */
        public IScriptEngine createScriptEngine()
        {
            return Registry.instance().GetEngine("Python");
        }


        /**
        * Creates a new instance of a typed Filter implementation. The type must
        * have been previously registered via addFilterType().
        *
        * @param type
        *      Type name of filter to create
        * @return
        *      New filter instance. Caller is responsible for deleting the
        *      return object.
        */
        public Filter createFilterByType(string type)
        {
#if TODO 
            //provisional code
            Filter f = null;
            if (type == "MathTransform")
            {
                f = new MathTransformFilter();
            }
            return f;
#endif

            string n = normalize(type);
            FilterFactory result;
            return filter_factories.TryGetValue(n, out result) ? result.createFilter() : null;

        }

        /**
         * Adds a filter type so that createFilterByType() can be called to create
         * filter instances.
         *
         * @param type
         *      Type name of the filter factory to install
         * @param factory
         *      Factory that will create instances of the filter type
         * @return
         *      True upon success, false upon failure
         */
        public bool addFilterType(string type, FilterFactory factory)
        {
            string n = normalize(type);
            filter_factories[n] = factory;
            //TODO osgGIS.notify( osg.DEBUG_INFO ) << "osgGIS.Registry: Registered filter type " << type << std.endl;
            return true;
        }

        /**
         * Creates a new instance of a typed Resource implementation. The type must
         * have been previously registered via addResourceType().
         *
         * @param type
         *      Type name of Resource to create
         * @return
         *      New Resource instance. Caller is responsible for deleting the
         *      return object.
         */
        public Resource createResourceByType(string type)
        {
            string n = normalize(type);
            ResourceFactory result;
            return resource_factories.TryGetValue(n, out result) ? result.createResource() : null;
        }

        /**
         * Adds a Resource type so that createResourceByType() can be called to create
         * Resource instances.
         *
         * @param type
         *      Type name of the Resource factory to install
         * @param factory
         *      Factory that will create instances of the Resource type
         * @return 
         *      True upon success, false upon failure
         */
        public bool addResourceType(string type, ResourceFactory factory)
        {
            string n = normalize(type);
            resource_factories[n] = factory;
            //TODO osgGIS.notify( osg.DEBUG_INFO ) << "osgGIS.Registry: Registered resource type " << type << std.endl;
            return true;
        }


        /**
         * Sets the absolute path of a directory in which system components can
         * store temporary files.
         *
         * @param abs_path
         *      Absolute pathname of a directory
         */
        public void setWorkDirectory(string abs_path)
        {
            work_dir = abs_path;
        }

        /**
         * Gets the absolute path of a directory in which system components can
         * store temporary files.
         *
         * @return Absolute pathname of a directory
         */
        public string getWorkDirectory()
        {
            return work_dir;
        }

        /**
         * Checks whether a work directory is set.
         *
         * @return True is a work directory is set; false if not.
         */
        public bool hasWorkDirectory()
        {
            return !string.IsNullOrEmpty(work_dir);
        }
#if TODO_DANI
        /**
         * Gets a reference to the registry-global mutex. Use sparingly.
         *
         * @return a re-entrant mutex;
         */
        public ReentrantMutex getGlobalMutex()
        {
            return global_mutex;
        }

        /**
         * Gets or creates a spatial index for a given feature store.
         *
         * @return A spatial index.
         */
        //SpatialIndex* getOrCreateSpatialIndex( FeatureStore* store );

#endif
        private Registry()
        {
            setSRSFactory(new SharpMapSpatialReferenceFactory());
#if TODO_PH
            setFeatureStoreFactory(new DefaultFeatureStoreFactory());
            setRasterStoreFactory(new DefaultRasterStoreFactory());
#endif
        }


         public void LoadAndRegistryEngine(string className, string AssemblyPath)
        {
            if (String.IsNullOrEmpty(className) || String.IsNullOrEmpty(AssemblyPath))
                throw new ArgumentException();
            Assembly assembly = Assembly.LoadFrom(AssemblyPath);
            try
            {
                IScriptEngine engine = assembly.CreateInstance(className) as IScriptEngine;
                if (engine == null)
                    throw new ArgumentException(string.Format("Engine class {0} not found. Check namespace and name", className));
                RegistryScriptEngine(engine);
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException(string.Format("Engine class {0} is not an IScriptEngine.", className));
            }
        }

         public void RegistryScriptEngine(IScriptEngine engine)
         {
             if (engine == null)
                 throw new ArgumentException();

             engines[engine.Language] = engine;
         }

         public IScriptEngine GetEngine(string lang)
         {
             if (!engines.ContainsKey(lang))
                 throw new ArgumentException("Language not supported");

             return engines[lang];
         }


#if TODO_PH
        private FeatureStoreFactory feature_store_factory;
        private RasterStoreFactory raster_store_factory;
#endif
         private SpatialReferenceFactory spatial_ref_factory;

        private Dictionary<string, IScriptEngine> engines = new Dictionary<string, IScriptEngine>();

        private FilterFactoryMap filter_factories = new FilterFactoryMap();
        private ResourceFactoryMap resource_factories = new ResourceFactoryMap();
        private string work_dir;
#if TODO_DANI
        private ReentrantMutex global_mutex;
#endif

        //typedef std.map< std.string, osg.ref_ptr<SpatialIndex> > SpatialIndexCache;
        //SpatialIndexCache spatial_index_cache;

        private static Registry singleton = null;

        string normalize(string input)
        {
            return input.Replace('-', '_').ToLowerInvariant();
        }
    }
}
#endif
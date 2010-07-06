using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
        * A Session-wide container for shared Resource objects.
        * 
        * Resources define external things that a compiler can apply to features
        * (like a texture skin or a hyperlink). They are all stored in the ResourceLibrary
        * so that Filter implementations can access them during compilation.
        *
        * The ResourceLibrary lives at the Session level; one instance is shared between
        * all FilterGraphs executing under the same session.
        *
        * This class is thread-safe.
        */
    public class ResourceLibrary
    {

#if TODO
        /**
         * Constructs a new Resource Library.
         *
         * @param mutex_to_use
         *      Mutex that will protect multi-threaded access to the library.
         *      Since the library typically exists within a Session, the Session
         *      shares its mutex with the library.
         */
        public ResourceLibrary(ReentrantMutex mutex_to_use)
        {
            this.mut = mutex_to_use;
            //NOP
        }
#endif
        public ResourceLibrary(ResourceLibrary parent)
        {
            throw new NotImplementedException();
        }

        /**
         * Adds a new resource instance to the library.
         *
         * @param resource
         *      Resource to add to the library
         */
        public void addResource(Resource resource)
        {
#if TODO
            if (resource != null)
            {
                //ScopedLock<ReentrantMutex> sl( mut );

                if (resource is SkinResource)
                {
                    SkinResource skin = (SkinResource)resource;
                    skins.Add(skin);
                    //TODO osgGIS.notify( osg.INFO ) << "ResourceLibrary: added skin " << skin.getAbsoluteURI() << std::endl;
                }
                else if (resource is ModelResource)
                {
                    ModelResource model = (ModelResource)resource;
                    models.Add(model);
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: added model " << model.getAbsoluteURI() << std::endl;

                    osgDB.Registry.instance().getDataFilePathList().push_back(
                        osgDB.getFilePath(model.getAbsoluteURI()));
                }
                else if (resource is RasterResource)
                {
                    RasterResource raster = (RasterResource)resource;
                    rasters.Add(raster);
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: added raster " << raster.getAbsoluteURI() << std::endl;
                }
                else if (resource is FeatureLayerResource)
                {
                    FeatureLayerResource flr = (FeatureLayerResource)resource;
                    feature_layers.add(flr);
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: added feature layer " << flr.getAbsoluteURI() << std::endl;
                }
                else if (resource is SRSResource)
                {
                    SRSResource srsr = (SRSResource)resource;
                    srs_list.Add(srsr);
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: added SRS " << srsr.getName() << std::endl;
                }
                else if (resource is PathResource)
                {
                    PathResource pr = (PathResource)resource;
                    paths.Add(pr);
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: added path " << pr.getAbsoluteURI() << std::endl;
                }

                resourcesetMutex(mut);
            }
#endif
            throw new NotImplementedException();
        }

        /**
         * Removes a resource from the library.
         *
         * @param resource
         *      Resource to remove from the library
         */
        public void removeResource(Resource resource)
        {
#if TODO
            bool done = false;

            if (resource != null)
            {
                //TODO ScopedLock<ReentrantMutex> sl( mut );

                if (resource is SkinResource)
                {
                    if (skins.Remove((SkinResource)resource))
                    {
                        //TODO  osgGIS::notify( osg::INFO ) << "ResourceLibrary: Removed skin \"" << resource.getName() << "\"" << std::endl;
                        done = true;
                    }
                }
                else if (resource is ModelResource)
                {
                    if (models.Remove((ModelResource)resource))
                    {
                        //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: Removed model \"" << resource.getName() << "\"" << std::endl;
                        done = true;
                    }
                }
                else if (resource is RasterResource)
                {
                    if (rasters.Remove((RasterResource)resource))
                    {
                        //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: Removed raster \"" << resource.getName() << "\"" << std::endl;
                        done = true;
                    }
                }
            }
            else if (resource is FeatureLayerResource)
            {
                if (feature_layers.Remove((FeatureLayerResource)resource))
                {
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: Removed feature layer \"" << resource.getName() << "\"" << std::endl;
                    done = true;
                }
            }
            else if (resource is SRSResource)
            {
                if (srs_list.Remove((SRSResource)resource))
                {
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: Removed SRS \"" << resource.getName() << "\"" << std::endl;
                    done = true;
                }
            }
            else if (resource is PathResource)
            {
                if (paths.Remove((PathResource)resource))
                {
                    //TODO osgGIS::notify( osg::INFO ) << "ResourceLibrary: Removed Path \"" << resource.getAbsoluteURI() << "\"" << std::endl;
                    done = true;
                }
            }
#endif
            throw new NotImplementedException();
        }


        /**
         * Gets a resource by name.
         *
         * @param name
         *      Name of the resource for which to search
         * @return
         *      Matching resource, or NULL if not found
         */
        public Resource getResource(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );
#if TODO
            Resource result = null;
            result = getSkin(name);
            if (result == null) result = getModel(name);
            if (result == null) result = getRaster(name);
            if (result == null) result = getPathResource(name);
            //if ( !result && parent_lib.valid() ) result = parent_lib.getResource( name );
            return result;
#endif
            throw new NotImplementedException();
        }

#if TODO
        /**
         * Gets a skin resource by name.
         *
         * @param name
         *      Name of the skin resource for which to search
         * @return
         *      Matching skin, or NULL if not found
         */
        public SkinResource getSkin(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            foreach (SkinResource i in skins)
            {
                if (i.getName() == name)
                    return i;
            }
            return null;
        }

        /**
         * Gets a list of all skin resources in the library.
         *
         * @return List of Resources
         */
        public ResourceList getSkins()
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            ResourceList result = new ResourceList(); ;

            foreach (SkinResource i in skins)
                result.Add(i);

            return result;
        }

        /**
         * Gets a list of all skin resources in the library that match the
         * search criteria in the provided query.
         *
         * @param query
         *      Search criteria for skin resources
         * @return
         *      List of resources that match the query
         */
        public ResourceList getSkins(SkinResourceQuery query)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            ResourceList result = new ResourceList();

            foreach (SkinResource r in skins)
            {
                if (query.hasTextureHeight() && (query.getTextureHeight() < r.getMinTextureHeightMeters() || query.getTextureHeight() >= r.getMaxTextureHeightMeters()))
                    continue;

                if (query.hasMinTextureHeight() && query.getMinTextureHeight() > r.getMaxTextureHeightMeters())
                    continue;

                if (query.hasMaxTextureHeight() && query.getMaxTextureHeight() <= r.getMinTextureHeightMeters())
                    continue;

                if (query.hasRepeatsVertically() && query.getRepeatsVertically() != r.getRepeatsVertically())
                    continue;

                if (query.getTags().Count > 0 && !r.containsTags(query.getTags()))
                    continue;

                result.Add(r);
            }

            return result;
        }

        /**
         * Gets the OSG state set corresponding to a skin resource.
         * A skin resource points to an image. This method returns a state set
         * that encapsulates that image in an osg::Texture2D and wraps that
         * texture in a state set.
         *
         * @param skin
         *      Skin resource for which to get a state set
         * @return
         *      A state set containing a 2D texture
         */
        //osg::StateSet* getStateSet( SkinResource* skin );

        /**
         * Creates a new query structure that you can use to search for
         * skin resources.
         *
         * (NOTE: this method just returns an empty object on the stack. The method
         * exists solely to facitate querying from within a runtime Lua script)
         *
         * @return A query structure.
         */
        public SkinResourceQuery newSkinQuery()
        {
            return new SkinResourceQuery();
        }


        /**
         * Gets a model resource by name.
         *
         * @param name
         *      Name of the model for which to search
         * @return
         *      Matching model resource, or NULL if not found
         */
        public ModelResource getModel(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            foreach (ModelResource i in models)
            {
                if (i.getName() == name)
                    return i;
            }
            return null;
        }

        /**
         * Gets a list of all model resource in the library.
         *
         * @return List of model resources
         */
        public ResourceList getModels()
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            ResourceList result = new ResourceList();

            foreach (ModelResource i in models)
                result.Add(i);

            return result;
        }

        /**
         * Gets a list of all model resources that match a set of
         * query parameters.
         *
         * @param query
         *      Query parameters 
         * @return
         *      List of models that match the query parameters
         */
        public ResourceList getModels(ModelResourceQuery query)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            ResourceList result = new ResourceList();

            foreach (ModelResource r in models)
            {

                if (query.getTags().Count > 0 && !r.containsTags(query.getTags()))
                    continue;

                result.Add(r);
            }

            return result;
        }

        /**
         * Creates a new query structure that you can use to search for
         * model resources.
         *
         * (NOTE: this method just returns an empty object on the stack. The method
         * exists solely to facitate querying from within a runtime Lua script)
         *
         * @return A query structure.
         */
        public ModelResourceQuery newModelQuery();

        /**
         * Gets an OSG node containing the scene graph corresponding to a model
         * resource. 
         *
         * @param model
         *      Model resource for which to return a scene graph
         * @param proxy
         *      Whether to load the node as a proxy node
         * @param optimize
         *      Whether to run the osgUtil::Optimizer on the model scene graph
         *      before returning it
         * @return
         *      An osg::Node containing the scene graph
         */
        //osg::Node* getNode( ModelResource* model, bool proxy =false, bool optimize =false );

        ///**
        // * Gets an OSG node that references the external path of the model resource
        // * from an osg::ProxyNode without actually loading the scene graph of the
        // * model itselg.
        // *
        // * @param model
        // *      Model resource for which to return a scene graph
        // * @return
        // *      An osg::ProxyNode pointing to the external model
        // */
        //osg::Node* getProxyNode( ModelResource* model );

        /**
         * Gets a Raster resource by name.
         *
         * @param name
         *      Name of the raster resource for which to search
         * @return
         *      A raster resource, or NULL if not found
         */
        public RasterResource getRaster(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            foreach (RasterResource i in rasters)
            {
                if (i.getName() == name)
                    return i;
            }
            return null;
        }

        /**
         * Gets a feature layer resource by name.
         *
         * @param name
         *      Name of the feature layer resource for which to search
         * @return
         *      A feature layer, of NULL if not found
         */
        public FeatureLayer getFeatureLayer(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            foreach (FeatureLayerResource i in feature_layers)
            {
                if (i.getName() == name)
                    return i.getFeatureLayer();
            }
            return null;
        }

        /**
         * Gets a spatial reference system (SRS) resource by name
         *
         * @param name
         *      Name of the SRS resource for which to search
         * @return
         *      A spatial reference system, or NULL if not found
         */
        public SpatialReference getSRS(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            foreach (SRSResource i in srs_list)
            {
                if (i.getName() == name)
                    return i.getSRS();
            }
            return null;
        }

        /**
         * Gets a Path Resource by name
         *
         * @param name
         *      Name of the path resource for which to search
         * @return
         *      The path corresponding to the resource, or "" if not found
         */
        public string getPath(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            foreach (PathResource i in paths)
            {
                if (i.getName() == name)
                    return i.getAbsoluteURI();
            }
            return "";
        }

        /**
         * Gets a Path resource by name
         *
         * @param name
         *      Name of the path resource for which to search
         * @return
         *      The path resource, or NULL if not found
         */
        public PathResource getPathResource(string name)
        {
            //TODO ScopedLock<ReentrantMutex> sl( mut );

            foreach (PathResource i in paths)
            {
                if (i.getName() == name)
                    return i;
            }
            return null;
        }



        private SkinResources skins;
        //typedef std::map< SkinResource*, osg::ref_ptr<osg::StateSet> > SkinStateSets;
        //SkinStateSets skin_state_sets;

        private ModelResources models;
        //typedef std::map< std::string, osg::ref_ptr<osg::Node> > ModelNodes;
        //ModelNodes model_nodes;

        private RasterResourceVec rasters;
        private FeatureLayerResourceVec feature_layers;
        private SRSResourceVec srs_list;
        private PathResourceVec paths;

        private ReentrantMutex mut;

        //osg::ref_ptr<ResourceLibrary> parent_lib;
#endif
    }
}

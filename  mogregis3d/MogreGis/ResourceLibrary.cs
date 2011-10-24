using System;
using System.Collections.Generic;

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
        private static string EMPTY_STRING = "";

        private static string normalize(string input)
        {
            string output = input.ToLower();
            return output;
        }

        /**
         * Constructs a new Resource Library.
         * 
         * @param SincornizeFlag
         *      Flag that will protect multi-threaded access to the library.
         *      Since the library typically exists withing a Session, the Session
         *      shares its mutex with the library.
         */
        public ResourceLibrary(Object SincronizeFlag)
        {
            sincronizeFlag = new Object();
        }

        #region No esta implementado en osgGIS
#if NOT_IMPLEMENTED_IN_OSGGIS
        public ResourceLibrary (ResourceLibrary parent);

        /**
         * Adds a new resource instance to the library.
         * 
         * @param resource
         *      Resource to add to the library
         */
        public void addResource(Resource resource);        
#endif
        #endregion

        /**
         * Add a new resource instance to the library.
         * 
         * @param resource
         *      Resource to add to the library
         */
        public void addResource(Resource resource)
        {
            if (resource != null)
            {
                lock (sincronizeFlag)
                {
#if TODO_PH
                    if (resource is SkinResource)
                    {
                        SkinResource skin = (SkinResource)resource;
                        skins.Add(skin);
                        //osgGIS::Notify(osg::INFO)<<"ResourceLibrary: added skin" <<skin->getAbsoluteURI()<<std::endl;
                    }
                    else if (resource is ModelResource)
                    {
                        ModelResource model = (ModelResource)resource;
                        models.Add(model);
                        //osgGIS::Notify(osg::INFO)<<"ResourceLibrary: added model" <<skin->getAbsoulteURI()<<std::endl;
                        //osgDB::Registry::instance()->getDataFilePathList().push_back(osgDB::getFilePath (model->getAbsoluteURI)
                    }
                    else if (resource is RasterResource)
                    {
                        RasterResource raster = (RasterResource)resource;
                        rasters.Add(raster);
                        //osgGIS::notify(osg::INFO)<<"ResourceLibrary: added raster"<<raster->getAbsoluteURI()<<std::endl;
                    }
                    else if (resource is FeatureLayerResource)
                    {
                        FeatureLayerResource flr = (FeatureLayerResource)resource;
                        featureLayers.Add(flr);
                        //osgGIS::notify(osg::INFO)<<"ResourceLibrary: added feature layer "<<flr->getAbsoluteURI()<<std::endl;
                    }
                    else
#endif
                    if (resource is SRSResource)
                    {
                        SRSResource srsr = (SRSResource)resource;
                        srsList.Add(srsr);
                        //osgGIS::notify(osg::INFO)<<"ResourceLibrary: added SRS" << srsr->getName()<<std::endl;
                    }
#if TODO_PH
                    else if (resource is PathResource)
                    {
                        PathResource pr = (PathResource)resource;
                        paths.Add(pr);
                        //osgGIS::notify(osg::INFO)<<"ResourceLibrary: added path" << pr->getAbsoluteURI()<<std::endl;
                    }
#endif
                    resource.SincronizedFlag = sincronizeFlag;
                }
            }
        }

        /**
        * Removes a resource from the library.
        * 
        * @param resource
        *      Resource to remove from the library
        */
        public void removeResource(Resource resource)
        {
            bool done = false;

            if (resource != null)
            {
                lock (sincronizeFlag)
                {
#if TODO_PH
                     if (resource is SkinResource)
                     {
                         done = skins.Remove((SkinResource)resource);
                         //osgGIS::notify(osg::INFO)<<"ResourceLibrary: Removed skin \""<<resource->getName()<<"\""<<std::endl;
                     }
                     else if (resource is ModelResource)
                     {
                         done = models.Remove((ModelResource)resource);
                         //osgGIS::notify(osg::INFO)<<"ResourceLibrary: Removed model\""<<resource->getName()<<"\""<<std::endl;
                     }
                     else if (resource is RasterResource)
                     {
                         done = rasters.Remove((RasterResource)resource);
                         //osgGIS::notify(osg::INFO)<<"ResourceLibrary: Removed raster \""<<resource->getName()<<"\""<<std::endl;
                     }
                     else if (resource is FeatureLayerResource)
                     {
                         done = featureLayers.Remove((FeatureLayerResource)resource);
                         //osgGIS::notify(osg::INFO)<<"ResourceLibrary: Removed feature layer \""<<resource->getName()<<"\""<<std::endl;
                     }
                     else
#endif
                    if (resource is SRSResource)
                    {
                        done = srsList.Remove((SRSResource)resource);
                        //osgGIS::notify(OSG::INFO)<<"ResourceLibrary: Removed SRS \"" << resource->getName()<<"\""<<std::endl;
                    }
#if TODO_PH
                     else if (resource is PathResource)
                     {
                         done = paths.Remove((PathResource)resource);
                         //osgGIS::notify(OSG::INFO)<<"ResourceLibrary: Removed Path\""<<resource->getName()<<"\""<<std::endl;
                     }
#endif
                }
            }
        }

        #region TODO Pablo Hernandez
#if TODO_PH
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
            lock (sincronizeFlag)
            {
                Resource result;
                result = getSkin(name);
                if (result == null)
                {
                    result = getModel(name);
                }
                if (result == null)
                {
                    result = getRaster(name);
                }
                if (result == null)
                {
                    result = getPathResource(name);
                }
                return result;
            }
        }

        /**
         * Gets a sking resource by name.
         * 
         * @param
         *      name of the skin resource for which to search
         * @return
         *      Matchin skin, or NULL if not found
         */
        public SkinResource getSkin(String name)
        {
            lock (sincronizeFlag)
            {
                foreach(SkinResource skin in skins)
                {
                    if (skin.Name == name)
                    {
                        return skin;
                    }
                }
                return null;
            }
        }

        /**
         * Gets a list of all skin resources in the library.
         * 
         * @return List of resources
         */
        public ResourceList getSkins()
        {
            lock (sincronizeFlag)
            {
                ResourceList result = new ResourceList();
                foreach (SkinResource skin in skins)
                {
                    result.Add(skin);
                }
                return result;
            }
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
            lock (sincronizeFlag)
            {
                ResourceList result = new ResourceList();

                foreach (SkinResource skin in skins)
                {
                    if (query.hasTextureHeight() && (query.getTextureHeight() < skin.getMinTextureHeightMeters()
                        || query.getTextureHeight() >= skin.getMaxTextureHeightMeters()))
                    {
                        continue;
                    }
                    if (query.hasMinTextureHeight() && query.getMinTextureHeight() > skin.getMinTextureHeightMeters())
                    {
                        continue;
                    }
                    if (query.hasMaxTextureHeight() && query.getMaxTextureHeight() <= skin.getMinTextureHeightMeters())
                    {
                        continue;
                    }
                    if (query.hasRepeatsVertically() && query.getRepeatsVertically() != skin.getRepeatsVertically())
                    {
                        continue;
                    }
                    if (query.getTags().Count > 0 && skin.containsTags(query.getTags()))
                    {
                        continue;
                    }
                    result.Add(skin);
                }
                return result;
            }
        }

        /**
         * Creates a new query structure that you can use to search for
         * skin resources.
         * (NOTE: this method just returns an empty object on the stack. The method
         * exists solely to facitate querying from within a runtime Lua script)
         * 
         * @return A query structure.
         */
        public SkinResourceQuery newSkinQuery();

        /**
         * Gets a model resource by name.
         * 
         * @param name
         *      Name of the model for which to search
         * @Return
         *      Matching model resource, or NULL if not found
         */
        public ModelResource getModel(string name);

        /**
         * Gets a list of all model resource in the library.
         * 
         * @Return List of model resources.
         */
        public ResourceList getModels();

        /**
         * Gets a list of all model resources that mach a set of 
         * query parameters.
         * 
         * @param query
         *      Query parameters
         * @return
         *      List of models that match the query parameters
         */
        public ResourceList getModels(ModelResourceQuery query);

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
         * Gets a Raster resource by name.
         * 
         * @param name
         *      Name of the raster resource for which to search
         * @return
         *      A raster resource, or NULL if not found
         */
        //public RasterResource getRaster(string name);

        /**
         * Gets a feature layer resource by name.
         * 
         * @param name
         *      Name of the feature layer resource for which to search
         * @return
         *      A feature layer, of NUYLL if not found
         */
        public FeatureLayer getFeatureLayer(string name);
#endif
        #endregion

        /**
         * Gets a spatial reference system (SRS) resoruce by name
         * 
         * @param name
         *      Name of the SRS resource for which to search
         * @return 
         *      a spatial reference system, or NULL if not found
         */
        public SpatialReference getSRS(string name)
        {
            lock (sincronizeFlag)
            {
                foreach (SRSResource srsResource in srsList)
                {
                    if (srsResource.Name == name)
                    {
                        return srsResource.getSRS();
                    }
                }
                return null;
            }
        }

        #region TODO Pablo Hernandez
#if TODO_PH
        /**
         * Gets a Path Resource by name
         * 
         * @param name
         *      Name of the path resource for which to search
         * @return
         *      The path corresponding to the resource, or "" if not found
         */
        public string getPath(string name);

        /**
         * Gets a Path resource by name
         * 
         * @param name
         *      Name of the path resource for which to search
         * @return
         *      The path resource, or NULL if not found
         */
        public PathResource getPathResource(string name);
#endif
        #endregion

        #region ATRIBUTOS
        private SRSResourceVec srsList;
        private Object sincronizeFlag;

        #region TODO Pablo Hernandez
#if TODO_PH
        private SkinResources skins;
        private ModelResources models;
        //private RasterResourceVec rasters;
        private FeatureLayerResourceVec featureLayers;
        private PathResourceVec paths;
#endif
        #endregion

        #endregion
    }
}
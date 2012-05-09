using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
namespace MogreGis
{
    /**
     * Base class for feature layer compilers.
     */
    public class LayerCompiler
    {

        /**
         * Constructs a new compiler.
         */
        protected LayerCompiler()
        {
            read_cb = new SmartReadCallback();
            render_order = -1;
            fade_lods = false;
            overlay = false;
            aoi_xmin = double.MaxValue;
            aoi_ymin = double.MaxValue;
            aoi_xmax = double.MinValue;
            aoi_ymax = double.MinValue;
            localize_resources = true;
            compress_textures = false;
        }


        public struct FilterGraphRange
        {
            public float min_range, max_range;
            public FilterGraph graph;
        }

        public class FilterGraphRangeList : List<FilterGraphRange> { }


        /**
         * Adds a graph that should be used to build geometry within
         * the specified visibility range.
         *
         * @param min_range
         *      Minimum visibility range - get any closer and the geometry 
         *      disappeares.
         * @param max_range
         *      Maximum visibility range - get any further and the geometry
         *      disappears.
         * @param graph
         *      Compile geometry using this graph for the given range.
         */
        public void addFilterGraph(float min_range, float max_range, FilterGraph graph)
        {
            FilterGraphRange slice;
            slice.min_range = min_range;
            slice.max_range = max_range;
            slice.graph = graph;

            graph_ranges.Add(slice);
        }

        /**
         * Gets the list of graphs and the visibility range associated 
         * with each one.
         */
        public FilterGraphRangeList getFilterGraphs()
        {
            return graph_ranges;
        }


        /** 
         * Sets the reference terrain against which to compile.
         */
        public void setTerrain(Node _terrain, SpatialReference _terrain_srs, GeoExtent _terrain_extent)
        {
            terrain = _terrain;
            terrain_srs = (SpatialReference)_terrain_srs;
            terrain_extent = _terrain_extent;
        }

        /** 
         * Sets the reference terrain against which to compile.
         */
        public void setTerrain(Node _terrain, SpatialReference _terrain_srs)
        {
            setTerrain(_terrain, _terrain_srs, GeoExtent.infinite());
        }


        /**
         * Gets the scene graph holding the reference terrain, if set.
         */
        public Node getTerrainNode()
        {
            return terrain;
        }

        /**
         * Gets the spatial reference system of the reference terrain, if set.
         */
        public SpatialReference getTerrainSRS()
        {
            return terrain_srs;
        }

        /**
         * Gets the geospatial extents of the reference terrain, if set.
         */
        public GeoExtent getTerrainExtent()
        {
            return terrain_extent;
        }

        /**
         * Sets the archive to which the compiler should write files,
         * if applicable.
         */
        public void setArchive(osgDB.Archive _archive, string _filename)
        {
            archive = _archive;
            archive_filename = _filename;
        }

        /**
         * Gets the archive to which the compiler should write files, if set.
         */
        public osgDB.Archive getArchive()
        {
            return archive;
        }

        public string getArchiveFileName()
        {
            return archive_filename;
        }

        /**
         * Sets whether to apply fading to LOD nodes.
         */
        public void setFadeLODs(bool value)
        {
            fade_lods = value;
        }

        /** 
         * Gets whether to apply fading to LOD nodes.
         */
        public bool getFadeLODs()
        {
            return fade_lods;
        }

        /**
         * Sets the render order for the layer. This will disable depth testing
         * for the output layer as well.
         * -1 means no ordered rendering.
         */
        public void setRenderOrder(int value)
        {
            render_order = value;
        }

        /**
         * Gets the render order, or -1 if none has been set.
         */
        public int getRenderOrder()
        {
            return render_order;
        }

        /**
         * Sets whether the compiled graph should be converted into an
         * overlay graph (i.e. projected onto the terrain)
         */
        public void setOverlay(bool value)
        {
            overlay = value;
        }

        /**
         * Gets whether the compiled graph should be returns as an 
         * overlay graph (i.e. projected onto the terrain)
         */
        public bool getOverlay()
        {
            return overlay;
        }

        /**
         * Assigns a task manager to use to build the layer. This will
         * enable multi-threaded layer compilation when it's possible.
         */
        public void setTaskManager(TaskManager tm)
        {
            task_manager = tm;
        }

        /**
         * Gets the task manager to use when compiling the layer.
         */
        public TaskManager getTaskManager()
        {
            return task_manager;
        }

        /**
         * Assigns a session object for this compiler to use. The session
         * tracks common data and stats across related compilations.
         */
        public void setSession(Session _session)
        {
            session = _session;
        }

        /**
         * Gets the session assigned to this compiler, or creates and
         * returns a new one if setSession() was never called.
         */
        public Session getSession()
        {
            if (session == null)
                session = new Session();

            return session;
        }


        /**
         * Sets the extent of the layer to compile. By default, the compiler
         * will just use the full extent of the feature layer's contents. You
         * can use this to override that value and specify an area of interest
         * manually.
         */
        public void setAreaOfInterest(double xmin, double ymin, double xmax, double ymax)
        {
            aoi_xmin = xmin;
            aoi_ymin = ymin;
            aoi_xmax = xmax;
            aoi_ymax = ymax;
        }

        /**
         * Gets the layer extent that this compiler will use. If this is set
         * to GeoExtent.Infinite, that compiler will query the feature layer
         * for its full extent and use that as the AOI.
         */
        public GeoExtent getAreaOfInterest(FeatureLayer layer)
        {
            if (aoi_xmin < aoi_xmax && aoi_ymin < aoi_ymax)
            {
                return new GeoExtent(aoi_xmin, aoi_ymin, aoi_xmax, aoi_ymax, layer.getSRS());
            }
            else
            {
                return layer.getExtent();
            }
        }

        public void setLocalizeResources(bool value)
        {
            localize_resources = value;
        }
        public bool getLocalizeResources()
        {
            return localize_resources;
        }

        /**
         * Sets whether to compress textures upon localization.
         */
        public void setCompressTextures(bool value)
        {
            compress_textures = value;
        }

        /**
         * Gets whether to compress textures upon localization.
         */
        public bool getCompressTextures()
        {
            return compress_textures;
        }


        public virtual Properties getProperties()
        {
            Properties props;
            //TODO - populate!
            return new Properties();
        }
        public virtual void setProperties(Properties input)
        {
            setFadeLODs(input.getBoolValue("fade_lods", getFadeLODs()));
            setRenderOrder(input.getIntValue("render_order", getRenderOrder()));
            setLocalizeResources(input.getBoolValue("localize_resources", getLocalizeResources()));
            setCompressTextures(input.getBoolValue("compress_textures", getCompressTextures()));

            aoi_xmin = input.getDoubleValue("aoi_xmin", double.MaxValue);
            aoi_ymin = input.getDoubleValue("aoi_ymin", double.MaxValue);
            aoi_xmax = input.getDoubleValue("aoi_xmax", double.MinValue);
            aoi_ymax = input.getDoubleValue("aoi_ymax", double.MinValue);
        }



        /* rewrites any texture image or proxy node file references so that they
           reference files we've moved inside the archive. */
        public void localizeResourceReferences(Node node)
        {


            if (node != null && getLocalizeResources())
            {
                RewriteImageVisitor v = new RewriteImageVisitor(getArchiveFileName(), getCompressTextures());
                node.accept(v);
            }
        }


        protected FilterGraphRangeList graph_ranges;
        protected Node terrain;
        protected SpatialReference terrain_srs;
        protected GeoExtent terrain_extent;
        protected SmartReadCallback read_cb;
        protected int render_order;
        protected bool fade_lods;
        protected bool overlay;
        protected osgDB.Archive archive;
        protected string archive_filename;
        protected TaskManager task_manager;
        protected Session session;
        protected double aoi_xmin, aoi_ymin, aoi_xmax, aoi_ymax;
        protected bool localize_resources;
        protected bool compress_textures;


        protected void localizeResources(string output_folder);

        protected Node convertToOverlay(Node input)
        {
            osgSim.OverlayNode o_node = new osgSim.OverlayNode();
            o_node.getOrCreateStateSet().setTextureAttribute(1, new osg.TexEnv(osg.TexEnv.DECAL));
            o_node.setOverlaySubgraph(input);
            o_node.setOverlayTextureSizeHint(1024);
            return o_node;
        }

        void LayerCompiler.localizeResources(string output_folder)
        {
            // collect the resources marked as used.
            ResourceList resources_to_localize = getSession().getResourcesUsed(true);


            osgDB.ReaderWriter.Options local_options = new osgDB.ReaderWriter.Options();

            foreach (Resource i in resources_to_localize)
            {
                Resource resource = i;

                // if we are localizing resources, attempt to copy each one to the output folder:
                if (getLocalizeResources())
                {
                    if (resource is SkinResource)
                    {
                        SkinResource skin = (SkinResource)resource;

                        /******/
                        Image image = null; //skin.getImage();
                        if (image != null)
                        {
                            string filename = osgDB.getSimpleFileName(image.getFileName());

                            Image output_image = image.get();
                            if (getCompressTextures())
                            {
                                output_image = ImageUtils.convertRGBAtoDDS(image.get());
                                filename = osgDB.getNameLessExtension(filename) + ".dds";
                                output_image.setFileName(filename);
                            }

                            if (getArchive() && !getArchive().fileExists(filename))
                            {
                                osgDB.ReaderWriter.WriteResult r = getArchive().writeImage(*(output_image.get()), filename, local_options.get());
                                if (r.error())
                                {
                                    //TODO osgGIS.notify( osg.WARN ) << "  Failure to copy image " << filename << " into the archive" << std.endl;
                                }
                            }
                            else
                            {
                                if (osgDB.fileExists(output_folder))
                                {
                                    if (!osgDB.writeImageFile(*(output_image.get()), PathUtils.combinePaths(output_folder, filename), local_options.get()))
                                    {
                                        //TODO  osgGIS.notify( osg.WARN ) << "  FAILED to copy image " << filename << " into the folder " << output_folder << std.endl;
                                    }
                                }
                                else
                                {
                                    //TODO osgGIS.notify( osg.WARN ) << "  FAILD to localize image " << filename << ", folder " << output_folder << " not found" << std.endl;
                                }
                            }
                        }
                    }

                    else if (resource is ModelResource)
                    {
                        ModelResource model = (ModelResource)resource;

                        osg.Node node = osgDB.readNodeFile(model.getAbsoluteURI());
                        if (node.valid())
                        {
                            string filename = osgDB.getSimpleFileName(model.getAbsoluteURI());
                            if (getArchive() != null)
                            {
                                osgDB.ReaderWriter.WriteResult r = getArchive().writeNode(*(node.get()), filename, local_options.get());
                                if (r.error())
                                {
                                    //TODO osgGIS.notify( osg.WARN ) << "  Failure to copy model " << filename << " into the archive" << std.endl;
                                }
                            }
                            else
                            {
                                if (osgDB.fileExists(output_folder))
                                {
                                    if (!osgDB.writeNodeFile(*(node.get()), osgDB.concatPaths(output_folder, filename), local_options.get()))
                                    {
                                        //TODO osgGIS.notify( osg.WARN ) << "  FAILED to copy model " << filename << " into the folder " << output_folder << std.endl;
                                    }
                                }
                                else
                                {
                                    //TODO osgGIS.notify( osg.WARN ) << "  FAILD to localize model " << filename << ", folder " << output_folder << " not found" << std.endl;
                                }
                            }
                        }
                    }
                }

                // now remove any single-use (i.e. non-shared) resources (whether we are localizing them or not)
                if (resource.isSingleUse())
                {
                    getSession().getResources().removeResource(resource);
                }
            }
        }
        internal class RewriteImageVisitor : osg.NodeVisitor
        {
            public RewriteImageVisitor(string _archive_name, bool _compress_textures) :
                base(osg.NodeVisitor.TRAVERSE_ALL_CHILDREN)
            {
                archive_name = osgDB.getSimpleFileName(_archive_name);
                compress_textures = _compress_textures;
                //TODO osgGIS.notify( osg.INFO ) << "LayerCompiler: Localizing resources references" << std.endl;
            }

            string archive_name;
            bool compress_textures;

            virtual void apply(osg.Node node)
            {
                osg.StateSet ss = node.getStateSet();
                if (ss) rewrite(ss);
                osg.NodeVisitor.apply(node);
            }

            virtual void apply(osg.Geode geode)
            {
                for (int i = 0; i < geode.getNumDrawables(); i++)
                {
                    osg.StateSet ss = geode.getDrawable(i).getStateSet();
                    if (ss) rewrite(ss);
                    osg.NodeVisitor.apply(geode);
                }
            }

            virtual void apply(osg.ProxyNode proxy)
            {
                proxy.setDatabasePath(archive_name);
                string name = proxy.getFileName(0);
                string simple = osgDB.getSimpleFileName(name);
                proxy.setFileName(0, simple);
                //TODO osgGIS.notify( osg.INFO ) << "  Rewrote " << name << " as " << simple << std.endl;
                osg.NodeVisitor.apply(proxy);
            }

            void rewrite(osg.StateSet* ss)
            {
                for (int i = 0; i < ss.getTextureAttributeList().Count; i++)
                {
                    osg.Texture2D tex = (osg.Texture2D)(ss.getTextureAttribute(i, osg.StateAttribute.TEXTURE));
                    if (tex && tex.getImage())
                    {
                        string name = tex.getImage().getFileName();

                        // fix the in-archive reference:
                        //if ( archive_name.length() > 0 )
                        //{
                        //    if ( !StringUtils.startsWith( name, archive_name ) )
                        //    {
                        //        std.string path = osgDB.concatPaths( archive_name, tex.getImage().getFileName() );
                        //        tex.getImage().setFileName( path );
                        //        osgGIS.notify(osg.INFO) << "  Rewrote " << name << " as " << path << std.endl;
                        //    }
                        //}
                        //else
                        {
                            string simple = osgDB.getSimpleFileName(name);

                            if (compress_textures)
                            {
                                simple = osgDB.getNameLessExtension(simple) + ".dds";
                            }

                            tex.getImage().setFileName(simple);
                            //TODO osgGIS.notify( osg.INFO ) << "  LayerCompiler.localizeResourceRefs, Rewrote " << name << " as " << simple << std.endl;
                        }
                    }
                }
            }
        }
    }
}

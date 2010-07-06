using System;
using System.IO;
#if TODO
using MogreGis;

namespace osgGISProjects
{
    public class Profile
    {
        public Profile() { }
    }


    public class CompileSession
    {

        public abstract Task getNextCompletedTask();

        public abstract double getElapsedTimeSeconds();

        public abstract double getPredicitedRemainingTimeSeconds();

        public abstract osg.Group getOrCreateSceneGraph();
    }


    /**
     * A callback that returns a value indicating whether to compile
     * the cell in question.
     */
    public class CellSelector
    {
        public abstract bool selectCell(Cell cell); //const std.string& cell_id ) const =0; 
    }


    /**
     * Compiles scene graphs from MapLayer definitions.
     */
    public class MapLayerCompiler
    {
        /**
         * Constructs a new map layer compiler.
         *
         * @param map_layer
         *      Layer that we are going to compile.
         *
         * @param session
         *      Session under which to compiler the map layer
         */
        public MapLayerCompiler(MapLayer map_layer)
            : this(map_layer, null)
        {

        }
        public MapLayerCompiler(MapLayer _layer, Session _session)
        {
            map_layer = _layer;
            session = _session;
            paged = true;
            depth_first = true;
        }

        /**
         * Gets the map layer that this compiler is meant to compile.
         *
         * @return Map layer.
         */
        public MapLayer getMapLayer()
        {
            return map_layer;
        }

        /**
         * Sets whether the map layer is to be compiled into paged geometry.
         *
         * @param paged
         *      True to compile into paged cells, false for non-paged cells.
         */
        public void setPaged(bool value)
        {
            if (paged != value)
            {
                paged = value;
            }
        }

        /**
         * Gets whether to build a paged dataset.
         *
         * @return True if the map cells will be paged; false if not
         */
        public bool getPaged()
        {
            return paged;
        }

        /**
         * Sets the absolute URI or pathname of the compiler output. This value will be
         * used as a template for creating output names for paged data cells as well as
         * localized resource files (textures, external models).
         *
         * This value is required if either the Paged of the CopyResourcesToOutputLocation
         * property is set to true. 
         *
         * @param uri
         *      Absolute URI/pathname
         */
        public void setAbsoluteOutputURI(string value)
        {
            output_uri = value;
        }

        /**
         * Assigns an optional resource packager to the compiler. The resource packager will
         * prepare referenced resources (like texture skins and external models) and copy them
         * to the output location.
         *
         * @param packager
         *      Packager to use with this compiler.
         */
        public void setResourcePackager(ResourcePackager value)
        {
            resource_packager = value;
        }

        /**
         * Gets the packager that will deploy resources and nodes for this compiler.
         */
        public ResourcePackager getResourcePackager()
        {
            return resource_packager;
        }

        /** 
         * Sets the reference terrain against which to compile.
         */
        //public  void setTerrain(
        //    osg.Node               terrain, 
        //      SpatialReference  terrain_srs =NULL,
        //      GeoExtent         terrain_extent =GeoExtent.infinite() )
        public void setTerrain(osg.Node _terrain,
                              SpatialReference _terrain_srs,
                              GeoExtent _terrain_extent)
        {
            terrain_node = _terrain;
            terrain_srs = (SpatialReference)_terrain_srs;
            terrain_extent = _terrain_extent;

            if (terrain_srs == null)
                terrain_srs = MogreGis.Registry.SRSFactory().createSRSfromTerrain(terrain_node.get());

            //if ( !terrain_srs.valid() )
            //    osgGIS.warn() << "[MapLayerCompiler] WARNING: cannot determine SRS of terrain!" << std.endl;
        }


        /**
         * Gets the scene graph holding the reference terrain, if set.
         */
        public osg.Node getTerrainNode()
        {
            return terrain_node;
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
         * Gets the session under which this compiler will operate.
         *
         * @return A session
         */
        public Session getSession()
        {
            if (session == null)
            {
                session = new Session();
            }
            return session;
        }


        /**
         * Gets the spatial reference system of the output data that this
         * compiler will generate.
         *
         * @return A spatial reference, or NULL if it cannot be determined.
         */
        public SpatialReference getOutputSRS()
        {
            return map_layer.getOutputSRS(getSession(), getTerrainSRS());
        }

        /**
         * Sets a callback that the compiler will use to determine whether
         * to queue a particular cell for compilation.
         *
         * @param selector
         *      Selector callback
         */
        public void setCellSelector(CellSelector _selector)
        {
            cell_selector = _selector;
        }

        /**
         * For multi-level map layers, whether to build all LODs in a cell before
         * moving on to the next cell.
         *
         * @param value
         *       True to build map layer levels depth-first; false to build
         *      map layers breadth first (i.e. build all level 0 cells before moving
         *      on to level 1.
         */
        public void setBuildDepthFirst(bool value)
        {
            depth_first = value;
        }


        /**
         * Compiles the entire cell graph.
         *
         * @param task_man
         *      Task manager to employ for parallel/distributed compilation
         *
         * @return
         *      True if the compilation succeeded, false if it failed.
         */
        //public  virtual osg.Group   compile( TaskManager  task_man =NULL)
        public virtual osg.Group compile(TaskManager my_task_man)
        {
            CompileSession cs = (CompileSessionImpl)startCompiling(my_task_man);

            while (continueCompiling(cs)) ;

            if (finishCompiling(cs))
            {
                osg.Group result = cs.getOrCreateSceneGraph();
                //result.ref(); // since the CS will be destroyed, unref'ing the result
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual osg.Group compileIndexOnly(CompileSession cs_interface)
        {
            CompileSessionImpl cs = (CompileSessionImpl)cs_interface;

            // make a profile describing this compilation setup:
            Profile profile = createProfile();

            buildIndex(profile, cs.getOrCreateSceneGraph());

            if (cs.getOrCreateSceneGraph() != null)
            {
                osgUtil.Optimizer opt;
                opt.optimize(cs.getOrCreateSceneGraph(),
                    osgUtil.Optimizer.SPATIALIZE_GROUPS |
                    osgUtil.Optimizer.STATIC_OBJECT_DETECTION |
                    osgUtil.Optimizer.SHARE_DUPLICATE_STATE);
            }

            return cs.getOrCreateSceneGraph();
        }



        //public  virtual CompileSession  startCompiling( TaskManager  task_man =NULL )
        public virtual CompileSession startCompiling(TaskManager my_task_man)
        {
            // make a task manager to run the compilation:
            TaskManager task_man = my_task_man;
            if (task_man == null)
                task_man = new TaskManager(0);

            // make a profile describing this compilation setup:
            Profile profile = createProfile();

            // create and queue up all the tasks to run:
            uint total_tasks = queueTasks(profile, task_man.get());

            // configure the packager so that skins and model end up in the right place:
            if (resource_packager != null)
            {
                resource_packager.setArchive(getArchive());
                resource_packager.setOutputLocation(osgDB.getFilePath(output_uri));
            }

            // pre-gen the spatial indexes (optional, but saves a little time)
            foreach (MapLayerLevelOfDetail i in map_layer.getLevels())
            {
                i.getFeatureLayer().assertSpatialIndex();
            }

            // make a reporting channel:
            Report default_report = new Report();

            // create and return a compile session that should be sent to continueCompiling()
            // and finishCompiling().
            return new CompileSessionImpl(task_man, profile, total_tasks, osg.Timer.instance().tick());
        }

        public virtual bool continueCompiling(CompileSession cs_interface)
        {
            CompileSessionImpl cs = (CompileSessionImpl)cs_interface;

            cs.clearTaskQueue();

            if (cs.getTaskManager().wait(1000L))
            {
                Task completed_task = cs.getTaskManager().getNextCompletedTask();
                if (completed_task != null)
                {
                    CellCompiler cell_compiler = (CellCompiler)completed_task;
                    if (cell_compiler.isInExceptionState())
                    {
                        //TODO: replace this with Report facility
                        //osgGIS.notify( osg.WARN ) << "ERROR: cell failed; unhandled exception state."
                        //    << std.endl;
                        cs.getTaskQueue().Enqueue(cell_compiler);
                    }
                    else if (cell_compiler.getResult().isOK())
                    {
                        cell_compiler.runSynchronousPostProcess(cs.getReport());

                        // give the layer compiler an opportunity to do something:
                        processCompletedTask(cell_compiler);

                        // record the completed task to the caller can see it
                        cs.getTaskQueue().Enqueue(cell_compiler);

                        uint total_tasks = cs.getTotalTasks();
                        uint tasks_completed = total_tasks - cs.getTaskManager().getNumTasks();

                        float p = 100.0f * (float)tasks_completed / (float)total_tasks;
                        float elapsed = (float)cs.getElapsedTimeSeconds();
                        float avg_task_time = elapsed / (float)tasks_completed;
                        float time_remaining = ((float)total_tasks - (float)tasks_completed) * avg_task_time;

                        uint hrs, mins, secs;
                        TimeUtils.getHMSDuration(time_remaining, hrs, mins, secs);

                        //char buf[10];
                        //sprintf( buf, "%02d:%02d:%02d", hrs, mins, secs );
                        // osgGIS.notify(osg.NOTICE) << tasks_completed << "/" << total_tasks 
                        //     << " tasks (" << (int)p << "%) complete, " 
                        //     << buf << " remaining" << std.endl;
                    }
                    else
                    {
                        //TODO: replace this with Report facility
                        //osgGIS.notify( osg.WARN ) << "ERROR: compilation of cell "
                        //     << cell_compiler.getName() << " failed : "
                        //     << cell_compiler.getResult().getMessage()
                        //     << std.endl;
                    }
                }
            }

            return cs.getTaskManager().hasMoreTasks();
        }

        public virtual bool finishCompiling(CompileSession cs_interface)
        {
            CompileSessionImpl cs = (CompileSessionImpl)cs_interface;

            cs.clearTaskQueue();

            buildIndex(cs.getProfile(), cs.getOrCreateSceneGraph());

            if (cs.getOrCreateSceneGraph() != null)
            {
                osgUtil.Optimizer opt;
                opt.optimize(cs.getOrCreateSceneGraph(),
                    osgUtil.Optimizer.SPATIALIZE_GROUPS |
                    osgUtil.Optimizer.STATIC_OBJECT_DETECTION |
                    osgUtil.Optimizer.SHARE_DUPLICATE_STATE);
            }

            //osgGIS.notify( osg.NOTICE )
            //    << "Compilation finished, total time = " << cs.getElapsedTimeSeconds() << " seconds"
            //    << std.endl;

            return true;
        }


        public virtual Profile createProfile();

        /**
         * Creates a cursor that you can use to iterate over all the cells comprising
         * the map layer under the specified profile.
         *
         * @return
         *      A cell cursor. Caller is responsible for deleting the return object.
         */
        public virtual CellCursor createCellCursor(Profile profile);

        public string createAbsPathFromTemplate(string core)
        {
            return Path.Combine(osgDB.getFilePath(output_uri),
                core + "." + osgDB.getLowerCaseFileExtension(output_uri));
        }

        public string createRelPathFromTemplate(string core)
        {
            return core + "." + osgDB.getLowerCaseFileExtension(output_uri);
        }



        //virtual void collectCells( Profile* ) =0;
        protected abstract uint queueTasks(Profile profile, TaskManager tm);
        protected abstract void buildIndex(Profile profile, osg.Group group);
        protected virtual void processCompletedTask(CellCompiler compiler) { }


        protected MapLayer map_layer;

        protected bool paged;
        protected string output_uri;
        protected ResourcePackager resource_packager;

        protected osgDB.Archiv archive;
        protected string archive_filename;

        protected osg.Node terrain_node;
        protected SpatialReference terrain_srs;
        protected GeoExtent terrain_extent;

        protected Session session;

        protected CellSelector cell_selector;
        protected bool depth_first;

        protected void setCenterAndRadius(osg.Node node, GeoExtent cell_extent, SmartReadCallback reader)
        {
            SpatialReference srs = map_layer.getOutputSRS(getSession(), getTerrainSRS());
            // first get the output srs centroid: 
            GeoPoint centroid = srs.transform(cell_extent.getCentroid());
            GeoPoint sw = srs.transform(cell_extent.getSouthwest());

            double radius = map_layer.getEncodeCellRadius() ?
                (centroid - sw).length() :
                -1.0;

            if (terrain_node.valid() && terrain_srs != null)
            {
                GeoPoint clamped;
                for (int t = 0; t < 5; t++)
                {
                    clamped = GeomUtils.clampToTerrain(centroid, terrain_node.get(), terrain_srs, reader);
                    if (!clamped.isValid())
                    {
                        // if the clamp failed, it's due to the geocentric intersection bug in which the isect
                        // fails when coplanar with a tile boundary/skirt. Fudge the centroid and try again.
                        double fudge = 0.001 * ((double)(1 + (Random.Rand() % 10)));
                        centroid.X += fudge;
                        centroid.Y -= fudge;
                        centroid.Z += fudge * fudge;
                    }
                    else
                    {
                        break;
                    }
                }

                if (!clamped.isValid())
                {
                    SpatialReference geo = srs.getGeographicSRS();
                    GeoPoint latlon = geo.transform(centroid);
                    //osgGIS.warn() << "*** UNABLE TO CLAMP CENTROID: ***" << latlon.toString() << std.endl;
                }
                else
                {
                    centroid = clamped;
                }
            }
            else
            {
                //osgGIS.warn() << "*** Failed to clamp Center/Radius for cell" << std.endl;
            }

            if (node is osg.LOD)
            {
                osg.LOD plod = (osg.LOD)node;
                plod.setCenter(centroid);
                plod.setRadius(radius);
            }
            else if (node is osg.ProxyNode)
            {
                osg.ProxyNode proxy = (osg.ProxyNode)node;
                proxy.setCenter(centroid);
                proxy.setRadius(radius);
            }
        }
    }

    internal class CompileSessionImpl : CompileSession
    {

        public Task getNextCompletedTask()
        {
            Task next = null;
            if (recently_completed_tasks.Count > 0)
            {
                next = recently_completed_tasks.Dequeue();
            }
            return next;
        }

        public double getElapsedTimeSeconds()
        {
            osg.Timer_t now = osg.Timer.instance().tick();
            return osg.Timer.instance().delta_s(start_time, now);
        }

        public double getPredicitedRemainingTimeSeconds()
        {
            return 0.0;
        }

        public osg.Group getOrCreateSceneGraph()
        {
            if (!scene_graph.valid())
            {
                scene_graph = new osg.Group();
            }
            return scene_graph.get();
        }


        public CompileSessionImpl(TaskManager _task_man,
                            Profile _profile,
                            uint _total_tasks,
                            osg.Timer_t _start_time)
        {
            task_man = _task_man;
            profile = _profile;
            total_tasks = _total_tasks; start_time = _start_time;
            //NOP
        }

        public void clearTaskQueue()
        {
            while (recently_completed_tasks.size() > 0)
                recently_completed_tasks.pop();
        }

        public TaskQueue getTaskQueue()
        {
            return recently_completed_tasks;
        }

        public TaskManager getTaskManager()
        {
            return task_man.get();
        }

        public uint getTotalTasks()
        {
            return total_tasks;
        }

        public Report getReport()
        {
            //if ( !report.valid() )
            //    report = new Report();
            return report;
        }

        public Profile getProfile()
        {
            return profile;
        }


        private TaskManager task_man;
        private osg.Timer_t start_time;
        private uint total_tasks;
        private osg.Timer_t current_time;
        private TaskQueue recently_completed_tasks;
        private osg.Group scene_graph;
        private Report report;
        private Profile profile;
    }
}
#endif
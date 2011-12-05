using System;
#if TODO
using MogreGis;

namespace osgGISProjects
{
    /**
     * Compiles quad-tree scene graphs from MapLayer definitions.
     */
    public class QuadTreeMapLayerCompiler : MapLayerCompiler
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
        public QuadTreeMapLayerCompiler(MapLayer map_layer) : this(map_layer, null) { }

        public QuadTreeMapLayerCompiler(MapLayer _layer, Session _session)
            : base(_layer, _session)
        {
            //NOP
        }


        // MapLayerCompiler interface

        public virtual Profile createProfile()
        {
            // determine the output SRS:
            SpatialReference out_srs = map_layer.getOutputSRS(getSession(), terrain_srs);
            if (out_srs == null)
            {
                //osgGIS.warn() << "Unable to figure out the output SRS; aborting." << std.endl;
                return null;
            }

            // figure out the bounds of the compilation area and create a Q map. We want a sqaure AOI..maybe
            GeoExtent aoi = map_layer.getAreaOfInterest();
            if (aoi == null)
            {
                //osgGIS.warn() << "Invalid area of interest in the map layer - no data?" << std.endl;
                return null;
            }

            QuadMap qmap;
            if (out_srs.isGeocentric())
            {
                // for a geocentric map, use a modified GEO quadkey:
                // (yes, that MIN_LAT of -180 is correct...we want a square)
                qmap = new QuadMap(new GeoExtent(-180.0, -180.0, 180.0, 90.0, Registry.SRSFactory().createWGS84()));
            }
            else
            {
                double max_span = Math.Max(aoi.getWidth(), aoi.getHeight());
                GeoPoint sw = aoi.getSouthwest();
                GeoPoint ne = new GeoPoint(sw.x() + max_span, sw.y() + max_span, aoi.getSRS());
                qmap = new QuadMap(new GeoExtent(sw, ne));
            }

#if TODO
    qmap.setStringStyle( QuadMap.STYLE_LOD_QUADKEY );
#endif

            //    osgGIS.notice()
            //        << "QMAP: " << std.endl
            //        << "   Top LOD = " << getTopLod( qmap, map_layer.get() ) << std.endl
            //        << "   Depth   = " << map_layer.getMaxDepth() << std.endl
            //        << "   Extent = " << qmap.getBounds().toString() << ", w=" << qmap.getBounds().getWidth() << ", h=" << qmap.getBounds().getHeight() << std.endl
            //        << std.endl;

            return new QuadTreeProfile(qmap);
        }



        public virtual CellCursor createCellCursor(Profile _profile)
        {
            QuadTreeProfile profile = (QuadTreeProfile)_profile;
            //TODO
            return null;
        }



        //virtual void collectCellKeys( Profile* profile );

        protected virtual uint queueTasks(Profile _profile, TaskManager task_man)
        {
            QuadTreeProfile profile = (QuadTreeProfile)_profile;
            if (profile != null)
            {
                // Now, build the index and collect the list of keys for which to compile data.
                QuadKeyList keys;
                collectGeometryKeys(profile.getQuadMap(), keys);

                // make a build task for each quad cell we collected:
                //int total_tasks = keys.size();
                foreach (QuadKey i in keys)
                {
                    Cell cell = new Cell(i.toString(), i.getExtent());
                    if (!cell_selector.valid() || cell_selector.selectCell(cell.get())) //i.toString() ) )
                    {
                        task_man.queueTask(createQuadKeyTask(*i));
                    }
                }

                return keys.size();
            }
            else
            {
                return 0;
            }
        }



        protected virtual void buildIndex(Profile _profile, osg.Group scene_graph)
        {
            QuadTreeProfile profile = (QuadTreeProfile)_profile;
            if (profile == null) return;

            //osgGIS.notice() << "Rebuilding index..." << std.endl;

            // first, determine the SRS of the output scene graph so that we can
            // make pagedlod/lod centroids.
            SpatialReference output_srs = map_layer.getOutputSRS(getSession(), getTerrainSRS());

            // first build the very top level.
            //scene_graph = new osg.Group();

            // the starting LOD is the best fit the the cell size:
            uint top_lod = getTopLod(profile.getQuadMap(), map_layer);

            SmartReadCallback reader = new SmartReadCallback();

            foreach (MapLayerLevelOfDetail i in map_layer.getLevels())
            {
                MapLayerLevelOfDetail level_def = i;
                uint lod = top_lod + level_def.getDepth();

                MapLayerLevelOfDetail sub_level_def = i + 1 != map_layer.getLevels().end() ? (i + 1).get() : null;
                float min_range, max_range;
                if (sub_level_def != null)
                {
                    min_range = sub_level_def.getMinRange();
                    max_range = sub_level_def.getMaxRange();
                }

                // get the extent of tiles that we will build based on the AOI:
                uint cell_xmin, cell_ymin, cell_xmax, cell_ymax;
                profile.getQuadMap().getCells(
                    map_layer.getAreaOfInterest(), lod,
                    cell_xmin, cell_ymin, cell_xmax, cell_ymax);

                for (uint y = cell_ymin; y <= cell_ymax; y++)
                {
                    for (uint x = cell_xmin; x <= cell_xmax; x++)
                    {
                        osg.Node node;

                        QuadKey key = new QuadKey(x, y, lod, profile.getQuadMap());

                        //osgGIS.notify( osg.NOTICE )
                        //    << "Cell: " << std.endl
                        //    << "   Quadkey = " << key.toString() << std.endl
                        //    << "   LOD = " << key.getLOD() << std.endl
                        //    << "   Extent = " << key.getExtent().toString() << " (w=" << key.getExtent().getWidth() << ", h=" << key.getExtent().getHeight() << ")" << std.endl
                        //    << std.endl;

                        node = sub_level_def ?
                            createIntermediateIndexNode(key, min_range, max_range, reader.get()) :
                            createLeafIndexNode(key, reader);

                        if (node.valid())
                        {
                            string out_file = createAbsPathFromTemplate("i" + key.toString());

                            if (!osgDB.writeNodeFile(*(node.get()), out_file))
                            {
                                // osgGIS.warn() << "FAILED to write index file " << out_file << std.endl;
                            }

                            // at the top level, assemble the root node
                            if (i == map_layer.getLevels().begin())
                            {
                                double top_min_range = sub_level_def != null ? 0 : level_def.getMinRange();

                                osg.PagedLOD plod = new osg.PagedLOD();
                                plod.setName(key.toString());
                                plod.setFileName(0, createRelPathFromTemplate("i" + key.toString()));
                                plod.setRange(0, top_min_range, level_def.getMaxRange());
                                plod.setPriorityScale(0, MY_PRIORITY_SCALE);
                                setCenterAndRadius(plod, key.getExtent(), reader.get());

                                //osgGIS.notice() << "QK=" << key.toString() << ", Ex=" << key.getExtent().toString() << ", Cen=" << key.getExtent().getCentroid().toString() << std.endl;


                                scene_graph.addChild(plod);
                            }
                        }
                    }
                }
            }
        }


        protected class QuadTreeProfile : Profile
        {
            public QuadTreeProfile(QuadMap _qmap)
                : base()
            {
                qmap = _qmap;
            }

            public QuadMap getQuadMap()
            {
                return qmap;
            }

            private QuadMap qmap;
        }

        protected osg.Node createIntermediateIndexNode(QuadKey key, float min_range, float max_range, SmartReadCallback cb)
        {
            osg.Group group = null;

            for (uint quadrant = 0; quadrant < 4; quadrant++)
            {
                QuadKey subkey = key.createSubKey(quadrant);

                if (osgDB.fileExists(createAbsPathFromTemplate("g" + subkey.toString())))
                {
                    if (!group)
                    {
                        group = new osg.Group();
                        group.setName(key.toString());
                    }

                    //osgGIS.notice() << "QK=" << subkey.toString() << ", Extent=" << subkey.getExtent().toString() << std.endl;

                    // enter the subtile set as a paged index node reference:
                    osg.PagedLOD plod = new osg.PagedLOD();
                    setCenterAndRadius(plod, subkey.getExtent(), reader);


#if USE_PAGEDLODS_IN_INDEX
            osg.PagedLOD* pointer = new osg.PagedLOD();
            pointer.setFileName( 0, createRelPathFromTemplate( "g" + subkey.toString() ) );
            pointer.setRange( 0, 0, 1e10 );
            pointer.setPriorityScale( 0, 1000.0f ); // top priority, hopefully
            pointer.setPriorityOffset( 0, 1000.0f );
            pointer.setCenter( plod.getCenter() );
            pointer.setRadius( plod.getRadius() );
#else
                    osg.ProxyNode pointer = new osg.ProxyNode();
                    pointer.setLoadingExternalReferenceMode(osg.ProxyNode.LOAD_IMMEDIATELY);
                    pointer.setFileName(0, createRelPathFromTemplate("g" + subkey.toString()));
                    //setCenterAndRadius( pointer, subkey.getExtent(), reader );
#endif

                    plod.addChild(pointer, max_range, 1e10);
                    plod.setFileName(1, createRelPathFromTemplate("i" + subkey.toString()));
                    plod.setRange(1, 0, max_range); // last one should always be min=0
                    plod.setPriorityScale(1, MY_PRIORITY_SCALE);

                    group.addChild(plod);



                    //osg.Geode* geode = new osg.Geode();

                    //osg.Sphere* g1 = new osg.Sphere( osg.Vec3(0,0,0), plod.getRadius() );
                    //osg.ShapeDrawable* sd1 = new osg.ShapeDrawable( g1 );
                    //sd1.setColor( osg.Vec4f(1,0,0,.2) );
                    //geode.addDrawable( sd1 );

                    ////osg.Vec3d p = terrain_srs.transform( subkey.getExtent().getCentroid() );
                    ////osg.Vec3d n = p; n.normalize();

                    ////osg.Geometry* g3 = new osg.Geometry();
                    ////osg.Vec3Array* v3 = new osg.Vec3Array(2);
                    ////(*v3)[0] = osg.Vec3d(0,0,0);
                    ////(*v3)[1] = n * 3000.0;
                    ////g3.setVertexArray( v3 );
                    ////osg.Vec4Array* c3 = new osg.Vec4Array(1);
                    ////(*c3)[0].set(1,1,0,1);
                    ////g3.setColorArray( c3 );
                    ////g3.setColorBinding(osg.Geometry.BIND_OVERALL);
                    ////g3.addPrimitiveSet( new osg.DrawArrays(GL_LINES, 0, 2) );
                    ////g3.getOrCreateStateSet().setMode( GL_LIGHTING, osg.StateAttribute.OFF );
                    ////geode.addDrawable( g3 );

                    ////osg.Sphere* g2 = new osg.Sphere( osg.Vec3(0,0,0), 25 );
                    ////osg.ShapeDrawable* sd2 = new osg.ShapeDrawable( g2 );
                    ////sd2.setColor( osg.Vec4f(1,1,0,1) );
                    ////geode.addDrawable( sd2 );

                    ////osg.Matrixd mx = osg.Matrixd.translate( p );
                    //osg.Matrixd mx = osg.Matrixd.translate( plod.getCenter() );
                    //osg.MatrixTransform* mt = new osg.MatrixTransform( mx );

                    //mt.addChild( geode );
                    //mt.getOrCreateStateSet().setMode( GL_BLEND, osg.StateAttribute.ON );
                    //mt.getOrCreateStateSet().setRenderingHint( osg.StateSet.TRANSPARENT_BIN );
                    //group.addChild( mt );
                }
            }

            return group;
        }

        protected osg.Node createLeafIndexNode(QuadKey key, SmartReadCallback cb)
        {
            osg.Group group = null;

            for (uint i = 0; i < 4; i++)
            {
                QuadKey quadrant_key = key.createSubKey(i);

                if (osgDB.fileExists(createAbsPathFromTemplate("g" + quadrant_key.toString())))
                {
                    if (!group)
                    {
                        group = new osg.Group();
                        group.setName(key.toString());
                    }

#if USE_PAGEDLODS_IN_INDEX
            osg.PagedLOD* pointer = new osg.PagedLOD();
            pointer.setFileName( 0, createRelPathFromTemplate( "g" + quadrant_key.toString() ) );
            pointer.setRange( 0, 0, 1e10 );
            pointer.setPriorityScale( 0, 1000.0f ); // top priority!
            pointer.setPriorityOffset( 0, 1000.0f );
            setCenterAndRadius( pointer, quadrant_key.getExtent(), reader );
#else
                    osg.ProxyNode pointer = new osg.ProxyNode();
                    pointer.setLoadingExternalReferenceMode(osg.ProxyNode.LOAD_IMMEDIATELY);
                    pointer.setFileName(0, createRelPathFromTemplate("g" + quadrant_key.toString()));
                    //setCenterAndRadius( pointer, quadrant_key.getExtent(), reader );
#endif

                    group.addChild(pointer);
                }
            }

            return group;
        }

        protected Task createQuadKeyTask(QuadKey key)
        {
            // construct a filter environment template to use for all tasks:
            FilterEnv cell_env = getSession().createFilterEnv();

            cell_env.setTerrainNode(getTerrainNode());
            cell_env.setTerrainSRS(getTerrainSRS());

            string abs_path = createAbsPathFromTemplate("g" + key.toString());

            Task task = null;

            MapLayerLevelOfDetail def = getDefinition(key.createParentKey(), map_layer);
            if (def != null)
            {
                cell_env.setInputSRS(def.getFeatureLayer().getSRS());
                cell_env.setExtent(map_layer.getAreaOfInterest().getSRS().transform(key.getExtent()));
                cell_env.setProperty(new Property("compiler.cell_id", key.toString()));
                foreach (Property i in def.getEnvProperties())
                    cell_env.setProperty(i);

                task = new CellCompiler(
                    key.toString(),
                    abs_path,
                    def.getFeatureLayer(),
                    def.getFilterGraph(),
                    def.getMinRange(),
                    def.getMaxRange(),
                    cell_env,
                    def.getResourcePackager() ? def.getResourcePackager() : resource_packager.get(),
                    getArchive(),
                    def.getUserData());

                // osgGIS.info()
                //    << "Task: Key = " << key.toString() << ", LOD = " << key.getLOD() << ", Extent = " << key.getExtent().toString() 
                //    << " (w=" << key.getExtent().getWidth() << ", h=" << key.getExtent().getHeight() << ")"
                //    << std.endl;
            }

            return task;
        }


        protected void collectGeometryKeys(QuadMap qmap, QuadKeyList geom_keys)
        {
            // the starting LOD is the best fit the the cell size:
            uint top_lod = getTopLod(qmap, map_layer);

            foreach (MapLayerLevelOfDetail i in map_layer.getLevels())
            {
                MapLayerLevelOfDetail level_def = i;
                uint lod = top_lod + level_def.getDepth();

                // get the extent of tiles that we will build based on the AOI:
                uint cell_xmin, cell_ymin, cell_xmax, cell_ymax;
                qmap.getCells(
                    map_layer.getAreaOfInterest(), lod,
                    cell_xmin, cell_ymin, cell_xmax, cell_ymax);

                for (uint y = cell_ymin; y <= cell_ymax; y++)
                {
                    for (uint x = cell_xmin; x <= cell_xmax; x++)
                    {
                        QuadKey key = new QuadKey(x, y, lod, qmap);
                        for (uint k = 0; k < 4; k++)
                        {
                            geom_keys.push_back(key.createSubKey(k));
                        }
                    }
                }
            }
        }

        static uint getTopLod(QuadMap qmap, MapLayer map_layer)
        {
            int bottom_lod = qmap.getBestLodForCellsOfSize(map_layer.getCellWidth(), map_layer.getCellHeight());
            uint max_depth = map_layer.getMaxDepth();
            long top_lod = Math.Max(0, bottom_lod - max_depth);
            return (uint)top_lod;
        }

        static MapLayerLevelOfDetail getDefinition(QuadKey key, MapLayer map_layer)
        {
            uint top_lod = getTopLod(key.getMap(), map_layer);
            uint depth_to_find = key.getLOD() - top_lod;
            foreach (MapLayerLevelOfDetail i in map_layer.getLevels())
            {
                if (i.getDepth() == depth_to_find)
                    return i;
            }
            return null;
        }


        protected const float MY_PRIORITY_SCALE = 1.0f; //0.5f

    }
}
#endif
using System;
#if TODO
using MogreGis;

namespace osgGISProjects
{
    public class Builder
    {

        public Builder(Project _project)
        {
            project = _project;
            num_threads = 0;
        }

        /** 
         * Sets the maximum number of parallel threads to use for
         * building each target. The default value is equal to the
         * number of LOGICAL processors detected in the system.
         */
        public void setNumThreads(int _num_threads)
        {
            num_threads = _num_threads;
        }


        public bool build()
        {
            if (project == null)
                return false;

            //osgGIS.notice() << "No targets specified; building all layers." << std.endl;

            bool ok = true;

            foreach (BuildLayer i in project.getLayers())
            {
                ok = build(i);
            }

            return ok;
        }


        public bool build(string target_name)
        {
            if (project == null)
                return false;

            if (!string.IsNullOrEmpty(target_name))
            {
                BuildTarget target = project.getTarget(target_name);
                if (target == null)
                    //osgGIS.warn() << "No target " << target_name << " found in the project." << std.endl;
                    return target != null ? build(target) : false;
            }
            else
            {
                return build();
            }
        }




        protected bool build(BuildTarget target)
        {
            if (project == null || target == null)
                return false;

            // osgGIS.notice() << "Building target \"" << target.getName() << "\"." << std.endl;

            bool ok = true;

            foreach (BuildLayer i in target.getLayers())
            {
                ok = build(i);
            }

            if (ok)
            {
                // osgGIS.notice() << "Done building target \"" << target.getName() << "\"." << std.endl;
            }
            return ok;
        }


        protected bool build(BuildLayer layer)
            {
     string work_dir_name = project.getAbsoluteWorkingDirectory();
    if ( string.IsNullOrEmpty(work_dir_name) )
        work_dir_name = "work_" + project.getName();

     string work_dir = PathUtils.combinePaths( 
        project.getBaseURI(),
        work_dir_name );

    if ( osgDB.makeDirectory( work_dir ) )
    {
        Registry.instance().setWorkDirectory( work_dir );
    }

    //osgGIS.notice() << "Building layer \"" << layer.getName() << "\"." << std.endl;

    // first create and initialize a Session that will share data across the build.
     Session  session = new Session();

    // add shared scripts to the session:
    foreach( Script  i in project.getScripts())
        session.addScript( i );

    // add shared resources to the session:
    foreach( Resource i in project.getResources() )
        session.getResources().addResource( i );

    // now establish the source data record form this layer and open a feature layer
    // that connects to that source.
    Source  source = layer.getSource(); // default source.. may be overriden in slices
    //if ( !source )
    //{
    //    //TODO: log error
    //    osgGIS.notify( osg.WARN ) 
    //        << "No source specified for layer \"" << layer.getName() << "\"." << std.endl;
    //    return false;
    //}



    // recursively build any sources that need building.
    if ( source && !build( source, session.get() ) )
    {
       // osgGIS.warn()
       //    << "Unable to build source \"" << source.getName() << "\" or one of its dependencies." 
       //     << std.endl;
        return false;
    }    

     FeatureLayer  feature_layer;

    if ( source != null)
    {
        feature_layer = Registry.instance().createFeatureLayer( source.getAbsoluteURI() );

        if ( feature_layer == null )
        {
            //TODO: log error
            //osgGIS.warn()
            //    << "Cannot access source \"" << source.getName() 
            //    << "\" for layer \"" << layer.getName() << "\"." << std.endl;
            return false;
        }
    }

    // The reference terrain:
     osg.Node         terrain_node;
     SpatialReference  terrain_srs;
    GeoExtent                      terrain_extent;

    Terrain  terrain = layer.getTerrain();
    if ( !getTerrainData( terrain, terrain_node, terrain_srs, terrain_extent ) )
        return false;

    // output file:
     string output_file = layer.getAbsoluteTargetPath();
    osgDB.makeDirectoryForFile( output_file );
    if ( !osgDB.fileExists( osgDB.getFilePath( output_file ) ) )
    {
        //osgGIS.warn()
        //    << "Unable to establish target location for layer \""
        //    << layer.getName() << "\" at \"" << output_file << "\"." 
        //    << std.endl;
        return false;
    }

    // whether to include textures in IVE files:
    bool inline_ive_textures = layer.getProperties().getBoolValue( "inline_textures", false );
    
    // TODO: deprecate this as we move towards the ResourcePackager...
    osgDB.ReaderWriter.Options  options;
    if ( inline_ive_textures )
    {
        options = new osgDB.ReaderWriter.Options( "noWriteExternalReferenceFiles useOriginalExternalReferences" );
    }
    else
    {
        options = new osgDB.ReaderWriter.Options( "noTexturesInIVEFile noWriteExternalReferenceFiles useOriginalExternalReferences" );
    }
    
    osgDB.Registry.instance().setOptions( options );


    osgDB.Archive archive;
    string archive_file = output_file;

    if ( osgDB.getLowerCaseFileExtension( output_file ) == "osga" )
    {
        archive = osgDB.openArchive( output_file, osgDB.Archive.CREATE, 4096 );
        output_file = "out.ive";

        // since there's no way to set the master file name...fake it out
        osg.Group dummy = new osg.Group();
        archive.writeNode( dummy , output_file );
    }

    // intialize a task manager if necessary:
    TaskManager manager = 
        num_threads > 1? new TaskManager( num_threads ) :
        num_threads < 1? new TaskManager() :
        null;




    // prep the map layer definition:
     MapLayer  map_layer = new MapLayer();

    // a resource packager if necessary will copy ext-ref files to the output location:
        ResourcePackager     packager = new ResourcePackager();
    packager.setArchive( archive.get() );
    packager.setOutputLocation( osgDB.getFilePath( output_file ) );

    if ( layer.getProperties().getBoolValue( "localize_resources", false ) )
    {
        packager.setMaxTextureSize( layer.getProperties().getIntValue( "max_texture_size", 0 ) );
        packager.setCompressTextures( layer.getProperties().getBoolValue( "compress_textures", false ) );
        packager.setInlineTextures( layer.getProperties().getBoolValue( "inline_textures", false ) );
    }

    if ( !addSlicesToMapLayer( layer.getSlices(), layer.getEnvProperties(), map_layer, packager, 0, session, source ) )
    {
        //osgGIS.warn() << "Failed to add all slices to layer " << layer.getName() << std.endl;
        return false;
    }

    // calculate the grid cell size:
    double col_size = layer.getProperties().getDoubleValue( "col_size", -1.0 );
    double row_size = layer.getProperties().getDoubleValue( "row_size", -1.0 );
    if ( col_size <= 0.0 || row_size <= 0.0 )
    {
        int num_cols = Math.Max( 1, layer.getProperties().getIntValue( "num_cols", 1 ) );
        int num_rows = Math.Max(1, layer.getProperties().getIntValue("num_rows", 1));
        col_size = map_layer.getAreaOfInterest().getWidth() / (double)num_cols;
        row_size = map_layer.getAreaOfInterest().getHeight() / (double)num_rows;
    }
    map_layer.setCellWidth( col_size );
    map_layer.setCellHeight( row_size );
    map_layer.setEncodeCellRadius( layer.getProperties().getBoolValue( "encode_cell_radius", true ) );


    MapLayerCompiler compiler;
    
    // figure out which compiler to use:
    if ( layer.getType() == BuildLayer.LayerType.TYPE_QUADTREE )
    {
        compiler = new QuadTreeMapLayerCompiler( map_layer, session );
    }
    else if (layer.getType() == BuildLayer.LayerType.TYPE_GRIDDED)
    {
        compiler = new GriddedMapLayerCompiler( map_layer.get(), session. );
    }
    else if (layer.getType() == BuildLayer.LayerType.TYPE_SIMPLE)
    {
        compiler = new SimpleMapLayerCompiler( map_layer, session );
    }

    if ( compiler.get() )
    {
        compiler.setAbsoluteOutputURI( output_file );
        compiler.setPaged( layer.getProperties().getBoolValue( "paged", true ) );
        compiler.setTerrain( terrain_node.get(), terrain_srs, terrain_extent );
        compiler.setArchive( archive.get(), archive_file );
        compiler.setResourcePackager( packager.get() );

        // build the layer and write the root file to output:
       osg.Group result = compiler.compile( manager.get() );

        if ( result != null )
        {
            packager.packageNode( result.get(), output_file );
        }
    }

    if ( archive != null )
    {
        archive.close();
    }

    //osgGIS.notice() << "Done building layer \"" << layer.getName() << "\"." << std.endl;

    return true;
}

        protected bool build(Source source, Session session)
        {
            //osgGIS.notice() << "Building source " << source.getName() << std.endl;

            // only need to build intermediate sources.
            if (!source.isIntermediate())
            {
                //osgGIS.info() << "...source " << source.getName() << " does not need building (not intermediate)." << std.endl;
                return true;
            }

            Source parent = source.getParentSource();
            if (parent == null)
            {
                //osgGIS.warn()  << "...ERROR: No parent source found for intermediate source \"" << source.getName() << "\"" << std.endl;
                return false;
            }

            // check whether a rebuild is required:
            if (parent.getTimeLastModified() < source.getTimeLastModified())
            {
                //osgGIS.info() << "...source " << source.getName() << " does not need building (newer than parent)." << std.endl;
                return true;
            }

            // build it's parent first:
            if (!build(parent, session))
            {
                //osgGIS.warn() << "...ERROR: Failed to build source \"" << parent.getName() << "\", parent of source \"" << source.getName() << "\"" << std.endl;
                return false;
            }

            // validate the existence of a filter graph:
            FilterGraph graph = source.getFilterGraph();
            if (graph == null)
            {
                //osgGIS.warn() << "...ERROR: No filter graph set for intermediate source \"" << source.getName() << "\"" << std.endl;
                return false;
            }

            // establish a feature layer for the parent source:
            FeatureLayer feature_layer = Registry.instance().createFeatureLayer(parent.getAbsoluteURI());
            if (feature_layer == null)
            {
                //osgGIS.warn() << "...ERROR: Cannot access source \"" << source.getName() << "\"" << std.endl;
                return false;
            }

            //TODO: should we allow terrains for a source compile?? No. Because we would need to transform
            // the source into terrain SRS space, which we do not want to do until we're building nodes.

            // initialize a source data compiler. we use a temporary session because this source build
            // is unrelated to the current "master" build. If we used the same session, the new feature
            // store would hang around and not get properly closed out for use in the next round.
            Session temp_session = session.derive();
            FilterEnv source_env = temp_session.createFilterEnv();

            FeatureStoreCompiler compiler = new FeatureStoreCompiler(feature_layer, graph);

            if (!compiler.compile(source.getAbsoluteURI(), source_env))
            {
                //osgGIS.warn() << "...ERROR: failure compiling source \"" << source.getName() << "\"" << std.endl;
                return false;
            }

            //osgGIS.notice() << "...done compiling source \"" << source.getName() << "\"" << std.endl;

            return true;
        }


        /*** Statics ********************************************************/

        static bool getTerrainData(Terrain terrain,
                                   out osg.Node out_terrain_node,
                                   out SpatialReference out_terrain_srs,
                                   out GeoExtent out_terrain_extent)
        {
            if (terrain != null)
            {
                if (!string.IsNullOrEmpty(terrain.getURI()))
                {
                    out_terrain_node = osgDB.readNodeFile(terrain.getAbsoluteURI());
                }

                // first check for an explicity defined SRS:
                out_terrain_srs = terrain.getExplicitSRS();
                if (out_terrain_srs != null && out_terrain_srs.isGeographic())
                {
                    // and make it geocentric if necessary..
                    out_terrain_srs = Registry.SRSFactory().createGeocentricSRS(out_terrain_srs.get());
                }

                if (out_terrain_node != null)
                {
                    // if the SRS wasn't explicit, try to read it from the scene graph:
                    if (out_terrain_srs == null)
                    {
                        out_terrain_srs = Registry.SRSFactory().createSRSfromTerrain(out_terrain_node.get());
                    }

                    //osgGIS.notice()
                    //    << "Loaded TERRAIN from \"" << terrain.getAbsoluteURI() << "\", SRS = "
                    //    << (out_terrain_srs != null? out_terrain_srs.getName() : "unknown")
                    //    << std.endl;
                }

                else if (!string.IsNullOrEmpty(terrain.getURI()))
                {
                    //osgGIS.warn()
                    //    << "Unable to load data for terrain \""
                    //    << terrain.getName() << "\"." 
                    //    << std.endl;

                    return false;
                }
            }

            out_terrain_extent = new GeoExtent(-180, -90, 180, 90,
                                    Registry.instance().getSRSFactory().createWGS84());

            return true;
        }


        /*** Class methods ***************************************************/



        bool Builder.addSlicesToMapLayer(BuildLayerSliceList slices,
                                     Properties env_properties,
                                     MapLayer map_layer,
                                     ResourcePackager default_packager,
                                     uint depth,
                                     Session session,
                                     Source parent_source)
        {
            foreach (BuildLayerSlice i in slices)
            {
                BuildLayerSlice slice = i;

                if (slice.getSource() != null && !build(slice.getSource(), session))
                {
                    // osgGIS.warn()
                    //    << "Unable to build source \"" << slice.getSource().getName() << "\" or one of its dependencies." 
                    //    << std.endl;
                    return false;
                }

                Source slice_source = slice.getSource() != null ? slice.getSource() : parent_source;

                ResourcePackager packager = default_packager ? default_packager.clone() : null;
                if (packager != null)
                {
                    packager.setMaxTextureSize(
                        slice.getProperties().getIntValue("max_texture_size", default_packager.getMaxTextureSize()));
                    packager.setCompressTextures(
                        slice.getProperties().getBoolValue("compress_textures", default_packager.getCompressTextures()));
                    packager.setInlineTextures(
                        slice.getProperties().getBoolValue("inline_textures", default_packager.getInlineTextures()));
                }

                if (slice_source != null)
                {
                    FeatureLayer feature_layer = Registry.instance().createFeatureLayer(
                        slice_source.getAbsoluteURI());

                    if (feature_layer == null)
                    {
                        //osgGIS.warn() << "Cannot access source \"" << slice_source.getName() << std.endl;
                        return false;
                    }

                    map_layer.push(
                        feature_layer,
                        slice.getFilterGraph(),
                        env_properties,
                        packager.get(),
                        slice.getMinRange(),
                        slice.getMaxRange(),
                        true,
                        depth,
                        null);
                }

                // now add any sub-slice children:
                if (!addSlicesToMapLayer(slice.getSubSlices(), env_properties, map_layer, packager.get(), depth + 1, session, slice_source))
                {
                    return false;
                }
            }

            return true;
        }


        private Project project;
        private int num_threads;


        private string resolveURI(string input);

        private bool addSlicesToMapLayer(
            BuildLayerSliceList slices,
             Properties env_props,
            MapLayer map_layer,
            ResourcePackager default_packager,
            uint lod,
            Session session,
            Source source);
    }
}
#endif
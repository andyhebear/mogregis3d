using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * Compiles all or part of a FeatureLayer into an OSG scene graph.
     *
     * This layer compiler runs feature data though one or more FilterGraphs
     * in order to produce an in-memory OSG scene graph. It does not support
     * PagedLODs or parallel compilation.
     */
    public class SimpleLayerCompiler : LayerCompiler
    {

        /**
         * Constructs a new compiler.
         */
        public SimpleLayerCompiler()
        {
            //NOP
        }

        /**
         * Constructs a new compiler.
         *
         * @param graph
         *      Single filter graph to compile.
         */
        public SimpleLayerCompiler(FilterGraph graph)
        {
            addFilterGraph(FLT_MIN, FLT_MAX, graph);
        }

        /**
         * Compiles a feature layer.
         *
         * @param layer
         *      Feature layer to compile
         * @param output_file
         *      If getLocalizeResources() == true, the compiler will localize
         *      external resources to the directory containing the specified
         *      file. This compiler does not actually write anything to the
         *      named file however.
         * @return
         *      Resulting scene graph, or NULL upon error
         */
        public Node compile(FeatureLayer layer)
        {
            return compile(layer, null);
        }
        public Node compile(FeatureLayer layer, string output_file)
        {
            FeatureCursor cursor = layer.getCursor();
            return compile(layer, cursor, output_file);
        }

        /**
         * Compiles a feature layer.
         *
         * @param layer
         *      Feature layer to compile
         * @param cursor
         *      Iterator over custom connection of features to compile
         * @param output_file
         *      If getLocalizeResources() == true, the compiler will localize
         *      external resources to the directory containing the specified
         *      file. This compiler does not actually write anything to the
         *      named file however.
         * @return
         *      Resulting scene graph, or NULL upon error
         */
        public Node compile(FeatureLayer layer, FeatureCursor cursor)
        {
            return compile(layer, cursor, "");
        }
        public Node compile(FeatureLayer layer, FeatureCursor cursor, string output_file)
        {
    osg.Node* result = NULL;

    if ( !layer ) {
        osgGIS.notify( osg.WARN ) << "Illegal null feature layer" << std.endl;
        return NULL;
    }
    
    osg.ref_ptr<osg.LOD> lod = new osg.LOD();

    if ( getFadeLODs() )
    {
        FadeHelper.enableFading( lod.getOrCreateStateSet() );
    }

    for( FilterGraphRangeList.iterator i = graph_ranges.begin(); i != graph_ranges.end(); i++ )
    {
        osg.Node* range = compile( layer, cursor, i.graph.get() );
        if ( range )
        {
            lod.addChild( range, i.min_range, i.max_range );

            if ( getFadeLODs() )
            {
                FadeHelper.setOuterFadeDistance( i.max_range, range.getOrCreateStateSet() );
                FadeHelper.setInnerFadeDistance( i.max_range - .2*(i.max_range-i.min_range), range.getOrCreateStateSet() );
            }
        }
    }

    if ( GeomUtils.hasDrawables( lod.get() ) )
    {
        if ( getOverlay() )
        {
            result = convertToOverlay( lod.get() );
        }
        else
        {
            result = lod.release();
        }
        
        osgUtil.Optimizer opt;
        opt.optimize( result, 
            osgUtil.Optimizer.SPATIALIZE_GROUPS |
            osgUtil.Optimizer.STATIC_OBJECT_DETECTION |
            osgUtil.Optimizer.SHARE_DUPLICATE_STATE );

        if ( getRenderOrder() >= 0 )
        {
             string  bin_name = result.getOrCreateStateSet().getBinName();
            result.getOrCreateStateSet().setRenderBinDetails( getRenderOrder(), bin_name );
            result.getOrCreateStateSet().setAttributeAndModes( new osg.Depth( osg.Depth.ALWAYS ), osg.StateAttribute.ON );
        }

        localizeResourceReferences( result );

        if ( output_file.length() > 0 )
        {
            localizeResources( osgDB.getFilePath( output_file ) );
        }
    }

    return result;
}


        public Node compile(FeatureLayer layer, FeatureCursor cursor, FilterGraph graph)
        {
            osg.ref_ptr<FilterEnv> env = getSession().createFilterEnv();
            env.setExtent(getAreaOfInterest(layer));
            env.setInputSRS(layer.getSRS());
            env.setTerrainNode(terrain.get());
            env.setTerrainSRS(terrain_srs.get());
            env.setTerrainReadCallback(read_cb.get());

            osg.Group* output;
            FilterGraphResult r = graph.computeNodes(cursor, env.get(), output);
            return r.isOK() ? output : NULL;
        }
    }
}

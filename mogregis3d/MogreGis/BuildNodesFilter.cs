using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
namespace MogreGis
{
    /**
     * Assembles Fragment instances into osg.Node instances.
     *
     * This is usually the final Filter in a FilterGraph. It takes as input one or
     * more Fragment objects and combines them under Geode and Group objects, optimizing
     * as appropriate.
     *
     * It can also apply finishing touches like mapping an overlay texture, enabling cluster
     * culling, or adjusting the overall GL state attributes line point size and line width.
     */
    public class BuildNodesFilter : NodeFilter
    {
        //TODO OSGGIS_META_FILTER( BuildNodesFilter );

        /**
         * Constructs a BuildNodes filter with default settings.
         */
        public BuildNodesFilter()
        {
            init();
        }

        /**
         * Copy constructor
         */
        public BuildNodesFilter(BuildNodesFilter rhs)
            : base(rhs)
        {
            line_width = rhs.line_width;
            point_size = rhs.point_size;
            draw_cluster_culling_normals = rhs.draw_cluster_culling_normals;
            raster_overlay_script = rhs.raster_overlay_script;
            raster_overlay_max_size = rhs.raster_overlay_max_size;
            cull_backfaces = rhs.cull_backfaces;
            apply_cluster_culling = rhs.apply_cluster_culling;
            disable_lighting = rhs.disable_lighting;
            optimize = rhs.optimize;
            embed_attrs = rhs.embed_attrs;
            alpha_blending = rhs.alpha_blending;
        }

        // properties   

        /**
         * Sets whether to disable GL lighting on the resulting node graphs.
         *
         * @param value
         *      True to disable lighting, false to leave it enabled.
         */
        public void setDisableLighting(bool value)
        {
            disable_lighting = value;
        }

        /** 
         * Gets whether to disable GL lighting on the resulting node graphs.
         *
         * @return
         *      True if lighting is disabled, false if not.
         */
        public bool getDisableLighting()
        {
            return disable_lighting;
        }

        /**
         * Sets whether to enable backface culling on the resulting node.
         *
         * @param value
         *      True to set backface culling; false to do nothing
         */
        public void setCullBackfaces(bool value)
        {
            cull_backfaces = value;
        }

        /**
         * Gets whether to enable backfase culling on the resulting node.
         *
         * @return
         *      True for backface culling; false otherwise
         */
        public bool getCullBackfaces()
        {
            return cull_backfaces;
        }

        /**
         * Sets whether to apply cluster culling to the resulting node. 
         * Cluster culling is a fast way to cull the entire node based on
         * a single normal vector. (This only applies to localized, geocentric
         * datasets.)
         *
         * @param value
         *      True to enable cluster culling for localized, geocentric data;
         *      False to disable it.
         */
        public void setApplyClusterCulling(bool value)
        {
            apply_cluster_culling = value;
        }

        /**
         * Gets whether to apply cluster culling to the resulting node.
         *
         * @return
         *      True if cluster culling will be installed
         */
        public bool getApplyClusterCulling()
        {
            return apply_cluster_culling;
        }

        /**
         * Sets whether to run the OSG optimizer on the resulting node.
         *
         * @param value
         *      True to enable node optimization
         */
        public void setOptimize(bool value)
        {
            optimize = value;
        }

        /**
         * Gets whether to run the OSG optimizer on the resulting node.
         *
         * @return True if resulting nodes will get optimized
         */
        public bool getOptimize()
        {
            return optimize;
        }

        /** 
         * Sets the line width to set on the resulting node.
         *
         * @param value
         *      The line width to use, or 0.0f to use the default
         */
        public void setLineWidth(float value)
        {
            line_width = value;
        }

        /**
         * Gets the line width the set on the resulting node.
         *
         * @return The line width, or 0.0f to use the default
         */
        public float getLineWidth()
        {
            return line_width;
        }

        /** 
         * Sets the point size to apply to the resulting node graph.
         *
         * @param value
         *      The point size, or 0.0 to use the default
         */
        public void setPointSize(float value)
        {
            point_size = value;
        }

        /**
         * Gets the point size to apply to the resulting node graph.
         *
         * @return The point size, or 0.0 to use the default
         */
        public float getPointSize()
        {
            return point_size;
        }

        /**
         * Sets a script that evaluates to the name of a RasterResource to use to
         * generate a texture that will by applied to the resulting scene graph. This is
         * useful for generating overlay textures, for example.
         *
         * @param Script
         *      Script that generates a RasterResource name
         */
        public void setRasterOverlayScript(Script script)
        {
            raster_overlay_script = script;
        }

        /**
         * Gets a script that evalutes to the name of a RasterResource to use as a
         * default scene graph texture.
         *
         * @return
         *      Script that generates a RasterResource name
         */
        public Script getRasterOverlayScript()
        {
            return raster_overlay_script;
        }

        /**
         * Sets the maximum allowable dimension (width or height) for an overlay texture.
         * The default is 1024.
         *
         * @param size
         *      Maximum texture size (in either dimension)
         */
        public void setRasterOverlayMaxSize(int size)
        {
            raster_overlay_max_size = size;
        }

        /**
         * Gets the maximum allowable dimension (width or height) for an overlay texture.
         *
         * @return
         *      Maximum texture size (in either dimension)
         */
        public int getRasterOverlayMaxSize()
        {
            return raster_overlay_max_size;
        }

        /**
         * Sets whether to embed the feature attributes into the scene graph as a
         * osgSim.ShapeAttributeList userdata structure. Default = false.
         *
         * @param value
         *      True to embed the attr name/value pairs; false if not.
         */
        public void setEmbedAttributes(bool value)
        {
            embed_attrs = value;
        }

        /**
         * Gets whether to embed the feature attributes into the scene graph as a
         * osgSim.ShapeAttributeList userdata structure. Default = false.
         *
         * @return True to embed the attr name/value pairs; false if not.
         */
        public bool getEmbedAttributes()
        {
            return embed_attrs;
        }

        /**
         * Sets whether to enable alpha blending on the node's stateset.
         *
         * @param value
         *      True to enable alpha blending (transparency)
         */
        public void setAlphaBlending(bool value)
        {
            alpha_blending = value;
        }

        /**
         * Gets whether to enable alpha blending on the node's stateset.
         *
         * @return True to enable transparency; false otherwise.
         */
        public bool getAlphaBlending()
        {
            return alpha_blending;
        }

        // Filter overrides    
        public virtual void setProperty(Property p)
        {
            if (p.getName() == "optimize")
                setOptimize(p.getBoolValue(getOptimize()));
            else if (p.getName() == "cull_backfaces")
                setCullBackfaces(p.getBoolValue(getCullBackfaces()));
            else if (p.getName() == "apply_cluster_culling")
                setApplyClusterCulling(p.getBoolValue(getApplyClusterCulling()));
            else if (p.getName() == "disable_lighting")
                setDisableLighting(p.getBoolValue(getDisableLighting()));
            else if (p.getName() == "line_width")
                setLineWidth(p.getFloatValue(getLineWidth()));
            else if (p.getName() == "point_size")
                setPointSize(p.getFloatValue(getPointSize()));
            else if (p.getName() == "raster_overlay")
                setRasterOverlayScript(new Script(p.getValue()));
            else if (p.getName() == "raster_overlay_max_size")
                setRasterOverlayMaxSize(p.getIntValue(getRasterOverlayMaxSize()));
            else if (p.getName() == "embed_attributes")
                setEmbedAttributes(p.getBoolValue(getEmbedAttributes()));
            else if (p.getName() == "alpha_blending")
                setAlphaBlending(p.getBoolValue(getAlphaBlending()));

            base.setProperty(p);
        }
        public virtual Properties getProperties()
        {
            Properties p = base.getProperties();
            p.Add(new Property("optimize", getOptimize()));
            p.Add(new Property("cull_backfaces", getCullBackfaces()));
            p.Add(new Property("apply_cluster_culling", getApplyClusterCulling()));
            p.Add(new Property("disable_lighting", getDisableLighting()));
            p.Add(new Property("line_width", getLineWidth()));
            p.Add(new Property("point_size", getPointSize()));
            if (getRasterOverlayScript() != null)
                p.Add(new Property("raster_overlay", getRasterOverlayScript().getCode()));
            if (getRasterOverlayMaxSize() != DEFAULT_RASTER_OVERLAY_MAX_SIZE)
                p.Add(new Property("raster_overlay_max_size", getRasterOverlayMaxSize()));
            if (getEmbedAttributes() != DEFAULT_EMBED_ATTRIBUTES)
                p.Add(new Property("embed_attributes", getEmbedAttributes()));
            if (getAlphaBlending() != DEFAULT_ALPHA_BLENDING)
                p.Add(new Property("alpha_blending", getAlphaBlending()));
            return p;
        }

        // NodeFilter overrides
        protected virtual AttributedNodeList process(FragmentList input, FilterEnv env)
        {
            AttributedNodeList nodes;

            osg.Geode geode = null;
            for (FragmentList.const_iterator i = input.begin(); i != input.end(); i++)
            {
                Fragment frag = i;

                AttributeList frag_attrs = frag.getAttributes();

                if (!geode)
                {
                    geode = new osg.Geode();
                    nodes.Add(new AttributedNode(geode, frag_attrs));
                }

                for (DrawableList.const_iterator d = frag.getDrawables().begin(); d != frag.getDrawables().end(); d++)
                {
                    geode.addDrawable(d.get());
                }

                bool retire_geode = false;

                // if a fragment name is set, apply it
                if (frag.hasName())
                {
                    geode.addDescription(frag.getName());
                    retire_geode = true;
                }

                if (getEmbedAttributes())
                {
                    embedAttributes(geode, frag_attrs);
                    retire_geode = true;
                }

                // If required, reset the geode point so that the next fragment gets placed in a new geode.
                if (retire_geode)
                {
                    geode = null;
                }
            }

            // with multiple geodes or fragment names, disable geode combining to preserve the node decription.
            if (nodes.Count > 1)
            {
                env.getOptimizerHints().exclude(osgUtil.Optimizer.MERGE_GEODES);
            }

            return process(nodes, env);
        }
        protected virtual AttributedNodeList process(AttributedNodeList input, FilterEnv env)
        {
     Node  result;

    if ( input.Count > 1 )
    {
        result = new osg.Group();
        for( AttributedNodeList.iterator i = input.begin(); i != input.end(); i++ )
        {
            osg.Node  node = i.get().getNode();
            if ( node != null)
            {
                if ( getEmbedAttributes() )
                    embedAttributes( node, i.get().getAttributes() );

                result.asGroup().addChild( node );
            }
        }
    }
    else if ( input.Count == 1 )
    {
        result = input[0].getNode();

        if ( getEmbedAttributes() )
            embedAttributes( result.get(), input[0].getAttributes() );
    }
    else
    {
        return new AttributedNodeList();
    }

    // if there are no drawables or external refs, toss it.
    if ( !GeomUtils.hasDrawables( result.get() ) )
    {
        return AttributedNodeList();
    }

    // NEXT create a XFORM if there's a localization matrix in the SRS. This will
    // prevent jittering due to loss of precision.
      SpatialReference  input_srs = env.getInputSRS();

    if ( env.getExtent().getArea() > 0 && !input_srs.getReferenceFrame().isIdentity() )
    {
        Vector3D centroid = new Vector3D( 0, 0, 0 );
        osg.Matrixd irf = input_srs.getInverseReferenceFrame();
        osg.Vec3d centroid_abs = centroid * irf;
        osg.MatrixTransform xform = new osg.MatrixTransform( irf );

        xform.addChild( result);
        result = xform;

        if ( getApplyClusterCulling() && input_srs.isGeocentric() )
        {    
            Vector3D normal = centroid_abs;
            normal.normalize();
            
            //osg.BoundingSphere bs = result.computeBound(); // force it            
            // radius = distance from centroid inside which to disable CC altogether:
            //float radius = bs.radius();
            //osg.Vec3d control_point = bs.center();

            Vector3D control_point = centroid_abs;
            GeoPoint env_cen = input_srs.transform( env.getCellExtent().getCentroid() );
            GeoPoint env_sw  = input_srs.transform( env.getCellExtent().getSouthwest() );
            float radius = (env_cen-env_sw).length();

            // dot product: 0 = orthogonal to normal, -1 = equal to normal
            float deviation = -radius/input_srs.getEllipsoid().getSemiMinorAxis();
            
            osg.ClusterCullingCallback ccc = new osg.ClusterCullingCallback();
            ccc.set( control_point, normal, deviation, radius );

            osg.Group cull_group = new osg.Group();
            cull_group.setCullCallback( ccc );
            cull_group.addChild( xform );
            result = cull_group;


            //osgGIS.notify(osg.NOTICE) << "CCC: radius = " << radius << ", deviation = " << deviation << std.endl;


            //if ( getDrawClusterCullingNormals() == true )
            //{
            //    //DRAW CLUSTER-CULLING NORMALS
            //    osg.Geode* geode = new osg.Geode();
            //    osg.Geometry* g = new osg.Geometry();
            //    osg.Vec3Array* v = new osg.Vec3Array(2);
            //    (*v)[0] = control_point; (*v)[1] = control_point + (normal*radius);
            //    g.setVertexArray( v );
            //    osg.Vec4Array* c = new osg.Vec4Array(1);
            //    (*c)[0] = osg.Vec4f( 0,1,0,1 );
            //    g.setColorArray( c );
            //    g.setColorBinding( osg.Geometry.BIND_OVERALL );
            //    g.addPrimitiveSet( new osg.DrawArrays( osg.PrimitiveSet.LINES, 0, 2 ) );
            //    geode.addDrawable( g );
            //    cull_group.addChild( geode );
            //}
        }
    }

    if ( getCullBackfaces() )
    {
        result.getOrCreateStateSet().setAttributeAndModes( new osg.CullFace(), osg.StateAttribute.ON );
    }

    if ( getDisableLighting() )
    {
        result.getOrCreateStateSet().setMode( GL_LIGHTING, osg.StateAttribute.OFF );
    }

    if ( getLineWidth() > 0.0f )
    {
        result.getOrCreateStateSet().setAttribute( new osg.LineWidth( line_width ), osg.StateAttribute.ON );
    }

    if ( getPointSize() > 0.0f )
    {
        osg.Point point = new osg.Point();
        point.setSize( point_size );
        result.getOrCreateStateSet().setAttribute( point, osg.StateAttribute.ON );
    }

    if ( getAlphaBlending() )
    {
        osg.BlendFunc blend_func = new osg.BlendFunc();
        //blend_func.setFunction( GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA );
        result.getOrCreateStateSet().setAttributeAndModes( blend_func, osg.StateAttribute.ON );
        result.getOrCreateStateSet().setRenderingHint( osg.StateSet.TRANSPARENT_BIN );     
    }

    if ( getRasterOverlayScript() )
    {
        ScriptResult r = env.getScriptEngine().run( getRasterOverlayScript(), env );
        if ( r.isValid() )
        {
            RasterResource* raster = env.getSession().getResources().getRaster( r.asString() );
            if ( raster )
            {
                osg.Image* image = NULL;

                std.stringstream builder;

                string cell_id = env.getProperties().getValue( "compiler.cell_id", "" );
                if ( cell_id.length() > 0 )
                {
                    builder << "r" << cell_id << ".jpg";
                }
                else
                {
                    double x = env.getExtent().getCentroid().x();
                    double y = env.getExtent().getCentroid().y();
                    builder << std.setprecision(10) << "r" << x << "x" << y << ".jpg";
                }

                if ( raster.applyToStateSet( result.getOrCreateStateSet(), env.getExtent(), getRasterOverlayMaxSize(), &image ) )
                {
                    // Add this as a skin resource so the compiler can properly localize and deploy it.
                    image.setFileName( builder.str() );

                    env.getResourceCache().addSkin( result.getOrCreateStateSet() );
                }
            }
        }
        else
        {
            env.getReport().error( r.asString() );
        }
    }

    if ( getOptimize() )
    {
        //osgGIS.notice() << "[BuildNodes] Optimizing..." << std.endl;

        osgUtil.Optimizer opt;
        int opt_mask = 
            osgUtil.Optimizer.DEFAULT_OPTIMIZATIONS |
            osgUtil.Optimizer.MERGE_GEODES |
            osgUtil.Optimizer.TRISTRIP_GEOMETRY |
            osgUtil.Optimizer.SPATIALIZE_GROUPS;

        // disable texture atlases, since they mess with our shared skin resources and
        // don't work correctly during multi-threaded building
        opt_mask &= ~osgUtil.Optimizer.TEXTURE_ATLAS_BUILDER;

        // I've seen this crash the app when dealing with certain ProxyNodes.
        // TODO: investigate this later.
        opt_mask &= ~osgUtil.Optimizer.REMOVE_REDUNDANT_NODES;

        // integrate the optimizer hints:
        opt_mask |= env.getOptimizerHints().getIncludedOptions();
        opt_mask &= ~( env.getOptimizerHints().getExcludedOptions() );

        opt.optimize( result.get(), opt_mask );

        GeometryCleaner cleaner;
        cleaner.clean( result.get() );
    }

    AttributedNodeList output;
    output.push_back( new AttributedNode( result.get() ) );

    return output;
}


        private float line_width;
        private float point_size;
        private bool draw_cluster_culling_normals;
        private Script raster_overlay_script;
        private int raster_overlay_max_size;
        private bool cull_backfaces;
        private bool apply_cluster_culling;
        private bool optimize;
        private bool disable_lighting;
        private bool embed_attrs;
        private bool alpha_blending;

        private void init()
        {
            cull_backfaces = DEFAULT_CULL_BACKFACES;
            apply_cluster_culling = DEFAULT_APPLY_CLUSTER_CULLING;
            disable_lighting = DEFAULT_DISABLE_LIGHTING;
            optimize = DEFAULT_OPTIMIZE;
            line_width = DEFAULT_LINE_WIDTH;
            point_size = DEFAULT_POINT_SIZE;
            draw_cluster_culling_normals = false;
            raster_overlay_max_size = DEFAULT_RASTER_OVERLAY_MAX_SIZE;
            embed_attrs = DEFAULT_EMBED_ATTRIBUTES;
            alpha_blending = DEFAULT_ALPHA_BLENDING;
        }


        //TODO OSGGIS_DEFINE_FILTER( BuildNodesFilter );

        private const int DEFAULT_RASTER_OVERLAY_MAX_SIZE = 0;
        private const float DEFAULT_POINT_SIZE = 0.0f;
        private const float DEFAULT_LINE_WIDTH = 0.0f;
        private const bool DEFAULT_CULL_BACKFACES = true;
        private const bool DEFAULT_APPLY_CLUSTER_CULLING = false;
        private const bool DEFAULT_DISABLE_LIGHTING = false;
        private const bool DEFAULT_OPTIMIZE = true;
        private const bool DEFAULT_EMBED_ATTRIBUTES = false;
        private const bool DEFAULT_ALPHA_BLENDING = false;


    }
}

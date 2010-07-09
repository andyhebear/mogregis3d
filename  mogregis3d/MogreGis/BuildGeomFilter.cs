﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sharp3D.Math.Core;
using Mogre;

namespace MogreGis
{
    /**
     * Assembles feature data into basic fragments (i.e. attributed drawables).
     *
     * This filter takes Feature data as input, and generates Fragment data as output. 
     * (A Fragment is an osg.Drawable with an AttributeTable.) In other words it
     * create basic OSG geometry from GIS feature data.
     */
    public class BuildGeomFilter : FragmentFilter
    {
        //TODO OSGGIS_META_FILTER( BuildGeomFilter );
        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new BuildGeomFilter(this); }
        public new static string getStaticFilterType() { return "BuildGeomFilter"; }
        public new static FilterFactory getFilterFactory() { return new FilterFactoryImpl<BuildGeomFilter>(); }     


        /**
         * Constructs a new filter for converting features into geometry.
         */
        public BuildGeomFilter()
        {
            overall_color = new Vector4D(1, 1, 1, 1);
            setRasterOverlayMaxSize(DEFAULT_RASTER_OVERLAY_MAX_SIZE);
        }

        /**
         * Copy constructor.
         */
        public BuildGeomFilter(BuildGeomFilter rhs)
            : base(rhs)
        {
            overall_color = rhs.overall_color;
            raster_overlay_max_size = rhs.raster_overlay_max_size;
            raster_overlay_script = rhs.raster_overlay_script;
            color_script = rhs.color_script;
            feature_name_script = rhs.feature_name_script;
        }


        //properties

        /**
         * Sets the overall color to assign to generated primitives.
         *
         * @param color
         *      A Vec4 color
         */
        public void setColor(Vector4D _color)
        {
            overall_color = _color;
        }

        /**
         * Gets the overall color to assign to generated primitives.
         *
         * @return OSG color vector
         */
        public Vector4D getColor()
        {
            return overall_color;
        }

        /**
         * Sets the script that evalutes to the color to apply to the geometry.
         *
         * @param script Script that generates the geometry color
         */
        public void setColorScript(Script script)
        {
            color_script = script;
        }

        /**
         * Gets the script that evaluates to the color to apply to the 
         * geometry.
         *
         * @return Script that generates the geometry color
         */
        public Script getColorScript()
        {
            return color_script;
        }

        /**
         * Sets a script that evaluates to the name of the RasterResource
         * to use to texture the geometry.
         *
         * @param script
         *      Script that generates the resource name
         */
        public void setRasterOverlayScript(Script script)
        {
            raster_overlay_script = script;
        }

        /**
         * Gets the script that evaluates to the name of the RasterResource
         * to use to texture the geometry.
         *
         * @return Script that generates the resource name
         */
        public Script getRasterOverlayScript()
        {
            return ((BuildGeomFilter)this).raster_overlay_script;
        }

        /**
         * Sets the maximum size (width or height) of a texture created from
         * the raster referenced by the raster script. Set this to 0 to disable
         * the capping.
         *
         * @param max_size
         *      Maximum width or height of the overlay texture
         */
        public void setRasterOverlayMaxSize(int max_size)
        {
            raster_overlay_max_size = max_size;
        }

        /**
         * Gets the maximum size (width or height) or a texture created from
         * the raster referenced by the raster script. A value less than or
         * equal to 0 means no capping.
         *
         * @return Maximum texture dimension
         */
        public int getRasterOverlayMaxSize()
        {
            return raster_overlay_max_size;
        }

        /**
         * Sets a script that evaluates to a string that this filter set as the
         * OSG node name.
         *
         * Important note: setting a feature name forces the filter to place 
         * each feature's geometry under a separate Geode. This prevents certain
         * optimizations (such as merging geometries) and can ultimatley affect
         * runtime performance.
         *
         * @param script
         *      Script that generates the feature node name
         */
        public void setFeatureNameScript(Script script)
        {
            feature_name_script = script;
        }

        /**
         * Gets a script that evaluates to the string that this filter sets as the
         * OSG node name.
         *
         * @return
         *      Script that generates the feature node name
         */
        public Script getFeatureNameScript()
        {
            return feature_name_script;
        }

        // Filter overrides    
        public override void setProperty(Property prop)
        {
            if (prop.getName() == "color")
                setColorScript(new Script(prop.getValue()));
            else if (prop.getName() == "raster_overlay")
                setRasterOverlayScript(new Script(prop.getValue()));
            else if (prop.getName() == "raster_overlay_max_size")
                setRasterOverlayMaxSize(prop.getIntValue(DEFAULT_RASTER_OVERLAY_MAX_SIZE));
            else if (prop.getName() == "feature_name")
                setFeatureNameScript(new Script(prop.getValue()));

            base.setProperty(prop);
        }

        public override Properties getProperties()
        {
            Properties p = base.getProperties();
            if (getColorScript() != null)
                p.Add(new Property("color", getColorScript().getCode()));
            if (getRasterOverlayScript() != null)
                p.Add(new Property("raster_overlay", getRasterOverlayScript().getCode()));
            if (getRasterOverlayMaxSize() != DEFAULT_RASTER_OVERLAY_MAX_SIZE)
                p.Add(new Property("raster_overlay_max_size", getRasterOverlayMaxSize()));
            if (getFeatureNameScript() != null)
                p.Add(new Property("feature_name", getFeatureNameScript()));
            return p;
        }


        // FragmentFilter overrides
        public override FragmentList process(FeatureList input, FilterEnv env)
        {
#if TODO
            // if features are arriving in batch, resolve the color here.
            // otherwise we will resolve it later in process(feature,env).
            is_batch = input.Count > 1;
            batch_feature_color = overall_color;
            if (is_batch && getColorScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getColorScript(), env);
                if (r.isValid())
                    batch_feature_color = r.asVec4();
                else
                    env.getReport().error(r.asString());
            }

            return base.process(input, env);
#endif 
            throw new NotImplementedException();
        }

        public override FragmentList process(Feature input, FilterEnv env)
        {
#if TODO
            FragmentList output;

            // LIMITATION: this filter assumes all feature's shapes are the same
            // shape type! TODO: sort into bins of shape type and create a separate
            // geometry for each. Then merge the geometries.
            bool needs_tessellation = false;

            Fragment frag = new Fragment();

            GeoShapeList shapes = input.getShapes();

            // if we're in batch mode, the color was resolved in the other process() function.
            // otherwise we still need to resolve it.
            Vector4D color = getColorForFeature(input, env);

            foreach (GeoShape s in shapes)
            {
                GeoShape shape = s;

                if (shape.getShapeType() == GeoShape.ShapeType.TYPE_POLYGON)
                {
                    needs_tessellation = true;
                }

                osg.Geometry geom = new osg.Geometry();

                // TODO: pre-total points and pre-allocate these arrays:
                osg.Vec3Array verts = new osg.Vec3Array();
                geom.setVertexArray(verts);
                uint vert_ptr = 0;

                // per-vertex coloring takes more memory than per-primitive-set coloring,
                // but it renders faster.
                osg.Vec4Array colors = new osg.Vec4Array();
                geom.setColorArray(colors);
                geom.setColorBinding(osg.Geometry.BIND_PER_VERTEX);

                //osg.Vec3Array* normals = new osg.Vec3Array();
                //geom.setNormalArray( normals );
                //geom.setNormalBinding( osg.Geometry.BIND_OVERALL );
                //normals.push_back( osg.Vec3( 0, 0, 1 ) );


                Mogre.PixelFormat prim_type =
                    shape.getShapeType() == GeoShape.ShapeType.TYPE_POINT ? osg.PrimitiveSet.POINTS :
                    shape.getShapeType() == GeoShape.ShapeType.TYPE_LINE ? osg.PrimitiveSet.LINE_STRIP :
                    osg.PrimitiveSet.LINE_LOOP;

                for (int pi = 0; pi < shape.getPartCount(); pi++)
                {
                    int part_ptr = vert_ptr;
                    GeoPointList points = shape.getPart(pi);
                    for (int vi = 0; vi < points.Count; vi++)
                    {
                        verts.Add(points[vi]);
                        vert_ptr++;
                        colors.Add(color);
                    }
                    geom.addPrimitiveSet(new osg.DrawArrays(prim_type, part_ptr, vert_ptr - part_ptr));
                }

                // tessellate all polygon geometries. Tessellating each geometry separately
                // with TESS_TYPE_GEOMETRY is much faster than doing the whole bunch together
                // using TESS_TYPE_DRAWABLE.
                if (needs_tessellation)
                {
                    osgUtil.Tessellator tess;
                    tess.setTessellationType(osgUtil.Tessellator.TESS_TYPE_GEOMETRY);
                    tess.setWindingType(osgUtil.Tessellator.TESS_WINDING_POSITIVE);
                    tess.retessellatePolygons(*geom);

                    applyOverlayTexturing(geom, input, env);
                }

                generateNormals(geom);

                frag.addDrawable(geom);
            }

            frag.addAttributes(input.getAttributes());
            applyFragmentName(frag, input, env);

            output.Add(frag);

            return output;
#endif
            throw new NotImplementedException();
        }

#if TODO
        protected virtual Vector4D getColorForFeature(Feature input, FilterEnv env)
        {

            Vector4D result = overall_color;

            if (is_batch)
            {
                result = batch_feature_color;
            }
            else if (getColorScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getColorScript(), input, env);
                if (r.isValid())
                    result = r.asVec4();
                else
                    env.getReport().error(r.asString());
            }

            return result;
        }
        protected void applyFragmentName(Fragment frag, Feature feature, FilterEnv env)
        {
            if (getFeatureNameScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getFeatureNameScript(), feature, env);
                if (r.isValid())
                    frag.setName(r.asString());
                else
                    env.getReport().error(r.asString());
            }
        }

        protected void applyOverlayTexturing(osg.Geometry geom, Feature input, FilterEnv env)
        {
            GeoExtent tex_extent;

            if (getRasterOverlayScript() != null)
            {
                // if there's a raster script for this filter, we're applying textures per-feature:
                tex_extent = new GeoExtent(
                    input.getExtent().getSouthwest().getAbsolute(),
                    input.getExtent().getNortheast().getAbsolute());
            }
            else
            {
                // otherwise prepare the geometry for an overlay texture covering the entire working extent:
                tex_extent = env.getExtent();
            }

            float width = (float)tex_extent.getWidth();
            float height = (float)tex_extent.getHeight();

            // now visit the verts and calculate texture coordinates for each one.
            osg.Vec3Array verts = (osg.Vec3Array)(geom.getVertexArray());
            if (verts != null)
            {
                // if we are dealing with geocentric data, we will need to xform back to a real
                // projection in order to determine texture coords:
                GeoExtent tex_extent_geo;
                if (env.getInputSRS().isGeocentric())
                {
                    tex_extent_geo = new GeoExtent(
                        tex_extent.getSRS().getGeographicSRS().transform(tex_extent.getSouthwest()),
                        tex_extent.getSRS().getGeographicSRS().transform(tex_extent.getNortheast()));
                }

                osg.Vec2Array texcoords = new osg.Vec2Array(verts.size());
                for (int j = 0; j < verts.size(); j++)
                {
                    // xform back to raw SRS w.o. ref frame:
                    GeoPoint vert = new GeoPoint(verts[j], env.getInputSRS());
                    GeoPoint vert_map = vert.getAbsolute();
                    float tu, tv;
                    if (env.getInputSRS().isGeocentric())
                    {
                        tex_extent_geo.getSRS().transformInPlace(vert_map);
                        tu = (vert_map.X - tex_extent_geo.getXMin()) / width;
                        tv = (vert_map.Y - tex_extent_geo.getYMin()) / height;
                    }
                    else
                    {
                        tu = (vert_map.X - tex_extent.getXMin()) / width;
                        tv = (vert_map.Y - tex_extent.getYMin()) / height;
                    }
                    (*texcoords)[j].set(tu, tv);
                }
                geom.setTexCoordArray(0, texcoords);
            }

            // if we are applying the raster per-feature, do so now.
            // TODO: deprecate? will we ever use this versus the BuildNodesFilter overlay? maybe
            if (getRasterOverlayScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getRasterOverlayScript(), input, env);
                if (r.isValid())
                {
                    RasterResource raster = env.getSession().getResources().getRaster(r.asString());
                    if (raster != null)
                    {
                        Image image = null;
                        std.stringstream builder;
                        builder << "rtex_" << input.getOID() << ".jpg"; //TODO: dds with DXT1 compression

                        osg.StateSet raster_ss = new osg.StateSet();
                        if (raster.applyToStateSet(raster_ss.get(), tex_extent, getRasterOverlayMaxSize(), out image))
                        {
                            image.setFileName(builder.str());
                            geom.setStateSet(raster_ss.get());

                            // add this as a skin resource so the compiler can properly localize and deploy it.
                            env.getResourceCache().addSkin(raster_ss.get());
                        }
                    }
                }
                else
                {
                    env.getReport().error(r.asString());
                }
            }
        }

        protected void generateNormals(osg.Geometry geom)
        {
            if (geom != null)
            {
                osgUtil.SmoothingVisitor smoother;
                smoother.smooth(out geom);
            }
        }
#endif

        protected Script color_script;
        protected Script feature_name_script;
        protected Vector4D overall_color;
        protected Script raster_overlay_script;
        protected int raster_overlay_max_size;
        protected bool embed_attributes;

        // transients
        private bool is_batch;
        private Vector4D batch_feature_color;

        //TODO OSGGIS_DEFINE_FILTER( BuildGeomFilter );

        private const int DEFAULT_RASTER_OVERLAY_MAX_SIZE = 0;

    }
}
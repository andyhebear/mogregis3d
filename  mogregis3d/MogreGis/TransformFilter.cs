using System;
using System.Drawing.Drawing2D;
using SharpMap.CoordinateSystems.Transformations;

namespace MogreGis
{
    /**
         * Transforms feature data by way of spatial reference reprojtion and/or
         * matrix transformation.
         * 
         * If you set an SRS (or set an implicit SRS by way of setUseTerrainSRS(true),
         * the filter performs the SRS transformation before applying any matrix
         * transformations.
         */
    public class TransformFilter : FeatureFilter
    {

        //TODO OSGGIS_META_FILTER( TransformFilter );

        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new TransformFilter(this); }
        public static string getStaticFilterType() { return "TransformFilter"; }
        public static FilterFactory getFilterFactory() { return new FilterFactoryImpl<TransformFilter>(); }

        private const bool DEFAULT_LOCALIZE = false;
        private const bool DEFAULT_USE_TERRAIN_SRS = false;


        /**
         * Constructs a new transform filter.
         */
        public TransformFilter()
        {
            xform_matrix = new Matrix(); // the identity matrix
            localize = DEFAULT_LOCALIZE;
            use_terrain_srs = DEFAULT_USE_TERRAIN_SRS;
        }

        /**
         * Copy constructor.
         */
        public TransformFilter(TransformFilter rhs)
#if TODO
            : base(rhs)
#endif
        {
            xform_matrix = rhs.xform_matrix;
            localize = rhs.localize;
            use_terrain_srs = rhs.use_terrain_srs;
            srs = rhs.srs;
            srs_script = rhs.srs_script;
            translate_script = rhs.translate_script;
        }

        /**
         * Constucts a new transform filter.
         *
         * @param matrix
         *      Matrix to use to transform feature data.
         */
        public TransformFilter(Matrix matrix)
        {
            throw new NotImplementedException();
        }


        //properties

        /**
         * Sets a transform matrix to use to perform a cartesian transformation
         * on feature shape data.
         *
         * @param xform_matrix
         *      Matrix to use to transform geodata. Note that SRS reprojection
         *      happens before matrix transformation.
         */
        public void setMatrix(Matrix _matrix)
        {
            xform_matrix = _matrix;
        }

        /**
         * Sets a transform matrix to use to perform a cartesian transformation
         * on feature shape data.
         * 
         * @return A transformation matrix.
         */
        public Matrix getMatrix()
        {
            return xform_matrix;
        }

        /**
         * Sets the spatial reference system into which to reproject feature geodata.
         *
         * @param srs
         *      Target spatial reference system.
         */
        public void setSRS(SpatialReference _srs)
        {
            srs = _srs;
        }

        /**
         * Gets the spatial reference system into which the filter will reproject
         * feature geodata.
         *
         * @return
         *      A spatial reference system.
         */
        public SpatialReference getSRS()
        {
            return srs;
        }

        /**
         * Indicates whether the filter should transform features into the terrain's
         * SRS as reported by the FilterEnv at compile time.
         */
        public void setUseTerrainSRS(bool value)
        {
            use_terrain_srs = value;
        }

        /**
         * Gets whether the filter should use the terrain SRS (as reported by the
         * FilterEnv at compile-time).
         */
        public bool getUseTerrainSRS()
        {
            return use_terrain_srs;
        }

        /**
         * Sets whether to localize the feature geodata by transforming it so it is
         * relative to the centroid of the graph's working extent. This is the standard
         * way to create a "local origin" for a grid cell so as to avoid precision
         * problems.
         *
         * @param value
         *      True to localize geodata; false to not localize
         */
        public void setLocalize(bool value)
        {
            localize = value;
        }

        /**
         * Gets whether to localize the feature geodata by transforming it so it is
         * relative to the centroid of the graph's working extent.
         *
         * @return True to localize, false to not localize.
         */
        public bool getLocalize()
        {
            return localize;
        }


        // script functions

        /**
         * Sets a script that will evaluate to the name of an SRSResource to use for
         * feature data reprojection.
         *
         * @param value
         *      Script that generates an SRSResource name
         */
        public void setSRSScript(Script value)
        {
            srs_script = value;
        }

        /**
         * Gets a script that will evaluate to the  name of an SRSResource to use for
         * feature data reprojection.
         *
         * @return Script that generates an SRSResource name
         */
        public Script getSRSScript()
        {
            return srs_script;
        }

        /**
         * Sets a script that will evaluate to a vec3 to use for cartesian translation
         * of the feature shape data.
         *
         * @param script
         *      Script that generates a vec3
         */
        public void setTranslateScript(Script value)
        {
            translate_script = value;
        }

        /**
         * Gets a script the evaluates to a vec3 to use for cartesian translation of
         * the feature shape data.
         */
        public Script getTranslateScript()
        {
            return translate_script;
        }



        public override FeatureList process(FeatureList input, FilterEnv env)
        {
#if TODO
            // first time through, establish a working SRS for output data.
            if (working_srs == null)
            {
                // first try to use the terrain SRS if so directed:
                SpatialReference new_out_srs = getUseTerrainSRS() ? env.getTerrainSRS() : null;
                if (new_out_srs == null)
                {
                    // failing that, see if we have an SRS in a resource:
                    if (getSRS() == null && getSRSScript() != null)
                    {
                        ScriptResult r = env.getScriptEngine().run(getSRSScript(), env);
                        if (r.isValid())
                            setSRS(env.getSession().getResources().getSRS(r.ToString()));
                        else
                            env.getReport().error(r.ToString());
                    }

                    new_out_srs = srs;
                }

                // set the "working" SRS that will be used for all features passing though this filter:
                working_srs = new_out_srs != null ? new_out_srs : env.getInputSRS();

                // LOCALIZE points around a local origin (the working extent's centroid)
                if (working_srs != null && getLocalize()) //&& env.getExtent().getArea() > 0.0 )
                {
                    if (env.getCellExtent().getSRS().isGeographic() && env.getCellExtent().getWidth() > 179.0)
                    {
                        //NOP - no localization for big geog extent ... needs more thought perhaps
                    }
                    else
                    {
                        GeoPoint centroid0 = new_out_srs != null ?
                            new_out_srs.transform(env.getCellExtent().getCentroid()) :
                            env.getCellExtent().getCentroid();

                        // we do want the localizer point on the surface if possible:
                        GeoPoint centroid = clampToTerrain(centroid0, env);
                        if (centroid == null)
                            centroid = centroid0;

                        Matrixd localizer;

                        // For geocentric datasets, we need a special localizer matrix:
                        if (working_srs.isGeocentric())
                        {
                            localizer = working_srs.getEllipsoid().createGeocentricInvRefFrame(centroid);
                            localizer.invert(localizer);
                        }

                        // For projected datasets, just a simple translation:
                        else
                        {
                            localizer = osg.Matrixd.translate(-centroid);
                        }

                        working_srs = working_srs.cloneWithNewReferenceFrame(localizer);
                    }
                }
            }

            // we have to assign the output SRS on each pass
            if (working_srs != null)
            {
                env.setOutputSRS(working_srs);
            }

            return base.process(input, env);
#endif
            throw new NotImplementedException();
        }

        public override FeatureList process(Feature input, FilterEnv env)
        {
            FeatureList output = new FeatureList();
            if (transform != null && !transform.Identity())
            {
                foreach (GeoShape shape in input.getShapes())
                {
                        XformVisitor visitor = new XformVisitor();
                        visitor.trans = transform;
                        shape.accept(visitor);
                }
            }

            output.Add(input);
            return output;
#if TODO
            // resolve the xlate shortcut
            Matrix working_matrix = xform_matrix;

            // TODO: this can go into process(FeatureList) instead of running for every feature..
            if (getTranslateScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getTranslateScript(), input, env);
                if (r.isValid())
                    working_matrix = Matrix.translate(r.asVec3());
                else
                    env.getReport().error(r.asString());
            }

            if (working_srs != null || (working_matrix != null && !working_matrix.IsIdentity))
            {
                foreach (GeoShape shape in input.getShapes())
                {
                    if (working_matrix != null && !working_matrix.IsIdentity)
                    {

                        XformVisitor visitor;
                        visitor.mat = working_matrix;
                        shape.accept(visitor);
                    }

                    if (working_srs != null && !working_srs.equivalentTo(env.getInputSRS()))
                    {
                        working_srs.transformInPlace(shape);
                    }
                }
            }

            output.Add(input);
            return output;
#endif
            throw new NotImplementedException();
        }

        class XformVisitor : GeoPointVisitor
        {
            public IMathTransform trans;
            public override GeoPoint visitPoint(GeoPoint p)
            {
                return new GeoPoint(trans.Transform(p.ToDoubleArray()));
#if TODO
                p.set(p * mat);
                p.setDim(3);
                return true;
#endif 
            }
        }

        static GeoPoint clampToTerrain(GeoPoint input, FilterEnv env)
        {
#if TODO
            if (env.getTerrainNode() != null)
            {
                return GeomUtil.clampToTerrain(input, env.getTerrainNode(), env.getTerrainSRS(), env.getTerrainReadCallback());
            }
            else
            {
                return input;
            }
            //if ( env.getTerrainNode() )
            //{
            //    osg.ref_ptr<osgUtil.LineSegmentIntersector> isector;

            //    if ( input.getSRS().isGeocentric() )
            //    {
            //        osg.Vec3d vec = input;
            //        vec.normalize();    
            //        isector = new osgUtil.LineSegmentIntersector(
            //            vec * input.getSRS().getEllipsoid().getSemiMajorAxis() * 1.2,
            //            osg.Vec3d(0,0,0) );
            //    }
            //    else
            //    {
            //        osg.Vec3d p = input;
            //        osg.Vec3d vec(0,0,1);
            //        isector = new osgUtil.LineSegmentIntersector(
            //            p + vec * 1e7,
            //            p - vec * 1e7 );
            //    }

            //    RelaxedIntersectionVisitor iv;
            //    iv.setIntersector( isector.get() );
            //    iv.setReadCallback( env.getTerrainReadCallback() );
            //    
            //    env.getTerrainNode().accept( iv );
            //    if ( isector.containsIntersections() )
            //    {
            //        return GeoPoint(
            //            isector.getFirstIntersection().getWorldIntersectPoint(),
            //            input.getSRS() );
            //    }
            //}
            //return input;
#endif
            throw new NotImplementedException();
        }

        public IMathTransform Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        public override void setProperty(Property p)
        {
            if (p.getName() == "localize")
                setLocalize(p.getBoolValue(getLocalize()));
            else if (p.getName() == "translate")
                setTranslateScript(new Script(p.getValue()));
            else if (p.getName() == "use_terrain_srs")
                setUseTerrainSRS(p.getBoolValue(getUseTerrainSRS()));
            else if (p.getName() == "srs")
                setSRSScript(new Script(p.getValue()));
            base.setProperty(p);
        }

        public override Properties getProperties()
        {
            Properties p = base.getProperties();
            if (getLocalize() != DEFAULT_LOCALIZE)
                p.Add(new Property("localize", getLocalize()));
            if (getUseTerrainSRS() != DEFAULT_USE_TERRAIN_SRS)
                p.Add(new Property("use_terrain_srs", getUseTerrainSRS()));
            if (getTranslateScript() != null)
                p.Add(new Property("translate", getTranslateScript().getCode()));
            if (getSRSScript() != null)
                p.Add(new Property("srs", getSRSScript().getCode()));
            return p;
        }


        // properties
        private Script srs_script;
        private Script translate_script;
        private bool localize;
        private bool use_terrain_srs;
        private SpatialReference srs;

        // transient
        private Matrix xform_matrix;
        private IMathTransform transform;
        private SpatialReference working_srs;
    }
}

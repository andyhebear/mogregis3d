﻿/**
 * TransformFilter
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{


    /**
     * Transforms feature data by way of spatial reference reprojtion and or 
     * matrix tranformation.
     * 
     * If you ser an SRS (or set an implicit SRS by way of serUseTerrainSRS(true),
     * the filter performs the SRS transformation before applying any matrix
     * transformations.
     */
    public class TransformFilter : FeatureFilter
    {
        #region XformVisitor
        public class XformVisitor : GeoPointVisitor
        {
            public Mogre.Matrix4 mat;
            public bool visitPoint(GeoPoint p)
            {
                //p.setSpatialReference (p * mat);
                p.setDim(3);
                return true;
            }
        }
        #endregion

        #region CONSTANTES
        public const Boolean DEFAULT_LOCALIZE = false;
        public const Boolean DEFAULT_USE_TERRAIN_SRS = false;
        #endregion

        #region CONSTRUCTORES
        /**
         * Constructs a new transform filter.
         */
        public TransformFilter()
        {
            Matrix = new Mogre.Matrix4();
            Localize = DEFAULT_LOCALIZE;
            UseTerrainSrs = DEFAULT_USE_TERRAIN_SRS;
        }

        /**
         * Copy constructor.
         */
        public TransformFilter(TransformFilter rhs)
        {
            throw new NotImplementedException();
#if TODO_PH
            Matrix = rhs.Matrix;
            Localize = rhs.Localize;
            UseTerrainSrs = rhs.UseTerrainSrs;
            Srs = rhs.Srs; //rhs.Srs.cloneWithNewReferenceFrame(Matrix); En la implementacion actual utiliza la matrid de sharpMap.
            SrsScript = rhs.SrsScript;
            TranslateScript = rhs.TranslateScript;
#endif
        }

        /**
         * Constructs a new transform filter.
         * 
         * @param matrix
         *      Matrix to use to transform feature data.
         */
        public TransformFilter(Mogre.Matrix4 matrix)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region PROPIEDADES

        private SpatialReference workingSrs;

        public Mogre.Matrix4 Matrix
        {
            /**
            * Sets a transform matrix to use to perform a cartesian transformation
            * on feature shape data.
            * 
            * @param xform_matrix
            *      Matrix to use to transform geodata. Note that SRS reprojection
            *      happens before matrix transformation.
            */
            set
            {
                Matrix = value;
            }

            /**
             * Gets a tranform matrix to  use to perform a cartesian transformation
             * on feature saphe data.
             * 
             * @return a transformation matrix.
             */
            get
            {
                return Matrix;
            }
        }

        public SpatialReference Srs
        {
            /**
             * Sets the spatial reference system into which to reproject feature geodata.
             * 
             * @param srs
             *      Target spatial reference system.
             */
            set
            {
                Srs = value;
            }
            /**
             * Gets the spatial reference sytem into which the filter will reproject
             * feature geodata.
             * 
             * @return
             *      A spatial reference system.
             */
            get
            {
                return Srs;
            }
        }

        public Boolean UseTerrainSrs
        {
            /**
             * Indicates whether the filter should transform fatures into the terrain's
             * SRS as reporeted by the FilterEnv at compile time.
             */
            set
            {
                UseTerrainSrs = value;
            }
            /**
             * Gets whether the filter should use the terrain SRS (as reported by the
             * FilterEnv at compile time
             */
            get
            {
                return UseTerrainSrs;
            }
        }

        public Boolean Localize
        {
            /**
             * Sets whther to localize the feature geodata by tranforming it so it is
             * relative to the centroid of the graph's working extent. Tis is the standard
             * way to create a "local origin" for a grid cell so as to avoid precision
             * problems.
             * 
             * @param enabled
             *      True to localize geodata; false to not localize
             */
            set
            {
                Localize = value;
            }

            /**
             * Gets whether to localize the feature geodata by transforming it so it is
             * relative to the centroid of the graph's working excent.
             * 
             * @return True to localize, false to not localize.
             */
            get
            {
                return Localize;
            }
        }
        #endregion

        #region METODOS
        //Script functions

        public Script SrsScript
        {
            /**
             * Sets a script that will evaluate to the name of an SRSResource to use for
             * feature data reprojection.
             * 
             * @param value
             *      Script that generates an SRSResource name     
             */
            set
            {
                SrsScript = value;
            }

            /**
             * Gets a script that will evaluate to the name of an SRSResource o use for
             * feature data reprojection.
             * 
             * @return Script that generates an SRSResource name
             */
            get
            {
                return SrsScript;
            }
        }

        public Script TranslateScript
        {
            /**
             * Setes a script that will evaluate to a vec3 to use for cartesian translation
             * of the feature shape data.
             * 
             * @param script
             *      Script that generates a vec3
             */
            set
            {
                TranslateScript = value;
            }

            /**
             * Gets a script the evaluates to a vec3 to use for cartesian translation of
             * the feature shape data.
             */
            get
            {
                return TranslateScript;
            }
        }

        override public FeatureList process(Feature input, FilterEnv env)
        {
            FeatureList output = new FeatureList();

            //resolve the xlate shortcut
            Mogre.Matrix4 workingMatrix = Matrix;

            //TODO: this can go into process (FeatureList) instead of running for every feature..
            if (TranslateScript != null)
            {
                ScriptResult r = env.getScriptEngine().run(TranslateScript, input, env);
                if (r.isValid())
                {
                    workingMatrix.MakeTrans(new Mogre.Vector3((float)r.asVec3().X, (float)r.asVec3().Y, (float)r.asVec3().Z));
                }
                else
                {
                    env.getReport().error(r.asString());
                }
            }
            if (workingSrs != null || (workingMatrix != null && workingMatrix != Mogre.Matrix4.IDENTITY))
            {
                foreach (GeoShape shape in input.getShapes())
                {
                    if (workingMatrix != null && !workingMatrix.Equals(Mogre.Matrix4.IDENTITY))
                    {

                        XformVisitor visitor = new XformVisitor();
                        visitor.mat = workingMatrix;
                        shape.accept(visitor);
                    }
                    if (workingSrs != null && !(workingSrs.equivalentTo(env.getInputSRS())))
                    {
                        workingSrs.transformInPlace(shape);
                    }
                }
            }
            output.Add(input);
            return output;
        }

        override public FeatureList process(FeatureList input, FilterEnv env)
        {
            //first time through, establish a working SRS for output data.
            if (workingSrs == null)
            {
                //first try to use the terrain SRS if so directed:
                SpatialReference newOutSrs = UseTerrainSrs ? env.getTerrainSRS() : null;
                if (newOutSrs == null)
                {
                    //failing that, see if we have an SRS in a resource:
                    if (Srs == null && SrsScript != null)
                    {
                        ScriptResult r = env.getScriptEngine().run(SrsScript, env);
                        if (r.isValid())
                        {
#if TODO_PH
                            Srs = (env.getSession().Resources.getSRS(r.asString()));
#endif
                            throw new NotImplementedException();
                        }
                        else
                        {
                            env.getReport().error(r.asString());
                        }
                    }
                    newOutSrs = Srs;
                }
                //set the "working" SRS that will be used for all features passing though this filter:
                workingSrs = newOutSrs == null ? newOutSrs : env.getInputSRS();

                //LOCALIZE points arround a local origin (the working extent's centroid)
                if (workingSrs != null && Localize)
                {
                    if (env.getCellExtent().getSRS().isGeographic() && env.getCellExtent().getWidth() > 179)
                    {
                        //NOP - no localization for big geog extent ... needs more thought perhaps
                    }
                    else
                    {
                        GeoPoint centroid0 = newOutSrs == null ?
                            newOutSrs.transform(env.getCellExtent()).getCentroid()
                            : env.getCellExtent().getCentroid();
                        //we do want the localizer point on the surface if possible:
                        GeoPoint centroid = ClampToTerrain(centroid0, env);
                        if (centroid == null)
                        {
                            centroid = centroid0;
                        }

                        Mogre.Matrix4 localizer = new Mogre.Matrix4();
                        //For geocentric datasets, we need a special localizer matrix:
                        if (workingSrs.isGeocentric())
                        {
                            localizer = workingSrs.getEllipsoid().createGeocentricInvRefFrame(centroid);
                            localizer = localizer.Inverse();
                        }
                        //For projected datasets, just a simple translation
                        else
                        {
                            localizer.SetTrans(new Mogre.Vector3((float)centroid.X, (float)centroid.Y, (float)0.0));
                        }
                        workingSrs = workingSrs.cloneWithNewReferenceFrame(localizer);
                    }
                }
            }
            //we have to assing the output SRS on each pass
            if (workingSrs != null)
            {
                env.setOutputSRS(workingSrs);
            }
            return base.process(input, env);
        }

        public void SetProperty(Property p)
        {
            if (p.getName() == "localize")
            {
                Localize = p.getBoolValue(Localize);
            }
            else if (p.getName() == "translate")
            {
                TranslateScript = new Script(p.getValue());
            }
            else if (p.getName() == "use_terrain_srs")
            {
                UseTerrainSrs = p.getBoolValue(UseTerrainSrs);
            }
            else if (p.getName() == "srs")
            {
                SrsScript = new Script(p.getValue());
            }
            base.setProperty(p);
        }

        override public Properties getProperties()
        {
            Properties p = base.getProperties();
            if (Localize != DEFAULT_LOCALIZE)
            {
                p.Add(new Property("localize", Localize));
            }
            if (UseTerrainSrs != DEFAULT_USE_TERRAIN_SRS)
            {
                p.Add(new Property("use_terrain_srs", UseTerrainSrs));
            }
            if (TranslateScript != null)
            {
                p.Add(new Property("translate", TranslateScript.getCode()));
            }
            if (SrsScript != null)
            {
                p.Add(new Property("srs", SrsScript.getCode()));
            }
            return p;
        }

#if IMPLETMENTADO_EN_UTILScppIIIIIIOSGgis
                                                                                                                                                                                    /**
         * GeoPoint

GeomUtils::clampToTerrain( const GeoPoint& input, osg::Node* terrain, SpatialReference* terrain_srs, SmartReadCallback* reader )

{

    GeoPoint output = GeoPoint::invalid();



    if ( terrain && terrain_srs )

    {

        double out_hat = 0;

        osg::ref_ptr<LineSegmentIntersector2> isector =

            createClampingIntersector( input, out_hat );



        //GeoPoint p_world = terrain_srs->transform( input );



        //osg::Vec3d clamp_vec;

        //osg::ref_ptr<osgUtil::LineSegmentIntersector> isector;



        //if ( terrain_srs->isGeocentric() )

        //{

        //    clamp_vec = p_world;

        //    clamp_vec.normalize();



        //    isector = new osgUtil::LineSegmentIntersector(

        //        clamp_vec * terrain_srs->getEllipsoid().getSemiMajorAxis() * 1.2,

        //        osg::Vec3d(0, 0, 0) );

        //}

        //else

        //{

        //    clamp_vec.set(0, 0, 1);

        //    osg::Vec3d ext_vec = clamp_vec * 1e6;

        //    isector = new LineSegmentIntersector(

        //        p_world + ext_vec,

        //        p_world - ext_vec );

        //}



        RelaxedIntersectionVisitor iv;

        iv.setIntersector( isector.get() );

        iv.setReadCallback( reader );



        //IntersectionVisitor iv;

        //iv.setIntersector( isector.get() );

        //iv.setReadCallback( new SimpleReader() );



        terrain->accept( iv );

        if ( isector->containsIntersections() )

        {

            output = GeoPoint( isector->getFirstIntersection().getWorldIntersectPoint(), terrain_srs );

        }

    }
         * 
         */
#endif

        private static GeoPoint ClampToTerrain(GeoPoint input, FilterEnv env)
        {
            //if (env.getTerrainNode() != null)
            //{
            //    //Falta por implementar GeomUtils.ClampToTerrain y env.getTerrainReadCallback.
            //    //return GeomUtils.ClampToTerrain (input, env.getTerrainNode(), env.getTerrainSRS(),env.getTerrainReadCallback());
            //    return
            //}
            //else
            //{
            return input;
            //}
        }

        override public MogreGis.Filter clone()
        {
            return new TransformFilter(this);
        }

        override public string getFilterType()
        {
            return "TRANSFORMFILTER";
        }
    }
        #endregion
}
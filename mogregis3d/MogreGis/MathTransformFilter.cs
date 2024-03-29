﻿#if TODO_PH
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;
//using SharpMap.CoordinateSystems.Transformations;
//using SharpMap.CoordinateSystems;

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
    public class MathTransformFilter : FeatureFilter
    {

        //TODO OSGGIS_META_FILTER( TransformFilter );

        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new MathTransformFilter(this); }
        public static string getStaticFilterType() { return "MathTransformFilter"; }
        public static FilterFactory getFilterFactory() { return new FilterFactoryImpl<MathTransformFilter>(); }



        private const bool DEFAULT_LOCALIZE = false;
        private const bool DEFAULT_USE_TERRAIN_SRS = false;


        /**
         * Constructs a new transform filter.
         */
        public MathTransformFilter()
        {
            xform_matrix = new Matrix(); // the identity matrix
            localize = DEFAULT_LOCALIZE;
            use_terrain_srs = DEFAULT_USE_TERRAIN_SRS;
        }

        /**
         * Copy constructor.
         */
        public MathTransformFilter(MathTransformFilter rhs)
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
        public MathTransformFilter(Matrix matrix)
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
            FeatureList output = new FeatureList();

            // HACER ALGO DEL ESTILO:

            if (transform == null)
            {
                //Create zone UTM 32N projection
                IProjectedCoordinateSystem utmProj = CreateUtmProjection(32);

                //Create geographic coordinate system (lets just reuse the CS from the projection)
                IGeographicCoordinateSystem geoCS = utmProj.GeographicCoordinateSystem;

                //Create transformation
                CoordinateTransformationFactory ctFac = new CoordinateTransformationFactory();

                // TODO DANI Mirar de donde viene este source y target
                ICoordinateTransformation Coordinatetransform = null;// TODO = ctFac.CreateFromCoordinateSystems(source, target);

                //cs
                string wkt = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
                //ICoordinateSystem cs = SharpMap.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wkt) as ICoordinateSystem;
                ICoordinateSystem cs = ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wkt) as ICoordinateSystem;
                //wgs84
                GeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

                //gcs
                CoordinateSystemFactory cFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
                //CoordinateSystemFactory cFac = new SharpMap.CoordinateSystems.CoordinateSystemFactory();
                //Create Bessel 1840 geographic coordinate system
                IEllipsoid ellipsoid = cFac.CreateFlattenedSphere("Bessel 1840", 6377397.155, 299.15281, LinearUnit.Metre);
                IHorizontalDatum datum = cFac.CreateHorizontalDatum("Bessel 1840", DatumType.HD_Geocentric, ellipsoid, null);
                IGeographicCoordinateSystem gcs = cFac.CreateGeographicCoordinateSystem("Bessel 1840", AngularUnit.Degrees, datum,
                    PrimeMeridian.Greenwich, new AxisInfo("Lon", AxisOrientationEnum.East),
                    new AxisInfo("Lat", AxisOrientationEnum.North));

                //coordsys
                //Collection<ProjectionParameter> parameters = new Collection<ProjectionParameter>(5);
                List<ProjectionParameter> parameters = new List<ProjectionParameter>();
                parameters.Add(new ProjectionParameter("latitude_of_origin", 0));
                parameters.Add(new ProjectionParameter("central_meridian", 110));
                parameters.Add(new ProjectionParameter("scale_factor", 0.997));
                parameters.Add(new ProjectionParameter("false_easting", 3900000));
                parameters.Add(new ProjectionParameter("false_northing", 900000));
                IProjection projection = cFac.CreateProjection("Mercator_1SP", "Mercator_1SP", parameters);
                IProjectedCoordinateSystem coordsys =
               cFac.CreateProjectedCoordinateSystem("Makassar / NEIEZ", gcs, projection, LinearUnit.Metre,
                                                    new AxisInfo("East", AxisOrientationEnum.East),
                                                    new AxisInfo("North", AxisOrientationEnum.North));

                Coordinatetransform = ctFac.CreateFromCoordinateSystems(gcs, coordsys);//gcsWGS84 -> gcenCsWGS84

                //Apply transformation
                transform = Coordinatetransform.MathTransform;

            }

            SharpMap.Geometries.Point p = new SharpMap.Geometries.Point(30.0, 20.0);

            p = GeometryTransform.TransformPoint(p,transform);
/*IMPORTANTE
            foreach (Feature feature in input)
            {
                feature.row.Geometry = GeometryTransform.TransformGeometry(feature.row.Geometry, transform);
                //feature.row.Geometry = GeometryTransform.TransformMultiPolygon(feature.row.Geometry, transform);
            }
IMPORTANTE*/
            foreach (Feature f in input)
            {
                output.Add(f);//output = input
            }

            // Cosas a cambiar:
            // Primero, la construccion del transform está siguiendo el ejemplo, pero hay que tener en cuenta los datos del xml y construirlo en consecuencia
            // Segundo, el filtro debe retornar una NUEVA lista, y no modificar la inicial. Ahora modifica los valores de la lista inicial
            // IMPORTANTE RETORNAR NUEVA LISTA OUTPUT <----------- FALTA POR HACER
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
            //throw new NotImplementedException();

            if (successor != null)
            {
                if (successor is FeatureFilter)
                {
                    FeatureFilter filter = (FeatureFilter)successor;
                    FeatureList l = filter.process(output, env);
                }
                else if (successor is FragmentFilter)
                {
                    FragmentFilter filter = (FragmentFilter)successor;
                    FragmentList l = filter.process(output, env);
                }
            }

            return output;
        }

        //Creates a Mercator projection
        private static ICoordinateSystem GetMercatorProjection()
        {
            var factory = new CoordinateSystemFactory();

            var wgs84 = factory.CreateGeographicCoordinateSystem("WGS 84",
                AngularUnit.Degrees, HorizontalDatum.WGS84, PrimeMeridian.Greenwich,
                new AxisInfo("north", AxisOrientationEnum.North), new
                AxisInfo("east", AxisOrientationEnum.East));

            var parameters = new List<ProjectionParameter>
            {
                new ProjectionParameter("semi_major", 6371000), // 6378137
                new ProjectionParameter("semi_minor", 6371000), // 6378137
                new ProjectionParameter("latitude_of_origin", 0.0),
                new ProjectionParameter("central_meridian", 0.0),
                new ProjectionParameter("scale_factor", 1.0),
                new ProjectionParameter("false_easting", 0.0),
                new ProjectionParameter("false_northing", 0.0)
            };
            var projection = factory.CreateProjection("Mercator", "mercator_1sp", parameters);
            var mercator = factory.CreateProjectedCoordinateSystem("Mercator",
                wgs84, projection, LinearUnit.Metre,
                new AxisInfo("East", AxisOrientationEnum.East),
                new AxisInfo("North", AxisOrientationEnum.North));
            return mercator;
        }

        /// <summary>
        /// Creates a UTM projection for the northern/// hemisphere based on the WGS84 datum
        /// </summary>
        /// <param name="utmZone">Utm Zone</param>
        /// <returns>Projection</returns>
        private IProjectedCoordinateSystem CreateUtmProjection(int utmZone)
        {
            CoordinateSystemFactory cFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            //CoordinateSystemFactory cFac = new SharpMap.CoordinateSystems.CoordinateSystemFactory();

            //Create geographic coordinate system based on the WGS84 datum
            IEllipsoid ellipsoid = cFac.CreateFlattenedSphere("WGS 84", 6378137, 298.257223563, LinearUnit.Metre);
            IHorizontalDatum datum = cFac.CreateHorizontalDatum("WGS_1984", DatumType.HD_Geocentric, ellipsoid, null);
            IGeographicCoordinateSystem gcs = cFac.CreateGeographicCoordinateSystem("WGS 84", AngularUnit.Degrees, datum,
              PrimeMeridian.Greenwich, new AxisInfo("Lon", AxisOrientationEnum.East),
              new AxisInfo("Lat", AxisOrientationEnum.North));

            //Create UTM projection
            List<ProjectionParameter> parameters = new List<ProjectionParameter>();
            parameters.Add(new ProjectionParameter("latitude_of_origin", 0));
            parameters.Add(new ProjectionParameter("central_meridian", -183 + 6 * utmZone));
            parameters.Add(new ProjectionParameter("scale_factor", 0.9996));
            parameters.Add(new ProjectionParameter("false_easting", 500000));
            parameters.Add(new ProjectionParameter("false_northing", 0.0));
            IProjection projection = cFac.CreateProjection("Transverse Mercator", "Transverse Mercator", parameters);

            return cFac.CreateProjectedCoordinateSystem("WGS 84 / UTM zone " + utmZone.ToString() + "N", gcs,
               projection, LinearUnit.Metre, new AxisInfo("East", AxisOrientationEnum.East),
               new AxisInfo("North", AxisOrientationEnum.North));
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
#endif
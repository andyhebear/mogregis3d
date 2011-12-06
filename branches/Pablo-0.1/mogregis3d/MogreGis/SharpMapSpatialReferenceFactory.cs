using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;

namespace MogreGis
{
    public class SharpMapSpatialReferenceFactory : SpatialReferenceFactory
    {
        private const string WKT_WGS84 = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]";
        private const string PROJ4_CEA_METERS = @"+proj=cea +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +units=m";

        private ProjNet.CoordinateSystems.ICoordinateSystem cs;

        /**
        * Creates a new WGS84 geographic SRS.
        *
        * @return A spatial reference. Caller is responsible for deleting
        *         the return object.
        */
        public SpatialReference createWGS84()
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new Cylindrical Equal Area projection.
         *
         * @return A spatial reference. Caller is responsible for deleting
         *         the return object.
         */
        public SpatialReference createCEA()
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new SRS from an OSG WKT string.
         *
         * @param wkt
         *      OGC WKT (well-known text) SRS definition
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createSRSfromWKT(string wkt)
        {
            Mogre.Matrix4 matrix = new Matrix4();
            //matrix.m00 = 7.3f;
            //matrix.m01 = 1.7f;
            return createSRSfromWKT(wkt, matrix);
            //return createSRSfromWKT(wkt, @"GEOGCS[""GCS_WGS_1984"",DATUM[""D_WGS_1984"",SPHEROID[""WGS_1984"",6378137,298.257223563]],PRIMEM[""Greenwich"",0],UNIT[""Degree"",0.0174532925199433]] ");
        }

        /**
         * Creates a new SRS from an OSG WKT string.
         *
         * @param wkt
         *      OGC WKT (well-known text) SRS definition
         * @param reference_frame
         *      Reference frame to apply to points in this SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createSRSfromWKT(string wkt, Mogre.Matrix4 reference_frame)
        {
            //Implementar el MatrixTransform
            //rellenar el MatrixTransform usando el reference_frame
            //SetUp coordinate transformation
            ProjNet.CoordinateSystems.CoordinateSystemFactory csf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            ProjNet.CoordinateSystems.ICoordinateSystem csSource = csf.CreateFromWkt(wkt);

            SharpMapSpatialReference sr = new SharpMapSpatialReference();
            sr.CoordinateSystem = csSource;
            sr.MathTransform = new MatrixTransform(csSource.Dimension, reference_frame);
            return sr;
        }

        public SpatialReference createSRSfromWKT(string wkt, ICoordinateSystem source)
        {
            SpatialReference result = null;
            //SetUp coordinate transformation
            ProjNet.CoordinateSystems.CoordinateSystemFactory csf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            cs = csf.CreateFromWkt(wkt);
            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctf = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            ctf.CreateFromCoordinateSystems(source, cs);
            ProjNet.CoordinateSystems.Transformations.ICoordinateTransformation ct = ctf.CreateFromCoordinateSystems(cs, source);
            //crear SharpMapSpatialReference usando el coordinateTranformation.
            throw new NotImplementedException();
        }

        //Este para que hace falta??
        public SpatialReference createSRSfromWKT(string wkt, IMathTransform transf)
        {
            ProjNet.CoordinateSystems.CoordinateSystemFactory csf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
           
            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctf = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            throw new NotImplementedException();
        }

        public SpatialReference createSRSfromWKT(string wkttarget, string wktsource)
        {
            ProjNet.CoordinateSystems.CoordinateSystemFactory csf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            ProjNet.CoordinateSystems.ICoordinateSystem csSource = csf.CreateFromWkt(wktsource);
            ProjNet.CoordinateSystems.ICoordinateSystem csTarget = csf.CreateFromWkt(wkttarget);
            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctf = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            ProjNet.CoordinateSystems.Transformations.ICoordinateTransformation ct = ctf.CreateFromCoordinateSystems(csSource, csTarget);
            SharpMapSpatialReference sr = new SharpMapSpatialReference();
            sr.CoordinateSystem = csSource;
            sr.MathTransform = ct.MathTransform;
            return sr;
        }

        /**
         * Creates a new SRS from an ESRI-style WKT/PRJ string.
         *
         * @param wkt
         *      ESRI-style WKT (well-known text) SRS definition
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createSRSfromESRI(string wkt)
        {
            SpatialReference result = null;
            //SetUp coordinate transformation
            ProjNet.CoordinateSystems.CoordinateSystemFactory csf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            cs = csf.CreateFromWkt(wkt);
            throw new NotImplementedException();
        }

        /**
         * Creates a new SRS from an ESRI-style WKT/PRJ string.
         *
         * @param wkt
         *      ESRI-style WKT (well-known text) SRS definition
         * @param reference_frame
         *      Reference frame to apply to points in this SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createSRSfromESRI(string wkt, Mogre.Matrix4 reference_frame)
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new SRS from an OSG WKT string in a file.
         *
         * @param abs_path
         *      Absoulte pathname of a text file that contains an
         *      OGC WKT (well-known text) SRS definition
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createSRSfromWKTfile(string abs_path)
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new SRS from a PROJ4 init string.
         *
         * @param proj4_def
         *      PROJ4 initialization string
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createSRSfromPROJ4(string proj4_def)
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new SRS from data found in a scene graph.
         *
         * @param node
         *      Scene graph for which to determine the SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createSRSfromTerrain(Node node)
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new geocentric SRS based on the WGS84 datum.
         *
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createGeocentricSRS()
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new geocentric SRS based on the user-supplied 
         * geographic basic SRS.
         *         * @param basis
         *      Geographic SRS upon which to base the new geocentric SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createGeocentricSRS(SpatialReference basis)
        {
            throw new NotImplementedException();
        }

        /**
         * Creates a new geocentric SRS based on the user-supplied 
         * geographic basic SRS.
         *
         * @param basis
         *      Geographic SRS upon which to base the new geocentric SRS
         * @param reference_frame
         *      Reference frame to apply to points in this SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createGeocentricSRS(SpatialReference basis, Mogre.Matrix4 reference_frame)
        {
            throw new NotImplementedException();
        }

#if TODO_PH
        /**
         * Creates a new SRS from data found in an osg::CoordinateSystemNode.
         *
         * @param cs_node
         *      Node for which to determine the SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference createGeocentricSRS(osg.CoordinateSystemNode cs_node);
#endif
        /**
         * Examines an SRS and if broken, attempts to repair it so that it works.
         *
         * @param validateSRS
         *      SRS to repair if necessary
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public SpatialReference validateSRS(SpatialReference srs_to_validate)
        {
            throw new NotImplementedException();
        }
    }
}

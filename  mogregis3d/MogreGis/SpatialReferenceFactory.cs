using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
using Sharp3D.Math.Core;

namespace MogreGis
{
    /**
     * Interface for creating SpatialReference instances.
     */
    public class SpatialReferenceFactory
    {

        /**
         * Creates a new WGS84 geographic SRS.
         *
         * @return A spatial reference. Caller is responsible for deleting
         *         the return object.
         */
        public abstract SpatialReference createWGS84();

        /**
         * Creates a new Cylindrical Equal Area projection.
         *
         * @return A spatial reference. Caller is responsible for deleting
         *         the return object.
         */
        public abstract SpatialReference createCEA();

        /**
         * Creates a new SRS from an OSG WKT string.
         *
         * @param wkt
         *      OGC WKT (well-known text) SRS definition
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference createSRSfromWKT(string wkt);

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
        public abstract SpatialReference createSRSfromWKT(string wkt, Matrix4D reference_frame);

        /**
         * Creates a new SRS from an ESRI-style WKT/PRJ string.
         *
         * @param wkt
         *      ESRI-style WKT (well-known text) SRS definition
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference createSRSfromESRI(string wkt);

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
        public abstract SpatialReference createSRSfromESRI(string wkt, Matrix4D reference_frame);

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
        public abstract SpatialReference createSRSfromWKTfile(string abs_path);

        /**
         * Creates a new SRS from a PROJ4 init string.
         *
         * @param proj4_def
         *      PROJ4 initialization string
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference createSRSfromPROJ4(string proj4_def);

        /**
         * Creates a new SRS from data found in a scene graph.
         *
         * @param node
         *      Scene graph for which to determine the SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference createSRSfromTerrain(Node node);

        /**
         * Creates a new geocentric SRS based on the WGS84 datum.
         *
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference createGeocentricSRS();

        /**
         * Creates a new geocentric SRS based on the user-supplied 
         * geographic basic SRS.
         *         * @param basis
         *      Geographic SRS upon which to base the new geocentric SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference createGeocentricSRS(SpatialReference basis);

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
        public abstract SpatialReference createGeocentricSRS(SpatialReference basis, Matrix4D reference_frame);

        /**
         * Creates a new SRS from data found in an osg::CoordinateSystemNode.
         *
         * @param cs_node
         *      Node for which to determine the SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference createGeocentricSRS(osg.CoordinateSystemNode cs_node);

        /**
         * Examines an SRS and if broken, attempts to repair it so that it works.
         *
         * @param validateSRS
         *      SRS to repair if necessary
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        public abstract SpatialReference validateSRS(SpatialReference srs_to_validate);
    }
}

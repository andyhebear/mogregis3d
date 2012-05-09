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
    public interface SpatialReferenceFactory
    {

        /**
         * Creates a new WGS84 geographic SRS.
         *
         * @return A spatial reference. Caller is responsible for deleting
         *         the return object.
         */
        SpatialReference createWGS84();

        /**
         * Creates a new Cylindrical Equal Area projection.
         *
         * @return A spatial reference. Caller is responsible for deleting
         *         the return object.
         */
        SpatialReference createCEA();

        /**
         * Creates a new SRS from an OSG WKT string.
         *
         * @param wkt
         *      OGC WKT (well-known text) SRS definition
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        SpatialReference createSRSfromWKT(string wkt);

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
        SpatialReference createSRSfromWKT(string wkt, Mogre.Matrix4 reference_frame);

        /**
         * Creates a new SRS from an ESRI-style WKT/PRJ string.
         *
         * @param wkt
         *      ESRI-style WKT (well-known text) SRS definition
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        SpatialReference createSRSfromESRI(string wkt);

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
        SpatialReference createSRSfromESRI(string wkt, Mogre.Matrix4 reference_frame);

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
        SpatialReference createSRSfromWKTfile(string abs_path);

        /**
         * Creates a new SRS from a PROJ4 init string.
         *
         * @param proj4_def
         *      PROJ4 initialization string
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        SpatialReference createSRSfromPROJ4(string proj4_def);

        /**
         * Creates a new SRS from data found in a scene graph.
         *
         * @param node
         *      Scene graph for which to determine the SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        SpatialReference createSRSfromTerrain(Node node);

        /**
         * Creates a new geocentric SRS based on the WGS84 datum.
         *
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        SpatialReference createGeocentricSRS();

        /**
         * Creates a new geocentric SRS based on the user-supplied 
         * geographic basic SRS.
         *         * @param basis
         *      Geographic SRS upon which to base the new geocentric SRS
         * @return
         *      A spatial reference. Caller is responsible for deleting
         *      the return object.
         */
        SpatialReference createGeocentricSRS(SpatialReference basis);

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
        SpatialReference createGeocentricSRS(SpatialReference basis, Mogre.Matrix4 reference_frame);

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
        SpatialReference validateSRS(SpatialReference srs_to_validate);
    }
}

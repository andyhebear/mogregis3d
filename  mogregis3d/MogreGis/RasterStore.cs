using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
namespace MogreGis
{
    /**
     * Interface to a raster data backing store.
     *
     * A RasterStore holds an arbitrarily large georeferenced image.
     */
    public class RasterStore
    {

        /**
         * Gets whether a connection to the store was successfully established.
         */
        public abstract bool isReady();

        /**
         * Gets the name of the store.
         */
        public abstract string getName();

        /**
         * Gets the spatial reference system (SRS) of the geodata in the
         * store. This may be NULL is no SRS is specified.
         */
        public abstract SpatialReference getSRS();

        /**
         * Gets the minimum bounding rectangle containing the entire raster.
         */
        public abstract GeoExtent getExtent();

        /**
         * Extracts an RGBA image from the raster store.
         *
         * @param aoi
         *      Spatial extent of the area of interest to extract
         * @param image_width
         *      Width of the resulting osg::Image (in pixels)
         * @param image_height
         *      Height of the resulting osg::Image (in pixels)
         * @return
         *      An osg::Image object. Caller is responsible for deleting the return object.
         */
        public abstract Mogre.Image createImage(GeoExtent aoi,
                                                uint image_width,
                                                uint image_height);

        /**
         * Gets the pixel format of images returned from createImage.
         *
         * @return A pixel format (e.g. GL_RGBA, GL_RGB, etc.)
         */
        public abstract Mogre.PixelFormat getImagePixelFormat();

        /**
         * Calculates and returns an image size that will best accomodate an osg::Image
         * that spans the provided AOI. Typically you would call this and use the generated
         * size in a call to createImage().
         *
         * @param aoi
         *      Spatial extent of raster area to query
         * @param max_pixel_span
         *      Maximum number of pixels in either dimension
         * @param force_power_of_2
         *      Round the resulting dimensions so that they are powers of 2
         * @param out_image_width
         *      Upon success, stores the calculated width to this variable
         * @param out_image_height
         *      Upon success, stores the calcluates height to this variable.
         *
         * @return
         *      True upon success, False upon failure.
         */
        public abstract bool getOptimalImageSize(GeoExtent aoi,
                                                uint max_pixel_span,
                                                bool force_power_of_2,
                                                out uint out_image_width,
                                                out uint out_image_height);

        /* (NOT YET IMPLEMENTED)
         * Extracts a height field from the raster store.
         *
         * @param aoi
         *      Spatial extent of the area of interest to extract
         * @return
         *      An OSG height field
         */
        public abstract osg.HeightField createHeightField(GeoExtent aoi);
    }
}

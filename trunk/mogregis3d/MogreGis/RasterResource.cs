using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
namespace MogreGis
{
    /**
     * A resource referencing a georeferenced raster that can be used as an 
     * overlay texture.
     *
     * The raster can be either a single-source raster or a composite
     * raster (a mosaic of multiple rasters stiched together).
     */
    public class RasterResource : Resource
    {
        //TODO  OSGGIS_META_RESOURCE(RasterResource);


        /**
         * Constructs an empty raster resource.
         */
        public RasterResource();

        /**
         * Constructs a named raster resource.
         *
         * @param name
         *     Readable name of the new resource
         */
        public RasterResource(string name);

        /**
         * Sets the OpenGL texture mapping mode for the image (default
         * is GL_MODULATE)
         *
         * @param mode
         *      OpenGL mode (GL_DECAL, GL_REPLACE, GL_MODULATE, GL_BLEND)
         */
        public void setTextureMode(osg.TexEnv.Mode mode);

        /**
         * Gets the OpenGL texture mapping mode for the image
         *
         * @return OpenGL mode (GL_DECAL, GL_REPLACE, GL_MODULATE, GL_BLEND)
         */
        public osg.TexEnv.Mode getTextureMode();

        /**
         * Assembles an osg::Image from the raster data and applies it to a state set
         * as a 2D texture map.
         *
         * @param state_set
         *      State set to which to apply the texture containing the extracted image
         * @param aoi
         *      Region of the raster to extract into the image
         * @param max_pixels
         *      Maximum width or height of the image to extract
         * @param image_name
         *      Name to assign to the extracted image
         * @param out_image
         *      Store the image pointer to this variable upon success
         *
         * @return true upon success, false upon failure
         */
        public bool applyToStateSet(
                Material state_set,
                 GeoExtent aoi,
                uint max_pixel_span,
                out Mogre.Image out_image);

        /**
         * Adds a part to a composite raster.
         *
         * @param uri
         *      URI (absolute, or relative to the base URI of this resource) of one
         *      part of the raster data.
         */
        public void addPartURI(string uri);



        public virtual void setProperty(Property prop);
        public virtual Properties getProperties();


        private void init();
        private void initParts();

        private RasterParts parts;
        private bool parts_initialized;
        private Mogre.PixelFormat pixel_format;
        private osg.TexEnv.Mode texture_mode;
    }

    // public class RasterPart : KeyValuePair<GeoExtent, string  > {}
    public class RasterParts : Dictionary<GeoExtent, string> { }

    public class RasterResourceVec : List<RasterResource> { }
}

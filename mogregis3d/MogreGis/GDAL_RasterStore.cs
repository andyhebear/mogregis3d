using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /* (internal - no api docs)
     * 
     * Implementation of a RasterStore that reads data via the GDAL library.
     */
    public class GDAL_RasterStore : RasterStore
    {

        /**
         * Constructs a new GDAL raster store
         *
         * @param uri
         *      URI of GDAL data store to which to connect
         */
        public GDAL_RasterStore(string uri);


        // osgGIS::RasterStore

        public bool isReady();

        public string getName();

        public SpatialReference getSRS();

        public GeoExtent getExtent();

        public GLenum getImagePixelFormat();

        //osg::Image* createImage( 
        //    const GeoExtent& extent, 
        //    int max_span_pixels,
        //    bool force_power_of_2_dimensions ) const;

        public Image createImage( GeoExtent extent,
                                  int image_width,
                                  int image_height);

        public bool getOptimalImageSize( GeoExtent aoi,
                                        int max_pixel_span,
                                        bool force_power_of_2,
                                        out int out_image_width,
                                        out int out_image_height);

        public osg.HeightField createHeightField(
              GeoExtent aoi);


        private string uri;
        private GDALDataset dataset;
        private double[] geo_transform = new double[6];
        private bool has_geo_transform;
        private int size_x, size_y, num_bands;
        private double res_x, res_y;
        private SpatialReference spatial_ref;
        private GeoExtent extent;

        private void calcExtent();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /* (internal class - no public api docs)
     *
     * The object that creates new raster store connections by default.
     */
    public class DefaultRasterStoreFactory : RasterStoreFactory
    {
        public DefaultRasterStoreFactory()
        {
            OGR_Utils.registerAll();
        }



        // RasterStoreFactory

        public RasterStore connectToRasterStore(string uri)
        {
            RasterStore result = null;

            result = new GDAL_RasterStore(uri);

            if (result == null)
            {
                //TODO osgGIS.notify( osg::WARN ) << "Cannot find an appropriate raster store to handle URI: " << uri << std::endl;
            }
            else if (!result.isReady())
            {
                //TODO osgGIS.notify( osg::WARN ) << "Unable to initialize raster store for URI: " << uri << std::endl;
                //result.unref();
                result = null;
            }

            return result;
        }
    }
}

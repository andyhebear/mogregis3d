using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * Factory interface for creating a connection to a RasterStore.
     */
	public class   RasterStoreFactory 
	{
	    /**
	     * Connects to an existing raster store and returns a handle.
         *
         * @param uri
         *      URI of the raster store to which to connect
         * @return
         *      A raster store connection
	     */
		public abstract RasterStore  connectToRasterStore(  string   uri ) ;		
	}
}

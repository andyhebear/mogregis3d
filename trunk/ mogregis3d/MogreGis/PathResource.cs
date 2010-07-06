using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * A Resource that encapsulates a simple URI or path.
     */
    public class PathResource : Resource
    {
        //TODO    OSGGIS_META_RESOURCE(PathResource);


        /**
         * Constructs a new, empty path resource.
         */
        public PathResource()
        {
            //NOP
        }

        public virtual void setProperty(Property prop)
        {
            base.setProperty(prop);
        }
        public virtual Properties getProperties()
        {
            Properties props = base.getProperties();
            return props;
        }
    }


    public class PathResourceVec : List<PathResource> { }
}

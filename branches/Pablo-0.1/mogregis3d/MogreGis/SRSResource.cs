using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
         * A Resource that references a SpatialReference.
         */
    public class SRSResource : Resource
    {
        //TODO OSGGIS_META_RESOURCE(SRSResource);
        public string getResourceType() { return getStaticResourceType(); }
        static string getStaticResourceType() { return "srsresource"; }
        static ResourceFactory getResourceFactory() { return new ResourceFactoryImpl<SRSResource>(); }

        static SRSResource()
        {
            bool _osggsis_dr = Registry.instance().addResourceType(SRSResource.getStaticResourceType(), SRSResource.getResourceFactory());
        }
        /**
         * Constructs a new SRS resource.
         */
        public SRSResource()
        {
            //NOP
        }

        // properties

        /**
         * Sets the spatial reference object in this resource.
         *
         * @param value
         *      SRS to set
         */
        public void setSRS(SpatialReference value)
        {
            srs = value;
        }

        /**
         * Gets the SRS referenced by this resource.
         *
         * @return The spatial reference.
         */
        public SpatialReference getSRS()
        {
            return srs;
        }




        public virtual void setProperty(Property prop)
        {
            if (prop.getName() == "wkt")
                setSRS(Registry.SRSFactory().createSRSfromWKT(prop.getValue()));
            //base.setProperty(prop);
        }
        public override Properties getProperties()
        {
            Properties props = base.getProperties();
            if (getSRS() != null)
                props.Add(new Property("wkt", getSRS().getWKT()));
            return props;
        }


        private SpatialReference srs;
    }



    public class SRSResourceVec : List<SRSResource> { }
}

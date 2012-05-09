using System;
using System.Collections.Generic;
#if TODO
using MogreGis;
#endif
namespace osgGISProjects
{

    public class Terrain
    {

        public Terrain() { throw new NotImplementedException(); }

        public Terrain(string _uri)
        {
            setURI(_uri);
        }

        public string getBaseURI() { throw new NotImplementedException(); }

        public void setBaseURI(string value)
        {
            base_uri = value;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string value)
        {
            name = value;
        }

        public string getURI()
        {
            return uri;
        }

        public void setURI(string value)
        {
            uri = value;
        }

#if TODO
        public string getAbsoluteURI()
        {
            return PathUtils.getAbsPath(base_uri, uri);
        }


        public void setExplicitSRS(SpatialReference srs)
        {
            explicit_srs = srs;
        }

        public SpatialReference getExplicitSRS()
        {
            return explicit_srs.get();
        }

#endif
        private string name;
        private string uri;
        private string base_uri;
#if TODO
        private MogreGIS.SpatialReference explicit_srs;
#endif
    }

    public class TerrainList : List<Terrain> { } ;

}
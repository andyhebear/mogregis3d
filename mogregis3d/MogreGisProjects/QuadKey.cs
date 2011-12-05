using System;
using System.Collections.Generic;

namespace osgGISProjects
{
    /* (internal)
     * A quad-tree map setup that defines the layout of a QuadKey cell
     * hierarchy.
     */
    public class QuadMap
    {
#if TODO
    public         QuadMap();

        public QuadMap(   QuadMap rhs );

        public QuadMap(   GeoExtent bounds );

        public bool isValid()  ;

        public   GeoExtent getBounds()  ;

public         void getCellSize( uint lod, out double  out_width, out double out_height )  ;

       public  uint getBestLodForCellsOfSize( double cell_width, double cell_height )  ;

        /**
         * Gets the cell range that incorporates the AOI at the specified LOD.
         */
        public bool getCells(
              GeoExtent  aoi, uint lod,
            out uint  out_cell_xmin, out uint  out_cell_ymin,
            out uint  out_cell_xmax, out uint  out_cell_ymax )  ;

    public         enum StringStyle {
            STYLE_QUADKEY, // default
            STYLE_LOD_QUADKEY
        };

        public void setStringStyle( StringStyle style );

        public StringStyle getStringStyle()  ;

    private         GeoExtent bounds;
        private StringStyle string_style;
#endif
    }

    /* (internal)
     * Quadkey implementation that's similar to MSVE's tiling structure, except
     * that at the top-level we only have two cells (0=western hemi, 1=eastern hemi)
     * and the other 2 cells (2 and 3) are below the south pole and unused.
     */
    public class QuadKey
    {
#if TODO
    public        QuadKey(   QuadKey  rhs );

        public QuadKey(   string  key_string,   QuadMap map );

       public  QuadKey( uint cell_x, uint cell_y, uint lod,   QuadMap map );

        public  GeoExtent getExtent()  ;

        public uint getLOD()  ;

        public void getCellSize( double& out_width, double& out_height )  ;

        public QuadKey createSubKey( uint quadrant )  ;

        public QuadKey createParentKey()  ;

        public string toString()  ;

        public QuadMap& getMap()  ;

        public bool operator< (   QuadKey& rhs )  ;

    private string qstr;
    private     QuadMap map;
       private  GeoExtent extent;
#endif
    } 

    public class QuadKeyList : List<QuadKey> {};
    public class QuadKeySet : HashSet<QuadKey> { };
}
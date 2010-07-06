using System;
#if TODO
using MogreGis;

namespace osgGISProjects
{

    /**
     * A complete definition of a single runtime map layer, which is a set of one or
     * more feature layers and their compilation configurations. This object is input
     * for a MapLayerCompiler.
     */
    public class MapLayer
    {
        /**
         * Constructs a new map layer definition.
         */
        public MapLayer()
        {
            grid_valid = false;
            encode_cell_radius = true;
            aoi_manual = GeoExtent.invalid();
            aoi_auto = GeoExtent.invalid();
        }

        /**
         * Sets the geospatial extent bounding the area we want to build.
         *
         * @param extent Geospatial area of interest
         */
        public void setAreaOfInterest(GeoExtent value)
        {
            aoi_manual = value;
            grid_valid = false;
        }

        /**
         * Gets the geospatial extent bounding the area to build.
         *
         * @return The geospatial area of interest
         */
        public GeoExtent getAreaOfInterest()
        {
            return aoi_manual.isValid() ? aoi_manual : aoi_auto;
        }

        /**
         * Sets the east-west extent of each cell in the map layer.
         *
         * @param width Extent (in source data units)
         */
        public void setCellWidth(double value)
        {
            cell_width = value;
            grid_valid = false;
        }

        /**
         * Gets the east-west extent of each cell in the map layer.
         *
         * @return Extent (in source data units)
         */
        public double getCellWidth()
        {
            return cell_width;
        }

        /**
         * Sets the north-source extent of each cell in the map layer.
         *
         * @param height Extent (in source data units)
         */
        public void setCellHeight(double value)
        {
            cell_height = value;
            grid_valid = false;
        }

        /**
         * Gets the north-south extent of each cell in the map layer.
         *
         * @return Extent (in source data units)
         */
        public double getCellHeight()
        {
            return cell_height;
        }

        /**
         * Pushes source data onto the level of detail queue. Levels of detail are
         * interpreted from front to back.
         *
         * @param layer
         *      Feature layer from which to read source data
         * @param graph
         *      Filter graph to use to build scene graph
         * @param min_range
         *      Minimum visibility range of this level of detail
         * @param max_range
         *      Maximum visibility range of this level of detail
         * @param replace
         *      If true, this detail level will replace the ones before it. If false, it
         *      will join the scene graph without removing the previous levels.
         * @param depth
         *      Level of detail depth (0 = top level)
         * @param user_data
         *      User-defined data to pass to the cell compiler
         */
        public void push(FeatureLayer layer,
                        FilterGraph graph,
                        Properties env_props,
                        ResourcePackager packager,
                        float min_range,
                        float max_range,
                        bool replace,
                        uint depth,
                        object user_data)
        {
            if (layer != null)
            {
                // update the automatic AOI:
                if (!aoi_auto.isValid())
                    aoi_auto = layer.getExtent();
                else
                    aoi_auto.expandToInclude(layer.getExtent());

                // store the LOD definition:
                levels.Add(new MapLayerLevelOfDetail(
                    layer, graph, env_props, packager, min_range, max_range, replace, depth, user_data));

                grid_valid = false;
            }
        }

        /**
         * Gets the maximum depth (LOD nesting level) in the map layer.
         *
         * @return Integer depth number (0=shallowest)
         */
        public uint getMaxDepth()
        {
            uint max_depth = 0;
            foreach (MapLayerLevelOfDetail i in levels)
            {
                if (i.getDepth() > max_depth)
                    max_depth = i.getDepth();
            }
            return max_depth;
        }

        /**
         * Gets the spatial reference system of the output when this layer is compiled.
         *
         * @param session
         *      Session under which to examine this map layer
         * @param terrain_srs
         *      Optional terrain SRS
         *
         * @return A spatial reference
         */
        public SpatialReference getOutputSRS(Session session)
        {
            return getOutputSRS(session, null);
        }
        public SpatialReference getOutputSRS(Session session, SpatialReference terrain_srs)
        {
            if (!grid_valid || output_srs == null)
            {
                if (levels.Count > 0 && levels[0].getFilterGraph() != null)
                {
                    FilterEnv env = session.createFilterEnv();
                    env.setTerrainSRS(terrain_srs);

                    FilterList filters = levels[0].getFilterGraph().getFilters();

                    for (int ind = filters.Count-1; ind >= 0; ind--) //reverse iterator?
                    {
                        if (output_srs != null)
                            break;
                        Filter i = filters[ind];
                        if (i is TransformFilter)
                        {
                            TransformFilter xf = (TransformFilter)i;
                            if (xf.getUseTerrainSRS())
                            {
                                if (env.getTerrainSRS() != null)
                                {
                                    this.output_srs = env.getTerrainSRS();
                                }
                            }
                            else if (xf.getSRS())
                            {
                                this.output_srs = (SpatialReference)(xf.getSRS());
                            }
                            else if (xf.getSRSScript())
                            {
                                ScriptResult r = env.getScriptEngine().run(xf.getSRSScript(), env);
                                if (r.isValid())
                                    this.output_srs = session.getResources().getSRS(r.asString());
                                else
                                    env.getReport().error(r.asString());
                            }
                        }
                    }

                    if (output_srs == null) // no reproject..assume input srs
                    {
                        this.output_srs = levels[0].getFeatureLayer().getSRS();
                    }
                }
            }

            return output_srs;
        }

        /**
         * Calculates the number of rows in the resulting cell grid.
         *
         * @return Integer column count
         */
        public uint getNumCellColumns()
        {
            if (!grid_valid)
                this.recalculateGrid();
            return num_cols;
        }

        /**
         * Calculates the number of rows in the resulting cell grid.
         *
         * @return Integer row count.
         */
        public uint getNumCellRows()
        {
            if (!grid_valid)
                this.recalculateGrid();
            return num_rows;
        }

        /**
         * Gets the number of detail levels at each grid location cell location.
         *
         * @return Number of detail levels
         */
        public uint getNumCellLevels()
        {
            return (uint)levels.Count;
        }

        /**
         * Creates and returns a cell definition for a given cell location.
         *
         * @param col, row
         *      Column and row for which to create a cell definition
         * @param level
         *      Detail level for which to create a cell definition
         * @return
         *      Cell definition
         */
        //        MapLayerCell* createCell( unsigned int col, unsigned int row, unsigned int level ) const;

        public MapLayerLevelsOfDetail getLevels()
        {
            return levels;
        }

        /**
         * Sets whether to explicity calculate and encode the radius of each cell in the
         * map structure. This is desirable for large-area datasets as it will improve
         * spatial organization, but undesirable for point-model substitution datasets
         * since it will cause odd switching thresholds.
         */
        public void setEncodeCellRadius(bool value)
        {
            encode_cell_radius = value;
        }

        public bool getEncodeCellRadius()
        {
            return encode_cell_radius;
        }

        private MapLayerLevelsOfDetail levels;
        private double cell_width, cell_height;
        private bool encode_cell_radius;
        private GeoExtent aoi_auto, aoi_manual;

        // cached calculated grid setup:
        private double dx, last_dx, dy, last_dy;
        private uint num_rows, num_cols;
        private bool grid_valid;
        private SpatialReference output_srs;

        private void recalculateGrid()
        {
            num_rows = 0;
            num_cols = 0;
            dx = 0.0;
            last_dx = 0.0;
            dy = 0.0;
            last_dy = 0.0;

            GeoExtent aoi = getAreaOfInterest();
            if (aoi.isValid())
            {
                GeoPoint sw = aoi.getSouthwest();
                GeoPoint ne = aoi.getNortheast();
                SpatialReference srs = aoi.getSRS();

                if (getCellHeight() > 0.0)
                {
                    num_rows = (uint)Math.Ceiling(aoi.getHeight() / getCellHeight());
                    dy = getCellHeight();
                    last_dy = aoi.getHeight() % getCellHeight();
                    if (last_dy == 0.0)
                        last_dy = dy;
                }
                else
                {
                    num_rows = 1;
                    dy = last_dy = aoi.getHeight();
                }

                if (getCellWidth() > 0.0)
                {
                    num_cols = (uint)Math.Ceiling(aoi.getWidth() / getCellWidth());
                    dx = getCellWidth();
                    last_dx = aoi.getWidth() % getCellWidth();
                    if (last_dx == 0.0)
                        last_dx = dx;
                }
                else
                {
                    num_cols = 1;
                    dx = last_dx = aoi.getWidth();
                }

                grid_valid = true;
            }
        }
    }
}
#endif
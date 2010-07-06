using System;
using System.Collections.Generic;
#if TODO
using MogreGis;

namespace osgGISProjects
{

    /**
     * 
     */
    public class BuildLayerLevel
    {

        /**
         * Constructs a new, empty layer level.
         */
        public BuildLayerLevel()
        {
            min_range = 0.0f;
            max_range = float.MaxValue;
            cell_width = 0.0;
            cell_height = 0.0;
            cell_size_factor = 1.0;
        }

        /**
         * Gets the data source to use when building this level.
         *
         * @return A data source
         */
        public Source getSource()
        {
            return source;
        }

        /**
         * Sets the data source to use when building this level.
         *
         * @param source A data source
         */
        public void setSource(Source value)
        {
            source = value;
        }

        /**
         * Gets the filter graph that will be used to build geometry for this
         * layer level.
         *
         * @return A Filter graph
         */
        public FilterGraph getFilterGraph()
        {
            return graph;
        }

        /**
         * Sets the filter graph that will generate geometry for this level.
         *
         * @param graph A filter graph
         */
        public void setFilterGraph(FilterGraph _graph)
        {
            graph = _graph;
        }

        /**
         * Gets the sub-levels under this level.
         *
         * @return A list of levels
         */
        public BuildLayerLevelList getSubLevels()
        {
            return sub_levels;
        }


        /**
         * Gets the minimum visibility range of this level.
         *
         * @return visibility distance (in scene units)
         */
        public float getMinRange()
        {
            return min_range;
        }

        /**
         * Sets the minimum visibility range of this level.
         *
         * @param value visibility distance (in scene units)
         */
        public void setMinRange(float value)
        {
            min_range = value;
        }

        /**
         * Gets the maximum visibility range of this level.
         *
         * @return visibility distance (in scene units)
         */
        public float getMaxRange()
        {
            return max_range;
        }

        /**
         * Sets the maximum visibility range of this level.
         *
         * @param value visibility distance (in scene units)
         */
        public void setMaxRange(float value)
        {
            max_range = value;
            if (max_range < 0) max_range = float.MaxValue;
        }

        /**
         * Gets the east-west extent of each cell that will be created
         * when this level is subdivided into a grid
         *
         * @return east-west extent (in Source units)
         */
        public double getCellWidth()
        {
            return cell_width;
        }

        /**
         * Sets the east-west extent of each cell that will be created
         * when this level is subdivided into a grid
         *
         * @param width east-west extent (in Source units)
         */
        public void setCellWidth(double value)
        {
            cell_width = value;
        }

        /**
         * Gets the north-south extent of each cell that will be created
         * when this level is subdivided into a grid
         *
         * @return north-south extent (in Source units)
         */
        public double getCellHeight()
        {
            return cell_height;
        }

        /**
         * Sets the north-south extent of each cell that will be created
         * when this level is subdivided into a grid
         *
         * @param height north-south extent (in Source units)
         */
        public void setCellHeight(double value)
        {
            cell_height = value;
        }

        /**
         * Gets a cell-size multiplier that expresses the cell width and cell height
         * for this level as a multiple of the parent level. For example, if the
         * parent cell width is 1000, and this level has a cell size factor of
         * 0.5, this level's cell width will be 500.
         *
         * @return A multiplication factor (0 < x < 1)
         */
        public double getCellSizeFactor()
        {
            return cell_size_factor;
        }


        /**
         * Gets a cell-size multiplier that expresses the cell width and cell height
         * for this level as a multiple of the parent level. For example, if the
         * parent cell width is 1000, and this level has a cell size factor of
         * 0.5, this level's cell width will be 500.
         *
         * @param factor Cell size factor multiplier (0 < x <= 1)
         */
        public void setCellSizeFactor(double value)
        {
            cell_size_factor = value;
        }


        private FilterGraph graph;
        private Source source;
        private float min_range, max_range;
        private double cell_width, cell_height;
        private double cell_size_factor;
        private BuildLayerLevelList sub_levels;
    }

    public class BuildLayerLevelList : List<BuildLayerLevel> { };
}
#endif
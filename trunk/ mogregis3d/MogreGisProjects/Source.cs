using System;
using System.Collections.Generic;
using System.IO;
using SharpMap.Data.Providers;

using MogreGis;

namespace osgGISProjects
{
    public class Source
    {
        public enum SourceType
        {
            TYPE_FEATURE,
            TYPE_RASTER
        }


        /**
         * Constructs a new, empty source.
         */
        public Source()
        {

        }

        /**
         * Constructs a new source that referes to the object at the specified URI.
         */
        public Source(string uri)
        {
            setURI(uri);
        }


        /**
         * Sets the URI at the base of relative paths used by this source.
         */
        public void setBaseURI(string val)
        {
            base_uri = val;
        }

        /**
         * Gets the name of this source, unique within the current build.
         */
        public string getName()
        {
            return name;
        }

        /**
         * Sets the name of this source, which must be unique within the current
         * build.
         */
        public void setName(string value)
        {
            name = value;
        }


        /**
         * Gets the source type (TYPE_FEATURE or TYPE_RASTER).
         */
        public SourceType getType()
        {
            return type;
        }

        /**
         * Sets the source type to TYPE_FEATURE or TYPE_RASTER.
         */
        public void setType(SourceType value)
        {
            type = value;
        }

        /**
         * Gets the URI of the object to which this source refers. The URI might
         * be absolute or relative.
         */
        public string getURI()
        {
            return uri;
        }

        /**
         * Gets an absolute reference to the object to which this source refers,
         * by considering both the URI and the base URI.
         */
        public string getAbsoluteURI()
        {
            return Path.GetFullPath(Path.Combine(base_uri, uri));
        }

        /**
         * Sets the URI of the object to which this source record refers.
         */
        public void setURI(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.EndsWith(".shp"))
                DataSource = new SharpMap.Data.Providers.ShapeFile(value);

            uri = value;
        }

#if TODO
        /**
         * Accesses a set of appliation-specific properties.
         */
        public Properties getProperties()
        {
            return props;
        }
#endif
        /**
         * Sets the name of the parent source, i.e. another source from which
         * this source will be derived. Setting the parent source marks this object
         * as an intermediate source.
         */
        public void setParentSource(Source value)
        {
            parent_source = value;
        }

        /**
         * Gets the name of the parent source, if one exists. If a parent source
         * exists, this source is considered an intermediate source that may
         * require building or rebuilding.
         */
        public Source getParentSource()
        {
            return this.parent_source;
        }

        /**
         * Reflects whether this source has a parent source, and is therefore
         * an intermediate source that may require building or rebuilding.
         */
        public bool isIntermediate()
        {
            return getParentSource() != null;
        }

        /**
         * Sets the name of filter graph that should be used to build this
         * source (assuming it's an intermediate source with a parent source).
         */
        public void setFilterGraph(FilterGraph value)
        {
            filter_graph = value;
        }

        /**
         * If this is an intermediate source (and has a parent Source),
         * gets the name of the filter graph that should be used to generate
         * this source (assuming it's an intermediate source).
         */
        public FilterGraph getFilterGraph()
        {
            return this.filter_graph;
        }

        /**
         * Reflects whether this source needs rebuilding. If this is an
         * intermediate data source (i.e. it points to a parent source)
         * then it will need rebuilding if the parent source has changed.
         */
        public bool needsRefresh()
        {
            //TODO
            return true;
        }

        /**
         * Gets the UTC timestamp of the last-modified date of the object to which
         * this source refers.
         */
        public DateTime getTimeLastModified()
        {
            FileInfo fileInfo = new FileInfo(getAbsoluteURI());
            return fileInfo.LastWriteTimeUtc; //. FileUtils.getFileTimeUTC(getAbsoluteURI());
        }

        /// <summary>
        /// Gets or sets the datasource
        /// </summary>
        public IProvider DataSource

        { 
            get { return provider; } 
            set { provider = value; } 
        }

        private string name;
        private SourceType type;
        private string uri;
        private string base_uri;
#if TODO
        private Properties props;
#endif
        private Source parent_source;
        private FilterGraph filter_graph;
        private IProvider provider;
    }

    public class SourceList : List<Source> { };
}

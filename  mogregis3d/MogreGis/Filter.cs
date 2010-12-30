using System.Collections.Generic;

namespace MogreGis
{
    /**
     * A discrete data-processing element in a FilterGraph.
     *
     * A Filter in a self-contained data processing unit. It takes input and generates
     * output data. When chained together in a FilterGraph, filters sequentially
     * process data in discrete steps which together form a complete, repeatable data
     * compilation procedure.
     *
     * A Filter's operation should not depend on any external factors, except for 
     * the information conveyed in the FilterEnv data structure to which
     * every filter has access while it is running. Filters should ideally
     * be stateless with respect to the data they are processing. This helps
     * increase maintainability and predictability.
     * 
     * Filter
     *   |
     *   |---- FeatureFilter: Feature -> Feature
     *   |       |
     *   |       |---- TransformFilter
     *   |
     *   |---- CollectionFilter
     *   |
     *   |---- FragmentFilter: Feature -> Fragment, Fragment -> Fragment
     *   |        |
     *   |        |---- BuildGeomFilter
     *   |
     *   |---- NodeFilter: Feature -> Node, Fragment -> Node, Node -> Node

     */
    public abstract class Filter
    {

        /**
         * Gets the filter type
         *
         * @return Human-readable filter type string.
         */
        public abstract string getFilterType();

        /**
         * Gets/Sets the name of this filter.
         * A name is for reference purposes and is
         * not strictly required for the operation of the filter.
         */
        public string Name
        {
            set
            {
                this.name = value;
            }

            get
            {
                return name;
            }
        }

        [System.Obsolete("use property Name instead")]
        public string getName()
        {
            return Name;
        }

        /**
         * Creates a new state object that can be used with this filter type. The
         * various compilers use this object to communicate data from filter to
         * filter.
         *
         * @return A new state object for use with this filter.
         */
        public abstract FilterState newState();

        /**
         * Creates an exact copy of this instance. Note to developers: complete 
         * implementation of this method is critical to the operation of the
         * FilterGraph!
         *
         * @return An exact Filter copy
         */
        public abstract Filter clone();



        /**
         * Sets one of the filter's properties.
         *
         * @param prop
         *      Property to set
         */
        public virtual void setProperty(Property prop)
        {
            //NOP
        }

        /**
         * Gets a collection of all this filter's properties
         *
         * @return A collection of Property objects
         */
        public virtual Properties getProperties()
        {
            return new Properties();
        }



        internal Filter()
        {
            //NOP
        }

        protected Filter(Filter rhs)
        {
            this.name = rhs.name;
            //NOP
        }

        /*public void SetSuccessor(Filter successor)
        {
            this.successor = successor;
        }*/

        private string name;

        protected Filter successor;

        public Filter Successor
        {
            set
            {
                this.successor = value;
            }

            get
            {
                return successor;
            }
        }
    }


    public class FilterList : List<Filter> { }

    /* Interface for creating a new filter */
    public interface FilterFactory
    {
        Filter createFilter();
    }

    /* Parameterized interface for creating a new filter */
    internal class FilterFactoryImpl<T> : FilterFactory where T : Filter, new()
    {
        public  Filter createFilter() { return new T(); }
    }

    public class FilterFactoryMap : Dictionary<string, FilterFactory> { }

#if TODO


    /* Handy macro for defining all the basic required methods in a Filter class definition */
//#define OSGGIS_META_FILTER(name) \
    public: \
        virtual std::string getFilterType() { return getStaticFilterType(); } \
        virtual osgGIS::Filter* clone() const { return new name ( *this ); } \
        static std::string getStaticFilterType() { return #name; } \
        static osgGIS::FilterFactory* getFilterFactory() { return new osgGIS::FilterFactoryImpl<name>(); }     

    /* Handy macro for defining all the basic required implementation methods in a Filter class */
// #define OSGGIS_DEFINE_FILTER(name) \
    static bool _osggsis_df_##name = osgGIS::Registry::instance()->addFilterType( name::getStaticFilterType(), name::getFilterFactory() )

#endif
}

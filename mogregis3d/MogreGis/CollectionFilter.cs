using System;
using System.Collections.Generic;


namespace MogreGis
{
    /**
     * A filter that collects incoming data and meters it out in batches.
     *
     * Normally, data (such as Feature or Fragment data) passes through a FilterGraph
     * one element at a time. A CollectionFilter will "collect" elements,
     * wait until a checkpoint is reached, and then meter the collected
     * elements out in batches.
     * 
     * Since most filters have the options of processing data either in 
     * batches or one element at a time, a collection filter is useful for
     * combining related features or for enabling optimization. 
     *
     * For example: The CombineLinesFilter optimizes line layers by combining
     * line segments that share endpoints into single line strips, reducing the
     * number of features and helping performance. Since this filter needs to
     * access all incoming features as a group and compare them, you must precede
     * it with a CollectionFilter.
     *
     * Another possible use for a collection filter would be to save the state
     * of the graph to support partial compilation or data caching. This is
     * not yet implemented but will be in the future.
     */
    public class CollectionFilter : Filter
    {
        //TODO  OSGGIS_META_FILTER( CollectionFilter );  
        public override string getFilterType() { return getStaticFilterType(); } 
        public override Filter clone()  { return new CollectionFilter(this ); } 
        public static string getStaticFilterType() { return "CollectionFilter"; }
        public static FilterFactory getFilterFactory() { return new FilterFactoryImpl<CollectionFilter>(); }     


        /**
         * Constructs a new collection filter with no metering limit;
         * i.e. it will collect all incoming elements into a single batch.
         */
        public CollectionFilter()
        {
            setMetering(DEFAULT_METERING);
        }

        /**
         * Copy constructor.
         */
        public CollectionFilter(CollectionFilter rhs)
            : base(rhs)
        {
            metering = rhs.metering;
            group_property_name = rhs.group_property_name;
        }

        // properties

        /**
         * Sets the metering size. This will determine how many collected
         * elements get sent out in each batch.
         *
         * @param value
         *      Batch size for metered output
         */
        public void setMetering(int value) { metering = value; }

        /**
         * Gets the number of collected elements that will be sent out in each batch.
         *
         * @return Batch size for metered output
         */
        public int getMetering() { return metering; }

        /**
         * Sets the name of the FilterEnv property to which to store the name of the
         * current metering group.
         *
         * @param value
         *      FilterEnv property name
         */
        public void setAssignmentNameProperty(string value)
        {
            group_property_name = value;
        }

        /**
         * Gets the name of the FilterEnv property to which to store the name of the
         * current metering group assignment.
         *
         * @return FilterEnv property name
         */
        public string getAssignmentNameProperty()
        {
            return group_property_name;
        }


        // Filter overrides
        public override FilterState newState()
        {
            return new CollectionFilterState((CollectionFilter)clone());
        }

        public override void setProperty(Property prop)
        {
            if (prop.getName() == "metering")
                setMetering(prop.getIntValue(DEFAULT_METERING));
            else if (prop.getName() == "group_property")
                setAssignmentNameProperty(prop.getValue());
            base.setProperty(prop);
        }

        public override Properties getProperties()
        {
            Properties p = base.getProperties();
            if (getMetering() != DEFAULT_METERING)
                p.Add(new Property("metering", getMetering()));
            if (!string.IsNullOrEmpty(getAssignmentNameProperty()))
                p.Add(new Property("group_property", getAssignmentNameProperty()));
            return p;
        }

        public virtual string assign(Feature feature, FilterEnv env) { return ""; }
        public virtual string assign(Fragment fragment, FilterEnv env) { return ""; }
        public virtual string assign(AttributedNode attr, FilterEnv env) { return ""; }

        public virtual void preMeter(FeatureList features, FilterEnv env) { }
        public virtual void preMeter(FragmentList drawables, FilterEnv env) { }
        public virtual void preMeter(AttributedNodeList nodes, FilterEnv env) { }



        private int metering;
        private string group_property_name;
        private const int DEFAULT_METERING = 0;

    }


    /**
     * A filter that collects incoming data and meters it out in batches.
     *
     * This class is the same as the CollectionFilter, but exists to change the
     * class name while preserving backwards compatibility.
     *
     * @see CollectionFilter
     */
    public class CollectFilter : CollectionFilter
    {
        //TODO OSGGIS_META_FILTER( CollectFilter );  


        public CollectFilter() { }
        public CollectFilter(CollectFilter rhs) : base(rhs) { }
    }
}

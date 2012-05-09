using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * Aligns point features to the nearest line feature segment.
     *
     * This filter calculates a heading for each incoming feature such that
     * the feature will be "aligned" with the nearest linear feature in a secondary
     * feature store.
     *
     * For example, you can use this feature to calculate a heading for
     * street lights (point features) so that they will always face the
     * closest street segment.
     *
     * This filter does not alter any feature geometry. Instead is stores
     * the calculated heading in an "output attribute". You can later use
     * the TransformFilter or the SubstituteModelFilter to apply that
     * heading information.
     *
     * NOTE: When generating a geocentric scene graph, use this filter BEFORE
     * applying the TransformFilter to convert into geocentric space.
     */
    public class AlignFilter : FeatureFilter
    {
        //TODO OSGGIS_META_FILTER( AlignFilter );  
        //TODO OSGGIS_DEFINE_FILTER( AlignFilter );



        /**
         * Constructs a new AlignFilter.
         */
        public AlignFilter()
        {
            setRadius(DEFAULT_RADIUS);
        }

        /**
         * Copy constructor.
         */
        public AlignFilter(AlignFilter rhs)
            : base(rhs)
        {
            radius = rhs.radius;
            output_attribute = rhs.output_attribute;
            alignment_layer_name = rhs.alignment_layer_name;
        }


        // properties

        /**
         * Sets the search radius (in meters). This is the radius around the
         * input point's centroid for which to search for a nearest feature
         * to which to align the point.
         *
         * @param value
         *      New search radius, in meters.
         */
        public void setRadius(double value)
        {
            radius = value;
        }

        /**
         * Gets the search radius (in meters). This is the radius around the
         * input point's centroid for which to search for a nearest feature
         * to which to align the point.
         *
         * @return Search radius, in meters.
         */
        public double getRadius()
        {
            return radius;
        }

        /**
         * Sets the name of the FeatureLayerResource that holds the linear
         * features to which we are aligning.
         *
         * @param name
         *      Name of FeatureLayerResource.
         */
        public void setAlignmentLayerResourceName(string name)
        {
            alignment_layer_name = name;
        }

        /**
         * Gets the name of the FeatureLayerResource that holds the linear
         * features to which we are aligning.
         *
         * @return Name of FeatureLayerResource.
         */
        public string getAlignmentLayerResourceName()
        {
            return alignment_layer_name;
        }

        /**
         * Sets the name of the feature attribute to which to assign
         * the calculated heading.
         *
         * @param name
         *      Output attribute name.
         */
        public void setOutputAttribute(string name)
        {
            output_attribute = name;
        }

        /**
         * Gets the name of the feature attribute to which to assign
         * the calculated heading.
         *
         * @return Output attribute name.
         */
        public string getOutputAttribute()
        {
            return output_attribute;
        }

        // FeatureFilter overrides
        public FeatureList process(Feature input, FilterEnv env);

        // Filter overrides
        public virtual void setProperty(Property p)
        {
            if (p.getName() == "radius")
                setRadius(p.getDoubleValue(getRadius()));
            else if (p.getName() == "output_attribute")
                setOutputAttribute(p.getValue());
            else if (p.getName() == "alignment_layer")
                setAlignmentLayerResourceName(p.getValue());

            base.setProperty(p);
        }

        public virtual Properties getProperties()
        {
            Properties p = base.getProperties();
            if (getRadius() != DEFAULT_RADIUS)
                p.Add(new Property("radius", getRadius()));
            if (getOutputAttribute().Length > 0)
                p.Add(new Property("output_attribute", getOutputAttribute()));
            if (getAlignmentLayerResourceName().Length> 0)
                p.Add(new Property("alignment_layer", getAlignmentLayerResourceName()));
            return p;
        }


        // properties
        protected double radius;
        protected string output_attribute;
        protected string alignment_layer_name;

        // transients
        protected FeatureLayer alignment_layer;
        protected double radius_input_srs;

        private const double DEFAULT_RADIUS = 50.0;

    }
}

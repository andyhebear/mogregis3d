using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    class AttributeFilter : FeatureFilter
    {

        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new AttributeFilter(this); }
        public static string getStaticFilterType() { return "AttributeFilter"; }
        public static FilterFactory getFilterFactory() { return new FilterFactoryImpl<AttributeFilter>(); }

        private string attributeName;
        private string attributeValue;

        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }

        public string AttributeValue
        {
            get { return attributeValue; }
            set { attributeValue = value; }
        }

        public AttributeFilter()
        {
        }

        public AttributeFilter(AttributeFilter rhs)
        {
            this.attributeName = rhs.attributeName;
            this.attributeValue = rhs.attributeValue;
        }

        public override FeatureList process(FeatureList input, FilterEnv env) 
        {
            FeatureList output = new FeatureList();
            foreach (Feature feature in input) 
            {
                if (feature.row[attributeName].ToString().ToLowerInvariant() == attributeValue)
                {
                    output.Add(feature);
                }
            }
            return output;
        }

        public override void setProperty(Property prop)
        {
            if (prop.getName() == "attributeName")
                attributeName = (prop.getValue());
            else if (prop.getName() == "attributeValue")
                attributeValue = (prop.getValue()).ToLowerInvariant();
            base.setProperty(prop);
        }


            



    }
}

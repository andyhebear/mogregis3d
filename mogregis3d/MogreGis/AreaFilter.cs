using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /*
     * Create a new list with features delimit by latitude and longitude  
     */
    public class AreaFilter : FeatureFilter
    {
        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new AreaFilter(this); }
        public static string getStaticFilterType() { return "AreaFilter"; }
        public static FilterFactory getFilterFactory() { return new FilterFactoryImpl<AreaFilter>(); }

        /*
         * Constructs a new area filter.
         */
        public AreaFilter()
        {

        }

        public AreaFilter(AreaFilter rhs)
            //: base(rhs)
        {

        }

        public override FeatureList process(FeatureList input, FilterEnv env)
        {
            FeatureList output = new FeatureList();

            //Boolean encontrado = false;
            SharpMap.Geometries.BoundingBox boundingBox = new SharpMap.Geometries.BoundingBox(longitudeMin,latitudeMin,longitudeMax,latitudeMax);

            foreach (Feature feature in input)
            {
                //if type of features is Point
                if (feature.row.Geometry is SharpMap.Geometries.Point)
                {
                    SharpMap.Geometries.Point p = (SharpMap.Geometries.Point)feature.row.Geometry;
                    if (boundingBox.Contains(p.GetBoundingBox()))
                    {
                        output.Add(feature);
                    }
                }
                //if type of features is Polygon
                else if (feature.row.Geometry is SharpMap.Geometries.Polygon)
                {
                    SharpMap.Geometries.Polygon polygon = (SharpMap.Geometries.Polygon)feature.row.Geometry;
                    if (boundingBox.Contains(polygon.GetBoundingBox()))
                    {
                        output.Add(feature);
                    }
                }
                //if type of features is MultiPolygon
                else if (feature.row.Geometry is SharpMap.Geometries.MultiPolygon)
                {
                    SharpMap.Geometries.MultiPolygon mp = (SharpMap.Geometries.MultiPolygon)feature.row.Geometry;
                    SharpMap.Geometries.BoundingBox bb = mp.GetBoundingBox();
                    if (boundingBox.Contains(bb))
                    {
                        output.Add(feature);
                    }
                }
                
            }

            if (successor != null)
            {
                if (successor is FeatureFilter)
                {
                    FeatureFilter filter = (FeatureFilter)successor;
                    FeatureList l = filter.process(output, env);
                }
                else if (successor is FragmentFilter)
                {
                    FragmentFilter filter = (FragmentFilter)successor;
                    FragmentList l = filter.process(output, env);
                }
            }

            return output;
        }

        public override void setProperty(Property prop)
        {
            if (prop.getName() == "longitudeMin")
                LongitudeMin = float.Parse(prop.getValue());
            else if (prop.getName() == "longitudeMax")
                LongitudeMax = float.Parse(prop.getValue());
            else if (prop.getName() == "latitudeMin")
                LatitudeMin = float.Parse(prop.getValue());
            else if (prop.getName() == "latitudeMax")
                LatitudeMax = float.Parse(prop.getValue());

            base.setProperty(prop);
        }

        public float LongitudeMin
        {
            set
            {
                this.longitudeMin = value;
            }

            get
            {
                return longitudeMin;
            }
        }

        public float LongitudeMax
        {
            set
            {
                this.longitudeMax = value;
            }

            get
            {
                return longitudeMax;
            }
        }

        public float LatitudeMin
        {
            set
            {
                this.latitudeMin = value;
            }

            get
            {
                return latitudeMin;
            }
        }

        public float LatitudeMax
        {
            set
            {
                this.latitudeMax = value;
            }

            get
            {
                return latitudeMax;
            }
        }

        protected float longitudeMin;
        protected float longitudeMax;
        protected float latitudeMin;
        protected float latitudeMax;

    }
}

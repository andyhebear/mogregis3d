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

            foreach (Feature feature in input)
            {
                //if type of features is Point
                if (feature.row.Geometry is SharpMap.Geometries.Point)
                {
                    SharpMap.Geometries.Point p = (SharpMap.Geometries.Point)feature.row.Geometry;

                    if ((p.Y > LatitudeMin) & (p.Y < LatitudeMax) & (p.X > LongitudeMin) & (p.X < LongitudeMax))
                    {
                        output.Add(feature);
                    }
                }
                //if type of features is Polygon
                else if (feature.row.Geometry is SharpMap.Geometries.Polygon)
                {
                    SharpMap.Geometries.Polygon polygon = (SharpMap.Geometries.Polygon)feature.row.Geometry;

                    if ((polygon.Centroid.Y > LatitudeMin) & (polygon.Centroid.Y < LatitudeMax) & (polygon.Centroid.X > LongitudeMin) & (polygon.Centroid.X < LongitudeMax))
                    {
                        output.Add(feature);
                    }
                }
                //if type of features is MultiPolygon
                else if (feature.row.Geometry is SharpMap.Geometries.MultiPolygon)
                {
                    SharpMap.Geometries.MultiPolygon mp = (SharpMap.Geometries.MultiPolygon)feature.row.Geometry;

                    foreach (SharpMap.Geometries.Polygon polygon in mp.Polygons)
                    {
                        /*foreach (SharpMap.Geometries.Point p in polygon.ExteriorRing.Vertices)
                        {
                            if ((p.Y > LatitudeMin) & (p.Y < LatitudeMax) & (p.X > LongitudeMin) & (p.X < LongitudeMax))
                            {
                                output.Add(feature);
                                encontrado = true;
                            }
                            if (encontrado)
                            {
                                break;
                            }
                        }
                        if (encontrado)
                        {
                            encontrado = false;
                            break;
                        }*/

                        /*String pais = "Thailand";
                        String pais2 = (String)feature.row.ItemArray[0];
                        if (pais.CompareTo(pais2) == 0)
                        {
                            pais = "Thailand";
                        }*/

                        if ((polygon.Centroid.Y > LatitudeMin) & (polygon.Centroid.Y < LatitudeMax) & (polygon.Centroid.X > LongitudeMin) & (polygon.Centroid.X < LongitudeMax))
                        {
                            output.Add(feature);
                            break;
                        }
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
                LongitudeMin = int.Parse(prop.getValue());
            else if (prop.getName() == "longitudeMax")
                LongitudeMax = int.Parse(prop.getValue());
            else if (prop.getName() == "latitudeMin")
                LatitudeMin = int.Parse(prop.getValue());
            else if (prop.getName() == "latitudeMax")
                LatitudeMax = int.Parse(prop.getValue());

            base.setProperty(prop);
        }

        public int LongitudeMin
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

        public int LongitudeMax
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

        public int LatitudeMin
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

        public int LatitudeMax
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

        protected int longitudeMin;
        protected int longitudeMax;
        protected int latitudeMin;
        protected int latitudeMax;

    }
}

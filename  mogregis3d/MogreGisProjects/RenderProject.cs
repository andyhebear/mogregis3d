using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data;
//using System.Drawing;
//using System.Windows.Forms;

using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;

using osgGISProjects;

namespace osgGISProjects //cambiar namespace a MogreGisProjects ???
{
    public class RenderProject : osgGISProjects.Project
    {
        public void render2d(string filename)
        {
            Project project = XmlSerializer.loadProject(filename);

            foreach (BuildLayer layer in project.getLayers())
            {
                Source source = layer.getSource();

                BoundingBox envelope = new BoundingBox(0.0, 0.0, 1000.0, 1000.0);
                FeatureDataSet ds = new FeatureDataSet();
                source.DataSource.Open();
                source.DataSource.ExecuteIntersectionQuery(envelope, ds);
                source.DataSource.Close();

                FeatureDataTable features = (FeatureDataTable)ds.Tables[0];

                string label = "Prueba:\n";

                foreach (FeatureDataRow row in features)
                {
                    foreach (Object item in row.ItemArray) //no se que es lo que ha cambiado en la estructura de datos
                        label += " - " + item;
                    label += "\n";
                }

                setLabel(label);

                //Show map
                SharpMap.Map myMap = new SharpMap.Map();
                SharpMap.Layers.VectorLayer myLayer = new SharpMap.Layers.VectorLayer("mapa");
                myLayer.DataSource = source.DataSource;
                myMap.Layers.Add(myLayer);

                myMap.ZoomToExtents();

                this.map = myMap;
            }

            
        }

        public void render3d(string filename)
        {
            //empty
        }

        public void setLabel(string label)
        {
            this.label = label;
        }

        public string getLabel()
        {
            return label;
        }

        public SharpMap.Map getMap()
        {
            return map;
        }

        private SharpMap.Map map;
        private string label;
    }
}

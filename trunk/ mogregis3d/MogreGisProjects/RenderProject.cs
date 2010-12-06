using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;

using Mogre;

using MogreGis;

using osgGISProjects;

namespace osgGISProjects //cambiar namespace a MogreGisProjects ???
{
    public class RenderProject : osgGISProjects.Project
    {
        public void render2d(Project project, PictureBox picBox)
        {

            SharpMap.Map myMap = new SharpMap.Map();

            foreach (BuildLayer layer in project.getLayers())
            {
                Source source = layer.getSource();

                BoundingBox envelope = new BoundingBox(-1000.0, -1000.0, 1000.0, 1000.0);//TODO
                FeatureDataSet ds = new FeatureDataSet();
                source.DataSource.Open();
                source.DataSource.ExecuteIntersectionQuery(envelope, ds);
                source.DataSource.Close();

                FeatureDataTable features = (FeatureDataTable)ds.Tables[0];

                string label = "Trace test:\n";

                foreach (FeatureDataRow row in features)
                {
                    foreach (Object item in row.ItemArray)
                        label += " - " + item;
                    label += "\n";
                }

                setLabel(label);

                //Show map

                //Filters
                FilterGraph graph = project.getFilterGraph(source.getName());
                if (graph != null)
                {
                    foreach (FeatureFilter filter in graph.getFilters())
                    {
                        //aplicar filtro segun el tipo
                        //FilterEnv env = new FilterEnv();
                        FeatureList list = Feature.DataTableToList(features);
                        filter.process(list, null);
                        //falta devolver la lista y procesarla ***************************************
                    }
                }

                SharpMap.Layers.VectorLayer myLayer = new SharpMap.Layers.VectorLayer(layer.getName());
                myLayer.DataSource = source.DataSource;
                myMap.Layers.Add(myLayer);
            }

            myMap.Size = new Size(picBox.Width, picBox.Height);
            myMap.ZoomToExtents();
            picBox.Image = myMap.GetMap();
            this.map = myMap;
        }

        public void render3d(Project project, SceneManager sceneMgr)
        {
            SharpMap.Map myMap = new SharpMap.Map();

            foreach (BuildLayer layer in project.getLayers())
            {
                Source source = layer.getSource();

                BoundingBox envelope = new BoundingBox(-1000.0, -1000.0, 1000.0, 1000.0);//TODO
                FeatureDataSet ds = new FeatureDataSet();
                source.DataSource.Open();
                source.DataSource.ExecuteIntersectionQuery(envelope, ds);
                source.DataSource.Close();

                FeatureDataTable features = (FeatureDataTable)ds.Tables[0];

                //El codigo del PFC

                //**********************************************************************************************************

                //Show map

                //Filters
                FilterGraph graph = project.getFilterGraph(source.getName());
                if (graph != null)
                {

                    graph.Successors();

                    FilterEnv env = new FilterEnv(sceneMgr, source.getName());
                    FeatureList list = Feature.DataTableToList(features);

                    if (graph.getFilter(1) is FeatureFilter)
                    {
                        FeatureFilter filter = (FeatureFilter)graph.getFilter(1);
                        FeatureList l = filter.process(list, env);
                    }
                    else if (graph.getFilter(1) is FragmentFilter)
                    {
                        FragmentFilter filter = (FragmentFilter)graph.getFilter(1);
                        FragmentList l = filter.process(list, env);
                    }

                }
         
                //**********************************************************************************************************

            }

            

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

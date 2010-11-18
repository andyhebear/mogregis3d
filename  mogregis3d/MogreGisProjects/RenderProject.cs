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

                FeatureList list = Feature.DataTableToList(features);

                //cuidado con las entidades dentro del for

                int i = 0;

                SceneNode node = null;
                point3d(source.getName(),i,0,0,0,ref node,sceneMgr);

                foreach (Feature feature in list)
                {
                    //if type of features is Point
                    if (string.Equals(feature.row.Geometry.GetType() , new SharpMap.Geometries.Point().GetType())) 
                    {
                        SharpMap.Geometries.Point p = (SharpMap.Geometries.Point)feature.row.Geometry;

                        i++;
                        point3d(source.getName(), i, (float)p.X, (float)p.Y, 0, ref node, sceneMgr);
                    }

                    //if type of features is MultiPolygon
                    if (string.Equals(feature.row.Geometry.GetType(), new SharpMap.Geometries.MultiPolygon().GetType()))
                    {
                        SharpMap.Geometries.MultiPolygon mp = (SharpMap.Geometries.MultiPolygon)feature.row.Geometry;

                        // 1 MultiPolygon = N polygon
                        foreach(SharpMap.Geometries.Polygon polygon in mp.Polygons)
                        {

                            //1 polygon = N vertices
                            foreach (SharpMap.Geometries.Point point in polygon.ExteriorRing.Vertices)
                            {
                                i++;
                                //if ((i % 3) == 0)//pinta menos para aligerar
                                //{
                                    point3d(source.getName(), i, (float)point.X, (float)point.Y, 0, ref node, sceneMgr);
                                //}
                                
                            }
                        }

                    }
                    /*
                     * countries = 147 multipolygon
                     * 1 MultiPolygon = N polygon
                     * 1 polygon = N vertices
                     */
                }

                i = 0;//breakpoint
         
                //**********************************************************************************************************

            }

            

        }

        public void point3d(string name, int id, float x, float y, float z, ref SceneNode node, SceneManager sceneMgr)
        {
            Entity ent;
            if (node == null)//point of reference 0,0,0
            {
                ent = sceneMgr.CreateEntity(name + "INI", "ninja.mesh");
                node = sceneMgr.RootSceneNode.CreateChildSceneNode(name + "NodeINI", new Vector3(y, z, x));
                node.AttachObject(ent);
            }
            else//create new point
            {
                float xAux = 0.0F;
                float yAux = 0.0F;
                SceneNode nodeAux;

                xAux = x * 51.0f; //longitud eje X
                yAux = y * 51.0f; //latitud eje Y

                ent = sceneMgr.CreateEntity(name + id, "cube.mesh");
                //ent.SetMaterialName("Examples/Chrome");
                nodeAux = node.CreateChildSceneNode(name + "Node_" + id, new Vector3(yAux, z, xAux));
                nodeAux.AttachObject(ent);
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

// Copyright 2007 - Rory Plaire (codekaizen@gmail.com)
//
// This file is part of SharpMap.
// SharpMap is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// SharpMap is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SharpMapExample.Properties;
using SharpMap.Data;
using SharpMap.Data.Providers;
using SharpMap.Forms;
using SharpMap.Geometries;
using SharpMap.Layers;
using GeoPoint = SharpMap.Geometries.Point;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;
using Mogre;
using MogreGis;
using osgGISProjects;

namespace SharpMapExample
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<string, Color> _colorTable = new Dictionary<string, Color>();

        private readonly Dictionary<string, ILayerFactory> _layerFactoryCatalog =
            new Dictionary<string, ILayerFactory>();

        private readonly Dictionary<string, Bitmap> _symbolTable = new Dictionary<string, Bitmap>();
        private osgGISProjects.MogreApp app;

        public MainForm()
        {
            InitializeComponent();

            registerSymbols();

            registerKnownColors(_colorTable);

            registerLayerFactories();

            MainMapImage.MapQueried += MainMapImage_MapQueried;
        }

        private void SetupMogre()
        {
            Vector3 position;
            Vector3 lookAt;
            Vector4 backGroundColor;
            int viewDistance;

            if (prj.CameraPosition == null)
            {
                position = new Vector3(0, 0, 0);
            }
            else
            {
                position = prj.CameraPosition;
            }
            if (prj.LookAt == null)
            {
                lookAt = new Vector3(0, 0, 0);
            }
            else
            {
                lookAt = prj.LookAt;
            }
            if (prj.getBackGroundColor() == null)
            {
                backGroundColor = new Vector4(0, 0, 0, 0);
            }
            else
            {
                backGroundColor = prj.getBackGroundColor();
            }
           
            viewDistance = prj.ViewDistance;
            app = new osgGISProjects.MogreApp(position, lookAt, backGroundColor, viewDistance, prj.getBackGroundMaterial());
            //prj.loadMogreResourceLocation(app);
        }

        private void registerSymbols()
        {
            _symbolTable["Notices"] = Resources.Chat;
            _symbolTable["Radioactive Fuel Rods"] = Resources.DATABASE;
            _symbolTable["Bases"] = Resources.Flag;
            _symbolTable["Houses"] = Resources.Home;
            _symbolTable["Measures"] = Resources.PIE_DIAGRAM;
            _symbolTable["Contacts"] = Resources.Women;
            _symbolTable["Prospects"] = Resources.Women_1;
        }

        private static void registerKnownColors(Dictionary<string, Color> colorTable)
        {
            foreach (string colorName in Enum.GetNames(typeof(KnownColor)))
            {
                KnownColor color = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                colorTable[colorName] = Color.FromKnownColor(color);
            }
        }

        private void registerLayerFactories()
        {
            //			ConfigurationManager.GetSection("LayerFactories");
            _layerFactoryCatalog[".shp"] = new ShapeFileLayerFactory();
        }

        private void addLayer(ILayer layer)
        {
            MainMapImage.Map.Layers.Add(layer);
            LayersDataGridView.Rows.Insert(0, true, getLayerTypeIcon(layer.GetType()), layer.LayerName);
        }

        private void addNewRandomGeometryLayer()
        {
            Random rndGen = new Random();
            Collection<Geometry> geometry = new Collection<Geometry>();

            VectorLayer layer = new VectorLayer(String.Empty);

            switch (rndGen.Next(3))
            {
                case 0:
                    {
                        generatePoints(geometry, rndGen);
                        KeyValuePair<string, Bitmap> symbolEntry = getSymbolEntry(rndGen.Next(_symbolTable.Count));
                        layer.Style.Symbol = symbolEntry.Value;
                        layer.LayerName = symbolEntry.Key;
                    }
                    break;
                case 1:
                    {
                        generateLines(geometry, rndGen);
                        KeyValuePair<string, Color> colorEntry = getColorEntry(rndGen.Next(_colorTable.Count));
                        layer.Style.Line = new Pen(colorEntry.Value);
                        layer.LayerName = String.Format("{0} lines", colorEntry.Key);
                    }
                    break;
                case 2:
                    {
                        generatePolygons(geometry, rndGen);
                        KeyValuePair<string, Color> colorEntry = getColorEntry(rndGen.Next(_colorTable.Count));
                        layer.Style.Fill = new SolidBrush(colorEntry.Value);
                        layer.LayerName = String.Format("{0} squares", colorEntry.Key);
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }

            GeometryProvider provider = new GeometryProvider(geometry);
            layer.DataSource = provider;

            addLayer(layer);
        }

        private KeyValuePair<string, Bitmap> getSymbolEntry(int index)
        {
            foreach (KeyValuePair<string, Bitmap> entry in _symbolTable)
            {
                if (index-- == 0)
                    return entry;
            }

            throw new InvalidOperationException();
        }

        private KeyValuePair<string, Color> getColorEntry(int index)
        {
            foreach (KeyValuePair<string, Color> entry in _colorTable)
            {
                if (index-- == 0)
                    return entry;
            }

            throw new InvalidOperationException();
        }

        private Color getRandomColor(Random rndGen)
        {
            return Color.FromArgb(rndGen.Next(255), rndGen.Next(255), rndGen.Next(255));
        }

        private void generatePolygons(Collection<Geometry> geometry, Random rndGen)
        {
            int numPolygons = rndGen.Next(10, 100);
            for (int polyIndex = 0; polyIndex < numPolygons; polyIndex++)
            {
                Polygon polygon = new Polygon();
                Collection<GeoPoint> verticies = new Collection<GeoPoint>();
                GeoPoint upperLeft = new GeoPoint(rndGen.NextDouble() * 1000, rndGen.NextDouble() * 1000);
                double sideLength = rndGen.NextDouble() * 50;

                // Make a square
                verticies.Add(upperLeft);
                verticies.Add(new GeoPoint(upperLeft.X + sideLength, upperLeft.Y));
                verticies.Add(new GeoPoint(upperLeft.X + sideLength, upperLeft.Y - sideLength));
                verticies.Add(new GeoPoint(upperLeft.X, upperLeft.Y - sideLength));
                polygon.ExteriorRing = new LinearRing(verticies);

                geometry.Add(polygon);
            }
        }

        private void generateLines(Collection<Geometry> geometry, Random rndGen)
        {
            int numLines = rndGen.Next(10, 100);
            for (int lineIndex = 0; lineIndex < numLines; lineIndex++)
            {
                LineString line = new LineString();
                Collection<GeoPoint> verticies = new Collection<GeoPoint>();

                int numVerticies = rndGen.Next(4, 15);

                GeoPoint lastPoint = new GeoPoint(rndGen.NextDouble() * 1000, rndGen.NextDouble() * 1000);
                verticies.Add(lastPoint);

                for (int vertexIndex = 0; vertexIndex < numVerticies; vertexIndex++)
                {
                    GeoPoint nextPoint = new GeoPoint(lastPoint.X + rndGen.Next(-50, 50),
                                                      lastPoint.Y + rndGen.Next(-50, 50));
                    verticies.Add(nextPoint);

                    lastPoint = nextPoint;
                }

                line.Vertices = verticies;

                geometry.Add(line);
            }
        }

        private void generatePoints(Collection<Geometry> geometry, Random rndGen)
        {
            int numPoints = rndGen.Next(10, 100);
            for (int pointIndex = 0; pointIndex < numPoints; pointIndex++)
            {
                GeoPoint point = new GeoPoint(rndGen.NextDouble() * 1000, rndGen.NextDouble() * 1000);
                geometry.Add(point);
            }
        }

        private void loadLayer()
        {
            DialogResult result = AddLayerDialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                foreach (string fileName in AddLayerDialog.FileNames)
                {
                    string extension = Path.GetExtension(fileName);
                    ILayerFactory layerFactory = null;

                    if (!_layerFactoryCatalog.TryGetValue(extension, out layerFactory))
                        continue;

                    ILayer layer = layerFactory.Create(Path.GetFileNameWithoutExtension(fileName), fileName);

                    addLayer(layer);
                }

                changeUIOnLayerSelectionChange();

                MainMapImage.Refresh();
            }
        }

        private void removeLayer()
        {
            if (LayersDataGridView.SelectedRows.Count == 0)
                return;

            string layerName = LayersDataGridView.SelectedRows[0].Cells[2].Value as string;

            ILayer layerToRemove = null;

            foreach (ILayer layer in MainMapImage.Map.Layers)
                if (layer.LayerName == layerName)
                {
                    layerToRemove = layer;
                    break;
                }

            MainMapImage.Map.Layers.Remove(layerToRemove);
            LayersDataGridView.Rows.Remove(LayersDataGridView.SelectedRows[0]);
        }

        private void zoomToExtents()
        {
            MainMapImage.Map.ZoomToExtents();
            MainMapImage.Refresh();
        }

        private void changeMode(MapImage.Tools tool)
        {
            MainMapImage.ActiveTool = tool;

            ZoomInModeToolStripButton.Checked = (tool == MapImage.Tools.ZoomIn);
            ZoomOutModeToolStripButton.Checked = (tool == MapImage.Tools.ZoomOut);
            PanToolStripButton.Checked = (tool == MapImage.Tools.Pan);
            QueryModeToolStripButton.Checked = (tool == MapImage.Tools.Query);
        }

        private object getLayerTypeIcon(Type type)
        {
            if (type == typeof(VectorLayer))
            {
                return Resources.polygon;
            }

            return Resources.Raster;
        }

        private void changeUIOnLayerSelectionChange()
        {
            bool isLayerSelected = false;
            int layerIndex = -1;

            if (LayersDataGridView.SelectedRows.Count > 0)
            {
                isLayerSelected = true;
                layerIndex = LayersDataGridView.SelectedRows[0].Index;
            }

            RemoveLayerToolStripButton.Enabled = isLayerSelected;
            RemoveLayerToolStripMenuItem.Enabled = isLayerSelected;

            if (layerIndex < 0)
            {
                MoveUpToolStripMenuItem.Visible = false;
                MoveDownToolStripMenuItem.Visible = false;
                LayerContextMenuSeparator.Visible = false;
                return;
            }
            else
            {
                MoveUpToolStripMenuItem.Visible = true;
                MoveDownToolStripMenuItem.Visible = true;
                LayerContextMenuSeparator.Visible = true;
            }

            if (layerIndex == 0)
            {
                MoveUpToolStripMenuItem.Enabled = false;
            }
            else
            {
                MoveUpToolStripMenuItem.Enabled = true;
            }

            if (layerIndex == LayersDataGridView.Rows.Count - 1)
            {
                MoveDownToolStripMenuItem.Enabled = false;
            }
            else
            {
                MoveDownToolStripMenuItem.Enabled = true;
            }
        }

        private void MainMapImage_MapQueried(FeatureDataTable data)
        {
            FeaturesDataGridView.DataSource = data;
        }

        private void AddLayerToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { loadLayer(); });
        }

        private void RemoveLayerToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { removeLayer(); });
        }

        private void AddLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { loadLayer(); });
        }

        private void RemoveLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { removeLayer(); });
        }

        private void ZoomToExtentsToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { zoomToExtents(); });
        }

        private void PanToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapImage.Tools.Pan); });
        }

        private void QueryModeToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapImage.Tools.Query); });
        }

        private void ZoomInModeToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapImage.Tools.ZoomIn); });
        }

        private void ZoomOutModeToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapImage.Tools.ZoomOut); });
        }

        private void MoveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LayersDataGridView.SelectedRows.Count == 0)
                return;

            if (LayersDataGridView.SelectedRows[0].Index == 0)
                return;

            int rowIndex = LayersDataGridView.SelectedRows[0].Index;
            DataGridViewRow row = LayersDataGridView.Rows[rowIndex];
            LayersDataGridView.Rows.RemoveAt(rowIndex);
            LayersDataGridView.Rows.Insert(rowIndex - 1, row);

            int layerIndex = MainMapImage.Map.Layers.Count - rowIndex - 1;
            ILayer layer = MainMapImage.Map.Layers[layerIndex];
            MainMapImage.Map.Layers.RemoveAt(layerIndex);
            MainMapImage.Map.Layers.Insert(layerIndex + 1, layer);
        }

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LayersDataGridView.SelectedRows.Count == 0)
                return;

            if (LayersDataGridView.SelectedRows[0].Index == LayersDataGridView.Rows.Count - 1)
                return;

            int rowIndex = LayersDataGridView.SelectedRows[0].Index;
            DataGridViewRow row = LayersDataGridView.Rows[rowIndex];
            LayersDataGridView.Rows.RemoveAt(rowIndex);
            LayersDataGridView.Rows.Insert(rowIndex + 1, row);

            int layerIndex = MainMapImage.Map.Layers.Count - rowIndex - 1;
            ILayer layer = MainMapImage.Map.Layers[layerIndex];
            MainMapImage.Map.Layers.RemoveAt(layerIndex);
            MainMapImage.Map.Layers.Insert(layerIndex - 1, layer);
        }

        private void LayersDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                changeUIOnLayerSelectionChange();

                if (LayersDataGridView.SelectedRows.Count > 0)
                {
                    MainMapImage.QueryLayerIndex =
                        LayersDataGridView.SelectedRows[0].Index;
                }
            });
        }

        private void MainMapImage_MouseMove(GeoPoint WorldPos, MouseEventArgs ImagePos)
        {
            CoordinatesLabel.Text = String.Format("Coordinates: {0:N5}, {1:N5}", WorldPos.X, WorldPos.Y);
        }

        private void AddNewRandomGeometryLayer_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { addNewRandomGeometryLayer(); });
        }

 

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MogreApp.setLocations(prj.getMogreLocations());
            SetupMogre();
            Mogre.SceneManager sm = app.SceneManager;

            foreach (MogreGis.FilterGraph graph in prj.getFilterGraphs())
            {

                foreach (osgGISProjects.Source source in prj.getSources())
                {
                    if (Path.GetExtension(source.getURI()) != ".shp")
                    {
                        throw new NotImplementedException();
                    }

                    if (source.getName() == graph.getName())
                    {
                        MogreGis.FilterEnv env = null;
                        MogreGis.FeatureList featureList = null;

                        FeatureDataSet ds = new FeatureDataSet();
                        source.DataSource.Open();
                        source.DataSource.ExecuteIntersectionQuery(source.DataSource.GetExtents(), ds);
                        source.DataSource.Close();
                        FeatureDataTable features = (FeatureDataTable)ds.Tables[0];
                        featureList = MogreGis.Feature.DataTableToList(features);

                        foreach (Script script in prj.getScripts())
                        {
                            Registry.instance().GetEngine("Python").run(script);
                        }

                        foreach (Feature feature in featureList)
                        {
                            SharpMapSpatialReferenceFactory smsrf = new SharpMapSpatialReferenceFactory();
                            ShapeFile shp = new ShapeFile(source.getURI());
                            shp.Open();
                            SharpMapSpatialReference sr;
                            sr = (SharpMapSpatialReference)smsrf.createSRSfromWKT(shp.CoordinateSystem.WKT);
                            feature.getGeometry().SpatialReference = sr;
                        }

                        env = new MogreGis.FilterEnv(sm, "env" + graph.getName());
                        env.setScriptEngine(Registry.instance().GetEngine("Python"));
                        foreach (MogreGis.Filter filter in graph.getFilters())
                        {
                            if (filter is MogreGis.FragmentFilter)
                            {
                                (filter as MogreGis.FragmentFilter).process(featureList, env);
                            }
                            if (filter is MogreGis.FeatureFilter)
                            {
                                featureList = (filter as MogreGis.FeatureFilter).process(featureList, env);
                            }
                        }
                    }
                }
            }
            app.getRoot().StartRendering();
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenProjectFileDialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                prj = osgGISProjects.XmlSerializer.loadProject(OpenProjectFileDialog.FileName);

                foreach (osgGISProjects.Source source in prj.getSources())
                {
                    string filename = source.getURI();

                    ILayerFactory layerFactory = null;
                    if (!_layerFactoryCatalog.TryGetValue(Path.GetExtension(filename), out layerFactory))
                        continue;
                    ILayer layer = layerFactory.Create(Path.GetFileNameWithoutExtension(filename), filename);
                    addLayer(layer);
                }
            }
        }

        private osgGISProjects.Project prj;

    }

    //public class MogreApp : Mogre.TutorialFramework.BaseApplication
    //{
    //    public MogreApp()
    //    {
    //        this.Setup();
    //    }

    //    protected override void CreateScene()
    //    { }

    //    public SceneManager SceneManager
    //    {
    //        get { return mSceneMgr; }
    //    }

    //    public Root getRoot()
    //    {
    //        return this.mRoot;
    //    }
    //}
}
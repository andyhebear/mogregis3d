using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using osgGISProjects;

using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;

namespace MainAppTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Project project = XmlSerializer.loadProject("Test1.xml");

            foreach (Source source in project.getSources())
            {
                BoundingBox envelope = new BoundingBox(0.0,0.0,1000.0,1000.0);
                FeatureDataSet ds = new FeatureDataSet();
                source.DataSource.Open();
                source.DataSource.ExecuteIntersectionQuery(envelope, ds);
                source.DataSource.Close();

                FeatureDataTable features = (FeatureDataTable)ds.Tables[0];

                foreach (FeatureDataRow row in features)
                { 
                    //imprimir alguna traza
                }
            }
        }
    }
}

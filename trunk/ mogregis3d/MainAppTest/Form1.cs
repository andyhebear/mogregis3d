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
            RenderProject project = new RenderProject();
            project.render2d("Test1.xml");

            label1.Text = project.getLabel();
            project.getMap().Size = new Size(600,300);
            pictureBox1.Image = project.getMap().GetMap();
        }
    }
}

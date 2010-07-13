using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using osgGISProjects;

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
            Project prj = XmlSerializer.loadProject("Test1.xml");
            project.render2d(prj, pictureBox1);

            label1.Text = project.getLabel();

        }
    }
}

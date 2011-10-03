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

        private void button1_Click(object sender, EventArgs e)
        {
            string fichero = "";
            if (radioButton1.Checked == true)
            {
                fichero = "Test1.xml";
            }
            else if (radioButton2.Checked == true)
            {
                fichero = "Test2.xml";
            }

            RenderProject project = new RenderProject();
            Project prj = XmlSerializer.loadProject(fichero);
            project.render2d(prj, pictureBox1);

            label1.Text = project.getLabel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Click += new System.EventHandler(button1_Click);
        }
    }
}

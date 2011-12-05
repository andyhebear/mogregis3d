using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using osgGISProjects;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //--> Define the data name and source
        private const string DATA_NAME = "countries";
        private const string DATA_PATH = @"../../../data/countries.shp";
        
        private const string DATA_NAME2 = "cities";
        private const string DATA_PATH2 = @"../../../data/cities.SHP";

        public Form1()
        {
            InitializeComponent();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.PageUp)
            {
                _sharpMap.Zoom += 10;
                RefreshMap();
                System.Console.WriteLine(_sharpMap.Zoom);
            }
            if (e.KeyData == Keys.PageDown)
            {
                _sharpMap.Zoom -= 10;
                RefreshMap();
                System.Console.WriteLine(_sharpMap.Zoom);
            }
            if (e.KeyData == Keys.Left)
            {
                _sharpMap.Center.X-=10;
                RefreshMap();
            }
            if (e.KeyData == Keys.Right)
            {
                _sharpMap.Center.X+=10;
                RefreshMap();
            }
            if (e.KeyData == Keys.Up)
            {
                _sharpMap.Center.Y += 10;
                RefreshMap();
            }
            if (e.KeyData == Keys.Down)
            {
                _sharpMap.Center.Y -= 10;
                RefreshMap();
            }
        }

        private void RefreshMap()
        {
            pictureBox1.Image = _sharpMap.GetMap();
        }

      
            
        private void button1_Click(object sender, EventArgs e)
        {
            string fichero = "";
            
                fichero = "Test1.xml";
            

            RenderProject project = new RenderProject();
            Project prj = XmlSerializer.loadProject(fichero);
            project.render2d(prj, pictureBox1);

        }
        
    }
}

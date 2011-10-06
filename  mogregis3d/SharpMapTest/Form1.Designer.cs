using System.Drawing;
namespace WindowsFormsApplication1
{
    partial class Form1
    {

        //--> Define the SharpMap object
        SharpMap.Map _sharpMap;

        //--> Set the zoom factor percentage
        const float ZOOM_FACTOR = 0.3f;

        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(720, 452);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 501);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);


            //--> Initialize the map
            _sharpMap = new SharpMap.Map(new Size(600, 300));
            _sharpMap.BackColor = Color.White;
            //--> Add the countries shapefile to the map
            SharpMap.Layers.VectorLayer placesLayer = new SharpMap.Layers.VectorLayer(DATA_NAME);
            placesLayer.DataSource = new SharpMap.Data.Providers.ShapeFile(DATA_PATH, true);
            _sharpMap.Layers.Add(placesLayer);
            SharpMap.Layers.VectorLayer waterWaysLayer = new SharpMap.Layers.VectorLayer(DATA_NAME2);
            waterWaysLayer.DataSource = new SharpMap.Data.Providers.ShapeFile(DATA_PATH2, true);
            _sharpMap.Layers.Add(waterWaysLayer);
            //--> Zoom the map to the entire extent
            _sharpMap.ZoomToExtents();

            placesLayer.Style.Fill = Brushes.LightGreen;
            placesLayer.Style.EnableOutline = true;
            placesLayer.Style.Outline = Pens.Black;

            waterWaysLayer.Style.Fill = Brushes.Aqua;
            waterWaysLayer.Style.EnableOutline = true;
            waterWaysLayer.Style.Outline = Pens.Blue;

            _sharpMap.BackColor = Color.LightBlue;

            RefreshMap();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
    }
}


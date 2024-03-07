using System;
using System.Drawing;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;

namespace PGTA_BYTES
{
    public partial class Mapa : Form
    {
        private double targetLatitud = 65; 
        private double targetLongitud = -60.6919; 
        private double stepSize = 0.07; 
        private Timer timer;

        public Mapa()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 5;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            double newLatitud = gMapControl1.Position.Lat + stepSize;
            double newLongitud = gMapControl1.Position.Lng;

            
            if (newLatitud >= targetLatitud)
            {
                
                timer.Stop();
                return;
            }

            gMapControl1.Position = new PointLatLng(newLatitud, newLongitud);
        }

        private void Mapa_Load(object sender, EventArgs e)
        {
            
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(32.7157, -117.1611);
            gMapControl1.Zoom = 3; // Zoom baix per veure més part del mapa
            gMapControl1.CanDragMap = false;
            timer.Start();            
            CargarImagen("C:\\Users\\Usuario-App\\Desktop\\5A\\PGTA-PROJECT\\PGTA-Bytes\\PGTA-BYTES\\avion4.png", 32.7157, -117.1611);
        }

        private void CargarImagen(string nombreImagen, double latitud, double longitud)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile(nombreImagen);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Width = 15; 
            pictureBox.Height = 15;          
            PointLatLng puntoMapa = new PointLatLng(latitud, longitud);
            GPoint puntoPantalla = gMapControl1.FromLatLngToLocal(puntoMapa);
            pictureBox.Location = new Point((int)puntoPantalla.X, (int)puntoPantalla.Y);

            gMapControl1.Controls.Add(pictureBox);
            pictureBox.BringToFront();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}



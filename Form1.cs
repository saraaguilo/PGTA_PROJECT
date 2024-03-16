using System;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using System.Drawing;

//namespace FlightTracker
//{
//    public partial class Form1 : Form
//    {
//        GMapOverlay planeOverlay;

//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            // Configurar el control GMapControl
//            gMapControl1.MapProvider = GMapProviders.GoogleMap;
//            gMapControl1.Position = new PointLatLng(40.712776, -74.005974); // Establecer la posición inicial del mapa (Nueva York, por ejemplo)
//            gMapControl1.MinZoom = 0;
//            gMapControl1.MaxZoom = 24;
//            gMapControl1.Zoom = 10;
//            gMapControl1.DragButton = MouseButtons.Left;

//            // Crear una capa para la trayectoria del avión
//            planeOverlay = new GMapOverlay("plane");
//            gMapControl1.Overlays.Add(planeOverlay);
//        }

//        private void DrawFlightPath(PointLatLng[] points)
//        {
//            // Crear una polilínea para representar la trayectoria del avión
//            GMapRoute route = new GMapRoute(points, "FlightPath")
//            {
//                Stroke = new Pen(Color.Red, 2)
//            };

//            // Agregar la polilínea a la capa de la trayectoria del avión
//            planeOverlay.Routes.Add(route);
//        }

//        // Ejemplo de cómo utilizar la función DrawFlightPath
//        private void btnDrawPath_Click(object sender, EventArgs e)
//        {
//            // Supongamos que tenemos una lista de puntos que representan la trayectoria del avión
//            PointLatLng[] flightPoints = new PointLatLng[]
//            {
//                new PointLatLng(40.712776, -74.005974), // Punto de partida
//                new PointLatLng(34.052235, -118.243683), // Punto intermedio
//                new PointLatLng(37.774929, -122.419416) // Punto de destino
//            };

//            // Dibujar la trayectoria del avión en el mapa
//            DrawFlightPath(flightPoints);
//        }
//    }
//}
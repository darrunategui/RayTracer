using CS3388_Graphics;
using CS3388_Graphics.Bresenham;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Threading.Tasks;

namespace NonRecursiveRayTracer
{
   public partial class SceneRenderer : Form
   {
      /// <summary>
      /// Camera position
      /// </summary>
      private Point3D _e = new Point3D(50, 0, 10);

      /// <summary>
      /// Gaze point
      /// </summary>
      private Point3D _g = new Point3D(0, 0, 9);

      /// <summary>
      /// Up direction
      /// </summary>
      private Vector3D _p = new Vector3D(0, 0, 1);

      /// <summary>
      /// List of objects to render
      /// </summary>
      private List<GenericObject> _objects = new List<GenericObject>();

      /// <summary>
      /// Light source 
      /// </summary>
      private PointLight _light =  new PointLight(Color.FromScRgb(1, 1, 1, 1), new Point3D(1000, 1000, 1000));

      /// <summary>
      /// Light source at infinity
      /// </summary>
      private DirectionalLight _sun = new DirectionalLight(Color.FromScRgb(1, 1, 1, 1), new Vector3D(0, 0, -1));

      /// <summary>
      /// Constructor
      /// </summary>
      public SceneRenderer()
      {
         InitializeComponent();
         Matrix3D m  = Matrix3D.Identity;

         #region Add objects to render
         // Plane
         _objects.Add(new Plane(m: Matrix3D.Identity,
            c: Color.FromRgb(0, 0, 0),
            sCol: Color.FromRgb(255, 255, 255),
            dCol: Colors.WhiteSmoke,
            aCol: Colors.WhiteSmoke,
            sCoef: 0.1,
            dCoef: 0.45,
            aCoef: 0.45,
            f: 80));

         // Hat
         m.M11 = 5; m.M22 = 5; m.M33 = 3;
         m.OffsetX = 20;
         m.OffsetY = 0;
         m.OffsetZ = 18;
         _objects.Add(new Cone(m: m,
            c: Color.FromRgb(0, 10, 0),
            sCol: Color.FromRgb(255, 255, 255),
            dCol: Colors.Cyan,
            aCol: Colors.Cyan,
            sCoef: 0.3,
            dCoef: 0.35,
            aCoef: 0.35,
            f: 13));

         // Face
         m.M11 = 8; m.M22 = 8; m.M33 = 8;
         m.OffsetX = 20;
         m.OffsetY = 0;
         m.OffsetZ = 8;
         _objects.Add(new Sphere(m: m,
            c: Color.FromRgb(0, 10, 0),
            sCol: Color.FromRgb(255, 255, 255),
            dCol: Colors.White,
            aCol: Colors.White,
            sCoef: 0.3,
            dCoef: 0.35,
            aCoef: 0.35,
            f: 13));

         // Left eye
         //m.M11 = 2; m.M22 = 2; m.M33 = 2;
         //m.OffsetX = 26;
         //m.OffsetY = -2;
         //m.OffsetZ = 11;
         //_objects.Add(new Sphere(m: m,
         //   c: Color.FromRgb(0, 10, 0),
         //   sCol: Color.FromRgb(255, 255, 255),
         //   dCol: Colors.Wheat,
         //   aCol: Colors.Wheat,
         //   sCoef: 0.3,
         //   dCoef: 0.35,
         //   aCoef: 0.35,
         //   f: 13));
         //// Left pupil
         //m.M11 = 0.3; m.M22 = 0.3; m.M33 = 0.3;
         //m.OffsetX = 27.8;
         //m.OffsetY = -1.5;
         //m.OffsetZ = 11.5;
         //_objects.Add(new Sphere(m: m,
         //   c: Color.FromRgb(0, 10, 0),
         //   sCol: Color.FromRgb(255, 255, 255),
         //   dCol: Colors.Black,
         //   aCol: Colors.Black,
         //   sCoef: 0.1,
         //   dCoef: 0.45,
         //   aCoef: 0.45,
         //   f: 13));

         //// Right eye
         //m.M11 = 2; m.M22 = 2; m.M33 = 2;
         //m.OffsetX = 26;
         //m.OffsetY = 2;
         //m.OffsetZ = 11;
         //_objects.Add(new Sphere(m: m,
         //   c: Color.FromRgb(0, 10, 0),
         //   sCol: Color.FromRgb(255, 255, 255),
         //   dCol: Colors.Wheat,
         //   aCol: Colors.Wheat,
         //   sCoef: 0.3,
         //   dCoef: 0.35,
         //   aCoef: 0.35,
         //   f: 13));
         //// Right pupil
         //m.M11 = 0.3; m.M22 = 0.3; m.M33 = 0.3;
         //m.OffsetX = 27.8;
         //m.OffsetY = 2.8;
         //m.OffsetZ = 11.5;
         //_objects.Add(new Sphere(m: m,
         //   c: Color.FromRgb(0, 10, 0),
         //   sCol: Color.FromRgb(255, 255, 255),
         //   dCol: Colors.Black,
         //   aCol: Colors.Black,
         //   sCoef: 0.1,
         //   dCoef: 0.45,
         //   aCoef: 0.45,
         //   f: 13));

         //// Mouth
         //m.M11 = 1; m.M22 = 1; m.M33 = 3;
         //m.OffsetX = 27;
         //m.OffsetY = 6;
         //m.OffsetZ = 0;
         //_objects.Add(new Cylinder(m: m*Extensions.XAxisRotationMatrix(Math.PI/2.0),
         //   c: Color.FromRgb(0, 10, 0),
         //   sCol: Color.FromRgb(255, 255, 255),
         //   dCol: Colors.LightBlue,
         //   aCol: Colors.LightBlue,
         //   sCoef: 0.3,
         //   dCoef: 0.35,
         //   aCoef: 0.35,
         //   f: 13));
         //// Mouth ends
         //m.M11 = 1; m.M22 = 1; m.M33 = 1;
         //m.OffsetX = 27;
         //m.OffsetY = 3;
         //m.OffsetZ = 6;
         //_objects.Add(new Sphere(m: m,
         //   c: Color.FromRgb(0, 10, 0),
         //   sCol: Color.FromRgb(255, 255, 255),
         //   dCol: Colors.LightBlue,
         //   aCol: Colors.LightBlue,
         //   sCoef: 0.3,
         //   dCoef: 0.35,
         //   aCoef: 0.35,
         //   f: 13));
         //m.M11 = 1; m.M22 = 1; m.M33 = 1;
         //m.OffsetX = 27;
         //m.OffsetY = -3;
         //m.OffsetZ = 6;
         //_objects.Add(new Sphere(m: m,
         //   c: Color.FromRgb(0, 10, 0),
         //   sCol: Color.FromRgb(255, 255, 255),
         //   dCol: Colors.LightBlue,
         //   aCol: Colors.LightBlue,
         //   sCoef: 0.3,
         //   dCoef: 0.35,
         //   aCoef: 0.35,
         //   f: 13));
         #endregion
      }

      /// <summary>
      /// Event occurs when the form loads.
      /// </summary>
      private void Form_Load(object sender, EventArgs e)
      {
         // Invalidate the pixel grid to render the scene
         //pixelGrid.Invalidate();
      }

      /// <summary>
      /// Event occurs when the pixel grid needs to be re-painted.
      /// </summary>
      private void pixelGrid_Paint(object sender, PaintEventArgs e)
      {
         // Initialize required objects
         SyntheticCamera c = new SyntheticCamera(_e, _g, _p, 15, 150, 45.0, 1.0, pixelGrid.Grid.Width, pixelGrid.Grid.Height);
         DrawingController drawer = new DrawingController(e.Graphics);
         RayTracer ray = new RayTracer(c, 1);

         int superWidth = pixelGrid.Grid.Width*ray.ResolutionMultiplier;
         int superHeight = pixelGrid.Grid.Height*ray.ResolutionMultiplier;

         // Buffer holds the super width and super height colors to eventually avg and render.
         Color[,] buffer = new Color[superHeight, superWidth];

         // Loop through each pixel and trace a ray through it to find an intersection with the object
         Parallel.For(0, superHeight, (y) =>
          {
             Parallel.For(0, superWidth, (x) =>
             {
                PointLight pointLight = new PointLight(Color.FromScRgb(1, 1, 1, 1), new Point3D(1000, 1000, 1000));
                DirectionalLight sun = new DirectionalLight(Color.FromScRgb(1, 1, 1, 1), new Vector3D(0, 0, -1));
                buffer[y, x] = ray.RayTrace(_objects, x, y, pointLight, sun);
             });
          });

         // Calculates the avg's of the super resolution buffer and displys to screen.
         for (int i = 0; i < buffer.GetLength(0); i+=ray.ResolutionMultiplier)
         {
            for (int j = 0; j < buffer.GetLength(1); j+=ray.ResolutionMultiplier)
            {
               // Add all the rbg values for each super resolution block of points.
               float r = 0, g = 0, b = 0;
               for (int m=0; m<ray.ResolutionMultiplier; ++m)
               {
                  for (int n=0; n<ray.ResolutionMultiplier; ++n)
                  {
                     r += buffer[i+m, j+n].ScR;
                     g += buffer[i+m, j+n].ScG;
                     b += buffer[i+m, j+n].ScB;
                  }
               }

               // Avg the block of points to 1 pixel
               float avgR = (float)(r/Math.Pow(ray.ResolutionMultiplier, 2));
               float avgG = (float)(g/Math.Pow(ray.ResolutionMultiplier, 2));
               float avgB = (float)(b/Math.Pow(ray.ResolutionMultiplier, 2));

               drawer.Color = Color.FromScRgb(1, avgR, avgG, avgB).ToDColor();
               drawer.DrawPoint(new System.Drawing.Point(j/ray.ResolutionMultiplier, i/ray.ResolutionMultiplier));
            }
         }
      }
   }
}

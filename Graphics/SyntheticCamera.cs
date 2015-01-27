using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CS3388_Graphics
{
   /// <summary>
   /// Simulates a synthetic camera
   /// </summary>
   public class SyntheticCamera
   {
      /// <summary>
      /// Gets the a value associated with the perspective projection transformation matrix.
      /// </summary>
      public double A 
      {
         get
         {
            return -1.0*(Far+Near)/(Far-Near);
         }
      }

      /// <summary>
      /// Gets the b value associated with the perspective projection transformation matrix.
      /// </summary>
      public double B
      {
         get
         {
            return -2.0*(Far*Near)/(Far-Near); ;
         }
      }

      /// <summary>
      /// Gets or sets the origin of the camera coordinate system.
      /// </summary>
      public Point3D E { get; set; }

      /// <summary>
      /// Gets or sets the point through which the gaze unit vector points to.
      /// </summary>
      public Point3D G { get; set; }

      /// <summary>
      /// Gets or sets the unit vector indicating the up direction.
      /// </summary>
      public Vector3D P { get; set; }

      /// <summary>
      /// Gets the gaze direction unit vector representing the n-axis of the camera coordinate system.
      /// </summary>
      public Vector3D N
      {
         get
         {
            Vector3D n = new Vector3D(E.X-G.X, E.Y-G.Y, E.Z-G.Z);
            n.Normalize();
            return n;
         }
      }

      /// <summary>
      /// Gets the unit vector representing the u-axis of the camera coordinate system.
      /// </summary>
      public Vector3D U
      {
         get
         {
            Vector3D u = Vector3D.CrossProduct(P, N);
            u.Normalize();
            return u;
         }
      }

      /// <summary>
      /// Gets the unit vector representing the v-axis of the camera coordinate system.
      /// </summary>
      public Vector3D V 
      { 
         get
         {
            Vector3D v = Vector3D.CrossProduct(N, U);
            v.Normalize();
            return v;
         }
      }

      /// <summary>
      /// Gets the camera transformation matrix.
      /// Transforms a point from the world coordinate system to the viewing (camera) coordinate system.
      /// </summary>
      public Matrix3D Mv 
      {
         get
         {
            // Save vector e (pointing to point e) to calculate the dot product in the matrix Mv.
            Vector3D e = new Vector3D(E.X, E.Y, E.Z);
            return new Matrix3D(U.X, U.Y, U.Z, -1.0*Vector3D.DotProduct(e, U), // -e.u
                                V.X, V.Y, V.Z, -1.0*Vector3D.DotProduct(e, V), // -e.v
                                N.X, N.Y, N.Z, -1.0*Vector3D.DotProduct(e, N), // -e.n
                                0,   0,   0,   1).Transpose();

         }
      }

      /// <summary>
      /// Gets or sets the near plane distance.
      /// </summary>
      public double Near { get; set; }

      /// <summary>
      /// Gets or sets the far plane distance.
      /// </summary>
      public double Far { get; set; }

      /// <summary>
      /// Gets the perspective projection transformation matrix for depth.
      /// </summary>
      public Matrix3D Mp 
      {
         get
         {
            return new Matrix3D(Near, 0,    0,  0,
                                0,    Near, 0,  0,
                                0,    0,    A,  B,
                                0,    0,    -1, 0).Transpose();
         }
      }

      /// <summary>
      /// Gets or sets the viewing angle of the camera.
      /// </summary>
      public double ViewingAngle { get; set; }

      /// <summary>
      /// Gets or sets the aspect ratio of the near plane.
      /// </summary>
      public double Aspect { get; set; }

      /// <summary>
      /// Gets the top boundary of the viewport.
      /// </summary>
      public double Top
      {
         get
         {
            return Near*Math.Tan(Math.PI/180*ViewingAngle/2.0);
         }
      }

      /// <summary>
      /// Gets the bottom boundary of the viewport.
      /// </summary>
      public double Bottom
      {
         get
         {
            return -Top;
         }
      }

      /// <summary>
      /// Gets the right boundary of the viewport.
      /// </summary>
      public double Right
      {
         get
         {
            return Aspect*Top;
         }
      }

      /// <summary>
      /// Gets the left boundary of the viewport.
      /// </summary>
      public double Left
      {
         get
         {
            return -Right;
         }
      }

      /// <summary>
      /// Gets the perspective projection transformation matrix for the viewport.
      /// </summary>
      public Matrix3D T1
      {
         get
         {
            return new Matrix3D(1, 0, 0, -(Right+Left)/2.0,
                                0, 1, 0, -(Top+Bottom)/2.0,
                                0, 0, 1, 0,
                                0, 0, 0, 1).Transpose();
         }
      }

      /// <summary>
      /// Gets the perspective projection transformation matrix for the viewport
      /// </summary>
      public Matrix3D S1
      {
         get
         {
            return new Matrix3D(2.0/(Right-Left), 0,                0, 0,
                                0,                2.0/(Top-Bottom), 0, 0,
                                0,                0,                1, 0,
                                0,                0,                0, 1).Transpose();
         }
      }

      /// <summary>
      /// Gets the width of the screen, in pixels.
      /// </summary>
      public int Width { get; private set; }

      /// <summary>
      /// Gets the height of the screen, in pixels.
      /// </summary>
      public int Height { get; private set; }

      /// <summary>
      /// Gets the transformation matrix to go from warmped viewing volume to screen coordinates.
      /// </summary>
      public Matrix3D WS2T2 
      { 
         get
         {
            return new Matrix3D(Width/2.0, 0,           0, Width/2,
                                0,         -Height/2.0, 0, -Height/2.0 + Height,
                                0,         0,           1, 0,
                                0,         0,           0, 1).Transpose();
         }
      }

      /// <summary>
      /// Gest the transformation matrix to go from viewing (camera) coordinates to canonical view volume.
      /// </summary>
      public Matrix3D S1T1Mp
      {
         get
         {
            return new Matrix3D(2.0*Near/(Right-Left), 0, (Right+Left)/(Right-Left), 0,
                                0, 2.0*Near/(Top-Bottom), (Top+Bottom)/(Top-Bottom), 0,
                                0, 0, -(Far+Near)/(Far-Near), -2.0*Far*Near/(Far-Near),
                                0, 0, -1, 0).Transpose();
         }
      }

      /// <summary>
      /// Initializes a new instance of a synthetic camera
      /// </summary>
      /// <param name="e">The location of the synthetic camera, in world coordinates</param>
      /// <param name="g">The location of the gaze point, in world coordinates</param>
      /// <param name="up">The up direction</param>
      /// <param name="near">The distance from the camera to the near plane</param>
      /// <param name="far">The distance from the camera to the far plane</param>
      /// <param name="viewingAngle">The viewing angle of the camera</param>
      /// <param name="aspect">The aspect ratio</param>
      /// <param name="width">The width of the screen on which to display the 3D image in 2D, in pixels</param>
      /// <param name="height">The height of the screen on which to display the 3D image in 2D, in pixels</param>
      public SyntheticCamera(Point3D e, 
                             Point3D g, 
                             Vector3D up, 
                             double near = 10, 
                             double far = 70, 
                             double viewingAngle = 45.0, 
                             double aspect = 1.0,
                             int width = 512,
                             int height = 512)
      {
         E = e;
         G = g;
         P = up;
         P.Normalize();
         Near = near;
         Far = far;
         ViewingAngle = viewingAngle;
         Aspect = aspect;
         Width = width;
         Height = height;
      }

      /// <summary>
      /// Transforms the input point to screen coordinates.
      /// </summary>
      /// <param name="worldPoint">The point to transform, in world coordinates.</param>
      /// <returns>The transformed point, in screen coordinates.</returns>
      public Point WorldToScreen(Point3D worldPoint)
      {
         Point3D p = WS2T2.Transform(S1T1Mp.Transform(Mv.Transform(worldPoint)));
         return new Point((int)(p.X), (int)(p.Y));
      }

      /// <summary>
      /// Checks if the given face represented by 3 points is within the view volume.
      /// </summary>
      /// <param name="p1">Point 1</param>
      /// <param name="p2">Point 2</param>
      /// <param name="p3">Point 3</param>
      /// <returns>True if the triangle is fully or somewhat in the view volume; otherwise, false.</returns>
      public bool IsTriangleInViewVolume(Point3D p1, Point3D p2, Point3D p3)
      {
         Point3D new_p0 = p1*Mv*S1T1Mp;
         Point3D new_p1 = p2*Mv*S1T1Mp;
         Point3D new_p2 = p3*Mv*S1T1Mp;
         return (IsBetwenNegative1and1(new_p0.X) && IsBetwenNegative1and1(new_p0.Y) && IsBetwenNegative1and1(new_p0.Y) ||
                 IsBetwenNegative1and1(new_p1.X) && IsBetwenNegative1and1(new_p1.Y) && IsBetwenNegative1and1(new_p1.Y) ||
                 IsBetwenNegative1and1(new_p2.X) && IsBetwenNegative1and1(new_p2.Y) && IsBetwenNegative1and1(new_p2.Y));
      }

      /// <summary>
      /// Determines if a number is between -1 and 1.
      /// </summary>
      /// <param name="number">The number to check.</param>
      /// <returns>True if the number is between -1 and 1, inclusive. Otherwise, false.</returns>
      private bool IsBetwenNegative1and1(double number)
      {
         return number >= -1 && number <= 1;
      }

   }
}

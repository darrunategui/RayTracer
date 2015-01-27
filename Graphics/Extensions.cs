using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;

namespace CS3388_Graphics
{
   /// <summary>
   /// Handy extensions class.
   /// </summary>
   public static class Extensions
   {
      /// <summary>
      /// Rotates a set of points around an axis.
      /// </summary>
      /// <param name="points">the points to rotate</param>
      /// <param name="axis">the axis to rotate around</param>
      /// <param name="angle">the rotation angle, in degrees</param>
      /// <returns>The rotated points.</returns>
      public static IEnumerable<Point3D> RotateAboutAxis(this IEnumerable<Point3D> points, Axis axis, double angle)
      {
         // Need to get the angle in radians
         double radians = DegreeToRadian(angle);

         Matrix3D rotationMatrix = Matrix3D.Identity;
         if (axis == Axis.X)
         {
            rotationMatrix = XAxisRotationMatrix(radians);
         }
         else if (axis == Axis.Y)
         {
            rotationMatrix = YAxisRotationMatrix(radians);
         }
         else if (axis == Axis.Z)
         {
            rotationMatrix = ZAxisRotationMatrix(radians);
         }

         // Get a new copy of the array of points to rotate
         Point3D[] returnValue = points.ToArray();
         rotationMatrix.Transform(returnValue);

         // Return the rotated points
         return returnValue;
      }

      /// <summary>
      /// Converts degrees to radians.
      /// </summary>
      /// <param name="angle">The angle to convert, in degrees.</param>
      /// <returns>The radian value.</returns>
      private static double DegreeToRadian(double angle)
      {
         return Math.PI*angle/180.0;
      }

      /// <summary>
      /// Gets the rotation matrix to rotate about the X axis.
      /// </summary>
      /// <param name="radians">the rotation angle, in radians</param>
      /// <returns>The rotation matrix</returns>
      public static Matrix3D XAxisRotationMatrix(double radians)
      {
         return new Matrix3D(1, 0,                 0,                  0,
                             0, Math.Cos(radians), -Math.Sin(radians), 0,
                             0, Math.Sin(radians), Math.Cos(radians),  0,
                             0, 0,                 0,                  1).Transpose();

      }
      
      /// <summary>
      /// Gets the rotation matrix to rotate about the Y axis.
      /// </summary>
      /// <param name="radians">the rotation angle, in radians</param>
      /// <returns>The rotation matrix</returns>
      public static Matrix3D YAxisRotationMatrix(double radians)
      {
         return new Matrix3D(Math.Cos(radians),  0, Math.Sin(radians), 0,
                             0,                  1, 0,                 0,
                             -Math.Sin(radians), 0, Math.Cos(radians), 0,
                             0,                  0, 0,                 1).Transpose();

      }
      
      /// <summary>
      /// Gets the rotation matrix to rotate about the Z axis.
      /// </summary>
      /// <param name="radians">the rotation angle, in radians</param>
      /// <returns>The rotation matrix</returns>
      private static Matrix3D ZAxisRotationMatrix(double radians)
      {
         return new Matrix3D(Math.Cos(radians), -Math.Sin(radians), 0, 0,
                             Math.Sin(radians), Math.Cos(radians),  0, 0,
                             0,                 0,                  1, 0,
                             0,                 0,                  0, 1).Transpose();

      }
   
      /// <summary>
      /// Transposes the matrix structure.
      /// </summary>
      /// <remarks>
      /// The Matrix3D struct seems to have the rows and colums flipped.
      /// This extension should allow easier reading when creating a new Matrix3D struct.
      /// </remarks>
      /// <param name="m">The matrix to transpose</param>
      /// <returns>The transpose of the matrix.</returns>
      public static Matrix3D Transpose(this Matrix3D m)
      {
         return new Matrix3D(m.M11, m.M21, m.M31, m.OffsetX,
                             m.M12, m.M22, m.M32, m.OffsetY,
                             m.M13, m.M23, m.M33, m.OffsetZ,
                             m.M14, m.M24, m.M34, m.M44);
      }
   
      /// <summary>
      /// Represents the point as a string with maximum 4 decimal places.
      /// </summary>
      /// <param name="p">The point.</param>
      /// <returns>The pretty string.</returns>
      public static string ToPrettyString(this Point3D p)
      {
         return string.Format("{0}, {1}, {2}", Math.Round(p.X, 4), Math.Round(p.Y, 4), Math.Round(p.Z, 4));
      }

      /// <summary>
      /// Represents the vector as a string with maximum 4 decimal places.
      /// </summary>
      /// <param name="v">The vector.</param>
      /// <returns>The pretty string.</returns>
      public static string ToPrettyString(this Vector3D v)
      {
         return string.Format("{0}, {1}, {2}", Math.Round(v.X, 4), Math.Round(v.Y, 4), Math.Round(v.Z, 4));
      }

      /// <summary>
      /// Gets the equivalent System.Windows.Media.Color.
      /// </summary>
      /// <param name="color">The System.Drawing.Color.</param>
      /// <returns>The equivalent System.Windows.Media.Color</returns>
      public static MColor ToMColor(this DColor color)
      {
         return MColor.FromArgb(color.A, color.R, color.G, color.B);
      }

      /// <summary>
      /// Gets the equivalent System.Drawing.Color.
      /// </summary>
      /// <param name="color">The System.Windows.Media.Color.</param>
      /// <returns>The equivalent System.Drawing.Color</returns>
      public static DColor ToDColor(this MColor color)
      {
         return DColor.FromArgb(color.A, color.R, color.G, color.B);
      }
   }
}

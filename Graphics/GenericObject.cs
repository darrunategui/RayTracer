using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CS3388_Graphics
{
   /// <summary>
   /// A generic object with material color properties.
   /// </summary>
   public class GenericObject
   {
      #region Fields
      #region Color
      /// <summary>
      /// Gets or sets the color attributes of the object.
      /// </summary>
      public Color Color { get; set; }
      /// <summary>
      /// Gets or sets the specular color of the object.
      /// </summary>
      public Color SpecularColor { get; set; }
      /// <summary>
      /// Gets or sets the diffuse color of the object.
      /// Should be similar if not the same as ambient color.
      /// </summary>
      public Color DiffuseColor { get; set; }
      /// <summary>
      /// Gets or sets the ambient color of the object.
      /// Should be similar if not the same as diffuse color.
      /// </summary>
      public Color AmbientColor { get; set; }
      #endregion

      // The coefficients should all sum to 1.
      #region Coefficients
      /// <summary>
      /// Gets or sets the specular coefficient.
      /// </summary>
      public double SpecularCoefficient { get; set; }
      /// <summary>
      /// Gets or sets the diffuse coefficient.
      /// </summary>
      public double DiffuseCoefficient { get; set; }
      /// <summary>
      /// Gets or sets the ambient coefficient.
      /// </summary>
      public double AmbientCoefficient { get; set; }
      #endregion

      /// <summary>
      /// Gets or sets the fallout exponent governing how much
      /// specular light reaches the eye as v moves away from r.
      /// </summary>
      public double F { get; set; }

      /// <summary>
      /// Gets or sets the transformation matrix that defines the shape in the scene.
      /// </summary>
      public Matrix3D M { get; set; }
      #endregion

      public GenericObject(Matrix3D m, 
                           Color c, Color sCol, Color dCol, Color aCol, 
                           double sCoef, double dCoef, double aCoef, double f)
      {
         Color = c;
         M = m;
         SpecularColor = sCol;
         DiffuseColor = dCol;
         AmbientColor = aCol;
         SpecularCoefficient = sCoef;
         DiffuseCoefficient = dCoef;
         AmbientCoefficient = aCoef;
         F = f;
      }

   }
}

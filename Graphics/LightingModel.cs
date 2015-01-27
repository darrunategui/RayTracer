using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CS3388_Graphics
{
   /// <summary>
   /// Helper class to get intensity from light.
   /// </summary>
   public class LightingModel
   {
      /// <summary>
      /// Gets the diffuse intensity.
      /// </summary>
      /// <param name="obj">The object</param>
      /// <param name="s">vector from intersection to light source</param>
      /// <param name="n">normal to the intersection point</param>
      /// <param name="light">The light source</param>
      /// <returns>The diffuse intensity for each color channel</returns>
      public static Color GetDiffuseIntensity(GenericObject obj, Vector3D s, Vector3D n, Light light)
      {
         float r, g, b;
         double multiplier = Math.Max(0, Cosine(s, n));

         float redLight = light.Color.ScR;
         r = (float)(redLight*obj.DiffuseCoefficient*multiplier);

         double greenLight = light.Color.ScG;
         g = (float)(greenLight*obj.DiffuseCoefficient*multiplier);

         double blueLight = light.Color.ScB;
         b = (float)(blueLight*obj.DiffuseCoefficient*multiplier);

         return Color.FromScRgb(1, r, g, b);
      }

      /// <summary>
      /// Gets the specular intensity.
      /// </summary>
      /// <param name="obj">The object</param>
      /// <param name="s">Vector from intersection to light source</param>
      /// <param name="n">The normal of at the point of intersection</param>
      /// <param name="v">vector from intersection to camera</param>
      /// <param name="light">The light source</param>
      /// <returns>The specular intensity for each color channel</returns>
      public static Color GetSpecularIntensity(GenericObject obj, Vector3D s, Vector3D n, Vector3D v, Light light)
      {
         float red, green, blue;

         Vector3D r = -s + 2*(Vector3D.DotProduct(s, n)/n.LengthSquared)*n;
         double multiplier = Math.Pow(Math.Max(0, Cosine(r, v)), obj.F);

         float redLight = light.Color.ScR;
         red = (float)(redLight*obj.SpecularCoefficient*multiplier);

         double greenLight = light.Color.ScG;
         green = (float)(greenLight*obj.SpecularCoefficient*multiplier);

         double blueLight = light.Color.ScB;
         blue = (float)(blueLight*obj.SpecularCoefficient*multiplier);

         return Color.FromScRgb(1, red, green, blue);
      }

      /// <summary>
      /// Gets the ambient Intensity
      /// </summary>
      /// <param name="obj">The object</param>
      /// <param name="light">The light source</param>
      /// <returns>The ambient intensity for each color channel</returns>
      public static Color GetAmbientIntensity(GenericObject obj)
      {
         Color ambientLight = Color.FromScRgb(1, 
            0.08f, 
            0.08f,
            0.08f);
         float multiplier = (float)obj.AmbientCoefficient;

         return ambientLight*multiplier;
      }

      /// <summary>
      /// Gets the cosine of theta, where theta is the angle between the two given vectors.
      /// </summary>
      /// <param name="v1">First vector</param>
      /// <param name="v2">Second vector</param>
      /// <returns>The cosine of the two vectors.</returns>
      private static double Cosine(Vector3D v1, Vector3D v2)
      {
         double cos = Vector3D.DotProduct(v1, v2)/(v1.Length*v2.Length);
         return cos;
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CS3388_Graphics
{
   public class RayTracer
   {
      /// <summary>
      /// The super resolution multiplier.
      /// </summary>
      public int ResolutionMultiplier { get; set; }

      /// <summary>
      /// Height of the near plane in viewing coordinates.
      /// </summary>
      public double H
      {
         get { return Camera.Near*Math.Tan(Camera.ViewingAngle*Math.PI/180/2); }
      }

      /// <summary>
      /// Width of the near plane in viewing coordinates.
      /// </summary>
      public double W
      {
         get { return H*Camera.Aspect; }
      }

      /// <summary>
      /// Gets the number of columns in the rendering window.
      /// </summary>
      public int n
      {
         get { return Camera.Width*ResolutionMultiplier; }
      }

      /// <summary>
      /// Gets the number of rows in the rendering window.
      /// </summary>
      public int m
      {
         get { return Camera.Height*ResolutionMultiplier; }
      }

      /// <summary>
      /// Gets or sets the synthetic camera.
      /// </summary>
      public SyntheticCamera Camera { get; set; }

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="sc">Synthetic camera</param>
      /// <param name="superResMultiplier">The super resolution multipier</param>
      public RayTracer(SyntheticCamera sc, int superResMultiplier)
      {
         Camera = sc;
         ResolutionMultiplier = superResMultiplier;
      }

      /// <summary>
      /// Computes the ray that traverses from the camera through a pixel at (column, row).
      /// The returned vector is d, in r(t) = e + dt.
      /// </summary>
      /// <param name="column">Column of pixel to intersect on the rendering window.</param>
      /// <param name="row">Row of pixel to intersect on the rendering window.</param>
      /// <returns>The vector d, in r(t) = e + dt.</returns>
      private Vector3D GetRayThroughPixel(int column, int row)
      {
         // NOTE: had to change the professors equation here by multiplying Uc and Vr by -1 since
         // I'm working my way from the top left down to the right. Should still work fine.
         Vector3D v = new Vector3D(-Camera.Near*Camera.N.X + W*(1.0 - 2.0*column/n)*Camera.U.X + H*(1.0 - 2.0*row/m)*Camera.V.X,
                                   -1.0*(-Camera.Near*Camera.N.Y + W*(1.0 - 2.0*column/n)*Camera.U.Y + H*(1.0 - 2.0*row/m)*Camera.V.Y),
                                   -Camera.Near*Camera.N.Z + W*(1.0 - 2.0*column/n)*Camera.U.Z + H*(1.0 - 2.0*row/m)*Camera.V.Z);
         return v;
      }

      public bool Intesects(GenericObject o, List<GenericObject> objects, Vector3D d, Point3D e)
      {
         foreach (GenericObject obj in objects.Where(x => x != o))
         {
            // We need the inverse of the transformation matrix
            Matrix3D m = obj.M;
            m.Invert();

            // The starting point of the ray transformed by M^-1
            // Treat it as a vector to do the following math
            Vector3D E = (Vector3D)(e*m);
            // The direction ray tranformed by m^-1
            Vector3D D = d*m;

            // The constant t to multiply the direction vector d
            double t = 0;

            if (obj is Sphere)
            {
               // Get the parameters for the quadratic equation
               double a = D.LengthSquared;
               double b = Vector3D.DotProduct(E, D);
               double c = E.LengthSquared - 1;

               // The discriminent will indicate how many solutions there are
               double discriminent = b*b - a*c;
               if (discriminent < 0) // discrim < 0 --> No intersection
               {
                  continue;
               }
               else if (discriminent == 0) // discrim = 0 --> One intersection
               {
                  t = -b/a;
               }
               else if (discriminent > 0) // discrim > 0 --> Two intersections
               {
                  double t1 = -(b + Math.Sqrt(discriminent))/a;
                  double t2 = -(b - Math.Sqrt(discriminent))/a;
                  t = Math.Min(t1, t2);
               }

               if (t <=0)
               {
                  continue; // We're only concerned with positive t values
               }

               return true;
            }
            else if (obj is Plane)
            {
               if (E.Z == 0)
               {
                  continue; // The plane is parallel to the viewing direction.
               }

               t = -E.Z/D.Z;

               if (t<=0)
               {
                  continue; // We're only concerned with positiv t values
               }

               return true;
            }
            else if (obj is Cylinder)
            {
               double a = Math.Pow(D.X, 2) + Math.Pow(D.Y, 2);
               double b = E.X*D.X + E.Y*D.Y;
               double c = Math.Pow(E.X, 2) + Math.Pow(E.Y, 2) - 1;

               // The discriminent will indicate how many solutions there are
               double discriminent = b*b - a*c;
               if (discriminent < 0) // discrim < 0 --> No intersection
               {
                  continue;
               }
               else if (discriminent == 0) // discrim = 0 --> One intersection
               {
                  t = -b/a;
               }
               else if (discriminent > 0) // discrim > 0 --> Two intersections
               {
                  double t1 = -(b + Math.Sqrt(discriminent))/a;
                  double t2 = -(b - Math.Sqrt(discriminent))/a;
                  if (t1 > 0 && t2 > 0)
                     t = Math.Min(t1, t2);
                  else if (t1 < 0 && t2 > 0)
                     t = t2;
                  else
                     continue;
               }

               if (t <= 0)
               {
                  continue;
               }

               Point3D int1 = (Point3D)(E + D*t);
               if (int1.Z < 1 && int1.Z > -1)
               {
                  return true;
               }

               Matrix3D topCap = Matrix3D.Identity; topCap.OffsetZ = 1;
               topCap.Invert();
               Matrix3D bottomCap = Matrix3D.Identity; bottomCap.OffsetZ = -1;
               bottomCap.Invert();

               Vector3D E_top = (Vector3D)(((Point3D)E)*topCap);
               Vector3D D_top = D*topCap;
               if (D_top.Z != 0)
               {
                  double t_top = -E_top.Z/D_top.Z;
                  if (t_top > 0)
                  {
                     Point3D int_top = (Point3D)(E_top + D_top*t_top);
                     if (Math.Pow(int_top.X, 2) + Math.Pow(int_top.Y, 2) <= 1)
                     {
                        return true;
                     }
                  }
               }

               Vector3D E_bottom = (Vector3D)(((Point3D)E)*bottomCap);
               Vector3D D_bottom = D*bottomCap;
               if (D_bottom.Z != 0)
               {
                  double t_bottom = -E_bottom.Z/D_bottom.Z;
                  if (t_bottom > 0)
                  {
                     Point3D int_bottom = (Point3D)(E_bottom + D_bottom*t_bottom);
                     if (Math.Pow(int_bottom.X, 2) + Math.Pow(int_bottom.Y, 2) <= 1)
                     {
                        return true;
                     }
                  }
               }
            }
            else if (obj is Cone)
            {
               double a = Math.Pow(D.X, 2) + Math.Pow(D.Y, 2) - 0.25*Math.Pow(D.Z, 2);
               double b = E.X*D.X + E.Y*D.Y + D.Z*(1-E.Z)/4.0;
               double c = Math.Pow(E.X, 2) + Math.Pow(E.Y, 2) - Math.Pow(1.0-E.Z, 2)/4.0;
               // The discriminent will indicate how many solutions there are
               double discriminent = b*b - a*c;
               if (discriminent < 0) // discrim < 0 --> No intersection
               {
                  continue;
               }
               else if (discriminent == 0) // discrim = 0 --> One intersection
               {
                  t = -b/a;
               }
               else if (discriminent > 0) // discrim > 0 --> Two intersections
               {
                  double t1 = -(b + Math.Sqrt(discriminent))/a;
                  double t2 = -(b - Math.Sqrt(discriminent))/a;
                  if (t1 > 0 && t2 > 0)
                     t = Math.Min(t1, t2);
                  else if (t1 < 0 && t2 > 0)
                     t = t2;
                  else
                     continue;
               }
               if (t <= 0)
               {
                  continue; // The intersection is behind the view point
               }

               Point3D int1 = (Point3D)(E + D*t);
               if (int1.Z < 1 && int1.Z > -1)
               {
                  return true;
               }

               Matrix3D bottomCap = Matrix3D.Identity; bottomCap.OffsetZ = -1;
               bottomCap.Invert();
               Vector3D E_bottom = (Vector3D)(((Point3D)E)*bottomCap);
               Vector3D D_bottom = D*bottomCap;
               if (D_bottom.Z != 0)
               {
                  double t_bottom = -E_bottom.Z/D_bottom.Z;
                  if (t_bottom > 0)
                  {
                     Point3D int_bottom = (Point3D)(E_bottom + D_bottom*t_bottom);
                     if (Math.Pow(int_bottom.X, 2) + Math.Pow(int_bottom.Y, 2) <= 1)
                     {
                        return true;
                     }
                  }
               }
            }
         }
         return false;
      }

      public Color RayTrace(List<GenericObject> objects, int column, int row, PointLight pointLight, DirectionalLight directionalLight)
      {
         Vector3D d = GetRayThroughPixel(column, row);
         return RayTrace(objects, d, Camera.E, pointLight, directionalLight);
      }
     
      public Color RayTrace(List<GenericObject> objects, Vector3D d, Point3D e, PointLight pointLight, DirectionalLight directionalLight)
      {
         Dictionary<GenericObject, double> hitTimes = new Dictionary<GenericObject, double>();
         foreach (GenericObject obj in objects)
         {
            // We need the inverse of the transformation matrix
            Matrix3D m = obj.M;
            m.Invert();

            // The starting point of the ray transformed by M^-1
            // Treat it as a vector to do the following math
            Vector3D E = (Vector3D)(e*m);
            // The direction ray tranformed by m^-1
            Vector3D D = d*m;
            // Consider this 't' in r(t) = e + d*t
            double directionMultiplier = 0;
            if (obj is Sphere)
            {
               #region Sphere
               // Get the parameters for the quadratic equation
               double a = D.LengthSquared;
               double b = Vector3D.DotProduct(E, D);
               double c = E.LengthSquared - 1;

               // The discriminent will indicate how many solutions there are
               double discriminent = b*b - a*c;
               if (discriminent < 0) // discrim < 0 --> No intersection
               {
                  continue;
               }
               else if (discriminent == 0) // discrim = 0 --> One intersection
               {
                  directionMultiplier = -b/a;
               }
               else if (discriminent > 0) // discrim > 0 --> Two intersections
               {
                  double t1 = -(b + Math.Sqrt(discriminent))/a;
                  double t2 = -(b - Math.Sqrt(discriminent))/a;
                  directionMultiplier = Math.Min(t1, t2);
               }

               // We're only concerned with positive t values
               if (directionMultiplier <=0)
               {
                  continue;
               }
               hitTimes.Add(obj, directionMultiplier);
               #endregion
            }
            else if (obj is Plane)
            {
               #region Plane
               if (D.Z == 0)
               {
                  continue; // The plane is parallel to the viewing direction.
               }

               directionMultiplier = -E.Z/D.Z;

               if (directionMultiplier <= 0)
               {
                  continue; // We're only concerned with positive t values
               }

               Point3D canonicalIntersection = Camera.S1T1Mp.Transform(Camera.Mv.Transform(e + d*directionMultiplier));
               if (canonicalIntersection.Z > 1)
               {
                  continue; // I'm only concerned with those z values that are in the viewing volume
               }

               hitTimes.Add(obj, directionMultiplier);
               #endregion
            }
            else if (obj is Cylinder)
            {
               #region Cylinder
               #region Rounded side
               double a = Math.Pow(D.X, 2) + Math.Pow(D.Y, 2);
               double b = E.X*D.X + E.Y*D.Y;
               double c = Math.Pow(E.X, 2) + Math.Pow(E.Y, 2) - 1;

               // The discriminent will indicate how many solutions there are
               double discriminent = b*b - a*c;
               if (discriminent < 0) // discrim < 0 --> No intersection
               {
                  continue;
               }
               else if (discriminent == 0) // discrim = 0 --> One intersection
               {
                  directionMultiplier = -b/a;
               }
               else if (discriminent > 0) // discrim > 0 --> Two intersections
               {
                  double t1 = -(b + Math.Sqrt(discriminent))/a;
                  double t2 = -(b - Math.Sqrt(discriminent))/a;
                  directionMultiplier = Math.Min(t1, t2);
               }

               if (directionMultiplier <= 0)
               {
                  continue;
               }

               Point3D int1 = (Point3D)(E + D*directionMultiplier);
               if (int1.Z < 1 && int1.Z > -1)
               {
                  hitTimes.Add(obj, directionMultiplier);
                  continue;
               }
               #endregion

               #region Caps
               Matrix3D topCap = Matrix3D.Identity; topCap.OffsetZ = 1;
               topCap.Invert();
               Matrix3D bottomCap = Matrix3D.Identity; bottomCap.OffsetZ = -1;
               bottomCap.Invert();

               Vector3D E_top = (Vector3D)(((Point3D)E)*topCap);
               Vector3D D_top = D*topCap;
               bool intersectsTop = false;
               double directionMultiplier_top = double.MaxValue;
               if (D_top.Z != 0)
               {
                  double t_top = -E_top.Z/D_top.Z;
                  if (t_top > 0 && Camera.S1T1Mp.Transform(Camera.Mv.Transform(e + d*t_top)).Z < 1)
                  {
                     Point3D int_top = (Point3D)(E_top + D_top*t_top);
                     if (Math.Pow(int_top.X, 2) + Math.Pow(int_top.Y, 2) <= 1)
                     {
                        // intersects top cap.
                        directionMultiplier_top = t_top;
                        intersectsTop = true;
                     }
                  }
               }

               Vector3D E_bottom = (Vector3D)(((Point3D)E)*bottomCap);
               Vector3D D_bottom = D*bottomCap;
               bool intersectsBottom = false;
               double directionMultiplier_bottom = double.MaxValue;
               if (D_bottom.Z != 0)
               {
                  double t_bottom = -E_bottom.Z/D_bottom.Z;
                  if (t_bottom > 0 && Camera.S1T1Mp.Transform(Camera.Mv.Transform(e + d*t_bottom)).Z < 1)
                  {
                     Point3D int_bottom = (Point3D)(E_bottom + D_bottom*t_bottom);
                     if (Math.Pow(int_bottom.X, 2) + Math.Pow(int_bottom.Y, 2) <= 1)
                     {
                        directionMultiplier_bottom = t_bottom;
                        intersectsBottom = true;
                     }
                  }
               }

               if (intersectsTop || intersectsBottom)
               {
                  directionMultiplier = Math.Min(directionMultiplier_bottom, directionMultiplier_top);
                  hitTimes.Add(obj, directionMultiplier);
               }
               #endregion
               #endregion
            }
            else if (obj is Cone)
            {
               #region Cone
               double a = Math.Pow(D.X, 2) + Math.Pow(D.Y, 2) - 0.25*Math.Pow(D.Z, 2);
               double b = E.X*D.X + E.Y*D.Y + D.Z*(1-E.Z)/4.0;
               double c = Math.Pow(E.X, 2) + Math.Pow(E.Y, 2) - Math.Pow(1.0-E.Z, 2)/4.0;
               // The discriminent will indicate how many solutions there are
               double discriminent = b*b - a*c;
               if (discriminent < 0) // discrim < 0 --> No intersection
               {
                  continue;
               }
               else if (discriminent == 0) // discrim = 0 --> One intersection
               {
                  directionMultiplier = -b/a;
               }
               else if (discriminent > 0) // discrim > 0 --> Two intersections
               {
                  double t1 = -(b + Math.Sqrt(discriminent))/a;
                  double t2 = -(b - Math.Sqrt(discriminent))/a;
                  directionMultiplier = Math.Min(t1, t2);
               }
               if (directionMultiplier <= 0)
               {
                  continue; // The intersection is behind the view point
               }

               Point3D int1 = (Point3D)(E + D*directionMultiplier);
               if (int1.Z < 1 && int1.Z > -1)
               {
                  hitTimes.Add(obj, directionMultiplier);
                  continue;
               }

               Matrix3D bottomCap = Matrix3D.Identity; bottomCap.OffsetZ = -1;
               bottomCap.Invert(); Vector3D E_bottom = (Vector3D)(((Point3D)E)*bottomCap);

               Vector3D D_bottom = D*bottomCap;
               if (D_bottom.Z != 0)
               {
                  double t_bottom = -E_bottom.Z/D_bottom.Z;
                  if (t_bottom > 0 && Camera.S1T1Mp.Transform(Camera.Mv.Transform(e + d*t_bottom)).Z < 1)
                  {
                     Point3D int_bottom = (Point3D)(E_bottom + D_bottom*t_bottom);
                     if (Math.Pow(int_bottom.X, 2) + Math.Pow(int_bottom.Y, 2) <= 1)
                     {
                        hitTimes.Add(obj, t_bottom);
                        continue;
                     }
                  }
               }
               #endregion
            }
         }

         if (hitTimes.Count == 0)
         {
            return new Color();
         }
         
         KeyValuePair<GenericObject, double> hit = hitTimes.OrderBy(x => x.Value).First();
         GenericObject ob = hit.Key;
         double t = hit.Value;

         // Move everything into generic form
         Matrix3D M = ob.M;
         M.Invert();

         Point3D cameraPos = e*M; // Camera in generic world coords
         Point3D lightPos = pointLight.Position*M; // Light position in generic world coords
         Point3D intersection = cameraPos + (d*M)*t; // Insersection point in generic world coords
         Vector3D n = GetNormal(ob, intersection); // The normal vector in generic world coords

         Vector3D s = (Vector3D)(lightPos - intersection); // Direction to the point light source from the intersection, in generic space
         Vector3D v = (Vector3D)(cameraPos - intersection); // Direction to the camera from the intersection, in generic space

         // Prepare the color values
         Color Id = Colors.Black; // Diffuse intensity
         Color Is = Colors.Black; // Specular intensity
         Color Ia = Colors.Black; // Ambient intensity
         Color diffuse = Colors.Black; // diffuse component
         Color specular = Colors.Black; // specular component
         Color ambient = Colors.Black; // ambient component

         bool inShadowOfPointLight = Intesects(ob, objects, (Vector3D)(pointLight.Position - (e + d*t)), e+d*t);

         if (!inShadowOfPointLight)
         {
            Id = LightingModel.GetDiffuseIntensity(ob, s, n, pointLight);
            diffuse += Color.FromScRgb(1, Id.ScR*ob.DiffuseColor.ScR, Id.ScG*ob.DiffuseColor.ScG, Id.ScB*ob.DiffuseColor.ScB);

            Is = LightingModel.GetSpecularIntensity(ob, s, n, v, pointLight);
            specular += Color.FromScRgb(1, Is.ScR*ob.SpecularColor.ScR, Is.ScG*ob.SpecularColor.ScG, Is.ScB*ob.SpecularColor.ScB);
         }

         bool inShadowOfDirectionalLight = Intesects(ob, objects, directionalLight.Direction*-1.0, e+d*t);

         if (!inShadowOfDirectionalLight)
         {
            Id = LightingModel.GetDiffuseIntensity(ob, directionalLight.Direction*-1.0, n, directionalLight);
            diffuse += Color.FromScRgb(1, Id.ScR*ob.DiffuseColor.ScR, Id.ScG*ob.DiffuseColor.ScG, Id.ScB*ob.DiffuseColor.ScB);
            
            Is = LightingModel.GetSpecularIntensity(ob, directionalLight.Direction*-1.0, n, v, pointLight);
            specular += Color.FromScRgb(1, Is.ScR*ob.SpecularColor.ScR, Is.ScG*ob.SpecularColor.ScG, Is.ScB*ob.SpecularColor.ScB);
         }

         // The scene will always have ambient light
         Ia = LightingModel.GetAmbientIntensity(ob);
         ambient = Color.FromScRgb(1, Ia.ScR*ob.AmbientColor.ScR, Ia.ScG*ob.AmbientColor.ScG, Ia.ScB*ob.AmbientColor.ScB);

         return diffuse + specular + ambient + ob.Color;
      }
      
      private Vector3D GetNormal(GenericObject obj, Point3D intersection)
      {
         Vector3D n = new Vector3D();
         
         // The normal vector depends on the objects shape. The following computations get the correct normal.
         if (obj is Sphere)
         {
            n = (Vector3D)intersection;
         }
         else if (obj is Plane)
         {
            n = new Vector3D(0, 0, 1);
         }
         else if (obj is Cylinder)
         {
            if (Math.Round(intersection.Z, 5) == 1.0)
            {
               n = new Vector3D(0, 0, 1);
            }
            else if (Math.Round(intersection.Z, 5) == -1.0)
            {
               n = new Vector3D(0, 0, -1);
            }
            else
            {
               n = new Vector3D(intersection.X, intersection.Y, 0);
            }
         }
         else if (obj is Cone)
         {
            if (Math.Round(intersection.Z, 5) == -1.0)
            {
               n = new Vector3D(0, 0, -1);
            }
            else
            {
               Vector3D tmp = new Vector3D(2.0*intersection.X, 2.0*intersection.Y, (1.0-intersection.Z)/2.0);
               double coeff = Math.Pow(2.0*intersection.X*intersection.X + 2.0*intersection.Y*intersection.Y + Math.Pow((1.0-intersection.Z)/2.0, 2), -0.5);
               n = tmp*coeff;
            }
         }
         return n;
      }
   }

}

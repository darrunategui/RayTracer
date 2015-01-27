using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CS3388_Graphics
{
   public class Sphere : GenericObject
   {
      public Sphere(Matrix3D m, 
                    Color c, Color sCol, Color dCol, Color aCol,
                    double sCoef, double dCoef, double aCoef, double f)
         : base(m, c, sCol, dCol, aCol, sCoef, dCoef, aCoef, f)
      {
      }
   }
}

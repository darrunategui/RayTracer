using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace CS3388_Graphics
{
   /// <summary>
   /// Represents a polygon wiremesh
   /// </summary>
   public class WireMesh
   {
      /// <summary>
      /// Gets or sets all vertices in the wiremesh.
      /// </summary>
      public Dictionary<int, Point3D> Vertices { get; set; }

      /// <summary>
      /// Gets or sets all the faces in the wiremesh.
      /// </summary>
      public Dictionary<int, Face> Faces { get; set; }

      /// <summary>
      /// Gets or sets all the normals in the wire mesh.
      /// </summary>
      public Dictionary<int, Vector3D> Normals { get; set; }

      /// <summary>
      /// Instantiates a new instance of a wire mesh object.
      /// </summary>
      /// <param name="vertices">List of vertices</param>
      /// <param name="faces">List of faces</param>
      /// <param name="normals">List of normals</param>
      public WireMesh(Dictionary<int, Point3D> vertices, Dictionary<int, Face> faces, Dictionary<int, Vector3D> normals)
      {
         Vertices = vertices;
         Faces = faces;
         Normals = normals;
      }
   }
}

using System.Diagnostics;

namespace CS3388_Graphics
{
   /// <summary>
   /// A planar surface represented by three vertices, having 3 normals at each vertice.
   /// </summary>
   [DebuggerDisplay("{V1}, {V2}, {V3}, {N1}, {N2}, {N3}")]
   public class Face
   {
      /// <summary>
      /// The ID of the first Vertice
      /// </summary>
      public int V1 { get; set; }

      /// <summary>
      /// The ID of the second Vertice
      /// </summary>
      public int V2 { get; set; }

      /// <summary>
      /// The ID of the third Vertice
      /// </summary>
      public int V3 { get; set; }

      /// <summary>
      /// The ID of the normal associated with the first vertice
      /// </summary>
      public int N1 { get; set; }

      /// <summary>
      /// The ID of the normal associated with the second vertice
      /// </summary>
      public int N2 { get; set; }

      /// <summary>
      /// The ID of the normal associated with the third vertice
      /// </summary>
      public int N3 { get; set; }

      /// <summary>
      /// Parameterless constructor to serialize.
      /// </summary>
      public Face()
      {}

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="v1">the ID first vertice</param>
      /// <param name="v2">the ID second vertice</param>
      /// <param name="v3">the iD third vertice</param>
      /// <param name="n1">the ID of the normal associated with the first vertice</param>
      /// <param name="n2">the ID of the normal associated with the second vertice</param>
      /// <param name="n3">the ID of the normal associated with the third vertice</param>
      public Face(int v1, int v2, int v3, int n1, int n2, int n3)
      {
         V1 = v1;
         V2 = v2;
         V3 = v3;
         N1 = n1;
         N2 = n2;
         N3 = n3;
      }
   }
}

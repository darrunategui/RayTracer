using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;

namespace CS3388_Graphics
{
   public class PolygonMeshSerializer
   {
      public void SerializeVertices(TextWriter writer, Dictionary<int, Point3D> vertices)
      {
         List<Entry<Point3D>> entries = new List<Entry<Point3D>>(vertices.Count);
         foreach (int key in vertices.Keys)
         {
            entries.Add(new Entry<Point3D>(key, vertices[key]));
         }
         XmlSerializer serializer = new XmlSerializer(typeof(List<Entry<Point3D>>));
         serializer.Serialize(writer, entries);
      }

      public void SerializeNormals(TextWriter writer, Dictionary<int, Vector3D> normals)
      {
         List<Entry<Vector3D>> entries = new List<Entry<Vector3D>>(normals.Count);
         foreach (int key in normals.Keys)
         {
            entries.Add(new Entry<Vector3D>(key, normals[key]));
         }
         XmlSerializer serializer = new XmlSerializer(typeof(List<Entry<Vector3D>>));
         serializer.Serialize(writer, entries);
      }

      public void SerializeFaces(TextWriter writer, Dictionary<int, Face> faces)
      {
         List<Entry<Face>> entries = new List<Entry<Face>>(faces.Count);
         foreach (int key in faces.Keys)
         {
            entries.Add(new Entry<Face>(key, faces[key]));
         }
         XmlSerializer serializer = new XmlSerializer(typeof(List<Entry<Face>>));
         serializer.Serialize(writer, entries);
      }

      public Dictionary<int, Point3D> DeserializeVertices(TextReader reader)
      {
         Dictionary<int, Point3D> vertices = new Dictionary<int, Point3D>();
         XmlSerializer serializer = new XmlSerializer(typeof(List<Entry<Point3D>>));
         List<Entry<Point3D>> list = (List<Entry<Point3D>>) serializer.Deserialize(reader);

         foreach (Entry<Point3D> entry in list)
         {
            vertices[entry.Key] = entry.Value;
         }
         return vertices;
      }

      public Dictionary<int, Vector3D> DeserializeNormals(TextReader reader)
      {
         Dictionary<int, Vector3D> normals = new Dictionary<int, Vector3D>();
         XmlSerializer serializer = new XmlSerializer(typeof(List<Entry<Vector3D>>));
         List<Entry<Vector3D>> list = (List<Entry<Vector3D>>)serializer.Deserialize(reader);

         foreach (Entry<Vector3D> entry in list)
         {
            normals[entry.Key] = entry.Value;
         }
         return normals;
      }

      public Dictionary<int, Face> DeserializeFaces(TextReader reader)
      {
         Dictionary<int, Face> faces = new Dictionary<int, Face>();
         XmlSerializer serializer = new XmlSerializer(typeof(List<Entry<Face>>));
         List<Entry<Face>> list = (List<Entry<Face>>)serializer.Deserialize(reader);

         foreach (Entry<Face> entry in list)
         {
            faces[entry.Key] = entry.Value;
         }
         return faces;
      }
   }
}

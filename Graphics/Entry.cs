using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;

namespace CS3388_Graphics
{
   /// <summary>
   /// Used to serialize dictionary objects
   /// </summary>
   /// <typeparam name="T">The type of data being stored.</typeparam>
   public class Entry<T>
   {
      public int Key;
      public T Value;

      public Entry()
      {
      }

      public Entry(int key, T value)
      {
         Key = key;
         Value = value;
      }
   }
}

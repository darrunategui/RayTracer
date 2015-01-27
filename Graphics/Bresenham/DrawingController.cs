using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3388_Graphics.Bresenham
{
   /// <summary>
   /// Draws shapes on an image.
   /// </summary>
   public class DrawingController
   {
      /// <summary>
      /// The graphics object.
      /// </summary>
      private Graphics _g;

      /// <summary>
      /// Gets or sets the drawing color.
      /// </summary>
      public Color Color { get; set; }

      /// <summary>
      /// Initializes a new instance of the drawing controller.
      /// </summary>
      /// <param name="g">The graphics object to draw with.</param>
      public DrawingController(Graphics g)
      {
         _g = g;
         Color = Color.Gray;
      }

      /// <summary>
      /// Draws a line between the two given points.
      /// </summary>
      /// <param name="p0">The initial point</param>
      /// <param name="p1">The end point</param>
      public void DrawLine(Point p0, Point p1)
      {
         BresenhamLine l = new BresenhamLine(_g);
         l.DrawLine(p0, p1);
      }

      /// <summary>
      /// Draws a point.
      /// </summary>
      /// <param name="p">The point to draw.</param>
      public void DrawPoint(Point p)
      {
         _g.FillRectangle(new SolidBrush(Color), p.X, p.Y, 1, 1);
      }
   }
}

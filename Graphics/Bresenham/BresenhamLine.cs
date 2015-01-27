using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CS3388_Graphics.Bresenham
{
   /// <summary>
   /// Draws lines using the bresenham algorithm.
   /// </summary>
   public class BresenhamLine
   {
      /// <summary>
      /// The graphics object.
      /// </summary>
      private Graphics _g;

      /// <summary>
      /// Initializes a new instance of the bresenham line class.
      /// </summary>
      /// <param name="g">The graphics object to draw lines.</param>
      public BresenhamLine(Graphics g)
      {
         _g = g;
      }

      /// <summary>
      /// Draws a line between the points.
      /// </summary>
      /// <param name="p0">The start point.</param>
      /// <param name="p1">The end point.</param>
      public void DrawLine(Point p0, Point p1)
      {
         if (p0 == p1) // If the input points are the same, the following math would result in an error.
         {
            // Draw the point where it should be and return;
            DrawPoints(new Point[] { p0 });
            return;
         }

         // First transfom the points since the input coordinate system has top right corner (0,0), increasing to the right and down.
         // This matrix flips the coordinate system to the usual XY system. Increasing to the right and up.
         Matrix m = new Matrix(1, 0,  // [1 0  0]
                               0, -1, // [0 -1 0]
                               0, 0); // [0 0  1]
         
         Point[] pts = new Point[] { p0, p1 };
         m.TransformPoints(pts);
         p0 = pts[0];
         p1 = pts[1];

         // First calculate the slope. The slope will determine which line algorithm will draw the line.
         float slope = (float)(p1.Y - p0.Y)/(p1.X - p0.X);
         
         // Depending on the slope, and the direction of the line (if p0 was the beginning of the line)
         // swap the end points and use symmetry to draw the lines.
         if ((slope > 0 && p0.X > p1.X) || // slope is positive and the first point is higher up and to the right.
             (slope < 0 && p0.X < p1.X) || // slope is negative and the first point is higher up and to the left.
             (slope == 0 && p0.X > p1.X)) // slope is 0 and the first point is to the right.
         {
            Swap(ref p0, ref p1);
         }


         Point[] pointsToDraw = null;
         // Get all the points to draw using the bresenham line algorithm.
         if (slope >= 0 && slope <= 1)
         {
            pointsToDraw = GetDrawingPoints_Slope1(p0.X, p1.X, p0.Y, p1.Y);
         }
         else if (slope > 1)
         {
            pointsToDraw = GetDrawingPoints_Slope2(p0.X, p1.X, p0.Y, p1.Y);
         }
         else if (slope <= -1)
         {
            pointsToDraw = GetDrawingPoints_Slope3(p0.X, p1.X, p0.Y, p1.Y);
         }
         else if (slope < 0 && slope > -1)
         {
            pointsToDraw = GetDrawingPoints_Slope4(p0.X, p1.X, p0.Y, p1.Y);
         }

         // Transform the points back to the original coordinate system.
         m.TransformPoints(pointsToDraw);

         DrawPoints(pointsToDraw);
      }

      /// <summary>
      /// Draws the series of points.
      /// </summary>
      /// <param name="points">The set of points to draw.</param>
      private void DrawPoints(IEnumerable<Point> points)
      {
         foreach (Point p in points)
         {
            _g.FillRectangle(Brushes.White, p.X, p.Y, 1, 1);
         }
      }

      /// <summary>
      /// Swaps the reference of two objects
      /// </summary>
      /// <typeparam name="T">the type</typeparam>
      /// <param name="lhs">left hand side</param>
      /// <param name="rhs">right hand side</param>
      private void Swap<T>(ref T lhs, ref T rhs)
      {
         T temp;
         temp = lhs;
         lhs = rhs;
         rhs = temp;
      }

      /// <summary>
      /// Bresenham line algorithm when the slope of the line is between 0 and 1
      /// </summary>
      /// <returns>The points on the line.</returns>
      private Point[] GetDrawingPoints_Slope1(int x0, int x1, int y0, int y1)
      {
         List<Point> pointsToDraw = new List<Point>();

         int dx = x1 - x0;
         int dy = y1 - y0;

         int d = 2*dy - dx;
         pointsToDraw.Add(new Point(x0, y0));

         int y = y0;
         for (int x = x0 + 1; x <= x1; ++x)
         {
            if (d > 0)
            {
               ++y;
               pointsToDraw.Add(new Point(x, y));
               d = d + 2*(dy - dx);
            }
            else
            {
               pointsToDraw.Add(new Point(x, y));
               d = d + 2*dy;
            }
         }
         return pointsToDraw.ToArray();
      }

      /// <summary>
      /// Bresenham line algorithm when the slope of the line is greater than 1
      /// </summary>
      /// <returns>The points on the line.</returns>
      private Point[] GetDrawingPoints_Slope2(int x0, int x1, int y0, int y1)
      {
         List<Point> pointsToDraw = new List<Point>();

         int dx = x1 - x0;
         int dy = y1 - y0;

         int d = 2*dx - dy;
         pointsToDraw.Add(new Point(x0, y0));

         int x = x0;
         for (int y = y0 + 1; y <= y1; ++y)
         {
            if (d > 0)
            {
               ++x;
               pointsToDraw.Add(new Point(x, y));
               d = d + 2*(dx - dy);
            }
            else
            {
               pointsToDraw.Add(new Point(x, y));
               d = d + 2*dx;
            }
         }
         return pointsToDraw.ToArray();
      }

      /// <summary>
      /// Bresenham line algorithm when the slope of the line is less than 1
      /// </summary>
      /// <returns>The points on the line.</returns>
      private Point[] GetDrawingPoints_Slope3(int x0, int x1, int y0, int y1)
      {
         List<Point> pointsToDraw = new List<Point>();
         
         int dx = x1 - x0;
         int dy = y1 - y0;

         int d = -2*dx - dy;
         pointsToDraw.Add(new Point(x0, y0));

         int x = x0;
         for (int y = y0 + 1; y <= y1; ++y)
         {
            if (d > 0)
            {
               --x;
               pointsToDraw.Add(new Point(x, y));
               d = d + 2*(-dx - dy);
            }
            else
            {
               pointsToDraw.Add(new Point(x, y));
               d = d - 2*dx;
            }
         }
         return pointsToDraw.ToArray();
      }

      /// <summary>
      /// Bresenham line algorithm when the slope of the line is between 0 and -1
      /// </summary>
      /// <returns>The points on the line.</returns>
      private Point[] GetDrawingPoints_Slope4(int x0, int x1, int y0, int y1)
      {
         List<Point> pointsToDraw = new List<Point>();

         int dx = x1 - x0;
         int dy = y1 - y0;

         int d = 2*dy + dx;
         pointsToDraw.Add(new Point(x0, y0));

         int y = y0;
         for (int x = x0 - 1; x >= x1; --x)
         {
            if (d > 0)
            {
               ++y;
               pointsToDraw.Add(new Point(x, y));
               d = d + 2*(dy + dx);
            }
            else
            {
               pointsToDraw.Add(new Point(x, y));
               d = d + 2*dy;
            }
         }
         return pointsToDraw.ToArray();
      }
   }
}

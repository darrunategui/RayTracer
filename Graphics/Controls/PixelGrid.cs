using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS3388_Graphics.Controls
{
   /// <summary>
   /// Pixel grid to draw on.
   /// </summary>
   public partial class PixelGrid : UserControl
   {
      public Bitmap Grid { get; set; }
      public PixelGrid()
      {
         InitializeComponent();
         DoubleBuffered = true;
      }

      /// <summary>
      /// Clears any graphics on the grid.
      /// </summary>
      public void Clear()
      {
         using (Graphics g = Graphics.FromImage(Grid))
         {
            g.Clear(Color.White);
         }
      }

      /// <summary>
      /// Ensures the bitmap grid covers the entire user control area.
      /// </summary>
      protected override void OnSizeChanged(EventArgs e)
      {
         Grid = new Bitmap(Width, Height);
         BackgroundImage = Grid;
         Clear();
         base.OnSizeChanged(e);
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);
      }
   }
}

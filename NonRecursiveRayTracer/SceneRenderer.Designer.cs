namespace NonRecursiveRayTracer
{
   partial class SceneRenderer
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SceneRenderer));
         this.pixelGrid = new CS3388_Graphics.Controls.PixelGrid();
         this.SuspendLayout();
         // 
         // pixelGrid
         // 
         this.pixelGrid.BackColor = System.Drawing.Color.White;
         this.pixelGrid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pixelGrid.BackgroundImage")));
         this.pixelGrid.Grid = ((System.Drawing.Bitmap)(resources.GetObject("pixelGrid.Grid")));
         this.pixelGrid.Location = new System.Drawing.Point(12, 12);
         this.pixelGrid.Name = "pixelGrid";
         this.pixelGrid.Size = new System.Drawing.Size(512, 512);
         this.pixelGrid.TabIndex = 0;
         this.pixelGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pixelGrid_Paint);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(538, 538);
         this.Controls.Add(this.pixelGrid);
         this.Name = "Form1";
         this.Text = "Form1";
         this.Load += new System.EventHandler(this.Form_Load);
         this.ResumeLayout(false);

      }

      #endregion

      private CS3388_Graphics.Controls.PixelGrid pixelGrid;
   }
}


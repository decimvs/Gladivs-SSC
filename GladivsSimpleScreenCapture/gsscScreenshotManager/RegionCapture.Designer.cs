﻿namespace gsscScreenshotManager
{
    partial class RegionCapture
    {
        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RegionCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 325);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RegionCapture";
            this.ShowInTaskbar = false;
            this.Text = "RegionCapture";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RegionForm_Paint);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RegionForm_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RegionForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RegionForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RegionForm_MouseUp);
            this.ResumeLayout(false);

        }
    }
}
namespace SLStudio.ViewsExtensions.CustomControls
{
    partial class CustomChangeStateButton
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.glyph = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // glyph
            // 
            this.glyph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glyph.Location = new System.Drawing.Point(0, 0);
            this.glyph.Margin = new System.Windows.Forms.Padding(0);
            this.glyph.Name = "glyph";
            this.glyph.Size = new System.Drawing.Size(45, 32);
            this.glyph.TabIndex = 0;
            this.glyph.Text = "/";
            this.glyph.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.glyph, "Maximize");
            this.glyph.UseCompatibleTextRendering = true;
            this.glyph.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.glyph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.glyph.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.glyph.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.glyph.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // CustomChangeStateButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.glyph);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CustomChangeStateButton";
            this.Size = new System.Drawing.Size(45, 32);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label glyph;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
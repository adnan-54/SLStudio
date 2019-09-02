namespace SLStudio.ViewsExtensions.CustomControls
{
    partial class CustomCloseButton
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
            this.labelClose = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // labelClose
            // 
            this.labelClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelClose.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F);
            this.labelClose.Location = new System.Drawing.Point(0, 0);
            this.labelClose.Margin = new System.Windows.Forms.Padding(0);
            this.labelClose.Name = "labelClose";
            this.labelClose.Size = new System.Drawing.Size(45, 32);
            this.labelClose.TabIndex = 0;
            this.labelClose.Text = "";
            this.labelClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.labelClose, "Close");
            this.labelClose.UseCompatibleTextRendering = true;
            this.labelClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.labelClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.labelClose.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.labelClose.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.labelClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // CustomCloseButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CausesValidation = false;
            this.Controls.Add(this.labelClose);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe MDL2 Assets", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CustomCloseButton";
            this.Size = new System.Drawing.Size(45, 32);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelClose;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

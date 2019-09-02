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
            this.icon = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // icon
            // 
            this.icon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.icon.Location = new System.Drawing.Point(0, 0);
            this.icon.Margin = new System.Windows.Forms.Padding(0);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(45, 32);
            this.icon.TabIndex = 0;
            this.icon.Text = "";
            this.icon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.icon, "Maximize");
            this.icon.UseCompatibleTextRendering = true;
            this.icon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.icon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.icon.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.icon.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.icon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // CustomChangeStateButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.icon);
            this.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CustomChangeStateButton";
            this.Size = new System.Drawing.Size(45, 32);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label icon;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

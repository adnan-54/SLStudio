namespace SLStudio.Views.Controls
{
    partial class RecentFilesList
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblPath = new System.Windows.Forms.Label();
            this.picture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(59, 7);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(85, 25);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "lblName";
            this.lblName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseDown);
            this.lblName.MouseEnter += new System.EventHandler(this.RecentFilesList_MouseEnter);
            this.lblName.MouseLeave += new System.EventHandler(this.RecentFilesList_MouseLeave);
            this.lblName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseUp);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.BackColor = System.Drawing.Color.Transparent;
            this.lblPath.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPath.Location = new System.Drawing.Point(61, 32);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(47, 17);
            this.lblPath.TabIndex = 1;
            this.lblPath.Text = "lblPath";
            this.lblPath.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseDown);
            this.lblPath.MouseEnter += new System.EventHandler(this.RecentFilesList_MouseEnter);
            this.lblPath.MouseLeave += new System.EventHandler(this.RecentFilesList_MouseLeave);
            this.lblPath.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseUp);
            // 
            // picture
            // 
            this.picture.Location = new System.Drawing.Point(3, 3);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(50, 50);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture.TabIndex = 2;
            this.picture.TabStop = false;
            this.picture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseDown);
            this.picture.MouseEnter += new System.EventHandler(this.RecentFilesList_MouseEnter);
            this.picture.MouseLeave += new System.EventHandler(this.RecentFilesList_MouseLeave);
            this.picture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseUp);
            // 
            // RecentFilesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picture);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.lblName);
            this.Name = "RecentFilesList";
            this.Size = new System.Drawing.Size(941, 57);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseDown);
            this.MouseEnter += new System.EventHandler(this.RecentFilesList_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.RecentFilesList_MouseLeave);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblPath_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.PictureBox picture;
    }
}

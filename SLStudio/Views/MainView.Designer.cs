namespace SLStudio.Views
{
    partial class MainView
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
            this.buttonClose = new SLStudio.ViewsExtensions.CustomControls.CustomCloseButton();
            this.buttonChangeState = new SLStudio.ViewsExtensions.CustomControls.CustomChangeStateButton();
            this.customMinimizeButton1 = new SLStudio.ViewsExtensions.CustomControls.CustomMinimizeButton();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.buttonClose.CausesValidation = false;
            this.buttonClose.Font = new System.Drawing.Font("Segoe MDL2 Assets", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.buttonClose.Location = new System.Drawing.Point(979, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(45, 32);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.TabStop = false;
            this.buttonClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonCloseOnMouseClick);
            // 
            // buttonChangeState
            // 
            this.buttonChangeState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.buttonChangeState.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F);
            this.buttonChangeState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.buttonChangeState.Location = new System.Drawing.Point(934, 0);
            this.buttonChangeState.Margin = new System.Windows.Forms.Padding(0);
            this.buttonChangeState.Name = "buttonChangeState";
            this.buttonChangeState.Size = new System.Drawing.Size(45, 32);
            this.buttonChangeState.TabIndex = 1;
            this.buttonChangeState.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonChangeStateOnMouseClick);
            // 
            // customMinimizeButton1
            // 
            this.customMinimizeButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.customMinimizeButton1.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customMinimizeButton1.Location = new System.Drawing.Point(889, 0);
            this.customMinimizeButton1.Margin = new System.Windows.Forms.Padding(0);
            this.customMinimizeButton1.Name = "customMinimizeButton1";
            this.customMinimizeButton1.ParentForm = this;
            this.customMinimizeButton1.Size = new System.Drawing.Size(45, 32);
            this.customMinimizeButton1.TabIndex = 2;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 720);
            this.Controls.Add(this.customMinimizeButton1);
            this.Controls.Add(this.buttonChangeState);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainView";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SLStudio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private ViewsExtensions.CustomControls.CustomCloseButton buttonClose;
        private ViewsExtensions.CustomControls.CustomChangeStateButton buttonChangeState;
        private ViewsExtensions.CustomControls.CustomMinimizeButton customMinimizeButton1;
    }
}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.ButtonClose = new SLStudio.ViewsExtensions.CustomControls.CustomCloseButton();
            this.ButtonChangeState = new SLStudio.ViewsExtensions.CustomControls.CustomChangeStateButton();
            this.ButtonMinimize = new SLStudio.ViewsExtensions.CustomControls.CustomMinimizeButton();
            this.customTitleBar1 = new SLStudio.ViewsExtensions.CustomControls.CustomTitleBar();
            this.ApplicationIcon = new SLStudio.ViewsExtensions.CustomControls.CustomApplicationIcon();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.ButtonClose.CausesValidation = false;
            this.ButtonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.ButtonClose.Location = new System.Drawing.Point(1142, 0);
            this.ButtonClose.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.ParentForm_ = this;
            this.ButtonClose.Size = new System.Drawing.Size(52, 37);
            this.ButtonClose.TabIndex = 4;
            this.ButtonClose.TabStop = false;
            // 
            // ButtonChangeState
            // 
            this.ButtonChangeState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonChangeState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.ButtonChangeState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ButtonChangeState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.ButtonChangeState.Location = new System.Drawing.Point(1090, 0);
            this.ButtonChangeState.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonChangeState.Name = "ButtonChangeState";
            this.ButtonChangeState.ParentForm_ = this;
            this.ButtonChangeState.Size = new System.Drawing.Size(52, 37);
            this.ButtonChangeState.TabIndex = 5;
            this.ButtonChangeState.TabStop = false;
            // 
            // ButtonMinimize
            // 
            this.ButtonMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.ButtonMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonMinimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.ButtonMinimize.Location = new System.Drawing.Point(1037, 0);
            this.ButtonMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonMinimize.Name = "ButtonMinimize";
            this.ButtonMinimize.ParentForm_ = this;
            this.ButtonMinimize.Size = new System.Drawing.Size(52, 37);
            this.ButtonMinimize.TabIndex = 6;
            this.ButtonMinimize.TabStop = false;
            // 
            // customTitleBar1
            // 
            this.customTitleBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.customTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customTitleBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.customTitleBar1.Location = new System.Drawing.Point(0, 0);
            this.customTitleBar1.Margin = new System.Windows.Forms.Padding(0);
            this.customTitleBar1.Name = "customTitleBar1";
            this.customTitleBar1.ParentForm_ = this;
            this.customTitleBar1.Size = new System.Drawing.Size(1195, 37);
            this.customTitleBar1.TabIndex = 9;
            this.customTitleBar1.TextFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTitleBar1.TitleAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customTitleBar1.TitleText = "SLStudio";
            // 
            // ApplicationIcon
            // 
            this.ApplicationIcon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.ApplicationIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.ApplicationIcon.Location = new System.Drawing.Point(0, 0);
            this.ApplicationIcon.Margin = new System.Windows.Forms.Padding(0);
            this.ApplicationIcon.Name = "ApplicationIcon";
            this.ApplicationIcon.OffFocusItem = ((System.Drawing.Image)(resources.GetObject("ApplicationIcon.OffFocusItem")));
            this.ApplicationIcon.OnFocusIcon = ((System.Drawing.Image)(resources.GetObject("ApplicationIcon.OnFocusIcon")));
            this.ApplicationIcon.ParentForm_ = this;
            this.ApplicationIcon.Size = new System.Drawing.Size(52, 37);
            this.ApplicationIcon.TabIndex = 10;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 831);
            this.Controls.Add(this.ApplicationIcon);
            this.Controls.Add(this.ButtonMinimize);
            this.Controls.Add(this.ButtonChangeState);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.customTitleBar1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(933, 692);
            this.Name = "MainView";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SLStudio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private ViewsExtensions.CustomControls.CustomCloseButton ButtonClose;
        private ViewsExtensions.CustomControls.CustomMinimizeButton ButtonMinimize;
        private ViewsExtensions.CustomControls.CustomChangeStateButton ButtonChangeState;
        private ViewsExtensions.CustomControls.CustomTitleBar customTitleBar1;
        private ViewsExtensions.CustomControls.CustomApplicationIcon ApplicationIcon;
    }
}
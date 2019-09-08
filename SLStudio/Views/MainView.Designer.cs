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
            this.MenuStrip = new XanderUI.XUIFlatMenuStrip();
            this.MenuStrip_File = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_File_New = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_File_New_Solution = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_File_New_Project = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_File_New_File = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip.SuspendLayout();
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
            this.customTitleBar1.TitleAlignment = System.Drawing.ContentAlignment.MiddleLeft;
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
            // MenuStrip
            // 
            this.MenuStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuStrip.BackColor = System.Drawing.Color.DodgerBlue;
            this.MenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MenuStrip.HoverBackColor = System.Drawing.Color.RoyalBlue;
            this.MenuStrip.HoverTextColor = System.Drawing.Color.White;
            this.MenuStrip.ItemBackColor = System.Drawing.Color.DodgerBlue;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStrip_File});
            this.MenuStrip.Location = new System.Drawing.Point(52, 9);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.SelectedBackColor = System.Drawing.Color.DarkOrchid;
            this.MenuStrip.SelectedTextColor = System.Drawing.Color.White;
            this.MenuStrip.SeperatorColor = System.Drawing.Color.White;
            this.MenuStrip.Size = new System.Drawing.Size(45, 24);
            this.MenuStrip.TabIndex = 11;
            this.MenuStrip.Text = "xuiFlatMenuStrip1";
            this.MenuStrip.TextColor = System.Drawing.Color.White;
            // 
            // MenuStrip_File
            // 
            this.MenuStrip_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStrip_File_New});
            this.MenuStrip_File.ForeColor = System.Drawing.Color.White;
            this.MenuStrip_File.Name = "MenuStrip_File";
            this.MenuStrip_File.Size = new System.Drawing.Size(37, 20);
            this.MenuStrip_File.Text = "File";
            // 
            // MenuStrip_File_New
            // 
            this.MenuStrip_File_New.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStrip_File_New_Solution,
            this.MenuStrip_File_New_Project,
            this.MenuStrip_File_New_File});
            this.MenuStrip_File_New.ForeColor = System.Drawing.Color.White;
            this.MenuStrip_File_New.Name = "MenuStrip_File_New";
            this.MenuStrip_File_New.Size = new System.Drawing.Size(98, 22);
            this.MenuStrip_File_New.Text = "New";
            // 
            // MenuStrip_File_New_Solution
            // 
            this.MenuStrip_File_New_Solution.ForeColor = System.Drawing.Color.White;
            this.MenuStrip_File_New_Solution.Name = "MenuStrip_File_New_Solution";
            this.MenuStrip_File_New_Solution.Size = new System.Drawing.Size(118, 22);
            this.MenuStrip_File_New_Solution.Text = "Solution";
            // 
            // MenuStrip_File_New_Project
            // 
            this.MenuStrip_File_New_Project.ForeColor = System.Drawing.Color.White;
            this.MenuStrip_File_New_Project.Name = "MenuStrip_File_New_Project";
            this.MenuStrip_File_New_Project.Size = new System.Drawing.Size(118, 22);
            this.MenuStrip_File_New_Project.Text = "Project";
            // 
            // MenuStrip_File_New_File
            // 
            this.MenuStrip_File_New_File.ForeColor = System.Drawing.Color.White;
            this.MenuStrip_File_New_File.Name = "MenuStrip_File_New_File";
            this.MenuStrip_File_New_File.Size = new System.Drawing.Size(118, 22);
            this.MenuStrip_File_New_File.Text = "File";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 831);
            this.Controls.Add(this.MenuStrip);
            this.Controls.Add(this.ApplicationIcon);
            this.Controls.Add(this.ButtonMinimize);
            this.Controls.Add(this.ButtonChangeState);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.customTitleBar1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(933, 692);
            this.Name = "MainView";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SLStudio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ViewsExtensions.CustomControls.CustomCloseButton ButtonClose;
        private ViewsExtensions.CustomControls.CustomMinimizeButton ButtonMinimize;
        private ViewsExtensions.CustomControls.CustomChangeStateButton ButtonChangeState;
        private ViewsExtensions.CustomControls.CustomTitleBar customTitleBar1;
        private ViewsExtensions.CustomControls.CustomApplicationIcon ApplicationIcon;
        private XanderUI.XUIFlatMenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuStrip_File;
        private System.Windows.Forms.ToolStripMenuItem MenuStrip_File_New;
        private System.Windows.Forms.ToolStripMenuItem MenuStrip_File_New_Solution;
        private System.Windows.Forms.ToolStripMenuItem MenuStrip_File_New_Project;
        private System.Windows.Forms.ToolStripMenuItem MenuStrip_File_New_File;
    }
}
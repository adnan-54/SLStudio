using SLStudio.Views.Themes;

namespace SLStudio.Views
{
    partial class frmStartScreen
    {
        ThemesManager themeManager = new ThemesManager();

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblOpenRecent = new System.Windows.Forms.Label();
            this.lblGetSarted = new System.Windows.Forms.Label();
            this.panelGetStarted = new MetroFramework.Controls.MetroPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTutorials = new MetroFramework.Controls.MetroTile();
            this.btnClone = new MetroFramework.Controls.MetroTile();
            this.btnOpenProject = new MetroFramework.Controls.MetroTile();
            this.btnCreateNew = new MetroFramework.Controls.MetroTile();
            this.lblContinueWithoutCode = new System.Windows.Forms.LinkLabel();
            this.panelOpen = new System.Windows.Forms.Panel();
            this.titleBar = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panelGetStarted.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.titleBar.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(23, 46);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(187, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SLStudio 2019";
            // 
            // lblOpenRecent
            // 
            this.lblOpenRecent.AutoSize = true;
            this.lblOpenRecent.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lblOpenRecent.Location = new System.Drawing.Point(25, 101);
            this.lblOpenRecent.Name = "lblOpenRecent";
            this.lblOpenRecent.Size = new System.Drawing.Size(119, 28);
            this.lblOpenRecent.TabIndex = 1;
            this.lblOpenRecent.Text = "Open recent";
            // 
            // lblGetSarted
            // 
            this.lblGetSarted.AutoSize = true;
            this.lblGetSarted.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lblGetSarted.Location = new System.Drawing.Point(607, 101);
            this.lblGetSarted.Name = "lblGetSarted";
            this.lblGetSarted.Size = new System.Drawing.Size(109, 28);
            this.lblGetSarted.TabIndex = 2;
            this.lblGetSarted.Text = "Get started";
            // 
            // panelGetStarted
            // 
            this.panelGetStarted.Controls.Add(this.tableLayoutPanel1);
            this.panelGetStarted.HorizontalScrollbarBarColor = true;
            this.panelGetStarted.HorizontalScrollbarHighlightOnWheel = false;
            this.panelGetStarted.HorizontalScrollbarSize = 10;
            this.panelGetStarted.Location = new System.Drawing.Point(612, 145);
            this.panelGetStarted.Margin = new System.Windows.Forms.Padding(10);
            this.panelGetStarted.Name = "panelGetStarted";
            this.panelGetStarted.Size = new System.Drawing.Size(345, 301);
            this.panelGetStarted.TabIndex = 4;
            this.panelGetStarted.VerticalScrollbarBarColor = true;
            this.panelGetStarted.VerticalScrollbarHighlightOnWheel = false;
            this.panelGetStarted.VerticalScrollbarSize = 10;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnTutorials, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnClone, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenProject, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCreateNew, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(345, 301);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnTutorials
            // 
            this.btnTutorials.ActiveControl = null;
            this.btnTutorials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTutorials.Location = new System.Drawing.Point(3, 228);
            this.btnTutorials.Name = "btnTutorials";
            this.btnTutorials.Size = new System.Drawing.Size(339, 70);
            this.btnTutorials.TabIndex = 3;
            this.btnTutorials.Text = "Tutorials";
            this.btnTutorials.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTutorials.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTutorials.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.btnTutorials.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.btnTutorials.UseSelectable = true;
            this.btnTutorials.Click += new System.EventHandler(this.OnTutorials);
            // 
            // btnClone
            // 
            this.btnClone.ActiveControl = null;
            this.btnClone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClone.Location = new System.Drawing.Point(3, 153);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(339, 69);
            this.btnClone.TabIndex = 2;
            this.btnClone.Text = "Clone or checkout project";
            this.btnClone.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClone.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClone.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.btnClone.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.btnClone.UseSelectable = true;
            this.btnClone.Click += new System.EventHandler(this.OnCheckoutProject);
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.ActiveControl = null;
            this.btnOpenProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenProject.Location = new System.Drawing.Point(3, 78);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(339, 69);
            this.btnOpenProject.TabIndex = 1;
            this.btnOpenProject.Text = "Open a project or solution";
            this.btnOpenProject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOpenProject.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenProject.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.btnOpenProject.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.btnOpenProject.UseSelectable = true;
            this.btnOpenProject.Click += new System.EventHandler(this.OnOpenProjectOrSolution);
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.ActiveControl = null;
            this.btnCreateNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateNew.Location = new System.Drawing.Point(3, 3);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(339, 69);
            this.btnCreateNew.Style = MetroFramework.MetroColorStyle.Blue;
            this.btnCreateNew.TabIndex = 0;
            this.btnCreateNew.Text = "Create a new project";
            this.btnCreateNew.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCreateNew.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateNew.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.btnCreateNew.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.btnCreateNew.UseSelectable = true;
            this.btnCreateNew.Click += new System.EventHandler(this.OnCreateNew);
            // 
            // lblContinueWithoutCode
            // 
            this.lblContinueWithoutCode.AutoSize = true;
            this.lblContinueWithoutCode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContinueWithoutCode.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblContinueWithoutCode.Location = new System.Drawing.Point(816, 449);
            this.lblContinueWithoutCode.Name = "lblContinueWithoutCode";
            this.lblContinueWithoutCode.Size = new System.Drawing.Size(138, 17);
            this.lblContinueWithoutCode.TabIndex = 5;
            this.lblContinueWithoutCode.TabStop = true;
            this.lblContinueWithoutCode.Text = "Continue without code";
            this.lblContinueWithoutCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblContinueWithoutCode.Click += new System.EventHandler(this.OnContinueWithoutCode);
            // 
            // panelOpen
            // 
            this.panelOpen.AutoScroll = true;
            this.panelOpen.Location = new System.Drawing.Point(30, 145);
            this.panelOpen.Name = "panelOpen";
            this.panelOpen.Size = new System.Drawing.Size(498, 489);
            this.panelOpen.TabIndex = 6;
            // 
            // titleBar
            // 
            this.titleBar.Controls.Add(this.tableLayoutPanel2);
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(983, 32);
            this.titleBar.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.Controls.Add(this.button1, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.button2, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(983, 32);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(938, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Arial", 14.25F);
            this.button2.Location = new System.Drawing.Point(893, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 32);
            this.button2.TabIndex = 1;
            this.button2.Text = "_";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // frmStartScreen
            // 
            this.BackColor = themeManager.styleBase;
            this.ClientSize = new System.Drawing.Size(983, 659);
            this.Controls.Add(this.titleBar);
            this.Controls.Add(this.panelOpen);
            this.Controls.Add(this.lblContinueWithoutCode);
            this.Controls.Add(this.panelGetStarted);
            this.Controls.Add(this.lblGetSarted);
            this.Controls.Add(this.lblOpenRecent);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmStartScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panelGetStarted.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.titleBar.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblOpenRecent;
        private System.Windows.Forms.Label lblGetSarted;
        private MetroFramework.Controls.MetroPanel panelGetStarted;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.LinkLabel lblContinueWithoutCode;
        private MetroFramework.Controls.MetroTile btnCreateNew;
        private MetroFramework.Controls.MetroTile btnTutorials;
        private MetroFramework.Controls.MetroTile btnClone;
        private MetroFramework.Controls.MetroTile btnOpenProject;
        private System.Windows.Forms.Panel panelOpen;
        private System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
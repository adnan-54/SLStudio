namespace SLStudio.Views
{
    partial class frmStartScreen
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblOpenRecent = new System.Windows.Forms.Label();
            this.lblGetSarted = new System.Windows.Forms.Label();
            this.panelOpen = new MetroFramework.Controls.MetroPanel();
            this.panelGetStarted = new MetroFramework.Controls.MetroPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTutorials = new MetroFramework.Controls.MetroTile();
            this.btnClone = new MetroFramework.Controls.MetroTile();
            this.btnOpenProject = new MetroFramework.Controls.MetroTile();
            this.btnCreateNew = new MetroFramework.Controls.MetroTile();
            this.lblContinueWithoutCode = new System.Windows.Forms.LinkLabel();
            this.panelGetStarted.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            // panelOpen
            // 
            this.panelOpen.HorizontalScrollbarBarColor = true;
            this.panelOpen.HorizontalScrollbarHighlightOnWheel = false;
            this.panelOpen.HorizontalScrollbarSize = 10;
            this.panelOpen.Location = new System.Drawing.Point(30, 145);
            this.panelOpen.Name = "panelOpen";
            this.panelOpen.Size = new System.Drawing.Size(498, 489);
            this.panelOpen.TabIndex = 3;
            this.panelOpen.VerticalScrollbar = true;
            this.panelOpen.VerticalScrollbarBarColor = true;
            this.panelOpen.VerticalScrollbarHighlightOnWheel = false;
            this.panelOpen.VerticalScrollbarSize = 10;
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
            this.btnCreateNew.Click += new System.EventHandler(this.BtnCreateNew_Click);
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
            // 
            // frmStartScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 659);
            this.Controls.Add(this.lblContinueWithoutCode);
            this.Controls.Add(this.panelGetStarted);
            this.Controls.Add(this.panelOpen);
            this.Controls.Add(this.lblGetSarted);
            this.Controls.Add(this.lblOpenRecent);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmStartScreen";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panelGetStarted.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblOpenRecent;
        private System.Windows.Forms.Label lblGetSarted;
        private MetroFramework.Controls.MetroPanel panelOpen;
        private MetroFramework.Controls.MetroPanel panelGetStarted;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.LinkLabel lblContinueWithoutCode;
        private MetroFramework.Controls.MetroTile btnCreateNew;
        private MetroFramework.Controls.MetroTile btnTutorials;
        private MetroFramework.Controls.MetroTile btnClone;
        private MetroFramework.Controls.MetroTile btnOpenProject;
    }
}
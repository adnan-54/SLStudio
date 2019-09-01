namespace SLStudio.Views
{
    partial class StartScreenView
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
            this.panelGetStarted = new MetroFramework.Controls.MetroPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTutorials = new System.Windows.Forms.Button();
            this.btnClone = new System.Windows.Forms.Button();
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.btnOpenProject = new System.Windows.Forms.Button();
            this.lblContinueWithoutCode = new System.Windows.Forms.LinkLabel();
            this.panelOpen = new System.Windows.Forms.Panel();
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
            this.lblGetSarted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGetSarted.AutoSize = true;
            this.lblGetSarted.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lblGetSarted.Location = new System.Drawing.Point(588, 101);
            this.lblGetSarted.Name = "lblGetSarted";
            this.lblGetSarted.Size = new System.Drawing.Size(109, 28);
            this.lblGetSarted.TabIndex = 2;
            this.lblGetSarted.Text = "Get started";
            // 
            // panelGetStarted
            // 
            this.panelGetStarted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGetStarted.Controls.Add(this.tableLayoutPanel1);
            this.panelGetStarted.HorizontalScrollbarBarColor = true;
            this.panelGetStarted.HorizontalScrollbarHighlightOnWheel = false;
            this.panelGetStarted.HorizontalScrollbarSize = 10;
            this.panelGetStarted.Location = new System.Drawing.Point(593, 145);
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
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnTutorials, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnClone, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCreateNew, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenProject, 0, 1);
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
            this.btnTutorials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTutorials.FlatAppearance.BorderSize = 0;
            this.btnTutorials.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTutorials.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnTutorials.Location = new System.Drawing.Point(0, 230);
            this.btnTutorials.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnTutorials.Name = "btnTutorials";
            this.btnTutorials.Size = new System.Drawing.Size(345, 71);
            this.btnTutorials.TabIndex = 8;
            this.btnTutorials.Text = "Tutorials";
            this.btnTutorials.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTutorials.UseVisualStyleBackColor = false;
            this.btnTutorials.Click += new System.EventHandler(this.OnTutorials);
            // 
            // btnClone
            // 
            this.btnClone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClone.FlatAppearance.BorderSize = 0;
            this.btnClone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClone.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnClone.Location = new System.Drawing.Point(0, 155);
            this.btnClone.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(345, 70);
            this.btnClone.TabIndex = 9;
            this.btnClone.Text = "Clone";
            this.btnClone.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClone.UseVisualStyleBackColor = false;
            this.btnClone.Click += new System.EventHandler(this.OnCheckoutProject);
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateNew.FlatAppearance.BorderSize = 0;
            this.btnCreateNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateNew.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnCreateNew.Location = new System.Drawing.Point(0, 5);
            this.btnCreateNew.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(345, 70);
            this.btnCreateNew.TabIndex = 2;
            this.btnCreateNew.Text = "Create new";
            this.btnCreateNew.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCreateNew.UseVisualStyleBackColor = false;
            this.btnCreateNew.Click += new System.EventHandler(this.OnCreateNew);
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenProject.FlatAppearance.BorderSize = 0;
            this.btnOpenProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenProject.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnOpenProject.Location = new System.Drawing.Point(0, 80);
            this.btnOpenProject.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(345, 70);
            this.btnOpenProject.TabIndex = 7;
            this.btnOpenProject.Text = "Open";
            this.btnOpenProject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOpenProject.UseVisualStyleBackColor = false;
            this.btnOpenProject.Click += new System.EventHandler(this.OnOpenProjectOrSolution);
            // 
            // lblContinueWithoutCode
            // 
            this.lblContinueWithoutCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContinueWithoutCode.AutoSize = true;
            this.lblContinueWithoutCode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContinueWithoutCode.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblContinueWithoutCode.Location = new System.Drawing.Point(797, 449);
            this.lblContinueWithoutCode.Name = "lblContinueWithoutCode";
            this.lblContinueWithoutCode.Size = new System.Drawing.Size(138, 17);
            this.lblContinueWithoutCode.TabIndex = 5;
            this.lblContinueWithoutCode.TabStop = true;
            this.lblContinueWithoutCode.Text = "Continue without code";
            this.lblContinueWithoutCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblContinueWithoutCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnContinueWithoutCode);
            // 
            // panelOpen
            // 
            this.panelOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOpen.AutoScroll = true;
            this.panelOpen.Location = new System.Drawing.Point(30, 145);
            this.panelOpen.Name = "panelOpen";
            this.panelOpen.Size = new System.Drawing.Size(479, 489);
            this.panelOpen.TabIndex = 6;
            // 
            // frmStartScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(981, 657);
            this.ControlBox = false;
            this.Controls.Add(this.panelOpen);
            this.Controls.Add(this.lblContinueWithoutCode);
            this.Controls.Add(this.panelGetStarted);
            this.Controls.Add(this.lblGetSarted);
            this.Controls.Add(this.lblOpenRecent);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(981, 657);
            this.Name = "frmStartScreen";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SLStudio 2019";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmStartScreen_KeyUp);
            this.panelGetStarted.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panelOpen;
        private System.Windows.Forms.Button btnCreateNew;
        private System.Windows.Forms.Button btnOpenProject;
        private System.Windows.Forms.Button btnTutorials;
        private System.Windows.Forms.Button btnClone;
    }
}
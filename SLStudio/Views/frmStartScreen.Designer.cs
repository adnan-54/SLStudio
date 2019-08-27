﻿namespace SLStudio.Views
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
            this.panelGetStarted = new MetroFramework.Controls.MetroPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblContinueWithoutCode = new System.Windows.Forms.LinkLabel();
            this.panelOpen = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelGetStarted.SuspendLayout();
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
            // lblContinueWithoutCode
            // 
            this.lblContinueWithoutCode.ActiveLinkColor = global::SLStudio.Properties.Settings.Default.selectionLight;
            this.lblContinueWithoutCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContinueWithoutCode.AutoSize = true;
            this.lblContinueWithoutCode.DataBindings.Add(new System.Windows.Forms.Binding("LinkColor", global::SLStudio.Properties.Settings.Default, "link", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblContinueWithoutCode.DataBindings.Add(new System.Windows.Forms.Binding("ActiveLinkColor", global::SLStudio.Properties.Settings.Default, "selectionLight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblContinueWithoutCode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContinueWithoutCode.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblContinueWithoutCode.LinkColor = global::SLStudio.Properties.Settings.Default.link;
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
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semilight", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(936, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnClose.Size = new System.Drawing.Size(45, 32);
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "x";
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.UseMnemonic = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCloseClick);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnCloseMouseEnter);
            // 
            // frmStartScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = global::SLStudio.Properties.Settings.Default.theme;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(981, 657);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panelOpen);
            this.Controls.Add(this.lblContinueWithoutCode);
            this.Controls.Add(this.panelGetStarted);
            this.Controls.Add(this.lblGetSarted);
            this.Controls.Add(this.lblOpenRecent);
            this.Controls.Add(this.lblTitle);
            this.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::SLStudio.Properties.Settings.Default, "font", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::SLStudio.Properties.Settings.Default, "theme", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = global::SLStudio.Properties.Settings.Default.font;
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
        private System.Windows.Forms.Button btnClose;
    }
}
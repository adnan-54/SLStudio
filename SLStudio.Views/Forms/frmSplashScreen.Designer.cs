namespace Views.Forms
{
    partial class frmSplashScreen
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
            this.xuiSplashScreen1 = new XanderUI.XUISplashScreen();
            this.SuspendLayout();
            // 
            // xuiSplashScreen1
            // 
            this.xuiSplashScreen1.AllowDragging = false;
            this.xuiSplashScreen1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.xuiSplashScreen1.BottomText = "Community edition";
            this.xuiSplashScreen1.BottomTextColor = System.Drawing.Color.White;
            this.xuiSplashScreen1.BottomTextSize = 16;
            this.xuiSplashScreen1.EllipseCornerRadius = 12;
            this.xuiSplashScreen1.IsEllipse = true;
            this.xuiSplashScreen1.LoadedColor = System.Drawing.Color.DodgerBlue;
            this.xuiSplashScreen1.ProgressBarStyle = XanderUI.XUIFlatProgressBar.Style.Material;
            this.xuiSplashScreen1.SecondsDisplayed = 2500;
            this.xuiSplashScreen1.ShowProgressBar = true;
            this.xuiSplashScreen1.SplashSize = new System.Drawing.Size(450, 280);
            this.xuiSplashScreen1.TopText = "SLStudio";
            this.xuiSplashScreen1.TopTextColor = System.Drawing.Color.White;
            this.xuiSplashScreen1.TopTextSize = 36;
            this.xuiSplashScreen1.UnloadedColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            // 
            // frmSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(419, 241);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSplashScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSplashScreen";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion
        private XanderUI.XUISplashScreen xuiSplashScreen1;
    }
}
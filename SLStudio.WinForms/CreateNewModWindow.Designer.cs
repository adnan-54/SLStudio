namespace SLStudio.WinForms
{
    partial class CreateNewModWindow
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
            this.LabelModName = new System.Windows.Forms.Label();
            this.TextBoxModName = new System.Windows.Forms.TextBox();
            this.LabelModDescription = new System.Windows.Forms.Label();
            this.TextBoxModDescription = new System.Windows.Forms.TextBox();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelModName
            // 
            this.LabelModName.AutoSize = true;
            this.LabelModName.Location = new System.Drawing.Point(12, 9);
            this.LabelModName.Name = "LabelModName";
            this.LabelModName.Size = new System.Drawing.Size(57, 13);
            this.LabelModName.TabIndex = 0;
            this.LabelModName.Text = "Mod name";
            // 
            // TextBoxModName
            // 
            this.TextBoxModName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxModName.Location = new System.Drawing.Point(100, 6);
            this.TextBoxModName.Name = "TextBoxModName";
            this.TextBoxModName.Size = new System.Drawing.Size(481, 20);
            this.TextBoxModName.TabIndex = 1;
            // 
            // LabelModDescription
            // 
            this.LabelModDescription.AutoSize = true;
            this.LabelModDescription.Location = new System.Drawing.Point(12, 39);
            this.LabelModDescription.Name = "LabelModDescription";
            this.LabelModDescription.Size = new System.Drawing.Size(82, 13);
            this.LabelModDescription.TabIndex = 2;
            this.LabelModDescription.Text = "Mod description";
            // 
            // TextBoxModDescription
            // 
            this.TextBoxModDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxModDescription.Location = new System.Drawing.Point(100, 32);
            this.TextBoxModDescription.Multiline = true;
            this.TextBoxModDescription.Name = "TextBoxModDescription";
            this.TextBoxModDescription.Size = new System.Drawing.Size(481, 309);
            this.TextBoxModDescription.TabIndex = 3;
            // 
            // ButtonOk
            // 
            this.ButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOk.Location = new System.Drawing.Point(506, 347);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(75, 23);
            this.ButtonOk.TabIndex = 4;
            this.ButtonOk.Text = "&Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnButtonOkClick);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(425, 347);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "C&ancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnButtonCancelClick);
            // 
            // CreateNewModWindow
            // 
            this.AcceptButton = this.ButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(596, 377);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.TextBoxModDescription);
            this.Controls.Add(this.LabelModDescription);
            this.Controls.Add(this.TextBoxModName);
            this.Controls.Add(this.LabelModName);
            this.MinimumSize = new System.Drawing.Size(372, 293);
            this.Name = "CreateNewModWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateNewModWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelModName;
        private System.Windows.Forms.TextBox TextBoxModName;
        private System.Windows.Forms.Label LabelModDescription;
        private System.Windows.Forms.TextBox TextBoxModDescription;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
    }
}
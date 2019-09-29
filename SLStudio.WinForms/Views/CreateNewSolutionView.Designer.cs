namespace SLStudio.WinForms.Views
{
    partial class CreateNewSolutionView
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
            this.SolutionName = new System.Windows.Forms.TextBox();
            this.SolutionDescription = new System.Windows.Forms.TextBox();
            this.Author = new System.Windows.Forms.TextBox();
            this.ButtoAddnAuthor = new System.Windows.Forms.Button();
            this.Authors = new System.Windows.Forms.ListBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SolutionName
            // 
            this.SolutionName.Location = new System.Drawing.Point(127, 12);
            this.SolutionName.Name = "SolutionName";
            this.SolutionName.Size = new System.Drawing.Size(265, 20);
            this.SolutionName.TabIndex = 0;
            // 
            // SolutionDescription
            // 
            this.SolutionDescription.Location = new System.Drawing.Point(127, 38);
            this.SolutionDescription.Multiline = true;
            this.SolutionDescription.Name = "SolutionDescription";
            this.SolutionDescription.Size = new System.Drawing.Size(265, 176);
            this.SolutionDescription.TabIndex = 1;
            // 
            // Author
            // 
            this.Author.Location = new System.Drawing.Point(127, 220);
            this.Author.Name = "Author";
            this.Author.Size = new System.Drawing.Size(235, 20);
            this.Author.TabIndex = 2;
            // 
            // ButtoAddnAuthor
            // 
            this.ButtoAddnAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtoAddnAuthor.Location = new System.Drawing.Point(368, 220);
            this.ButtoAddnAuthor.Name = "ButtoAddnAuthor";
            this.ButtoAddnAuthor.Size = new System.Drawing.Size(24, 21);
            this.ButtoAddnAuthor.TabIndex = 3;
            this.ButtoAddnAuthor.Text = "+";
            this.ButtoAddnAuthor.UseCompatibleTextRendering = true;
            this.ButtoAddnAuthor.UseVisualStyleBackColor = true;
            this.ButtoAddnAuthor.Click += new System.EventHandler(this.ButtoAddnAuthor_Click);
            // 
            // Authors
            // 
            this.Authors.FormattingEnabled = true;
            this.Authors.Location = new System.Drawing.Point(127, 247);
            this.Authors.Name = "Authors";
            this.Authors.Size = new System.Drawing.Size(265, 225);
            this.Authors.TabIndex = 4;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(202, 478);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(92, 24);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOk
            // 
            this.ButtonOk.Location = new System.Drawing.Point(300, 478);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(92, 24);
            this.ButtonOk.TabIndex = 6;
            this.ButtonOk.Text = "&Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Solution Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Authors";
            // 
            // CreateNewSolutionView
            // 
            this.AcceptButton = this.ButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.Authors);
            this.Controls.Add(this.ButtoAddnAuthor);
            this.Controls.Add(this.Author);
            this.Controls.Add(this.SolutionDescription);
            this.Controls.Add(this.SolutionName);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "CreateNewSolutionView";
            this.Text = "CreateNewSolutionView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SolutionName;
        private System.Windows.Forms.TextBox SolutionDescription;
        private System.Windows.Forms.TextBox Author;
        private System.Windows.Forms.Button ButtoAddnAuthor;
        private System.Windows.Forms.ListBox Authors;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
using SLStudio.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomFlatButton : Button
    {
        public CustomFlatButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Settings.Default.themeDark;
            this.FlatAppearance.MouseDownBackColor = Settings.Default.selection;
            this.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            this.UseCompatibleTextRendering = true;
            this.Text = "button";
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MouseEnter += new EventHandler(this.onMouseEnter);
            this.MouseDown += new MouseEventHandler(this.onMouseDown);
            this.MouseUp += new MouseEventHandler(this.onMouseUp);
            this.DataBindings.Add(new Binding("Backcolor", Settings.Default, "theme"));
        }

        private void onMouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Settings.Default.themeDark;
            this.FlatAppearance.MouseDownBackColor = Settings.Default.selection;
            this.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
        }

        private Font mouseDownFont, mouseUpFont;
        private void onMouseDown(object sender, EventArgs e)
        {
            mouseUpFont = this.Font;
            float newSize = mouseUpFont.Size + 1;
            mouseDownFont  = new Font(mouseUpFont.FontFamily, newSize);
            this.Font = mouseDownFont;
        }

        private void onMouseUp(object sender, EventArgs e)
        {
            this.Font = mouseUpFont;
        }
    }
}

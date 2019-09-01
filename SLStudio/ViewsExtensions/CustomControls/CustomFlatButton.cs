using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.Properties;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomFlatButton : Button, IThemedControl
    {
        private Font mouseDownFont, mouseUpFont;

        private Theme theme = new Theme(DefaultThemes.UserDefault);
        public Theme Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
            }
        }

        public CustomFlatButton()
        {
            UpdateTheme();
            ThemeManager.AddControl(this);
        }

        public void UpdateTheme()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = theme.themeDark;
            this.FlatAppearance.MouseDownBackColor = theme.selection;
            this.FlatAppearance.MouseOverBackColor = theme.themeLight;
            this.UseCompatibleTextRendering = true;
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MouseDown += new MouseEventHandler(this.onMouseDown);
            this.MouseUp += new MouseEventHandler(this.onMouseUp);
        }

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

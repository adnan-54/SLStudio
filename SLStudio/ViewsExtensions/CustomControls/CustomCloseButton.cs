using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SLStudio.Extensions.Interfaces;
using SLStudio.ViewsExtensions.Themes;
using SLStudio.Extensions.Enums;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomCloseButton : UserControl, IThemedControl
    {
        private Color onHoverBackColor;
        private Color onHoverForeColor;
        private Color onClickBackColor;
        private Color onClickForeColor;

        public CustomCloseButton()
        {
            InitializeComponent();

            UpdateTheme();
            ThemeManager.AddControl(this);
        }

        #region IThemedControl
        private Theme theme = new Theme(DefaultThemes.UserDefault);
        public Theme Theme { get => theme; set => theme = value; }

        public void UpdateTheme()
        {
            this.BackColor = theme.theme;
            this.ForeColor = theme.font;

            labelClose.BackColor = theme.theme;
            labelClose.ForeColor = theme.font;

            onHoverBackColor = theme.error;
            onHoverForeColor = theme.fontLight;
            onClickBackColor = theme.selection;
            onClickForeColor = theme.font;
        }

        #endregion IThemedControl

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            labelClose.BackColor = onHoverBackColor;
            labelClose.ForeColor = onHoverForeColor;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            labelClose.BackColor = theme.theme;
            labelClose.ForeColor = theme.font;
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            labelClose.BackColor = theme.selection;
            labelClose.ForeColor = theme.fontLight;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            labelClose.BackColor = onHoverBackColor;
            labelClose.ForeColor = onHoverForeColor;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }
    }
}

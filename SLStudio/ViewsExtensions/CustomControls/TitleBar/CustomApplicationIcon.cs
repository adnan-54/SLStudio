using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomApplicationIcon : UserControl, IThemedControl
    {
        private Image onFocusIcon;
        public Image OnFocusIcon
        {
            get => onFocusIcon;
            set
            {
                onFocusIcon = value;
                icon.Image = value;
            }
        }

        private Image offFocusItem;
        public Image OffFocusItem { get => offFocusItem; set => offFocusItem = value; }

        private CustomBorderLessForm parentForm;
        public CustomBorderLessForm ParentForm_ { get => parentForm; set => parentForm = value; }

        public CustomApplicationIcon()
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
        }
        #endregion IThemedControl

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(ParentForm_ != null && e.Button == MouseButtons.Left)
            {
                ParentForm_.Close();
                this.Dispose();
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (ParentForm_ != null)
            {
                ParentForm_.Activated += (s, args) => icon.Image = OnFocusIcon;
                ParentForm_.Deactivate += (s, args) => icon.Image = OffFocusItem;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (ParentForm_ != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ParentForm_.ShowSystemMenu(e.Button, new Point(0, this.Height));
                }
                else
                if(e.Button == MouseButtons.Right)
                {
                    ParentForm_.ShowSystemMenu(e.Button);
                }
            }
        }
    }
}

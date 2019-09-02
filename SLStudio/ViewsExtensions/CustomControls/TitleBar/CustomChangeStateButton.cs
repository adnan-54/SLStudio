using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.ViewsExtensions.Language;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Drawing;
using System.Windows.Forms;
using Transitions;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomChangeStateButton : UserControl, IThemedControl, IMultiLanguageControl
    {
        private CustomBorderLessForm parent;
        public CustomBorderLessForm ParentForm_
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        private string maximizeIcon = Char.ConvertFromUtf32(0xE922);
        private string restoreIcon = Char.ConvertFromUtf32(0xE923);

        private string toolTipString = Resources.Messages.Global.maximize;

        public CustomChangeStateButton()
        {
            InitializeComponent();

            string fontName = "Segoe MDL2 Assets";
            float fontSize = 9;


            using (Font fontTester = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel))
            {
                if (fontTester.Name != fontName)
                {
                    var marlett = new Font("Marlett", 9.0f);
                    icon.Font = marlett;

                    maximizeIcon = "2";
                    restoreIcon = "1";
                }
            }

            UpdateTheme();
            ThemeManager.AddControl(this);

            UpdateLanguage();
            LanguageManager.AddControl(this);

        }

        #region IThemedControl, IMultiLanguageControl
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

        public void UpdateTheme()
        {
            this.BackColor = theme.theme;
            this.ForeColor = theme.font;

            if (parent.WindowState == FormWindowState.Maximized)
            {
                icon.Text = restoreIcon;
            }
            else
            {
                icon.Text = maximizeIcon;
            }
        }

        public void UpdateLanguage()
        {
            if (parent != null)
            {
                if (parent.WindowState == FormWindowState.Maximized)
                {
                    toolTipString = Resources.Messages.Global.restore;
                }
                else
                    toolTipString = Resources.Messages.Global.maximize;
            }
        }
        #endregion IThemedControl, IMultiLanguageControl

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Transition.run(icon, "BackColor", theme.selection, new TransitionType_Linear(120));
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Transition.run(icon, "BackColor", theme.themeLight, new TransitionType_Linear(120));
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Transition.run(icon, "BackColor", theme.theme, new TransitionType_Linear(120));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Transition.run(icon, "BackColor", theme.themeLight, new TransitionType_Linear(120));
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && parent != null)
            {
                if (parent.WindowState == FormWindowState.Maximized)
                {
                    icon.Text = restoreIcon;
                    parent.WindowState = FormWindowState.Normal;
                }
                else
                {
                    icon.Text = maximizeIcon;
                    parent.WindowState = FormWindowState.Maximized;
                }

                UpdateLanguage();
            }
        }
    }
}

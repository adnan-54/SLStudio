using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.ViewsExtensions.CustomControls;
using SLStudio.ViewsExtensions.Language;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class MainView : CustomBorderLessForm, IThemedControl, IMultiLanguageControl
    {
        private IStartScreen parent;

        public MainView(IStartScreen parent = null)
        {
            InitializeComponent();
            SetupForm();

            this.parent = parent;

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
            /*btnClose.FlatAppearance.MouseOverBackColor = theme.error;
            btnClose.FlatAppearance.MouseDownBackColor = theme.style;

            btnMinimize.FlatAppearance.MouseOverBackColor = theme.themeLight;
            btnChangeState.FlatAppearance.MouseOverBackColor = theme.themeLight;

            btnMinimize.FlatAppearance.MouseDownBackColor = theme.style;
            btnChangeState.FlatAppearance.MouseDownBackColor = theme.style;*/

            this.BackColor = theme.theme;
            this.ForeColor = theme.font;
        }

        public void UpdateLanguage()
        {
        }

        #endregion IThemedControl, IMultiLanguageControl

        #region Events

        private void OnButtonCloseClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                this.Close();
            }
        }

        private void OnMinimizeClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void OnButtonChangeStateClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void MainViewOnClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        #endregion Events
    }
}
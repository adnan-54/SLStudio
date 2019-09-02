using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.Properties;
using SLStudio.ViewsExtensions.CustomControls;
using SLStudio.ViewsExtensions.Language;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class MainView : CustomBorderLessForm, IThemedControl, IMultiLanguageControl
    {
        public MainView()
        {
            InitializeComponent();
            SetupForm();

            UpdateTheme();
            ThemeManager.AddControl(this);

            UpdateLanguage();
            LanguageManager.AddControl(this);

            buttonChangeState.State = this.WindowState;
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
        }

        public void UpdateLanguage()
        {

        }

        #endregion IThemedControl, IMultiLanguageControl

        #region Events
        private void ButtonCloseOnMouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                Application.Exit();
        }

        private void ButtonChangeStateOnMouseClick(object sender, MouseEventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;

            buttonChangeState.State = this.WindowState;
        }
        #endregion Events

    }
}
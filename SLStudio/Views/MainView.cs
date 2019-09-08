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
        }

        #region IThemedControl, IMultiLanguageControl
        public Theme Theme { get; set; }

        public void UpdateTheme()
        {
            Theme = new Theme(DefaultThemes.UserDefault);

            this.BackColor = Theme.theme;
            this.ForeColor = Theme.font;
        }

        public void UpdateLanguage()
        {

        }

        #endregion IThemedControl, IMultiLanguageControl

        #region Events

        #endregion Events
    }
}
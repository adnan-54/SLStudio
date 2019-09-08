using SLStudio.Extensions.Interfaces;
using SLStudio.ViewsExtensions.CustomControls;
using SLStudio.ViewsExtensions.Language;
using SLStudio.ViewsExtensions.Themes;

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
            Theme = ThemeManager.GetDefaultTheme();

            this.BackColor = Theme.theme;
            this.ForeColor = Theme.font;

            MenuStrip.BackColor = Theme.theme;
            MenuStrip.HoverBackColor = Theme.themeLight;
            MenuStrip.HoverTextColor = Theme.font;
            MenuStrip.ItemBackColor = Theme.themeDark;
            MenuStrip.SelectedBackColor = Theme.themeDark;
            MenuStrip.SelectedTextColor = Theme.font;
            MenuStrip.SeperatorColor = Theme.theme;
            MenuStrip.TextColor = Theme.font;
        }

        public void UpdateLanguage()
        {
            MenuStrip.Items["MenuStrip_File"].Text = Resources.Forms.MainView.file;
            MenuStrip_File.DropDownItems["MenuStrip_File_New"].Text = Resources.Forms.MainView._new;
            MenuStrip_File_New.DropDownItems["MenuStrip_File_New_Solution"].Text = Resources.Forms.MainView.solution;
            MenuStrip_File_New.DropDownItems["MenuStrip_File_New_Project"].Text = Resources.Forms.MainView.project;
            MenuStrip_File_New.DropDownItems["MenuStrip_File_New_File"].Text = Resources.Forms.MainView.file;
        }
        #endregion IThemedControl, IMultiLanguageControl
    }
}
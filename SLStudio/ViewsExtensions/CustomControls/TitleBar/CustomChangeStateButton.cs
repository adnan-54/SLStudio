using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.ViewsExtensions.Language;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Windows.Forms;
using Transitions;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomChangeStateButton : UserControl, IThemedControl, IMultiLanguageControl
    {
        public FormWindowState State
        {
            set
            {
                ChangeIconLabel(value);
            }
        }
        private string toolTipString = Resources.Messages.Global.maximize;

        public CustomChangeStateButton()
        {
            InitializeComponent();

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
        }

        public void UpdateLanguage()
        {
            this.toolTip.SetToolTip(this.icon, toolTipString);
        }
        #endregion IThemedControl, IMultiLanguageControl

        private void ChangeIconLabel(FormWindowState state)
        {
            if(state == FormWindowState.Maximized)
            {
                icon.Text = Char.ConvertFromUtf32(0xE923);
                toolTipString = Resources.Messages.Global.restore;
            }
            else
            {
                icon.Text = Char.ConvertFromUtf32(0xE922);
                toolTipString = Resources.Messages.Global.maximize;
            }

            UpdateLanguage();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

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
    }
}

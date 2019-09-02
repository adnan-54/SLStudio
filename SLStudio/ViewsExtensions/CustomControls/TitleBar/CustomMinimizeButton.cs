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
using Transitions;
using SLStudio.ViewsExtensions.Language;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomMinimizeButton : UserControl, IThemedControl, IMultiLanguageControl
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

        public CustomMinimizeButton()
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
            this.toolTip.SetToolTip(this.label1, Resources.Messages.Global.minimize);
        }
        #endregion IThemedControl, IMultiLanguageControl

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && parent != null)
            {
                parent.WindowState = FormWindowState.Minimized;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Transition.run(label1, "BackColor", theme.selection, new TransitionType_Linear(120));
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Transition.run(label1, "BackColor", theme.themeLight, new TransitionType_Linear(120));
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Transition.run(label1, "BackColor", theme.theme, new TransitionType_Linear(120));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Transition.run(label1, "BackColor", theme.themeLight, new TransitionType_Linear(120));
        }
    }
}

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
using SLStudio.ViewsExtensions.Language;
using Transitions;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomCloseButton : UserControl, IThemedControl, IMultiLanguageControl
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

        public CustomCloseButton()
        {
            InitializeComponent();

            UpdateTheme();
            ThemeManager.AddControl(this);

            UpdateLanguage();
            LanguageManager.AddControl(this);
        }

        #region IThemedControl, IMultiLanguageControl
        private Theme theme = new Theme(DefaultThemes.UserDefault);
        public Theme Theme { get => theme; set => theme = value; }

        public void UpdateTheme()
        {
            this.BackColor = theme.theme;
            this.ForeColor = theme.font;

            labelClose.BackColor = theme.theme;
            labelClose.ForeColor = theme.font;
        }
        public void UpdateLanguage()
        {
            this.toolTip.SetToolTip(this.labelClose, Resources.Messages.Global.close);
        }
        #endregion IThemedControl, IMultiLanguageControl

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Transition.run(labelClose, "BackColor", theme.error, new TransitionType_Linear(120));
            Transition.run(labelClose, "ForeColor", theme.fontLight, new TransitionType_Linear(120));
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Transition.run(labelClose, "BackColor", theme.theme, new TransitionType_Linear(120));
            Transition.run(labelClose, "ForeColor", theme.font, new TransitionType_Linear(120));
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Transition.run(labelClose, "BackColor", theme.selection, new TransitionType_Linear(120));
            Transition.run(labelClose, "ForeColor", theme.fontLight, new TransitionType_Linear(120));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Transition.run(labelClose, "BackColor", theme.error, new TransitionType_Linear(120));
            Transition.run(labelClose, "ForeColor", theme.fontLight, new TransitionType_Linear(120));
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && parent != null)
            {
                parent.Close();
            }
        }
    }
}

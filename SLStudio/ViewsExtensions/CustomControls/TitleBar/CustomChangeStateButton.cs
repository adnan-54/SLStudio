﻿using SLStudio.Extensions.Enums;
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

            string fontName = "Segoe MDL2 Assets";
            float fontSize = 9;

            using (Font fontTester = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel))
            {
                if (fontTester.Name == fontName)
                {
                    return;
                }
                else
                {
                    MessageBox.Show("n tem instalado essa merda");
                }
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
            
            this.toolTip.SetToolTip(this.icon, toolTipString);
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
                    icon.Text = Char.ConvertFromUtf32(0xE922);
                    toolTipString = Resources.Messages.Global.maximize;
                    parent.WindowState = FormWindowState.Normal;
                }
                else
                {
                    icon.Text = Char.ConvertFromUtf32(0xE923);
                    toolTipString = Resources.Messages.Global.restore;
                    parent.WindowState = FormWindowState.Maximized;
                }

                UpdateLanguage();
            }
        }
    }
}

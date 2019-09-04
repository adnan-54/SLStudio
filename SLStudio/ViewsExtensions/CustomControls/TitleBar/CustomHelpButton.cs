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
    public partial class CustomHelpButton : UserControl, IThemedControl, IMultiLanguageControl
    {
        private CustomBorderLessForm parentForm;
        public CustomBorderLessForm ParentForm_ { get => parentForm; set => parentForm = value; }

        public CustomHelpButton()
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

            glyph.BackColor = theme.theme;
            glyph.ForeColor = theme.font;
        }
        public void UpdateLanguage()
        {
            this.toolTip.SetToolTip(this.glyph, Resources.Messages.Global.help);
        }
        #endregion IThemedControl, IMultiLanguageControl

        private void OnClick(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Transition.run(glyph, "BackColor", theme.selection, new TransitionType_Linear(120));
            }
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Transition.run(glyph, "BackColor", theme.themeLight, new TransitionType_Linear(120));
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Transition.run(glyph, "BackColor", theme.theme, new TransitionType_Linear(120));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Transition.run(glyph, "BackColor", theme.themeLight, new TransitionType_Linear(120));
            }
            else
            if (e.Button == MouseButtons.Right)
            {
                ParentForm_.ShowSystemMenu(e.Button);
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            string fontName = "Segoe MDL2 Assets";
            float fontSize = 9.0f;

            using (Font fontTester = new Font(fontName, fontSize, GraphicsUnit.Pixel))
            {
                if (fontTester.Name == fontName)
                {
                    var segoeMdl2Assets = new Font("Segoe MDL2 Assets", 9.0f);
                    glyph.Font = segoeMdl2Assets;

                    glyph.Text = Char.ConvertFromUtf32(0xE897);
                }
                else
                {
                    var marlett = new Font("Marlett", 12.0f);
                    glyph.Font = marlett;

                    glyph.Text = "s";
                }
            }
        }
    }
}

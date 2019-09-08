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
    public partial class CustomCloseButton : UserControl, IThemedControl, IMultiLanguageControl
    {
        private CustomBorderLessForm parentForm;
        public CustomBorderLessForm ParentForm_ { get => parentForm; set => parentForm = value; }

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

            glyph.BackColor = theme.theme;
            glyph.ForeColor = theme.font;
        }
        public void UpdateLanguage()
        {
            this.toolTip.SetToolTip(this.glyph, Resources.Messages.Global.close);
        }
        #endregion IThemedControl, IMultiLanguageControl

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Transition.run(glyph, "BackColor", theme.error, new TransitionType_Linear(120));
            Transition.run(glyph, "ForeColor", theme.fontLight, new TransitionType_Linear(120));
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Transition.run(glyph, "BackColor", theme.theme, new TransitionType_Linear(120));
            Transition.run(glyph, "ForeColor", theme.font, new TransitionType_Linear(120));
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Transition.run(glyph, "BackColor", theme.selection, new TransitionType_Linear(120));
                Transition.run(glyph, "ForeColor", theme.fontLight, new TransitionType_Linear(120));
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Transition.run(glyph, "BackColor", theme.error, new TransitionType_Linear(120));
                Transition.run(glyph, "ForeColor", theme.fontLight, new TransitionType_Linear(120));
            }
            else
            if (e.Button == MouseButtons.Right)
            {
                ParentForm_.ShowSystemMenu(e.Button);
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ParentForm_ != null)
            {
                this.Dispose();
                ParentForm_.Close();
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

                    glyph.Text = Char.ConvertFromUtf32(0xE8BB);
                }
                else
                {
                    var marlett = new Font("Marlett", 12.0f);
                    glyph.Font = marlett;

                    glyph.Text = "r";
                }
            }

            if (ParentForm_ != null)
            {
                ParentForm_.Activated += (s, args) => glyph.ForeColor = theme.font;
                ParentForm_.Deactivate += (s, args) => glyph.ForeColor = theme.fontDark;
            }
        }
    }
}
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
    public partial class CustomChangeStateButton : UserControl, IThemedControl, IMultiLanguageControl
    {
        private CustomBorderLessForm parentForm;
        public CustomBorderLessForm ParentForm_ { get => parentForm; set => parentForm = value; }

        private string maximizeIcon = Char.ConvertFromUtf32(0xE922);
        private string restoreIcon = Char.ConvertFromUtf32(0xE923);
        private string toolTipString = Resources.Messages.Global.maximize;

        private FormWindowState oldParentState;

        private Timer refreshGlyphTimer;

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
        public Theme Theme { get => theme; set => theme = value; }

        public void UpdateTheme()
        {
            this.BackColor = theme.theme;
            this.ForeColor = theme.font;
        }

        public void UpdateLanguage()
        {
            this.toolTip.SetToolTip(this.glyph, toolTipString);
        }
        #endregion IThemedControl, IMultiLanguageControl

        private void UpdateState()
        {
            if (ParentForm_ != null)
            {
                if (ParentForm_.WindowState == FormWindowState.Maximized)
                    ParentForm_.WindowState = FormWindowState.Normal;
                else
                    ParentForm_.WindowState = FormWindowState.Maximized;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Transition.run(glyph, "BackColor", theme.selection, new TransitionType_Linear(120));
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
                Transition.run(glyph, "BackColor", theme.themeLight, new TransitionType_Linear(120));
            else
            if (e.Button == MouseButtons.Right)
                ParentForm_.ShowSystemMenu(e.Button);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                UpdateState();
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

                    maximizeIcon = Char.ConvertFromUtf32(0xE922);
                    restoreIcon = Char.ConvertFromUtf32(0xE923);
                }
                else
                {
                    var marlett = new Font("Marlett", 12.0f);
                    glyph.Font = marlett;

                    maximizeIcon = "1";
                    restoreIcon = "2";
                }
            }

            refreshGlyphTimer = new Timer();
            refreshGlyphTimer.Interval = 100;
            refreshGlyphTimer.Start();
            refreshGlyphTimer.Tick += RefreshGlyphTimerOnTick;

            if (ParentForm_ != null)
            {
                ParentForm_.Activated += (s, args) => glyph.ForeColor = theme.font;
                ParentForm_.Deactivate += (s, args) => glyph. ForeColor = theme.fontDark;
            }
        }

        private void RefreshGlyphTimerOnTick(object sender, EventArgs e)
        {
            if (ParentForm_ != null && ParentForm_.WindowState != oldParentState)
            {
                oldParentState = ParentForm_.WindowState;

                if (ParentForm_.WindowState == FormWindowState.Maximized)
                {
                    toolTipString = Resources.Messages.Global.restore;
                    glyph.Text = restoreIcon;
                }
                else
                {
                    toolTipString = Resources.Messages.Global.maximize;
                    glyph.Text = maximizeIcon;
                }

                UpdateLanguage();
            }
        }
    }
}
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

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomTitleBar : UserControl, IThemedControl
    {
        private Theme theme = new Theme(Extensions.Enums.DefaultThemes.UserDefault);
        private DateTime titleClickTime = DateTime.MinValue;
        private Point titleClickPosition = Point.Empty;

        public CustomTitleBar()
        {
            InitializeComponent();

            UpdateTheme();
            ThemeManager.AddControl(this);
        }

        public Theme Theme { get => theme; set => theme = value; }

        public CustomBorderLessForm ParentForm_ { get; set; }

        public string TitleText
        {
            get { return title.Text; }
            set { title.Text = value; }
        }

        public ContentAlignment TitleAlignment
        {
            get { return title.TextAlign; }
            set { title.TextAlign = value; }
        }

        public Font TextFont
        {
            get { return title.Font; }
            set { title.Font = value; }
        }

        public void UpdateTheme()
        {
            this.BackColor = theme.theme;
            this.ForeColor = theme.font;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (ParentForm_ != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    ParentForm_.ShowSystemMenu(e.Button);
                }
                else
                if (e.Button == MouseButtons.Middle)
                {
                    ParentForm_.Close();
                    this.Dispose();
                }
            }
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ParentForm_ != null)
            {
                if (ParentForm_.WindowState == FormWindowState.Maximized)
                    ParentForm_.WindowState = FormWindowState.Normal;
                else
                    ParentForm_.WindowState = FormWindowState.Maximized;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (ParentForm_ != null)
            {
                ParentForm_.TextChanged += (s, args) => TitleText = ParentForm_.Text;
                ParentForm_.Activated += (s, args) => this.ForeColor = theme.font;
                ParentForm_.Deactivate += (s, args) => this.ForeColor = theme.fontDark;
            }
        }
        
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ParentForm_ != null)
            {
                var clickTime = (DateTime.Now - titleClickTime).TotalMilliseconds;
                if (clickTime < SystemInformation.DoubleClickTime && e.Location == titleClickPosition)
                {
                    return;
                }
                else
                {
                    titleClickTime = DateTime.Now;
                    titleClickPosition = e.Location;
                    ParentForm_.DecorationMouseDown(HitTestValues.HTCAPTION);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Themes
{
    public class Theme
    {
        public Theme() { }

        public Theme(string path)
        {

        }

        public string ThemeName { get; set; }
        public string ThemeDescription { get; set; }
        public string ThemeAuthor { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        //Backstage
        public Color BackstageButtonBackground { get; set; }
        public Color BackstageDelimiter { get; set; }
        public Color BackstageEditorBackground { get; set; }
        public Color BackstageFocused { get; set; }
        public Color BackstageForeground { get; set; }
        public Color BackstageHoverBackground { get; set; }
        public Color BackstageSelectionBackground { get; set; }
        public Color BackstageWindowBackground { get; set; }

        //Button
        public Color ButtonBackground { get; set; }

        //Control
        public Color ControlBackground { get; set; }
        public Color ControlForeground { get; set; }
        public Color ControlHoverBackground { get; set; }
        public Color ControlHoverForeground { get; set; }
        public Color ControlSelectionBackground { get; set; }
        public Color ControlSelectionForeground { get; set; }

        //Custom
        public Color CustomBlue { get; set; }
        public Color CustomGreen { get; set; }
        public Color CustomRed { get; set; }

        //Editor
        public Color EditorBackground { get; set; }
        public Color EditorDelimiter { get; set; }

        //General
        public Color Background { get; set; }
        public Color Border { get; set; }
        public Color Delimiter { get; set; }
        public Color Focused { get; set; }
        public Color Foreground { get; set; }
        public Color HoverBackground { get; set; }
        public Color HoverBorder { get; set; }
        public Color HoverForeground { get; set; }
        public Color SelectionBackground { get; set; }
        public Color SelectionBorder { get; set; }
        public Color SelectionForeground { get; set; }

        //Window
        public Color WindowBackground { get; set; }
        public Color WindowCloseButtonHoverBackground { get; set; }
        public Color WindowCloseButtonSelectionBackground { get; set; }
        public Color WindowHeaderButtonHoverBackground { get; set; }
        public Color WindowHeaderButtonSelectionBackground { get; set; }

    }
}

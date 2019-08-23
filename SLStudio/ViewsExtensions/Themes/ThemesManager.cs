using SLStudio.ViewsExtensions.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SLStudio.Views.Themes
{
    public class ThemesManager
    {
        public List<Theme> AvaliableThemes;
        public Theme currentTheme;

        public ThemesManager()
        {

        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public ThemesManager Load(string themeName)
        {
            throw new NotImplementedException();
        }

        public ThemesManager LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(ThemesManager theme)
        {
            throw new NotImplementedException();
        }

        public void Export(ThemesManager theme)
        {
            throw new NotImplementedException();
        }

        public ThemesManager Import(string path)
        {
            throw new NotImplementedException();
        }

        public ThemesManager CreateNew()
        {
            throw new NotImplementedException();
        }

        public void Delete(string name)
        {
            throw new NotImplementedException();
        }

        public void Refresh(Form parent)
        {
            try
            {
                foreach (Control children in parent.Controls)
                {
                    if (children.HasChildren)
                        Refresh(children);
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private void Refresh(Control parent)
        {
            try
            {
                foreach (Control children in parent.Controls)
                {
                    if (children.HasChildren)
                        Refresh(children);
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}

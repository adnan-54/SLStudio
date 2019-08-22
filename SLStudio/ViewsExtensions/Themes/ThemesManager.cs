using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SLStudio.Views.Themes
{
    public class ThemesManager
    {
        public string name;
        public List<ThemesManager> themes;

        public Color styleTheme;
        public Color styleBase;
        public Color styleSelected;

        public ThemesManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            try
            {
                this.styleTheme = ViewsExtensions.Themes.DefaultTheme.Default.styleTheme;
                this.styleBase = ViewsExtensions.Themes.DefaultTheme.Default.styleBase;
                this.styleSelected = ViewsExtensions.Themes.DefaultTheme.Default.styleSelected;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }
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

        public void Refresh(Form form)
        {
            throw new NotImplementedException();
        }
    }
}

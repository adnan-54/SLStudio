using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presenters
{
    public static class Project
    {
        public static void Save(Models.Project project)
        {
            try
            {
                Models.Project projeto = project;
                projeto.Save();
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                throw ex;
            }
        }

        public static Models.Project Open()
        {
            try
            {
                string path;

                using(OpenFileDialog openFileDialog = new OpenFileDialog() {Multiselect = false, Filter = "SLStudio projects (*.sls) |*.sls"})
                {
                    openFileDialog.ShowDialog();
                    path = openFileDialog.FileName;
                }

                return new Models.Project().Open(path);
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                throw ex;
            }
        }

        public static Models.Project CreateNew(string name, string path)
        {
            try
            {
                return new Models.Project().CreateNew(name, path + "\\");
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                throw ex;
            }
        }
    }
}

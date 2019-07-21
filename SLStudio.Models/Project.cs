using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Project
    {
        public string name;
        public string path;

        public DateTime date;

        public List<Directory> directories = new List<Directory>();

        public Project() { }

        public Project(string name, string path)
        {
            this.name = name;
            this.path = path;
            this.date = DateTime.Now;
        }

        public void Save()
        {
            try
            {
                /*if (!System.IO. Directory.Exists(logPath))
                {
                    System.IO.Directory.CreateDirectory(logPath);
                }*/

                if (!System.IO.File.Exists(path))
                {
                    FileStream fs = System.IO.File.Create(path);
                    fs.Close();
                }

                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Project Open(string path)
        {
            try
            {
                string file = System.IO.File.ReadAllText(path);
                return JsonConvert.DeserializeObject<Project>(file);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Project CreateNew(string name, string path)
        {
            try
            {
                Project project = new Project(name, path);

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                if (!System.IO.File.Exists(name + ".sls"))
                {
                    FileStream fs = System.IO.File.Create(name + ".sls");
                    fs.Close();
                }

                project.Save();

                return project;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Solution : IDisposable
    {
        private string name;
        private string title;
        private string description;
        private string author;

        private DateTime creationDate;
        private DateTime lastModified;

        private List<string> files;

        public Solution(string name, string title, string description, string author)
        {
            this.name = name;
            this.title = title;
            this.description = description;
            this.author = author;
        }

        public string GetName()
        {
            return name;
        }

        public void Create(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = path + "\\" + name;

                Directory.CreateDirectory(path);

                this.creationDate = this.lastModified = DateTime.Now;

                File.WriteAllText(path + ".sls", JsonConvert.SerializeObject(this));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                name = null;
                title = null;
                description = null;
                author = null;
                files.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Solution()
        {
            Dispose(false);
        }
    }
}

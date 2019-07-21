using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Directory
    {
        public string name;
        public string path;

        public List<Directory> directories = new List<Directory>();
        public List<File> files = new List<File>();
    }
}

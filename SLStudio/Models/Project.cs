using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public class Project
    {
        public string projectName;
        public string projectDescription;
        public List<string> projectAuthors = new List<string>();

        public string projectFileName;

        public string projectVersion;
        public int projectBuild;

        public bool projectIsReleased;

        public DateTime projectCreationDate;
        public DateTime projectLastEditDate;

        public Project()
        {
        
        }
    }
}

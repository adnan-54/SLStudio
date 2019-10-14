using System.Collections.Generic;

namespace SLStudio.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string Directory { get; set; }

        public List<Project> ExternalLinks { get; set; }

        public Project()
        {

        }
    }
}

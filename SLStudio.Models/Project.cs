using System.Collections.Generic;

namespace SLStudio.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Authors { get; set; }
        public List<Rpk> Rpks { get; set; }

        public Project()
        {

        }
    }
}

using System.Collections.Generic;

namespace SLStudio.Models
{
    public class Solution
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Authors { get; set; }
        public List<Project> Rpks { get; set; }

        public Solution()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public class Solution
    {
        public List<Project> solutionProjects = new List<Project>();

        public string solutionName;
        public string solutionDescription;
        public List<string> authors = new List<string>();

        public DateTime solutionCreationDate;
        public DateTime solutionLastEditDate; 

        public Solution()
        {
        
        }
    }
}

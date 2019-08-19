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
        public List<string> solutionAuthors = new List<string>();

        public string solutionFileName;

        public string solutionVersion;
        public int solutionBuild;

        public bool solutionIsReleased;

        public DateTime solutionCreationDate;
        public DateTime solutionLastEditDate; 

        public Solution()
        {
        
        }

        public void Create()
        {
            try
            {

            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }
        }
    }
}

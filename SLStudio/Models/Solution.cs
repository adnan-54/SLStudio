using SLStudio.Extensions.Enums;
using SLStudio.Util;
using System;
using System.Collections.Generic;

namespace SLStudio.Models
{
    public class Solution
    {
        public List<Project> solutionProjects = new List<Project>();

        public string solutionName;
        public string solutionDescription;
        public List<string> solutionAuthors = new List<string>();

        public SolutionTargetVersion solutionTargetVersion;

        public string solutionFileName;

        public string solutionVersion;
        public int solutionBuild;

        public bool solutionIsReleased;

        public DateTime solutionCreationDate;
        public DateTime solutionLastEditDate; 

        public Solution()
        {
        
        }

        public Solution Create()
        {
            try
            {
                Solution solution = new Solution();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                ShowMessage.Error(ex);
            }

            return new Solution();
        }
    }
}

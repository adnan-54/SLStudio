using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
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

        public Solution Create(string name, string description, List<string> authors, string fileName, SolutionTargetVersion targetVersion)
        {
            try
            {
                Solution solution = new Solution();

                solution.solutionCreationDate = DateTime.Now;
                solution.solutionName = name;
                solution.solutionDescription = description;
                solution.solutionAuthors = authors;
                solution.solutionFileName = fileName;
                solution.solutionTargetVersion = targetVersion;
                solution.solutionLastEditDate = DateTime.Now;

                return solution;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                ShowMessage.Error(ex);
                return new Solution();
            }
        }

        public bool Save(Solution solution, string path)
        {
            try
            {

                return true;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                ShowMessage.Error(ex);

                return false;
            }
        }
    }
}

using SLStudio.Extensions.Enums;
using SLStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Extensions.Interfaces
{
    public interface ISolution
    {
        List<Project> SolutionProjects { get; set; }

        string SolutionName { get; set; }
        string SolutionDescription { get; set; }
        List<string> SolutionAuthors { get; set; }

        SolutionTargetVersion SolutionTargetVersion { get; set; }

        string SolutionFileName { get; set; }

        string SolutionVersion { get; set; }
        int SolutionBuild { get; set; }

        bool SolutionIsReleased { get; set; }

        DateTime SolutionCreationDate { get; set; }
        DateTime SolutionLastEditDate { get; set; }

    }
}

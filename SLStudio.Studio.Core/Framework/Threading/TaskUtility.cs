using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Framework.Threading
{
    public class TaskUtility
    {
        public static readonly Task Completed = Task.FromResult(true);
    }
}
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IConsole
    {
        void AppendLine(string sender, string message);
        void Clear();
        void ToggleTextWrapping();
    }
}

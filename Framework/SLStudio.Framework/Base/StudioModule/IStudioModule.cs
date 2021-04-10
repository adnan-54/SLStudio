using System.Threading.Tasks;

namespace SLStudio
{
    public interface IStudioModule : IHaveName
    {
        string Author { get; }
        int Priority { get; }
        bool ShouldBeLoaded { get; }
        bool IsLoaded { get; }

        void RegisterModule(IModuleRegister register);
    }
}
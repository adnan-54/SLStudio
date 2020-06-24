using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IAssemblyLoader
    {
        void Initialize(ISplashScreen splashScreen);

        Task Load();
    }
}
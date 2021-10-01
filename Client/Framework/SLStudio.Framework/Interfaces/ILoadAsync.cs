using System;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface ILoadAsync
    {
        event EventHandler Loaded;

        bool IsLoaded { get; }

        Task LoadAsync();
    }
}
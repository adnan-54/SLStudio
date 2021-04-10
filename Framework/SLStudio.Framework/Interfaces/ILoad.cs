using System;

namespace SLStudio
{
    public interface ILoad
    {
        event EventHandler Loaded;

        bool IsLoaded { get; }

        void Load();
    }
}
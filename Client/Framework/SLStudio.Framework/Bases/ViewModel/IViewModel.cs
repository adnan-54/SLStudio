using System;

namespace SLStudio
{
    public interface IViewModel
    {
        event EventHandler Loaded;

        event EventHandler Unloaded;
    }
}
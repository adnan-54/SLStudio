using System;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio
{
    public interface IApplicationInfo : IService
    {
        Application Application { get; }

        Dispatcher Dispatcher { get; }

        Window CurrentWindow { get; }

        void MergeResource(Uri uri);
    }
}
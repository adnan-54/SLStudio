using System;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio
{
    public interface IApplicationInfo
    {
        Application Application { get; }

        Dispatcher Dispatcher { get; }

        Window MainWindow { get; }

        Window CurrentWindow { get; }

        void MergeResource(Uri uri);
    }
}
﻿namespace SLStudio.Studio.Core.Modules.Output.Views
{
    public interface IOutputView
    {
        void Clear();
        void ScrollToEnd();
        void AppendText(string text);
        void SetText(string text);
    }
}

﻿using System.Windows.Input;

namespace SLStudio
{
    public interface IMenuButton : IMenuItem
    {
        KeyGesture Shortcut { get; }

        ICommand Command { get; }

        bool IsExecuting { get; }

        bool CanExecute { get; }

        void Execute();
    }
}
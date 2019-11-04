﻿using SLStudio.Studio.Core.Framework.Commands;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using SLStudio.Studio.Core.Properties;

namespace SLStudio.Studio.Core.Modules.Shell.Commands
{
    [CommandDefinition]
    public class ViewFullScreenCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.FullScreen";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.ViewFullScreenCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.ViewFullScreenCommandToolTip; }
        }

        public override Uri IconSource
        {
            get { return new Uri("pack://application:,,,/SLStudio.Studio.Core;component/Resources/Icons/FullScreen.png"); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<ViewFullScreenCommandDefinition>(new KeyGesture(Key.Enter, ModifierKeys.Shift | ModifierKeys.Alt));
    }
}
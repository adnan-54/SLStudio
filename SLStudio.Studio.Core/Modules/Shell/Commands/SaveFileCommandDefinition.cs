﻿using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Properties;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Modules.Shell.Commands
{
    [CommandDefinition]
    public class SaveFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.SaveFile";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.FileSaveCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.FileSaveCommandToolTip; }
        }

        public override Uri IconSource
        {
            get { return new Uri("pack://application:,,,/SLStudio.Studio.Core;component/Resources/Icons/Save.png"); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture(Key.S, ModifierKeys.Control));
    }
}
using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Properties;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Modules.UndoRedo.Commands
{
    [CommandDefinition]
    public class UndoCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.Undo";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.EditUndoCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.EditUndoCommandToolTip; }
        }

        public override Uri IconSource
        {
            get { return new Uri("pack://application:,,,/SLStudio.Studio.Core;component/Resources/Icons/Undo.png"); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<UndoCommandDefinition>(new KeyGesture(Key.Z, ModifierKeys.Control));
    }
}
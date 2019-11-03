namespace SLStudio.Studio.Core.Framework.Commands
{
    public class ExcludeCommandKeyboardShortcut
    {
        public CommandKeyboardShortcut KeyboardShortcut { get; }

        public ExcludeCommandKeyboardShortcut(CommandKeyboardShortcut keyboardShortcut)
        {
            KeyboardShortcut = keyboardShortcut;
        }
    }
}
﻿using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Framework.Commands
{
    [Export(typeof(ICommandKeyGestureService))]
    public class CommandKeyGestureService : ICommandKeyGestureService
    {
        private readonly CommandKeyboardShortcut[] _keyboardShortcuts;
        private readonly ICommandService _commandService;

        [ImportingConstructor]
        public CommandKeyGestureService(
            [ImportMany] CommandKeyboardShortcut[] keyboardShortcuts,
            [ImportMany] ExcludeCommandKeyboardShortcut[] excludeKeyboardShortcuts,
            ICommandService commandService)
        {
            _keyboardShortcuts = keyboardShortcuts
                .Except(excludeKeyboardShortcuts.Select(x => x.KeyboardShortcut))
                .OrderBy(x => x.SortOrder)
                .ToArray();
            _commandService = commandService;
        }

        public void BindKeyGestures(UIElement uiElement)
        {
            foreach (var keyboardShortcut in _keyboardShortcuts)
                if (keyboardShortcut.KeyGesture != null)
                    uiElement.InputBindings.Add(new InputBinding(
                        _commandService.GetTargetableCommand(_commandService.GetCommand(keyboardShortcut.CommandDefinition)),
                        keyboardShortcut.KeyGesture));
        }

        public KeyGesture GetPrimaryKeyGesture(CommandDefinitionBase commandDefinition)
        {
            var keyboardShortcut = _keyboardShortcuts.FirstOrDefault(x => x.CommandDefinition == commandDefinition);
            return keyboardShortcut?.KeyGesture;
        }
    }
}
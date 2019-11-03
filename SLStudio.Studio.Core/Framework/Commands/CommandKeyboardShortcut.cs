﻿using Caliburn.Micro;
using System;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Framework.Commands
{
    public abstract class CommandKeyboardShortcut
    {
        private readonly Func<CommandDefinitionBase> _commandDefinition;
        private readonly KeyGesture _keyGesture;
        private readonly int _sortOrder;

        public CommandDefinitionBase CommandDefinition
        {
            get { return _commandDefinition(); }
        }

        public KeyGesture KeyGesture
        {
            get { return _keyGesture; }
        }

        public int SortOrder
        {
            get { return _sortOrder; }
        }

        protected CommandKeyboardShortcut(KeyGesture keyGesture, int sortOrder, Func<CommandDefinitionBase> commandDefinition)
        {
            _commandDefinition = commandDefinition;
            _keyGesture = keyGesture;
            _sortOrder = sortOrder;
        }
    }

    public class CommandKeyboardShortcut<TCommandDefinition> : CommandKeyboardShortcut
        where TCommandDefinition : CommandDefinition
    {
        public CommandKeyboardShortcut(KeyGesture keyGesture, int sortOrder = 5)
            : base(keyGesture, sortOrder, () => IoC.Get<ICommandService>().GetCommandDefinition(typeof(TCommandDefinition)))
        {
            
        }
    }
}
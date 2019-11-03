using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace SLStudio.Studio.Core.Framework.Commands
{
    [Export(typeof(ICommandService))]
    public class CommandService : ICommandService
    {
        private readonly Dictionary<Type, CommandDefinitionBase> _commandDefinitionsLookup;
        private readonly Dictionary<CommandDefinitionBase, Command> _commands;
        private readonly Dictionary<Command, TargetableCommand> _targetableCommands;

        [ImportMany]
        private CommandDefinitionBase[] _commandDefinitions;

        public CommandService()
        {
            _commandDefinitionsLookup = new Dictionary<Type, CommandDefinitionBase>();
            _commands = new Dictionary<CommandDefinitionBase, Command>();
            _targetableCommands = new Dictionary<Command, TargetableCommand>();
        }

        public CommandDefinitionBase GetCommandDefinition(Type commandDefinitionType)
        {
            if (!_commandDefinitionsLookup.TryGetValue(commandDefinitionType, out CommandDefinitionBase commandDefinition))
                commandDefinition = _commandDefinitionsLookup[commandDefinitionType] =
                    _commandDefinitions.First(x => x.GetType() == commandDefinitionType);
            return commandDefinition;
        }

        public Command GetCommand(CommandDefinitionBase commandDefinition)
        {
            if (!_commands.TryGetValue(commandDefinition, out Command command))
                command = _commands[commandDefinition] = new Command(commandDefinition);
            return command;
        }

        public TargetableCommand GetTargetableCommand(Command command)
        {
            if (!_targetableCommands.TryGetValue(command, out TargetableCommand targetableCommand))
                targetableCommand = _targetableCommands[command] = new TargetableCommand(command);
            return targetableCommand;
        }
    }
}
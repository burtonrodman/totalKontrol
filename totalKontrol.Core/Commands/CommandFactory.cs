using System;
using System.Collections.Generic;
using System.Linq;

namespace totalKontrol.Core.Commands
{
    public static class CommandFactory
    {
        private static Dictionary<string, Type> _commands;

        private static void InitializeCommands()
        {
            if (_commands is null)
            {
                var asm = typeof(ICommand).Assembly;
                _commands = asm.GetTypes()
                    .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                    .ToDictionary(c => c.Name);
            }
        }

        public static ICommand Create(string name, IDeviceLocator deviceLocator)
        {
            InitializeCommands();

            if (_commands.TryGetValue(name, out var _commandType) ||
                _commands.TryGetValue(name + "Command", out _commandType))
            {
                var commandInstance = Activator.CreateInstance(_commandType) as ICommand;
                if (commandInstance != null)
                {
                    commandInstance.Initialize(deviceLocator);
                }
                return commandInstance;
            }
            return null;
        }
    }
}

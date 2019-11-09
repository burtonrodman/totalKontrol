using System;
using System.Collections.Generic;
using System.Linq;
using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public static class CommandFactory
    {
        private static Dictionary<string, Type> _commands;

        private static object _controllerCommandsLock = new object();
        private static Dictionary<int, ICommand> _controllerCommands;

        private static void InitializeCommands()
        {
            if (_commands is null)
            {
                var asm = typeof(ICommand).Assembly;
                _commands = asm.GetTypes()
                    .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                    .ToDictionary(c => c.Name);

                _controllerCommands = new Dictionary<int, ICommand>();
            }
        }

        public static ICommand Create(string name, Control control, ControlGroup controlGroup, MidiController midiController, ILogger logger, IDeviceLocator deviceLocator)
        {
            lock (_controllerCommandsLock)
            {
                InitializeCommands();

                if (_controllerCommands.TryGetValue(control.Controller, out var commandInstance))
                {
                    return commandInstance;
                }
                else
                {
                    if (_commands.TryGetValue(name, out var _commandType) ||
                        _commands.TryGetValue(name + "Command", out _commandType))
                    {
                        commandInstance = Activator.CreateInstance(_commandType) as ICommand;
                        if (commandInstance != null)
                        {
                            commandInstance.Initialize(control, controlGroup, midiController, logger, deviceLocator);
                        }
                        _controllerCommands.Add(control.Controller, commandInstance);
                        return commandInstance;
                    }
                }
                return null;
            }
        }
    }
}

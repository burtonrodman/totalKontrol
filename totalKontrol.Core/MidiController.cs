using NAudio.Midi;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using totalKontrol.Core.Commands;
using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core
{
    public class MidiController : IDisposable
    {
        private readonly string _definitionPath;
        private readonly string _profilePath;
        private readonly ILogger _logger;
        private readonly IDeviceLocator _deviceLocator;
        private MidiOut _midiOut;
        private MidiIn _midiIn;
        private ControllerDefinition _controllerDef;
        private UserProfile _userProfile;
        private ConcurrentQueue<MidiInMessageEventArgs> _eventQueue;
        private Timer _timer;

        public MidiController(string definitionPath, string profilePath, ILogger logger, IDeviceLocator deviceLocator)
        {
            _eventQueue = new ConcurrentQueue<MidiInMessageEventArgs>();
            _definitionPath = definitionPath;
            _profilePath = profilePath;
            _logger = logger;
            _deviceLocator = deviceLocator;
        }

        public void Start()
        {
            _controllerDef = JsonConvert.DeserializeObject<ControllerDefinition>(
                File.ReadAllText(_definitionPath));
            _userProfile = JsonConvert.DeserializeObject<UserProfile>(
                File.ReadAllText(_profilePath));

            _midiIn = FindMidiIn(_controllerDef.MidiInName);
            _midiOut = FindMidiOut(_controllerDef.MidiOutName);
            _midiIn.MessageReceived += MidiIn_MessageReceived;
            _midiIn.ErrorReceived += MidiIn_ErrorReceived;
            _midiIn.Start();
        }

        private void StartTimer()
        {
            if (_timer is null)
            {
                _timer = new Timer(TimerCallback, null, 250, 250);
            }
        }

        internal void SendControlChange(int controller, int value)
        {
            var midiEvent = new ControlChangeEvent(0, _controllerDef.Channel, (NAudio.Midi.MidiController)controller, value);
            _midiOut.Send(midiEvent.GetAsShortMessage());
        }

        private void StopTimer()
        {
            _timer.Dispose();
            _timer = null;
        }

        private void TimerCallback(object state)
        {

            StopTimer();

            var commandsToExecute = new Dictionary<int, ICommand>();
            while (_eventQueue.TryDequeue(out var e))
            {
                if (e.MidiEvent is ControlChangeEvent cce)
                {
                    var control = _controllerDef.Controls.FirstOrDefault(c => c.Controller == (int)cce.Controller);
                    if (control != null)
                    {
                        var command = HandleButtonEvent(control, cce.ControllerValue);
                        if (command != null && !commandsToExecute.ContainsKey(control.Controller))
                        {
                            commandsToExecute.Add(control.Controller, command);
                        }
                    }
                }
                else
                {
                    _logger.WriteLine($"Message 0x{e.RawMessage:X8} Event {e.MidiEvent}");
                }
            }

            foreach (var command in commandsToExecute.Values)
            {
                command.ExecuteCommand();
            }

        }

        public ICommand HandleButtonEvent(Control control, int value)
        {
            var controlGroup = GetControlGroup(control.Name);
            foreach (var mapping in _userProfile.CommandMappings)
            {
                if (Regex.IsMatch(control.Name, mapping.ControlName))
                {
                    var command = CommandFactory.Create(mapping.Command, control, controlGroup, this, _logger, _deviceLocator);
                    if (command?.ProcessEvent(value) ?? false)
                    {
                        return command;
                    }
                }
            }
            return null;
        }

        private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            _eventQueue.Enqueue(e);
            StartTimer();
        }


        private ControlGroup GetControlGroup(string controlName)
        {
            var controlGroupDef = _controllerDef.ControlGroups.FirstOrDefault(g => g.ControlNames.Contains(controlName));
            var controlGroup = _userProfile.ControlGroups.FirstOrDefault(g => g.Name == controlGroupDef?.Name);
            if (controlGroupDef != null && controlGroup is null)
            {
                controlGroup = new ControlGroup() { Name = controlGroupDef.Name };
                _userProfile.ControlGroups.Add(controlGroup);
            }
            return controlGroup;
        }

        private void MidiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            _logger.WriteLine($"Message 0x{e.RawMessage:X8} Event {e.MidiEvent}\n");
        }

        public MidiIn FindMidiIn(string name)
        {
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                if (MidiIn.DeviceInfo(i).ProductName.ToLower().Contains(name.ToLower()))
                {
                    _logger.WriteLine($@"Found MidiIn: {MidiIn.DeviceInfo(i).ProductName}");
                    return new MidiIn(i);
                }
            }
            return null;
        }

        public MidiOut FindMidiOut(string name)
        {
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                if (MidiOut.DeviceInfo(i).ProductName.ToLower().Contains(name.ToLower()))
                {
                    _logger.WriteLine($@"Assigning MidiOut: {MidiOut.DeviceInfo(i).ProductName}");
                    return new MidiOut(i);
                }
            }
            return null;
        }

        public void Dispose()
        {
            _midiIn?.Dispose();
            _midiOut?.Dispose();
            _timer?.Dispose();
        }

        public delegate void ControlChangedEventHandler(object sender, ControlChangedEventArgs e);
        public event ControlChangedEventHandler ControlChanged;
        public void OnControlChanged(ControlChangedEventArgs args)
        {
            ControlChanged?.Invoke(this, args);
        }

    }

    public class ControlChangedEventArgs : EventArgs
    {
        public ControlGroup ControlGroup { get; set; }
        public string ControlName { get; set; }
        public int Value { get; set; }
        public bool IsPressed { get; set; }
    }
}

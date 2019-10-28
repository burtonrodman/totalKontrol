using NAudio.Midi;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public MidiController(string definitionPath, string profilePath, ILogger logger, IDeviceLocator deviceLocator)
        {
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

        private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            if (e.MidiEvent is ControlChangeEvent cce)
            {
                foreach (var control in _controllerDef.Controls)
                {
                    if (control.Controller == (int)cce.Controller)
                    {
                        HandlerResult result = null;
                        switch (control.Type)
                        {
                            case "Button":
                                result = HandleButtonEvent(control, cce.ControllerValue);
                                break;
                            case "Fader":
                                result = HandleFaderEvent(control, cce.ControllerValue);
                                break;
                        }

                        if (result?.ReflectEvent ?? false)
                        {
                            _midiOut.Send(cce.GetAsShortMessage());
                        }
                        return;
                    }
                }
            }
            else
            {
                _logger.WriteLine($"Message 0x{e.RawMessage:X8} Event {e.MidiEvent}");
            }
        }

        public HandlerResult HandleButtonEvent(Control control, int value)
        {
            var controlGroup = GetControlGroup(control.Name);
            foreach (var mapping in _userProfile.CommandMappings)
            {
                if (Regex.IsMatch(control.Name, mapping.ControlName))
                {
                    var command = CommandFactory.Create(mapping.Command, _deviceLocator);
                    command?.Execute(value, controlGroup);
                }
            }

            switch (value)
            {
                case 0:
                    _logger.WriteLine($"button {control.Name} released");
                    OnControlChanged(new ControlChangedEventArgs() { ControlGroup = controlGroup, ControlName = control.Name, Value = value, IsIlluminated = false });
                    return new SetLightOffResult();
                case 127:
                    _logger.WriteLine($"button {control.Name} pressed");
                    OnControlChanged(new ControlChangedEventArgs() { ControlGroup = controlGroup, ControlName = control.Name, Value = value, IsIlluminated = true });
                    return new SetLightOnResult();
            }
            return null;
        }

        public HandlerResult HandleFaderEvent(Control control, int value)
        {
            var controlGroup = GetControlGroup(control.Name);
            foreach (var mapping in _userProfile.CommandMappings)
            {
                if (Regex.IsMatch(control.Name, mapping.ControlName))
                {
                    var command = CommandFactory.Create(mapping.Command, _deviceLocator);
                    command?.Execute(value, controlGroup);
                }
            }
            OnControlChanged(new ControlChangedEventArgs() { ControlGroup = controlGroup, ControlName = control.Name, Value = value });
            return null;
        }

        private ControlGroup GetControlGroup(string controlName)
        {
            var controlGroupDef = _controllerDef.ControlGroups.FirstOrDefault(g => g.ControlNames.Contains(controlName));
            var controlGroup = _userProfile.ControlGroups.FirstOrDefault(g => g.Name == controlGroupDef.Name);
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
            _midiIn.Dispose();
            _midiOut.Dispose();
        }

        public delegate void ControlChangedEventHandler(object sender, ControlChangedEventArgs e);
        public event ControlChangedEventHandler ControlChanged;
        protected void OnControlChanged(ControlChangedEventArgs args)
        {
            ControlChanged?.Invoke(this, args);
        }
    }

    public class ControlChangedEventArgs : EventArgs
    {
        public ControlGroup ControlGroup { get; set; }
        public string ControlName { get; set; }
        public int Value { get; set; }
        public bool IsIlluminated { get; set; }
    }
}

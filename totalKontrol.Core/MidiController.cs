using NAudio.Midi;
using Newtonsoft.Json;
using System;
using System.IO;
using totalKontrol.Core.Commands;

namespace totalKontrol.Core
{
    public class MidiController : IDisposable
    {
        private readonly string _definitionPath;
        private readonly string _profilePath;
        private readonly ILogger _logger;
        private MidiOut _midiOut;
        private MidiIn _midiIn;
        private ControllerDefinition _controllerDef;
        private UserProfile _userProfile;

        public MidiController(string definitionPath, string profilePath, ILogger logger)
        {
            _definitionPath = definitionPath;
            _profilePath = profilePath;
            _logger = logger;
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
            switch (value)
            {
                case 0:
                    _logger.WriteLine($"button {control.Name} released");
                    return new SetLightOffResult();
                case 127:
                    _logger.WriteLine($"button {control.Name} pressed");
                    return new SetLightOnResult();
            }
            return null;
        }

        public HandlerResult HandleFaderEvent(Control control, int value)
        {
            _logger.WriteLine($"fader {control.Name} changed to {value}");
            foreach (var mapping in _userProfile.CommandMappings)
            {
                if (mapping.ControlName == control.Name)
                {
                    var command = CommandFactory.Create(mapping.Command);
                    command.Execute(value, null);
                }
            }
            return null;
        }

        private void MidiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            var midi = e.MidiEvent;
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
    }
}

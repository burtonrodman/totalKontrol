using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class ChangeOutVolumeCommand : ICommand
    {
        private IDeviceLocator _deviceLocator;
        private ControlGroup _controlGroup;
        private int _value = 0;

        public void Initialize(Control control, ControlGroup controlGroup, MidiController midiController, ILogger logger, IDeviceLocator deviceLocator)
        {
            _deviceLocator = deviceLocator;
            _controlGroup = controlGroup;
        }

        public bool ProcessEvent(int value)
        {
            _value = value;
            return true;
        }

        public void ExecuteCommand()
        {

            if (!string.IsNullOrWhiteSpace(_controlGroup?.DeviceOrSession))
            {
                var newVolume = ((float)_value / 127.0f);
                foreach (var volumeTarget in _deviceLocator.FindVolumeOutTargetsBySubstring(_controlGroup.DeviceOrSession))
                {
                    // when we figure out the performance issue, maybe we can bring this back.
                    //if (Math.Abs(volumeTarget.GetVolume() - newVolume) < 0.05)
                    //{
                       volumeTarget.SetVolume(newVolume);
                    //}
                }
            }
        }
    }
}

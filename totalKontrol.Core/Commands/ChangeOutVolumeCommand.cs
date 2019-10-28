using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class ChangeOutVolumeCommand : ICommand
    {
        private readonly IDeviceLocator _deviceLocator;

        public ChangeOutVolumeCommand(IDeviceLocator deviceLocator)
        {
            _deviceLocator = deviceLocator;
        }

        public void Execute(int value, ControlGroup controlGroup)
        {

            if (!string.IsNullOrWhiteSpace(controlGroup?.DeviceOrSession))
            {
                var newVolume = ((float)value / 127.0f);
                foreach (var volumeTarget in _deviceLocator.FindVolumeOutTargetsBySubstring(controlGroup.DeviceOrSession))
                {
                    //if (Math.Abs(volumeTarget.GetVolume() - newVolume) < 0.05)
                    //{
                    volumeTarget.SetVolume(newVolume);
                    //}
                }
            }

        }
    }
}

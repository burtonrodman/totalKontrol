using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class ChangeOutVolumeCommand : ICommand
    {
        private IDeviceLocator _deviceLocator;

        public void Initialize(IDeviceLocator deviceLocator)
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

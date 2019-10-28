using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class MuteOutVolumeCommand : ICommand
    {
        private readonly IDeviceLocator _deviceLocator;

        public MuteOutVolumeCommand(IDeviceLocator deviceLocator)
        {
            _deviceLocator = deviceLocator;
        }

        public void Execute(int value, ControlGroup controlGroup)
        {

            //if (value == 127) // press
            //{
            //    controlGroup.IsMuted = !controlGroup.IsMuted;

            //    foreach (var volumeTarget in _deviceLocator.FindVolumeOutTargetsBySubstring(controlGroup.DeviceOrSession))
            //    {
            //        volumeTarget.SetMute(controlGroup.IsMuted);
            //    }

            //}

        }

    }
}
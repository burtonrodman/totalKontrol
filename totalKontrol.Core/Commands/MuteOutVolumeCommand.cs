using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class MuteOutVolumeCommand : ButtonCommandBase
    {
        private readonly IDeviceLocator _deviceLocator;

        public MuteOutVolumeCommand(IDeviceLocator deviceLocator)
        {
            _deviceLocator = deviceLocator;
        }

        protected override void OnPress(int value, ControlGroup controlGroup)
        {
            base.OnPress(value, controlGroup);
            if (controlGroup.DeviceOrSession == "Master")
            {
                KeyboardHelpers.SendKeyPress(KeyCode.VOLUME_MUTE);
            }
            else
            {
                base.OnPress(value, controlGroup);
                controlGroup.IsMuted = !controlGroup.IsMuted;

                foreach (var volumeTarget in _deviceLocator.FindVolumeOutTargetsBySubstring(controlGroup.DeviceOrSession))
                {
                    volumeTarget.SetMute(controlGroup.IsMuted);
                }
            }

        }
    }
}
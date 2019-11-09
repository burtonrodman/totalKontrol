using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class MuteOutVolumeCommand : ButtonCommandBase
    {
        public bool IsMuted { get; set; }

        protected override bool OnPress()
        {
            base.OnPress();
            IsMuted = !IsMuted;
            return true;
        }

        protected override void OnExecuteCommand()
        {
            base.OnExecuteCommand();

            if (ControlGroup.DeviceOrSession == "Master")
            {
                KeyboardHelpers.SendKeyPress(KeyCode.VOLUME_MUTE);
            }
            else
            {
                foreach (var volumeTarget in DeviceLocator.FindVolumeOutTargetsBySubstring(ControlGroup.DeviceOrSession))
                {
                    volumeTarget.SetMute(IsMuted);
                }
            }
        }
    }
}
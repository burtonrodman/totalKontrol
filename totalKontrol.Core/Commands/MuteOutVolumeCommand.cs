using System;
using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class MuteOutVolumeCommand : ButtonCommandBase
    {
        public bool IsMuted { get; set; }
        private DateTime _lastPress; 

        protected override (bool shouldExecute, bool isIlluminated) OnPress()
        {
            IsMuted = !IsMuted;
            _lastPress = DateTime.UtcNow;
            return (true, IsMuted);
        }

        protected override (bool shouldExecute, bool isIlluminated) OnRelease()
        {
            if (DateTime.UtcNow.Subtract(_lastPress).TotalMilliseconds > 250)
            {
                IsMuted = !IsMuted;
                return (true, IsMuted);
            }
            return (false, IsMuted);
        }

        protected override void OnExecuteCommand()
        {
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
using NAudio.CoreAudioApi;

namespace totalKontrol.Core.Commands
{
    public class MuteOutVolumeCommand : ICommand
    {
        private static AudioEndpointVolume _volume = null;

        public void Execute(int value, string[] parameters)
        {
            if (_volume is null)
            {
                using (var enumerator = new MMDeviceEnumerator())
                {
                    var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                    _volume = device.AudioEndpointVolume;
                }
            }

            if (value == 127) // press
            {
                var currentMute = _volume.Mute;
                _volume.Mute = !currentMute;
            }

            // get current mute status
            // get new must status
            // if button press
            // if button release

        }
    }
}
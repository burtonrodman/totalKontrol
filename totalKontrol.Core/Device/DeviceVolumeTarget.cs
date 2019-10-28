using NAudio.CoreAudioApi;

namespace totalKontrol.Core.Device
{
    public class DeviceVolumeTarget : IVolumeTarget
    {
        private readonly MMDevice _device;

        public DeviceVolumeTarget(MMDevice device)
        {
            _device = device;
        }

        public bool GetMute()
        {
            return _device.AudioEndpointVolume.Mute;
        }

        public float GetVolume()
        {
            return _device.AudioEndpointVolume.MasterVolumeLevelScalar;
        }

        public void SetMute(bool isMuted)
        {
            _device.AudioEndpointVolume.Mute = isMuted;
        }

        public void SetVolume(float value)
        {
            _device.AudioEndpointVolume.MasterVolumeLevelScalar = value;
        }
    }
}

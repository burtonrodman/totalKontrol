using NAudio.CoreAudioApi;

namespace totalKontrol.Core.Device
{
    public class SessionVolumeTarget : IVolumeTarget
    {
        private readonly AudioSessionControl _session;

        public SessionVolumeTarget(AudioSessionControl session)
        {
            _session = session;
        }

        public bool GetMute()
        {
            return _session.SimpleAudioVolume.Mute;
        }

        public float GetVolume()
        {
            return _session.SimpleAudioVolume.Volume;
        }

        public void SetMute(bool isMuted)
        {
            _session.SimpleAudioVolume.Mute = isMuted;
        }

        public void SetVolume(float value)
        {
            _session.SimpleAudioVolume.Volume = value;
        }
    }
}

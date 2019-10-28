namespace totalKontrol.Core.Device
{
    public interface IVolumeTarget
    {
        float GetVolume();
        void SetVolume(float value);
        bool GetMute();
        void SetMute(bool isMuted);
    }
}

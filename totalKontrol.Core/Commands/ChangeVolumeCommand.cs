using NAudio.CoreAudioApi;

namespace totalKontrol.Core.Commands
{
    public class ChangeVolumeCommand : ICommand
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

            _volume.MasterVolumeLevelScalar = ((float)value / 127.0f);
            //    foreach (var device in devices)
            //    {
            //        _logger.WriteLine(device.DeviceFriendlyName);

            //        if (device.FriendlyName.Contains("Mpow"))
            //        {
            //            device.AudioEndpointVolume.MasterVolumeLevelScalar = 0.75f;
            //        }

            //        var sessions = device.AudioSessionManager.Sessions;
            //        for (int i = 0; i < sessions.Count; i++)
            //        {
            //            var session = sessions[i];
            //            if (session.State != AudioSessionState.AudioSessionStateExpired)
            //            {
            //                _logger.WriteLine($"\t*{session.DisplayName}");

            //                if (session.IsSystemSoundsSession)
            //                {
            //                    _logger.WriteLine("\tSystem sounds");
            //                }
            //                else
            //                {
            //                    var process = Process.GetProcessById((int)session.GetProcessID);
            //                    _logger.WriteLine($"\t{process?.ProcessName}");
            //                }
            //            }
            //        }
            //    }

            //}
        }
    }
}

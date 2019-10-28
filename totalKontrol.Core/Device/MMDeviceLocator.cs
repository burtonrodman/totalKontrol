using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace totalKontrol.Core.Device
{
    public class MMDeviceLocator : IDeviceLocator, IDisposable, IMMNotificationClient
    {
        private IList<MMDevice> _deviceCache;
        private MMDeviceEnumerator _deviceEnumerator;

        public IEnumerable<IVolumeTarget> FindVolumeOutTargetsBySubstring(string substring)
        {
            if (_deviceCache is null)
            {
                _deviceCache = new List<MMDevice>();
                if (_deviceEnumerator is null)
                {
                    _deviceEnumerator = new MMDeviceEnumerator();
                    _deviceEnumerator.RegisterEndpointNotificationCallback(this);
                }
                var defaultDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                var devices = _deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                foreach (var device in devices)
                {
                    _deviceCache.Add(device);
                }
            }

            return _deviceCache
                .Where(d => d.FriendlyName.Contains(substring) || substring == "Master")
                .Select(d => new DeviceVolumeTarget(d))
                .ToList();
        }

        public IEnumerable<IVolumeTarget> FindVolumeInTargetsBySubstring(string substring)
        {
            throw new NotImplementedException();
        }


        #region " IMMNotificationClient "

        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            // throw new NotImplementedException();
        }

        public void OnDeviceAdded(string pwstrDeviceId)
        {
            _deviceCache = null;
        }

        public void OnDeviceRemoved(string deviceId)
        {
            _deviceCache = null;
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            _deviceCache = null;
        }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {
            // throw new NotImplementedException();
        }

        #endregion

        public void Dispose()
        {
            if (_deviceEnumerator != null)
            {
                _deviceEnumerator.UnregisterEndpointNotificationCallback(this);
                _deviceEnumerator.Dispose();
            }
        }
    }

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

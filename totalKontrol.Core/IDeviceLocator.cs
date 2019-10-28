using System;
using System.Collections.Generic;
using totalKontrol.Core.Device;

namespace totalKontrol.Core
{
    public interface IDeviceLocator : IDisposable
    {
        IEnumerable<IVolumeTarget> FindVolumeOutTargetsBySubstring(string substring);
        IEnumerable<IVolumeTarget> FindVolumeInTargetsBySubstring(string substring);
    }
}

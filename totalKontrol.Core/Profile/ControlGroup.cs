using System.Collections.Generic;

namespace totalKontrol.Core.Profile
{
    public class ControlGroup
    {
        public string Name { get; set; }
        public string DeviceOrSession { get; set; }

        public bool IsMuted { get; set; }
    }
}

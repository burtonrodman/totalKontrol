using System;
using System.Collections.Generic;

namespace totalKontrol.Core
{
    public class ControllerDefinition
    {
        public string MidiInName { get; set; }
        public string MidiOutName { get; set; }
        public int Channel { get; set; }
        public ICollection<Control> Controls { get; set; }
    }
}

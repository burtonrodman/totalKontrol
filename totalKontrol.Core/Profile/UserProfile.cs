using System.Collections.Generic;

namespace totalKontrol.Core.Profile
{
    public class UserProfile
    {
        public ICollection<CommandMapping> CommandMappings { get; set; }
        public ICollection<ControlGroup> ControlGroups { get; set; }
    }
}

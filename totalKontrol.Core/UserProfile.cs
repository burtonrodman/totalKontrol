using System.Collections.Generic;

namespace totalKontrol.Core
{
    public class UserProfile
    {
        public ICollection<CommandMapping> CommandMappings { get; set; }
    }
}

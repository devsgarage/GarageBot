using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingProviders.models
{
    public class CommandUsedInfo
    {
        public string Username { get; set; }
        public string CommandName { get; set; }
        public string FiredDateTime { get; set; }
        public bool InCooldown { get; set; }
    }
}

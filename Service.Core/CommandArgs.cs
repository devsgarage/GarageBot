using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class CommandArgs
    {
        public string UserName { get; set; }
        public bool IsBroadcaster { get; set; }
        public ReadOnlyMemory<char> Text { get; set; }
        public TwitchTeam Team { get; set; }
    }
}

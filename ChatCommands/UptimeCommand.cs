using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class UptimeCommand : IChatCommand
    {
        public string Command => "uptime";
        public string Description => "Displays the uptime of the stream";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(5);
             
        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            var uptime = await service.GetUptime();
            await service.SendMessage($"Streams been running for {uptime.ToString(@"hh\:mm\:ss")}");
        }
    }
}

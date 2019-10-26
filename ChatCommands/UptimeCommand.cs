using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class UptimeCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "uptime" };
        public string Description => "Displays the uptime of the stream";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(5);
             
        public async Task Execute(IChatService service, CommandArgs args)
        {
            var uptime = await service.GetUptime();
            await service.SendMessage($"Streams been running for {uptime.ToString(@"hh\:mm\:ss")}");
        }
    }
}

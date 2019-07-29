using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class AlertCommand : IChatCommand
    {
        IHubService hubService;
        public AlertCommand(IHubService hubService) 
        {
            this.hubService = hubService;
        }

        public string Command => "alert";
        public string Description => "Get broadcasters attention";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            await hubService.SendAlert(userName);
        }
    }
}

using Service.Core;
using System;
using System.Collections.Generic;
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

        public IEnumerable<string> Command => new[] { "alert" };
        public string Description => "Get broadcasters attention";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public async Task Execute(IChatService service, CommandArgs args)
        {
            await hubService.SendAlert(args.UserName);
        }
    }
}

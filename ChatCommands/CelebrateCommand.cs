using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class CelebrateCommand : IChatCommand
    {
        private IHubService hubService;
        public IEnumerable<string> Command => new[] { "celebrate" };
        public string Description => "BROADCASTER ONLY - Launch a celebration";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public CelebrateCommand(IHubService hubService)
        {
            this.hubService = hubService;
        }

        public async Task Execute(IChatService service, CommandArgs args)
        {
            if (args.IsBroadcaster)
                await hubService.SendCelebration(args.UserName);
        }
    }
}

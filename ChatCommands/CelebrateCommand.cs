using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class CelebrateCommand : IChatCommand
    {
        private IHubService hubService;
        public string Command => "celebrate";
        public string Description => "BROADCASTER ONLY - Launch a celebration";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public CelebrateCommand(IHubService hubService)
        {
            this.hubService = hubService;
        }

        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            if (isBroadcaster)
                await hubService.SendCelebration(userName);
        }
    }
}

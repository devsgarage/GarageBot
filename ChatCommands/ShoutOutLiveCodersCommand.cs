using Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class ShoutOutLiveCodersCommand : IChatCommand
    {
        IChatService chatService;
        List<string> teammates;

        public ShoutOutLiveCodersCommand(IChatService chatService)
        {
            this.chatService = chatService;
            LoadTeammates();
        }

        private async Task LoadTeammates()
        {
            var teammates = await chatService.GetTwitchTeam("livecoders");
            this.teammates = teammates?.Users?.Select(x => x.Name).ToList();
        }

        public string Command => "userjoined";

        public string Description => "BROADCASTER ONLY -> Automatic shout out to Live Coders teammates";

        public TimeSpan? Cooldown => TimeSpan.FromMilliseconds(1);

        public bool CanBeListed() => false;

        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            if (!isBroadcaster)
                return;

            var isPartOfTeam = teammates.Exists(x => x == userName);
            if (isPartOfTeam)
                await service.SendMessage($"Check out another great streamer @{userName} over on https://twitch.tv/{userName}");
        }
    }
}

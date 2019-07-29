using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class ShoutOutCommand : IChatCommand
    {
        public string Command => "so";
        public string Description => "BROADCASTER ONLY -- Gives a shout out to a fellow streamer!";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            if (isBroadcaster)
                await service.SendMessage($"Check out another great streamer @{text} over on https://twitch.tv/{text}");
        }
    }
}

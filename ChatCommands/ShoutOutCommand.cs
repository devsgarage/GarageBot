using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class ShoutOutCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "so" };
        public string Description => "BROADCASTER ONLY -- Gives a shout out to a fellow streamer!";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public async Task Execute(IChatService service, CommandArgs args)
        {
            if (args.IsBroadcaster)
                await service.SendMessage($"Check out another great streamer @{args.Text} over on https://twitch.tv/{args.Text}");
        }
    }
}

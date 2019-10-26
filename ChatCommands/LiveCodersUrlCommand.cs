using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class LiveCodersUrlCommand : IChatCommand
    {
        private const string liveCodersUrl = "https://twitch.tv/team/livecoders";
        public IEnumerable<string> Command => new[] { "livecoders" };

        public string Description => "Displays Twitch URL for Live Coders team";

        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public bool CanBeListed() => true;

        public async Task Execute(IChatService service, CommandArgs args)
        {
            await service.SendMessage($"Check out all the Live Coders at {liveCodersUrl}");
        }
    }
}

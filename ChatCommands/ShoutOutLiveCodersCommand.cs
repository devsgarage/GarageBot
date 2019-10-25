﻿using Service.Core;
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
        Task<List<string>> teammates;

        public ShoutOutLiveCodersCommand(IChatService chatService)
        {
            this.chatService = chatService;
            this.teammates = LoadTeammates();
        }

        private async Task<List<string>> LoadTeammates()
        {
            var teammates = await chatService.GetTwitchTeam(Constants.ChatService.LiveCoders);
            return teammates?.Users?.Select(x => x.Name).ToList();
        }

        public IEnumerable<string> Command => new[] { "userjoined" };

        public string Description => "BROADCASTER ONLY -> Automatic shout out to Live Coders teammates";

        public TimeSpan? Cooldown => TimeSpan.FromMilliseconds(1);

        public bool CanBeListed() => false;

        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            if (!isBroadcaster)
                return;

            var mates = await teammates;
            var isPartOfTeam = mates?.Exists(x => x == userName) ?? false;
            if (isPartOfTeam)
                await service.SendMessage($"Check out another member of the Live Coders, @{userName}, over on https://twitch.tv/{userName}");
        }
    }
}

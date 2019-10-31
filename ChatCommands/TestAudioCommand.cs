﻿using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class TestAudioCommand : IChatCommand
    {
        IHubService hubService;
       
        public TestAudioCommand(IHubService hubService)
        {
            this.hubService = hubService;
            this.hubService.Connect();
        }

        public IEnumerable<string> Command => new[] { "testaudio" };
        public string Description => "Tests the ability of the system to play audio";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(5);
        
        public async Task Execute(IChatService service, CommandArgs args)
        {
            await hubService.SendAlert(args.UserName);
        }
    }
}

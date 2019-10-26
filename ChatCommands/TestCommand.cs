using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class TestCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "test" };
        public string Description => "Tests whether or not the bot system is functioning";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        

        public async Task Execute(IChatService service, CommandArgs args)
        {
            if (args.IsBroadcaster)
                await service.SendMessage("This is a test. This station is conducting a test of the Emergency Broadcast System. This is only a test.");
        }
    }
}

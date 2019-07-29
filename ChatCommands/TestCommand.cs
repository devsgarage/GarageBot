using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class TestCommand : IChatCommand
    {
        public string Command => "test";
        public string Description => "Tests whether or not the bot system is functioning";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        

        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            if (isBroadcaster)
                await service.SendMessage("This is a test. This station is conducting a test of the Emergency Broadcast System. This is only a test.");
        }
    }
}

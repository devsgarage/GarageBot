using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class StartStreamCommand : IChatCommand
    {
        private IStreamingService streamingService;

        public string Command => "startStream";

        public string Description => "Sets the stream live flag to true";

        public TimeSpan? Cooldown => TimeSpan.FromSeconds(1);

        public bool CanBeListed() => false;

        public Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            streamingService.StartStream();
            return Task.CompletedTask;
        }

        public StartStreamCommand(IStreamingService streamingService)
        {
            this.streamingService = streamingService;
        }
    }
}

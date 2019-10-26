using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class StartStreamCommand : IChatCommand
    {
        private IStreamingService streamingService;

        public IEnumerable<string> Command => new[] { "startStream" };

        public string Description => "Sets the stream live flag to true";

        public TimeSpan? Cooldown => TimeSpan.FromSeconds(1);

        public bool CanBeListed() => false;

        public Task Execute(IChatService service, CommandArgs args)
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

using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class StopStreamCommand : IChatCommand
    {
        private IStreamingService streamingService;

        public IEnumerable<string> Command => new[] { "stopStream" };

        public string Description => "Sets the is stream live flag to false";

        public TimeSpan? Cooldown => TimeSpan.FromSeconds(1);

        public bool CanBeListed() => false;

        public Task Execute(IChatService service, CommandArgs args)
        {
            streamingService.StopStream();
            return Task.CompletedTask;
        }

        public StopStreamCommand(IStreamingService streamingService)
        {
            this.streamingService = streamingService;
        }
    }
}

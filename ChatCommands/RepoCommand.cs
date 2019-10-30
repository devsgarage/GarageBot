using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class RepoCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "repo" };
        public string Description => "Get link to GitHub repository";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        private string defaultRepo = "https://github.com/devsgarage";
        private string workingRepo;

        public Task Execute(IChatService service, CommandArgs args)
        {
            if (string.IsNullOrWhiteSpace(workingRepo))
                service.SendMessage(defaultRepo);
            else
                service.SendMessage(workingRepo);

            return Task.CompletedTask;
        }
    }
}

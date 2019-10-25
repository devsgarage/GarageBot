using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class ProjectCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "project" };
        public string Description => "Gets or Sets (BROADCASTER ONLY) the current project being worked on stream.";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(5);

        private string currentProject = "";

        public async Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            if (isBroadcaster && !text.IsEmpty)
                currentProject = text.ToString();
            if (string.IsNullOrWhiteSpace(currentProject))
                await service.SendMessage("Hey @developersgarage, what are we working on today?");
            else
                await service.SendMessage(currentProject);
        }
    }
}

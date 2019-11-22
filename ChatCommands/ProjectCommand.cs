using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ConstantAlias = Service.Core.Constants;

namespace ChatCommands
{
    ConstantAlias.ChatService ChatName = new ConstantAlias.ChatService();

    public class ProjectCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "project" };
        public string Description => "Gets or Sets (BROADCASTER ONLY) the current project being worked on stream.";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(5);

        private string currentProject = "";

        public async Task Execute(IChatService service, CommandArgs args)
        {
            if (args.IsBroadcaster && !args.Text.IsEmpty)
                currentProject = args.Text.ToString();
            if (string.IsNullOrWhiteSpace(currentProject))
                await service.SendMessage("Hey "+ ChatName.ChannelName +", what are we working on today?");
            else
                await service.SendMessage(currentProject);
        }
    }
}

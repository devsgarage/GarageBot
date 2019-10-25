using Service.Core;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ChatCommands
{
    public class ListCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "list", "help" };
        public string Description => "Displays a list of available commands (ex. !list), or description of a specific command (ex. !list project)";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        private IServiceProvider serviceProvider;

        public ListCommand(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            string message;
            if (text.IsEmpty)
                message = GetAllCommandsAvailable();
            else
                message = GetCommandDescription(text);

            service.SendMessage(message);
            return Task.CompletedTask;
        }

        private string GetCommandDescription(ReadOnlyMemory<char> text)
        {
            string commandMessage;
            var command = serviceProvider.GetServices<IChatCommand>().Where(c => text.Span.Equals(c.Command.First(), StringComparison.OrdinalIgnoreCase) && c.CanBeListed()).FirstOrDefault();
            if (command == null)
                commandMessage = $@"Command ""{text.ToString()}"" is not a valid command";
            else
                commandMessage = $"{command.Command} - {command.Description}";
            return commandMessage;
        }

        private string GetAllCommandsAvailable()
        {
            var commands = serviceProvider.GetServices<IChatCommand>().Where(x=>x.CanBeListed()).ToArray();
            StringBuilder sb = new StringBuilder();
            var s = string.Join(" ", commands.Select(c => $"{c.Command}"));
            sb.Append("Available commands: ").Append(s);
            return sb.ToString();
        }
    }
}

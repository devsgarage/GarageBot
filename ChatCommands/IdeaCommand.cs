using Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class IdeaCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "idea" };
        public string Description => "Submit an idea, suggestion or feedback to broadcaster";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(1);

        public Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text)
        {
            if (text.IsEmpty)
                return Task.CompletedTask;

            var ideaPath = $@"c:\dev\ideas-{DateTime.Now.ToString("MM-dd-yyyy")}.txt";
            var file = File.OpenWrite(ideaPath);
            file.Seek(0, SeekOrigin.End);
            var writer = new StreamWriter(file);
            writer.WriteLine($"{userName} - {text}");
            writer.Dispose();

            return Task.CompletedTask;
        }
    }
}

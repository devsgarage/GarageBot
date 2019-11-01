using Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class IdeaCommand : IChatCommand
    {
        public IdeaCommand() : this(WriteIdeaToFile)
        {

        }

        public IdeaCommand(Action<string, CommandArgs> ideaWriterAction)
        {
            _ideaWriterAction = ideaWriterAction;
        }

        private readonly Action<string, CommandArgs> _ideaWriterAction;

        public IEnumerable<string> Command => new[] { "idea" };
        public string Description => "Submit an idea, suggestion or feedback to broadcaster";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(1);

        public Task Execute(IChatService service, CommandArgs args)
        {
            if (args.Text.IsEmpty)
                return Task.CompletedTask;

            var ideaPath = $@"c:\dev\ideas-{DateTime.Now:MM-dd-yyyy}.txt";

            _ideaWriterAction.Invoke(ideaPath, args);

            return Task.CompletedTask;
        }

        // TODO: This needs further abstraction to allow unit testing of what is being written
        private static void WriteIdeaToFile(string ideaPath, CommandArgs args)
        {
            var file = File.OpenWrite(ideaPath);
            file.Seek(0, SeekOrigin.End);
            var writer = new StreamWriter(file);
            writer.WriteLine($"{args.UserName} - {args.Text}");
            writer.Dispose();
        }
    }
}

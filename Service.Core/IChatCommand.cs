using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IChatCommand
    {
        string Command { get; }

        string Description { get; }

        TimeSpan? Cooldown { get; }

        Task Execute(IChatService service, bool isBroadcaster, string userName, ReadOnlyMemory<char> text);
    }
}

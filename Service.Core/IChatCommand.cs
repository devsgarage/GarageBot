using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IChatCommand
    {
        IEnumerable<string> Command { get; }

        string Description { get; }

        TimeSpan? Cooldown { get; }

        bool CanBeListed() => true;

        Task Execute(IChatService service, CommandArgs args);
    }
}

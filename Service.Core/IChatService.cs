using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IChatService
    {
        event EventHandler<ChatMessageReceivedArgs> ChatMessageReceieved;

        Task Start();

        Task<bool> SendMessage(string message);

        Task<TimeSpan> GetUptime();
    }
}

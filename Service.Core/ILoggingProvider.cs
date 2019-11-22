using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface ILoggingProvider
    {
        Task StartNewStream(string streamDescription);

        Task StopStream();

        Task UserJoined(string username, DateTime joinedDateTime);

        Task UserLeft(string username, DateTime leftDateTime);

        Task ChatMessages(string username, string chatMessage, DateTime messageDateTime);

        Task CommandUsed(string username, string commandName, DateTime executedDateTime, bool inCooldown);

        Task LogMessage(string message, SeverityLevel severity);

        Task LogMessage(Exception exception, SeverityLevel severity);

        Task IdeaSuggested(string idea, DateTime ideaRecievedDateTime);

        Task NewFollower(string username, DateTime followReceived);

        Task NewSubscriber(string username, DateTime subscriptionReceivedDateTime);
    }
}

using Service.Core;
using System;
using System.Threading.Tasks;

namespace LoggingProviders
{
    public class FileLoggingProvider 
    {
        public Task ChatMessages(string username, string chatMessage, DateTime MessageDateTime)
        {
            throw new NotImplementedException();
        }

        public Task CommandUsed(string username, string commandName, DateTime executedDateTime, bool inCooldown)
        {
            throw new NotImplementedException();
        }

        public Task IdeaSuggested(string idea, DateTime ideaRecievedDateTime)
        {
            throw new NotImplementedException();
        }

        public Task LogMessage(string message, SeverityLevel severity)
        {
            throw new NotImplementedException();
        }

        public Task LogMessage(Exception exception, SeverityLevel severity)
        {
            throw new NotImplementedException();
        }

        public Task NewFollower(string username, DateTime followReceived)
        {
            throw new NotImplementedException();
        }

        public Task NewSubscriber(string username, DateTime SubscriptionReceivedDateTime)
        {
            throw new NotImplementedException();
        }

        public Task StartNewStream(string streamDescription)
        {
            throw new NotImplementedException();
        }

        public Task UserJoined(string username, DateTime JoinedDateTime)
        {
            throw new NotImplementedException();
        }

        public Task UserLeft(string username, DateTime LeftDateTime)
        {
            throw new NotImplementedException();
        }
    }
}

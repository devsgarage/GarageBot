using Service.Core;
using System;
using System.Threading.Tasks;

namespace LoggingProviders
{
    public class FileLoggingProvider : ILoggingProvider
    {
        public Task LogMessage(string message, SeverityLevel severity)
        {
            throw new NotImplementedException();
        }

        public Task LogMessage(Exception exception, SeverityLevel severity)
        {
            throw new NotImplementedException();
        }
    }
}

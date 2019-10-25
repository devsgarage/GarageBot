using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface ILoggingProvider
    {
        Task LogMessage(string message, SeverityLevel severity);

        Task LogMessage(Exception exception, SeverityLevel severity);
    }
}

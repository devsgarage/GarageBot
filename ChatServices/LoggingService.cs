using Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServices
{
    public class LoggingService
    {
        private IEnumerable<ILoggingProvider> loggingProviders;
        private IStreamingService streamingService;

        public LoggingService(IEnumerable<ILoggingProvider> loggingProviders, IStreamingService streamingService) 
        { 
            this.loggingProviders = loggingProviders; 
            this.streamingService = streamingService; 
        }

        public async Task LogMessage(string message, SeverityLevel severity)
        {
            if (!streamingService.IsStreamLive) return;

            var tasks = loggingProviders.Select(lp => lp.LogMessage(message, severity)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return;
        }

        public async Task LogMessage(Exception message, SeverityLevel severity)
        {
            if (!streamingService.IsStreamLive) return;

            var tasks = loggingProviders.Select(lp => lp.LogMessage(message, severity)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return;
        }




    }
}

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
            this.streamingService.StateChanged += async(s, a) => {
                if (!streamingService.IsStreamLive) return;
                var tasks = loggingProviders.Select(lp =>
                {
                    return lp.StartNewStream($"Stream {DateTime.Today}");
                }).ToArray();
                await Task.WhenAll(tasks).ConfigureAwait(false);
            };
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

        public async Task LogUserJoined(string username, DateTime joinedDateTime)
        {
            if (!streamingService.IsStreamLive) return;
            var tasks = loggingProviders.Select(lp => lp.UserJoined(username, joinedDateTime)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return;
        }

        public async Task LogUserLeft(string username, DateTime leftDateTime)
        {
            var tasks = loggingProviders.Select(lp => lp.UserLeft(username, leftDateTime)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return;
        }

        public async Task ChatMessages(string username, string chatMessage, DateTime receivedDateTime)
        {
            var tasks = loggingProviders.Select(lp => lp.ChatMessages(username, chatMessage, receivedDateTime)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return;
        }

        public async Task IdeaSuggested(string idea, DateTime ideaReceived)
        {
            var tasks = loggingProviders.Select(lp => lp.IdeaSuggested(idea, ideaReceived)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return;
        }


    }
}

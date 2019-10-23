using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServices
{
    public class StreamingService : IStreamingService
    {
        public bool IsStreamLive { get; private set; } = false;

        public void StartStream() => IsStreamLive = true;

        public void StopStream() => IsStreamLive = false;
    }
}

using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServices
{
    public class StreamingService : IStreamingService
    {
        public event EventHandler<EventArgs> StateChanged;

        public bool IsStreamLive { get; private set; } = false;

        public void StartStream() 
        {
            IsStreamLive = true;
            StateChanged?.Invoke(this, new EventArgs());
        }

        public void StopStream()
        {
            IsStreamLive = false;
            StateChanged?.Invoke(this, new EventArgs());
        }
    }
}

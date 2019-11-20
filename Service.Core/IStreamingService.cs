using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public interface IStreamingService
    {
        event EventHandler<EventArgs> StateChanged;
        bool IsStreamLive { get; }
        void StartStream();
        void StopStream();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public interface IStreamingService
    {
        bool IsStreamLive { get; }
        void StartStream();
        void StopStream();
    }
}

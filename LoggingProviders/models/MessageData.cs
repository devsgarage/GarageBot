using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingProviders.models
{
    public class MessageData
    {
        public string MessageText { get; set; }
        public string Severity { get; set; }
        public string ReceivedDateTime { get; set; }
    }
}

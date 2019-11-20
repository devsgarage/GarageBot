using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingProviders.models
{
    public class ChatMessage
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public string MessageRecivedDateTime { get; set; }
    }
}

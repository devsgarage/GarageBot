using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Twitch.Models
{
    public class NewMessageEventArgs : EventArgs
    {
        public string UserName { get; set; }
        public string Message { get; set; }

        public string[] Badges { get; set; }
    }
}

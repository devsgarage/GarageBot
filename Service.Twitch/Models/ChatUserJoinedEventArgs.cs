using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Twitch.Models
{
    public class ChatUserJoinedEventArgs : EventArgs
    {
        public string UserName { get; set; }
    }
}

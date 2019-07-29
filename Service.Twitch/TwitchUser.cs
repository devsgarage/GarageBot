using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Twitch
{
    public class TwitchUser
    {
        public string From_id { get; set; }

        public string From_name { get; set; }

        public string To_id { get; set; }

        public string To_name { get; set; }

        public string Followed_at { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Twitch
{
    public class FollowersResponse
    {
        public int Total { get; set; }
        public List<TwitchUser> Data { get; set; }
        public Paging Pagination { get; set; }
    }
}

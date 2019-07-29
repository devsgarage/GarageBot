using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Twitch
{
    public class Proxy
    {
        HttpClient client;
        private const string developersGarageTwitchId = "245291776";
        private const string channelName = "developersgarage";
        public Proxy(HttpClient client, TwitchSettings settings            )
        {
            this.client = client;
            ConfigureClient(settings);
        }

        private void ConfigureClient(TwitchSettings settings)
        {
            client.BaseAddress =  new Uri("https://api.twitch.tv");
            client.DefaultRequestHeaders.Add("Client-ID", settings.ClientId);
        }

        private async Task<string> GetFollowers(string pageCursor = "")
        {
            var result = await client.GetAsync($"helix/users/follows?to_id={developersGarageTwitchId}&&after={pageCursor}");
            var x = await result.Content.ReadAsStringAsync();
            return x;
        }

        public async Task<int> GetFollowersCount()
        {
            var result = await GetFollowers();
            var jObj = JsonConvert.DeserializeObject<JObject>(result);
            return jObj.Value<int>("total");
        }

        public async Task<IList<string>> GetAllFollowers()
        {
            var list = new List<string>();
            var returnedCount = 0;
            var pageCursor = "";
            do
            {
                var results = await GetFollowers(pageCursor);
                var x = JsonConvert.DeserializeObject<FollowersResponse>(results);
                list.AddRange(x.Data.Select(z => z.From_name));
                returnedCount = x.Data.Count();
                pageCursor = x.Pagination.Cursor;
            } while (returnedCount > 0);

            return list;

        }

        public async Task<string> GetStreamMetaData()
        {
            var result = await client.GetAsync($"helix/streams?user_login={channelName}");
            var x = await result.Content.ReadAsStringAsync();
            return x;
        }
    }
}

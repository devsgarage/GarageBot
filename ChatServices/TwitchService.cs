using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using Service.Twitch;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatServices
{
    public class TwitchService : IChatService
    {
        TwitchChatClient chatClient;
        Proxy proxy;
        HttpClient client;

        public TwitchService(HttpClient client, Proxy proxy, TwitchChatClient chatClient)
        {
            this.client = client;
            this.proxy = proxy;
            this.chatClient = chatClient;
        }

        public event EventHandler<ChatMessageReceivedArgs> ChatMessageReceieved;
        public event EventHandler<UserJoinedChatArgs> UserJoinedChat;

        public Task<bool> SendMessage(string message)
        {
            chatClient.PostMessage(message);
            return Task.FromResult(true);
        }

        public Task Start()
        {
            chatClient.NewMessage += ChatClient_NewMessage;
            chatClient.Connected += ChatClient_Connected;
            chatClient.UserJoined += ChatClient_UserJoined;
            chatClient.Init();
            return Task.CompletedTask;
        }

        private void ChatClient_UserJoined(object sender, Service.Twitch.Models.ChatUserJoinedEventArgs e)
        {
            UserJoinedChat?.Invoke(this, new UserJoinedChatArgs { UserName = e.UserName });
        }

        private void ChatClient_Connected(object sender, Service.Twitch.Models.ChatConnectedEventArgs e)
        {
            
        }

        private void ChatClient_NewMessage(object sender, Service.Twitch.Models.NewMessageEventArgs e)
        {
            ChatMessageReceieved?.Invoke(this, new ChatMessageReceivedArgs
            {
                IsBroadcaster = e.UserName == "developersgarage",
                Message = e.Message,
                UserName = e.UserName
            });
        }

        public async Task<TimeSpan> GetUptime()
        {
            var results = await proxy.GetStreamMetaData();
            StreamData x = ParseStreamMetaData(results);
            var uptime = DateTime.UtcNow - x.StartedAt;
            return uptime;
        }

        public async Task<TwitchTeam> GetTwitchTeam(string teamName)
        {
            var teamString = await proxy.GetTeamInfo(teamName);
            var team = TwitchTeam.FromJson(teamString);
            return team;
        }

        private static StreamData ParseStreamMetaData(string results)
        {
            var obj = JsonConvert.DeserializeObject<JObject>(results);
            var data = obj.GetValue("data")[0];
            var streamData = JsonConvert.DeserializeObject<StreamData>(data.ToString());
            return streamData;
        }
    }
}

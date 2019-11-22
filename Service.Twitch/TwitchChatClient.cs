//Base code for the chat client is from @csharpfritz and his TwitchLib project

using Service.Core;
using Service.Twitch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Service.Twitch
{


    public class TwitchChatClient : IDisposable
    {
        private TcpClient _TcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        private readonly CancellationTokenSource _Shutdown;

        private const string chatBotName = "developersgarage_bot";
        private const string chatChannelName = "developersgarage";

        internal static readonly Regex reUserName = new Regex(@"!([^@]+)@");
        internal static readonly Regex reBadges = new Regex(@"@badges=([^;]*)");
        internal static Regex reChatMessage;
        internal static Regex reWhisperMessage;

        private Thread _ReceiveMessagesThread;
        private bool disposedValue;
        private string oauthToken;

        public event EventHandler<ChatConnectedEventArgs> Connected;
        public event EventHandler<NewMessageEventArgs> NewMessage;
        public event EventHandler<ChatUserJoinedEventArgs> UserJoined;
        public event EventHandler<ChatUserLeftEventArgs> UserLeft;

        public TwitchChatClient(TwitchSettings settings)
        {            
            reChatMessage = new Regex($@"PRIVMSG #{chatChannelName} :(.*)$");
            reWhisperMessage = new Regex($@"WHISPER {chatBotName} :(.*)$");

            oauthToken = settings.OauthToken;
            _Shutdown = new CancellationTokenSource();
        }

        public void Init()
        {
            Connect();
            _ReceiveMessagesThread = new Thread(ReceiveMessagesOnThread);
            _ReceiveMessagesThread.Start();
        }

        public void PostMessage(string message)
        {

            var fullMessage = $":{chatBotName}!{chatBotName}@{chatBotName}.tmi.twitch.tv PRIVMSG #{chatChannelName} :{message}";

            SendMessage(fullMessage);

        }

        ~TwitchChatClient()
        {

            try
            {
                Console.WriteLine("GC the ChatClient");
            }
            catch { }
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {

            try
            {
                //Logger?.LogWarning("Disposing of ChatClient");
                Console.WriteLine("Disposing of ChatClient");
            }
            catch { }

            if (!disposedValue)
            {
                if (disposing)
                {
                    _Shutdown.Cancel();
                }

                _TcpClient?.Dispose();
                disposedValue = true;
            }
        }

        private void Connect()
        {

            _TcpClient = new TcpClient("irc.chat.twitch.tv", 80);

            inputStream = new StreamReader(_TcpClient.GetStream());
            outputStream = new StreamWriter(_TcpClient.GetStream());

            //Logger.LogTrace("Beginning IRC authentication to Twitch");
            Console.WriteLine("Beginning IRC authentication to Twitch");
            outputStream.WriteLine("CAP REQ :twitch.tv/tags twitch.tv/commands twitch.tv/membership");
            outputStream.WriteLine($"PASS oauth:{oauthToken}");
            outputStream.WriteLine($"NICK {chatBotName}");
            outputStream.WriteLine($"USER {chatBotName} 8 * :{chatBotName}");
            outputStream.Flush();

            outputStream.WriteLine($"JOIN #{chatChannelName}");
            outputStream.Flush();

            //Connected?.Invoke(this, new ChatConnectedEventArgs());

        }

        private void ReceiveMessagesOnThread()
        {

            var lastMessageReceivedTimestamp = DateTime.Now;
            var errorPeriod = TimeSpan.FromSeconds(60);

            while (true)
            {

                Thread.Sleep(50);

                if (DateTime.Now.Subtract(lastMessageReceivedTimestamp) > errorPeriod)
                {
                    //Logger.LogError($"Haven't received a message in {errorPeriod.TotalSeconds} seconds");
                    Console.WriteLine($"Haven't received a message in {errorPeriod.TotalSeconds} seconds");
                    lastMessageReceivedTimestamp = DateTime.Now;
                }

                if (_Shutdown.IsCancellationRequested)
                {
                    break;
                }

                if (_TcpClient.Connected && _TcpClient.Available > 0)
                {

                    var msg = ReadMessage();
                    if (string.IsNullOrEmpty(msg))
                    {
                        continue;
                    }

                    lastMessageReceivedTimestamp = DateTime.Now;
                    //Logger.LogTrace($"> {msg}");
                    Console.WriteLine($"> {msg}");

                    // Handle the Twitch keep-alive
                    if (msg.StartsWith("PING"))
                    {
                        //Logger.LogWarning("Received PING from Twitch... sending PONG");
                        Console.WriteLine("Received PING from Twitch... sending PONG");
                        SendMessage($"PONG :{msg.Split(':')[1]}");
                        continue;
                    }

                    ProcessMessage(msg);

                }
                else if (!_TcpClient.Connected)
                {
                    // Reconnect
                    //Logger.LogWarning("Disconnected from Twitch.. Reconnecting in 2 seconds");
                    Console.WriteLine("Disconnected from Twitch.. Reconnecting in 2 seconds");
                    Thread.Sleep(2000);
                    this.Init();
                    return;
                }

            }

            //Logger.LogWarning("Exiting ReceiveMessages Loop");
            Console.WriteLine("Exiting ReceiveMessages Loop");
        }

        private string ReadMessage()
        {

            string message = null;

            try
            {
                message = inputStream.ReadLine();
            }
            catch (Exception ex)
            {
                //Logger.LogError("Error reading messages: " + ex);
                Console.WriteLine("Error reading messages: " + ex);
            }

            return message ?? "";

        }

        private void SendMessage(string message, bool flush = true)
        {

            //var throttled = CheckThrottleStatus();

            //Thread.Sleep(throttled.GetValueOrDefault(TimeSpan.FromSeconds(0)));

            outputStream.WriteLine(message);
            if (flush)
            {
                outputStream.Flush();
            }

        }

        private void ProcessMessage(string msg)
        {

            // Logger.LogTrace("Processing message: " + msg);

            Console.WriteLine("Processing message: " + msg);

            var userName = "";
            var message = "";

            userName = TwitchChatClient.reUserName.Match(msg).Groups[1].Value;
            if (userName == chatBotName) return; // Exit and do not process if the bot posted this message

            var badges = TwitchChatClient.reBadges.Match(msg).Groups[1].Value.Split(',');

            if (!string.IsNullOrEmpty(userName) && msg.Contains($" JOIN #{chatChannelName}"))
            {
                UserJoined?.Invoke(this, new ChatUserJoinedEventArgs { UserName = userName });
            }
            else if (!string.IsNullOrEmpty(userName) && msg.Contains($" PART #{chatChannelName}"))
            {
                UserLeft?.Invoke(this, new ChatUserLeftEventArgs { UserName = userName });
            }

            //// Review messages sent to the channel
            if (reChatMessage.IsMatch(msg))
            {
                message = TwitchChatClient.reChatMessage.Match(msg).Groups[1].Value;
                Console.WriteLine($"Message received from '{userName}': {message}");
                //Logger.LogTrace($"Message received from '{userName}': {message}");
                NewMessage?.Invoke(this, new NewMessageEventArgs
                {
                    UserName = userName,
                    Message = message,
                    Badges = badges
                });
            }
            else if (reWhisperMessage.IsMatch(msg))
            {

                message = TwitchChatClient.reWhisperMessage.Match(msg).Groups[1].Value;
                Console.WriteLine($"Whisper received from '{userName}': {message}");
                //Logger.LogTrace($"Whisper received from '{userName}': {message}");

                NewMessage?.Invoke(this, new NewMessageEventArgs
                {
                    UserName = userName,
                    Message = message,
                    Badges = (badges ?? new string[] { })
                });

            }

        }

        public void Dispose()
        {
            
        }
    }
}

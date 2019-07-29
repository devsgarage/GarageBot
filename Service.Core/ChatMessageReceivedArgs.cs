namespace Service.Core
{
    public class ChatMessageReceivedArgs
    {
        public bool IsBroadcaster { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}
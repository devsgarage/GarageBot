using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingProviders.models
{
    public class CosmosStreamData
    {        
        public Guid Id { get; set; }
        public string Month { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Description { get; set; }
        public UserEvent[] UsersJoined { get; set; }
        public UserEvent[] UsersLeft { get; set; }
        public ChatMessage[] Messages { get; set; }
        public CommandUsedInfo[] CommandsUsed { get; set; }
        public MessageData[] Exceptions { get; set; }
        public MessageData[] Information { get; set; }
        public Idea[] Ideas { get; set; }
        public UserEvent[] NewFollowers { get; set; }
        public UserEvent[] NewSubscribers { get; set; }
    }
}


using Azure.Cosmos;
using LoggingProviders.models;
using Microsoft.Extensions.Options;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingProviders
{
    public class StreamLogProvider : ILoggingProvider
    {
        CosmosClient client;
        CosmosSettings settings;
        CosmosStreamData streamData;

        public StreamLogProvider(IOptions<CosmosSettings> settings)
        {
            this.settings = settings.Value;
            ConfigureCosmosClient();
        }

        private void ConfigureCosmosClient()
        {
            var cosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    IgnoreNullValues = false,
                    Indented = true,
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            client = new CosmosClient(settings.EndpointUrl, settings.AuthKey, cosmosClientOptions);
        }

        public async Task StartNewStream(string streamDescription)
        {
            streamData = new CosmosStreamData
            {
                Id = Guid.NewGuid(),
                Description = streamDescription,
                Month = DateTime.Now.ToString("MMMM"),
                Start = string.Format("{0:MM/dd/yy H:mm:ss}", DateTime.Now),
                End = null,
                UsersJoined = Array.Empty<UserEvent>(),
                UsersLeft = Array.Empty<UserEvent>(),
                CommandsUsed = Array.Empty<CommandUsedInfo>(),
                Messages = Array.Empty<ChatMessage>(),
                Information = Array.Empty<MessageData>(),
                Exceptions = Array.Empty<MessageData>(),
                Ideas = Array.Empty<Idea>(),
                NewFollowers = Array.Empty<UserEvent>(),
                NewSubscribers = Array.Empty<UserEvent>()
            };

            var container = client.GetContainer(settings.DatabaseId, settings.ContainerId);
            streamData = await container.CreateItemAsync(streamData, new PartitionKey(streamData.Month));
        }

        public Task ChatMessages(string username, string chatMessage, DateTime messageDateTime)
        {
            streamData.Messages = streamData.Messages.Append(new ChatMessage
            {
                Message = chatMessage,
                Username = username,
                MessageRecivedDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", messageDateTime)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task CommandUsed(string username, string commandName, DateTime executedDateTime, bool inCooldown)
        {
            streamData.CommandsUsed = streamData.CommandsUsed.Append(new CommandUsedInfo
            {
                CommandName = commandName,
                Username = username,
                InCooldown = inCooldown,
                FiredDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", executedDateTime)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task IdeaSuggested(string idea, DateTime ideaRecievedDateTime)
        {
            streamData.Ideas = streamData.Ideas.Append(new Idea
            {
                IdeaText = idea,
                IdeaReceived = string.Format("{0:MM/dd/yy H:mm:ss}", ideaRecievedDateTime)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task LogMessage(string message, SeverityLevel severity)
        {
            streamData.Information = streamData.Information.Append(new MessageData
            {
                MessageText = message,
                Severity = severity.ToString(),
                ReceivedDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", DateTime.Now)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task LogMessage(Exception exception, SeverityLevel severity)
        {
            streamData.Information = streamData.Information.Append(new MessageData
            {
                MessageText = exception.ToString(),
                Severity = severity.ToString(),
                ReceivedDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", DateTime.Now)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task NewFollower(string username, DateTime followReceived)
        {
            streamData.NewFollowers = streamData.NewFollowers.Append(new UserEvent
            {
                Username = username,
                ActionDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", followReceived)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task NewSubscriber(string username, DateTime subscriptionReceivedDateTime)
        {
            streamData.NewSubscribers= streamData.NewSubscribers.Append(new UserEvent
            {
                Username = username,
                ActionDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", subscriptionReceivedDateTime)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task UserJoined(string username, DateTime joinedDateTime)
        {
            streamData.UsersJoined = streamData.UsersJoined.Append(new UserEvent
            {
                Username = username,
                ActionDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", joinedDateTime)
            }).ToArray();

            return Task.CompletedTask;
        }

        public Task UserLeft(string username, DateTime leftDateTime)
        {
            streamData.UsersJoined = streamData.UsersJoined.Append(new UserEvent
            {
                Username = username,
                ActionDateTime = string.Format("{0:MM/dd/yy H:mm:ss}", leftDateTime)
            }).ToArray();

            return Task.CompletedTask;
        }

        private async Task UpdateLog()
        {
            var container = client.GetContainer(settings.DatabaseId, settings.ContainerId);
            await container.UpsertItemAsync(streamData, new PartitionKey(streamData.Month));
        }
    }
}

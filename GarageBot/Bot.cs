﻿using ChatCommands;
using ChatServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Core;
using Service.Twitch;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GarageBot
{
    public class Bot : IHostedService
    {
        private const string USER_JOINED_COMMAND_NAME = "userjoined";
        public IEnumerable<IChatCommand> commands;
        private IChatService service;
        private ConcurrentDictionary<string, DateTime> commandLastExecution = new ConcurrentDictionary<string, DateTime>();
        private ILogger<Bot> logger;
        private LoggingService loggingService;

        public Bot(ILogger<Bot> logger, IEnumerable<IChatCommand> chatCommands, IChatService twitchService, LoggingService loggingService)
        {
            this.logger = logger;
            this.commands = chatCommands;
            this.loggingService = loggingService;
            service = twitchService;
            service.ChatMessageReceieved += Service_ChatMessageReceieved;
            service.UserJoinedChat += Service_UserJoinedChat;
            service.Start();
        }

        private void Service_UserJoinedChat(object sender, UserJoinedChatArgs e)
        {
            var commandsToExecute =
                commands.Where(c => c.Command.Any(cmd => USER_JOINED_COMMAND_NAME.AsSpan().Equals(cmd.AsSpan(), StringComparison.OrdinalIgnoreCase) &&
                                                         !CommandInCooldown(cmd, c.Cooldown)));
            foreach (var commandToExecute in commandsToExecute)
            {
                commandToExecute.Execute(service, true, e.UserName, null);
                commandLastExecution[commandToExecute.Command.First()] = DateTime.UtcNow;
            }
        }

        private void Service_ChatMessageReceieved(object sender, ChatMessageReceivedArgs e)
        {
            Console.WriteLine("Bot recieved messsage");
            ProcessIncomingMessage(e.Message, e.IsBroadcaster, e.UserName);
        }

        private void ProcessIncomingMessage(string message, bool isBroadcaster, string userName)
        {
            if (message.FirstOrDefault() != '!')
                return;

            var command = ParseCommand(message);

            var commandsToExecute =
                commands.Where(c => c.Command.Any(cmd => command.command.Span.Equals(cmd.AsSpan(), StringComparison.OrdinalIgnoreCase) &&
                                                         !CommandInCooldown(cmd, c.Cooldown)));

            foreach (var commandToExecute in commandsToExecute)
            {
                try
                {
                    commandToExecute.Execute(service, isBroadcaster, userName, command.parameter);
                    commandLastExecution[commandToExecute.Command.First()] = DateTime.UtcNow;
                } 
                catch(Exception ex)
                {
                    Console.WriteLine($"command {commandToExecute.GetType().ToString()} threw an exception: {ex.Message}");
                }
            }

            (ReadOnlyMemory<char> command, ReadOnlyMemory<char> parameter) ParseCommand(string message)
            {
                var command = message.AsMemory(1);
                var commandParameter = ReadOnlyMemory<char>.Empty;
                var splitLocation = command.Span.IndexOf(' ');
                if (splitLocation != -1)
                {
                    commandParameter = command.Slice(splitLocation + 1);
                    command = command.Slice(0, splitLocation);
                }
                return (command, commandParameter);
            };
        }
        
        private bool CommandInCooldown(string command, TimeSpan? cooldown = null)
        {
            DateTime lastExecuted;
            var gotLastExecution = commandLastExecution.TryGetValue(command, out lastExecuted);
            if (gotLastExecution && (lastExecuted + cooldown) > DateTime.UtcNow)
            {
                Console.WriteLine($"Ignoring {command} command because it's still in cool down");
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

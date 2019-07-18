using DiscomonProject.Discord.Entities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscomonProject.Discord
{
    public class Connection
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly DiscordLogger _logger;

        public Connection(DiscordLogger logger, DiscordSocketClient client, CommandService commands)
        {
            _logger = logger;
            _commands = commands;
            _client = client;
        }

        public async Task ConnectAsync(MonBotConfig config)
        {
            _client.Log += _logger.Log;

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();

            _client.JoinedGuild += HandleGuildJoin;

            _commands.CommandExecuted += CommandExecutedAsync;

            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            await Task.Delay(-1);
        }

        private async Task HandleGuildJoin(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync("Hello! I am MonBot. By default, my command prefix is **!** \nIf you would like to change this or other settings, type **!settings**. \nIf you would like to learn more about me and what I do, use **!info**. \nTo create a character, type **!createcharacter**. \nTo create a town, type **!foundtown**.");
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) || 
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;
            
            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            //Update user's info
            UserHandler.UpdateUserInfo(context.User.Id, context.User.Username, context.User.Mention, context.User.GetAvatarUrl());

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(context, argPos, services: null);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                System.Console.WriteLine($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }
                

            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                System.Console.WriteLine($"Command [{command.Value.Name}] executed for -> [{context.User.Username}]");
                return;
            }
                

            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
        }

    }
}
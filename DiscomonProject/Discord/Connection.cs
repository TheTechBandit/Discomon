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
            
            _client.MessageReceived += MessageRecieved;

            _client.ReactionAdded += ReactionReceived;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            await Task.Delay(-1);
        }

        private async Task HandleGuildJoin(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync("Hello! I am MonBot. By default, my command prefix is **!** \nIf you would like to change this or other settings, type **!settings**. \nIf you would like to learn more about me and what I do, use **!info**. \nTo create a character, type **!startadventure**. \nTo create a town, type **!foundtown**.");
        }

        private async Task MessageRecieved(SocketMessage messageParam)
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

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(context, argPos, services: null);

            //Update user's info
            UserHandler.UpdateUserInfo(context.User.Id, context.User.GetOrCreateDMChannelAsync().Result.Id, context.User.Username, context.User.Mention, context.User.GetAvatarUrl());
        }

        public async Task ReactionReceived(Cacheable<IUserMessage, ulong> cacheMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if(reaction.User.Value.IsBot)
                return;

            var message = await cacheMessage.GetOrDownloadAsync();
            var user = UserHandler.GetUser(reaction.UserId);
            ContextIds idList = new ContextIds()
            {
                UserId = reaction.UserId,
                ChannelId = reaction.Channel.Id,
                GuildId = 0
            };
            
            if(user.ReactionMessages.ContainsKey(message.Id))
            {
                var messageType = user.ReactionMessages[message.Id];

                //Attack Screen Main
                if(messageType == 0)
                {
                    //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
                    try
                    {
                        await UserHandler.CharacterExists(idList);
                        await UserHandler.CharacterInCombat(idList);
                    }
                    catch(InvalidCharacterStateException)
                    {
                        return;
                    }
                    
                    if(reaction.Emote.Name == "⚔")
                    {
                        await MessageHandler.MoveScreen(user.UserId);
                        user.ReactionMessages.Remove(message.Id);
                    }
                    else if(reaction.Emote.Name == "👜")
                    {
                        await MessageHandler.SendDM(user.UserId, "BAG not implemented yet!");
                    }
                    else if(reaction.Emote.Name == "🔁")
                    {
                        await MessageHandler.SendDM(user.UserId, "SWITCH not implemented yet!");
                    }
                    else if(reaction.Emote.Name == "🏃")
                    {
                        await MessageHandler.SendDM(user.UserId, "RUN not implemented yet!");
                    }
                }
                //Move Screen
                else if(messageType == 1)
                {
                    //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
                    try
                    {
                        await UserHandler.CharacterExists(idList);
                        await UserHandler.CharacterInCombat(idList);
                    }
                    catch(InvalidCharacterStateException)
                    {
                        return;
                    }
                    
                    System.Console.WriteLine("5");
                    if(reaction.Emote.Name == "1\u20E3")
                    {
                        System.Console.WriteLine("6");
                        if(user.Char.ActiveMon.ActiveMoves[0].Name != "None")
                        {
                            user.ReactionMessages.Remove(message.Id);
                            await CombatHandler.ParseMoveSelection(user, 0);
                        }
                    }
                    else if(reaction.Emote.Name == "2\u20E3")
                    {
                        if(user.Char.ActiveMon.ActiveMoves[1].Name != "None")
                        {
                            user.ReactionMessages.Remove(message.Id);
                            await CombatHandler.ParseMoveSelection(user, 1);
                        }
                    }
                    else if(reaction.Emote.Name == "3\u20E3")
                    {
                        if(user.Char.ActiveMon.ActiveMoves[2].Name != "None")
                        {
                            user.ReactionMessages.Remove(message.Id);
                            await CombatHandler.ParseMoveSelection(user, 2);
                        }
                    }
                    else if(reaction.Emote.Name == "4\u20E3")
                    {
                        if(user.Char.ActiveMon.ActiveMoves[3].Name != "None")
                        {
                            user.ReactionMessages.Remove(message.Id);
                            await CombatHandler.ParseMoveSelection(user, 3);
                        }
                    }
                }
            }
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
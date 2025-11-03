using Discord;
using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Extensions;
using MyBot.Messages;
using MyBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot
{
    internal class MyBotManager
    {
        private static DiscordSocketClient _client;
        private static MessageHandler _messageHandler;

        public static async Task StartAsync()
        {
            try
            {
                BotConfiguration configurationData = ConfigurationLoader.LoadConfiguration();
                string token = configurationData.Token;
                string prefix = configurationData.Prefix;

                DiscordSocketConfig discordSocketConfig = SetDiscordSocketConfig();
                _client = new DiscordSocketClient(discordSocketConfig);
                _messageHandler = new MessageHandler(prefix);

                AddEvents();

                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();

                await Task.Delay(-1);
            }
            catch(Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.CRITICAL);
            }
        }

        private static DiscordSocketConfig SetDiscordSocketConfig()
            => new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged 
                | GatewayIntents.GuildMessages 
                | GatewayIntents.GuildMessageReactions 
                | GatewayIntents.Guilds 
                | GatewayIntents.DirectMessages 
                | GatewayIntents.MessageContent 
                | GatewayIntents.GuildMembers,
            };

        private static void AddEvents()
        {
            _client.Log += RejestLogs;

            _client.MessageReceived += OnMessageReceived;

            _client.Ready += OnClientReady;
        }
        private static async Task OnMessageReceived(SocketMessage message)
            => await _messageHandler.HandleMessage(message);

        private static async Task OnClientReady()
        {
            foreach (SocketGuild? g in _client.Guilds)
            {
                Console.WriteLine($"Downloading users for guild: {g.Name}");
                await g.DownloadUsersAsync();
            }
            Console.WriteLine("All guild users downloaded!");
        }

        private static async Task RejestLogs(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            await LogManager.LogBotLifeCycle(msg.ToString());
        }
    }
}

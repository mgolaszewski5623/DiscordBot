using Discord;
using Discord.WebSocket;
using MyBot.DataManager;
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
                Console.WriteLine($"Error starting bot: {ex.GetCompleteMessage()}");
            }
        }

        private static DiscordSocketConfig SetDiscordSocketConfig()
            => new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMessages | GatewayIntents.GuildMessageReactions | GatewayIntents.Guilds | GatewayIntents.DirectMessages | GatewayIntents.MessageContent,
            };

        private static void AddEvents()
        {
            _client.Log += LogRejestration;

            _client.MessageReceived += async (message) 
                => await _messageHandler.HandleMessage(message);
        }

        private static Task LogRejestration(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

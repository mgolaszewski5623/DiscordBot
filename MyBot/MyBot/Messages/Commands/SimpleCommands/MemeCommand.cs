using Discord.WebSocket;
using MyBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class MemeCommand : ISimpleCommand
    {
        public string Name => "meme";

        public string Description => "Sends a random meme.";

        private const string MEME_API_URL = "https://meme-api.com/gimme";

        public async Task Execute(SocketMessage message)
        {
            try
            {
                await SendMemeAsync(message);
            }
            catch (MyBotException ex)
            {
                Console.WriteLine($"Error fetching a meme: {ex.Message}");
                await message.Channel.SendMessageAsync($"Sorry, I couldn't fetch a meme at the moment.");
            }
        }

        private async Task SendMemeAsync(SocketMessage message)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(MEME_API_URL);
                var meme = JsonSerializer.Deserialize<MemeResponse>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if (meme == null)
                    throw new MyBotException("Failed to parse meme response");
                var embed = new Discord.EmbedBuilder()
                    .WithTitle(meme.Title)
                    .WithUrl(meme.PostLink)
                    .WithImageUrl(meme.Url)
                    .WithFooter($"👍 {meme.Ups} | r/{meme.Subreddit}")
                    .WithColor(Discord.Color.Purple)
                    .Build();
                await message.Channel.SendMessageAsync(embed: embed);
            }
        }

        private class MemeResponse
        {
            public string PostLink { get; set; } = "";
            public string Subreddit { get; set; } = "";
            public string Title { get; set; } = "";
            public string Url { get; set; } = "";
            public int Ups { get; set; } = 0;
        }
    }
}

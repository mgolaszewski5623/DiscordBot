using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class MemeCommand : BaseSimpleCommand
    {
        private const string MEME_API_URL = "https://meme-api.com/gimme";

        public override string Name => "meme";

        public override string Description => "Sends a random meme.";

        protected override async Task<object> CreateMessageToSend(SocketMessage message)
        {
            try
            {
                return await GetMemeEmbedAsync();
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
                return $"Sorry, I couldn't fetch a meme at the moment.";
            }
        }

        private async Task<Discord.Embed> GetMemeEmbedAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string response = await httpClient.GetStringAsync(MEME_API_URL);
                MemeResponse? meme = JsonSerializer.Deserialize<MemeResponse>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if (meme == null)
                    throw new MyBotException("Failed to parse meme response");
                return new Discord.EmbedBuilder()
                    .WithTitle(meme.Title)
                    .WithUrl(meme.PostLink)
                    .WithImageUrl(meme.Url)
                    .WithFooter($"👍 {meme.Ups} | r/{meme.Subreddit}")
                    .WithColor(Discord.Color.Purple)
                    .Build();
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

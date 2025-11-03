using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Exceptions;
using MyBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class JokeCommand : BaseSimpleCommand
    {
        public override string Name => "joke";

        public override string Description => "Tells a random joke.";

        private const string JOKE_API_URL = "https://v2.jokeapi.dev/joke/Any";

        protected override async Task<object> CreateMessageToSend(SocketMessage message)
        {
            try
            {
                return await GetRandomJokeAsync();
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
                return "Sorry, I couldn't fetch a joke at the moment.";
            }
        }

        private async Task<string> GetRandomJokeAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string response = await httpClient.GetStringAsync(JOKE_API_URL);
                JokeResponse? joke = JsonSerializer.Deserialize<JokeResponse>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if (joke == null)
                    throw new MyBotException("Failed to parse joke response");

                return joke.Type switch
                {
                    "single" => joke.Joke,
                    "twopart" => $"{joke.Setup}\n{joke.Delivery}",
                    _ => throw new MyBotException($"Unknown joke type: {joke.Type}")
                };
            }
        }

        private class JokeResponse
        {
            public string Type { get; set; } = "";
            public string Joke { get; set; } = "";
            public string Setup { get; set; } = "";
            public string Delivery { get; set; } = "";
        }
    }
}
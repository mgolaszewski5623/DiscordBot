using Discord.WebSocket;
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

        protected override string CreateMessageToSend(SocketMessage message)
        {
            try
            {
                return GetRandomJokeAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching joke: {ex.GetCompleteMessage()}");
                return "Sorry, I couldn't fetch a joke at the moment.";
            }
        }

        private async Task<string> GetRandomJokeAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(JOKE_API_URL);
                var joke = JsonSerializer.Deserialize<JokeResponse>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if (joke == null)
                    throw new MyBotException("Failed to parse joke response");
                if (joke.Type == "single")
                    return joke.Joke;
                if (joke.Type == "twopart")
                    return $"{joke.Setup}\n{joke.Delivery}";
                throw new MyBotException($"Unknown joke type: {joke.Type}");
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
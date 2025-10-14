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
    internal class QuoteCommand : BaseSimpleCommand
    {
        public override string Name => "quote";
        
        public override string Description => "Sends a random quote.";

        protected override bool CanBotSendMessage => false;

        private const string QUOTE_API_URL = "https://zenquotes.io/api/random";

        protected override string CreateMessageToSend(Discord.WebSocket.SocketMessage message)
        {
            try
            {
                return Task.Run(() => GetRandomQuoteAsync()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quote. {ex.GetCompleteMessage()}");
                return "Sorry, I couldn't fetch a quote at this time.";
            }
        }

        private async Task<string> GetRandomQuoteAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string response = await httpClient.GetStringAsync(QUOTE_API_URL).ConfigureAwait(false);

                List<QuoteResponse>? quotes = JsonSerializer.Deserialize<List<QuoteResponse>>(
                    response,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (quotes == null || quotes.Count == 0)
                    throw new MyBotException("Failed to parse quote response");

                QuoteResponse q = quotes[0];
                return $"💡 \"{q.Q}\" — {q.A}";
            }
        }

        private class QuoteResponse
        {
            public string Q { get; set; } = "";
            public string A { get; set; } = "";
        }
    }
}

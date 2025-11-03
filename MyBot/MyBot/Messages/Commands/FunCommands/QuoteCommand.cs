using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Exceptions;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.FunCommands
{
    internal class QuoteCommand : BaseCommand
    {
        private const string QUOTE_API_URL = "https://zenquotes.io/api/random";

        public override string Name => "quote";
        
        public override string Description => "Sends a random quote.";

        protected override bool CanBotSendMessage => false;

        protected override bool AllowParameters => false;

        protected override async Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null)
        {
            try
            {
                return await GetRandomQuoteAsync();
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
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

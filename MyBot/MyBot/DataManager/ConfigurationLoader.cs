using MyBot.Exceptions;
using MyBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.DataManager
{
    public static class ConfigurationLoader
    {
        public static BotConfiguration LoadConfiguration(string path = "Data/configuration.json")
        {
            if(!File.Exists(path))
                throw new MyBotException($"Configuration file not found at path: {path}");
            var json = File.ReadAllText(path);
            var configuration = System.Text.Json.JsonSerializer.Deserialize<BotConfiguration>(json);
            if(configuration == null || string.IsNullOrWhiteSpace(configuration.Token) || string.IsNullOrWhiteSpace(configuration.Prefix))
                throw new MyBotException("Configuration file is invalid or missing required fields.");
            SetConfiguration(configuration);
            return configuration;
        }

        private static void SetConfiguration(BotConfiguration configuration)
        {
            WarningManager.MaxWarnings = configuration.MaxWarnings;
            WarningManager.WarningDuration = configuration.WarningDurationDays;
        }
    }
}

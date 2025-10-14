using MyBot.Extensions;
using MyBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBot.DataManager
{
    public static class WarningManager
    {
        public static int WarningDuration;

        public static int MaxWarnings;

        private const string warningPath = "Data/Warnings/";

        public static async Task SaveWarning(Warning warning)
        {
            await RemoveOldWarnings(warning.GuildId, warning.GuildName);
            string warningFile = Path.Combine(warningPath, GetGuildFile(warning.GuildId, warning.GuildName));
            List<Warning> warnings = new();
            if(File.Exists(warningFile))
            {
                string json = await File.ReadAllTextAsync(warningFile);
                if(!string.IsNullOrWhiteSpace(json))
                    warnings = JsonSerializer.Deserialize<List<Warning>>(json) ?? new();
            }
            warnings.Add(warning);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(warnings, options);
            await File.WriteAllTextAsync(warningFile, updatedJson);
        }

        private static async Task RemoveOldWarnings(ulong guildId, string guildName)
        {
            PathExtensions.CreateLogDirectory(warningPath);
            string warningFile = Path.Combine(warningPath, GetGuildFile(guildId, guildName));
            if(!File.Exists(warningFile))
                return;
            string json = await File.ReadAllTextAsync(warningFile);
            List<Warning> warnings = JsonSerializer.Deserialize<List<Warning>>(json) ?? new List<Warning>();
            DateTime cutoffData = DateTime.UtcNow.AddDays(-WarningDuration);
            warnings = warnings.Where(w => w.Date >= cutoffData).ToList();

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(warnings, options);
            await File.WriteAllTextAsync(warningFile, updatedJson);
        }

        public static async Task<List<Warning>> GetWarnings(ulong guildId, string guildName, ulong userId)
        {
            await RemoveOldWarnings(guildId, guildName);
            string warningFile = Path.Combine(warningPath, GetGuildFile(guildId, guildName));
            if(!File.Exists(warningFile))
                return new List<Warning>();
            string json = await File.ReadAllTextAsync(warningFile);
            return JsonSerializer.Deserialize<List<Warning>>(json)?
                .Where(w => w.WarnedUserId == userId)
                .ToList() ?? new List<Warning>();
        }

        public static async Task<bool> HasReachedMaxWarnings(ulong guildId, string guildName, ulong userId)
        {
            List<Warning> warnings = await GetWarnings(guildId, guildName, userId);
            return warnings.Count > MaxWarnings;
        }

        private static string GetGuildFile(ulong guildId, string guildName)
            => $"S_{guildName}_{guildId}_warnings.json".SanitizeFilePath();
    }
}

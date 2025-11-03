using MyBot.Enums;
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

        public static async Task SaveWarning(WarningModel warning)
        {
            try
            {
                await RemoveOldWarnings(warning.GuildId, warning.GuildName);
                string warningFile = Path.Combine(warningPath, GetGuildFile(warning.GuildId, warning.GuildName));
                List<WarningModel> warnings = new();
                if (File.Exists(warningFile))
                {
                    string json = await File.ReadAllTextAsync(warningFile);
                    if (!string.IsNullOrWhiteSpace(json))
                        warnings = JsonSerializer.Deserialize<List<WarningModel>>(json) ?? new();
                }
                warnings.Add(warning);
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                string updatedJson = JsonSerializer.Serialize(warnings, options);
                await File.WriteAllTextAsync(warningFile, updatedJson);
            }
            catch(Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
                throw new Exception("An error occurred while saving the warning.", ex);
            }
        }

        private static async Task RemoveOldWarnings(ulong guildId, string guildName)
        {
            try
            {
                PathExtensions.CreateDirectory(warningPath);
                string warningFile = Path.Combine(warningPath, GetGuildFile(guildId, guildName));
                if (!File.Exists(warningFile))
                    return;
                string json = await File.ReadAllTextAsync(warningFile);
                List<WarningModel> warnings = JsonSerializer.Deserialize<List<WarningModel>>(json) ?? new List<WarningModel>();
                DateTime cutoffData = DateTime.UtcNow.AddDays(-WarningDuration);
                warnings = warnings.Where(w => w.Date >= cutoffData).ToList();

                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                string updatedJson = JsonSerializer.Serialize(warnings, options);
                await File.WriteAllTextAsync(warningFile, updatedJson);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while removing old warnings.", ex);
            }
        }

        public static async Task<List<WarningModel>> GetWarnings(ulong guildId, string guildName, ulong userId)
        {
            try
            {
                await RemoveOldWarnings(guildId, guildName);
                string warningFile = Path.Combine(warningPath, GetGuildFile(guildId, guildName));
                if (!File.Exists(warningFile))
                    return new List<WarningModel>();
                string json = await File.ReadAllTextAsync(warningFile);
                return JsonSerializer.Deserialize<List<WarningModel>>(json)?
                    .Where(w => w.TargetUserId == userId)
                    .ToList() ?? new List<WarningModel>();
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
                throw new Exception("An error occurred while retrieving warnings.", ex);
            }
        }

        public static async Task<bool> HasReachedMaxWarnings(ulong guildId, string guildName, ulong userId)
        {
            try
            {
                List<WarningModel> warnings = await GetWarnings(guildId, guildName, userId);
                return warnings.Count > MaxWarnings;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking warnings.", ex);
            }
        }

        private static string GetGuildFile(ulong guildId, string guildName)
            => $"S_{guildName}_{guildId}_warnings.json".SanitizeFilePath();
    }
}

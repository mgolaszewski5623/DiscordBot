using Discord.WebSocket;
using MyBot.Enums;
using MyBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.DataManager
{
    public static class LogManager
    {
        private const char PATH_SEPARATOR = '_';
        private const string LOGS_DIRECTORY = "Data/Logs/";
        private const string COMMANDS_LOG_PATH = "Commands/";
        private const string EXCEPTIONS_LOG_PATH = "Exceptions/";

        public static async Task LogCommand(SocketMessage message)
        {
            string fullLogPath = Path.Combine(LOGS_DIRECTORY, COMMANDS_LOG_PATH);
            PathExtensions.CreateDirectory(fullLogPath);
            string logFile = Path.Combine(fullLogPath, GetLogFile(message).SanitizeFilePath());
            string user = message.Author.Username;
            ulong userId = message.Author.Id;
            string channel = (message.Channel as SocketTextChannel)?.Name ?? "DM";
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string logLine = $"[{time}] {user}({userId}) in {channel} : {message.Content}";
            await File.AppendAllTextAsync(logFile, logLine + Environment.NewLine);
        }

        private static string GetLogFile(SocketMessage message)
        {
            if (message.Channel is SocketTextChannel textChannel)
            {
                string serverName = textChannel.Guild.Name;
                string serverId = textChannel.Guild.Id.ToString();
                return $"S{PATH_SEPARATOR}{serverName}{PATH_SEPARATOR}{serverId}.log";
            }
            else if (message.Channel is SocketDMChannel)
            {
                string userName = message.Author.Username;
                string userId = message.Author.Id.ToString();
                return $"DM{PATH_SEPARATOR} {userName} {PATH_SEPARATOR}{userId}.log";
            }
            return "UnknownChannel.log";
        }

        public static async Task LogException(Exception exception, ExceptionType level = ExceptionType.INFORMATION)
        {
            string fullLogPath = Path.Combine(LOGS_DIRECTORY, EXCEPTIONS_LOG_PATH);
            PathExtensions.CreateDirectory(fullLogPath);
            string logFile = Path.Combine(fullLogPath, GetExceptionLogFile(level).SanitizeFilePath());
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logLine = $"[{time}] Message: {exception.Message} | StackTrace: {exception.StackTrace}";
            await File.AppendAllTextAsync(logFile, logLine + Environment.NewLine);
        }

        private static string GetExceptionLogFile(ExceptionType level)
            => $"E{PATH_SEPARATOR}{level}.log";
    }
}

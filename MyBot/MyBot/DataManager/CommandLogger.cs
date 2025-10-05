using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.DataManager
{
    public static class CommandLogger
    {
        private const char FLOOR_CHAR = '_';
        private const string logPath = "Data/Logs/";

        public static async Task LogCommand(SocketMessage message)
        {
            CreateLogDirectory();
            var logFile = $"{logPath}{SanitizeFileName(GetLogFile(message))}";

            var user = message.Author.Username;
            var userId = message.Author.Id;
            var channel = (message.Channel as SocketTextChannel)?.Name ?? "DM";
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string logLine = $"[{time}] {user}({userId}) in {channel} : {message.Content}";
            await File.AppendAllTextAsync(logFile, logLine + Environment.NewLine);
        }

        private static void CreateLogDirectory()
        {
            if(!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
        }

        private static string GetLogFile(SocketMessage message)
        {
            if(message.Channel is SocketTextChannel textChannel)
            {
                string serverName = textChannel.Guild.Name.Replace(" ", "_");
                string serverId = textChannel.Guild.Id.ToString();
                return $"S_{serverName}_{serverId}_commands.log";
            }
            else if(message.Channel is SocketDMChannel)
            {
                string userName = message.Author.Username;
                string userId = message.Author.Id.ToString();
                return $"DM_{userName}_{userId}_commands.log";
            }
            return "UnknownChannel_commands.log";
        }

        private static string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, FLOOR_CHAR);
            return name;
        }
    }
}

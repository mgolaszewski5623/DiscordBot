using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.DataManager
{
    public static class CommandManager
    {
        private static List<IMyBotCommand> _commands = new();

        private static bool _initialized = false;

        public static void Initialize()
        {
            if (_initialized)
                return;

            IEnumerable<Type> commandTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IMyBotCommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract && t.IsClass);

            foreach (Type type in commandTypes)
            {
                try
                {
                    if(Activator.CreateInstance(type) is IMyBotCommand commandInstance)
                        _commands.Add(commandInstance);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"❌ Failed to load command {type.Name}: {ex.Message}");
                }
            }

            _initialized = true;
            Console.WriteLine($"✅ Loaded {_commands.Count} commands.");
        }

        public static IMyBotCommand? GetCommand(string name, string prefix)
            => _commands.FirstOrDefault(c => string.Equals($"{prefix}{c.Name}", name, StringComparison.OrdinalIgnoreCase));

        public static List<IMyBotCommand> GetAllCommands()
            => _commands;
    }
}

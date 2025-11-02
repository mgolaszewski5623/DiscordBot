using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Extensions
{
    public static class PathExtensions
    {
        private const char FLOOR_CHAR = '_';

        public static string SanitizeFilePath(this string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, FLOOR_CHAR);
            return name;
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}

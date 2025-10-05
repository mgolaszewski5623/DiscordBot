using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Extensions
{
    public static class ExceptionExtensions
    {
        private const string SEPARATOR = " -> ";
        public static string GetCompleteMessage(this Exception exception)
        {
            string message = GetAllMessages(exception);
            return removeLastSeparator(message);
        }

        private static string GetAllMessages(Exception exception)
        {
            string message = "";
            while (exception != null)
            {
                string? exceptionMessage = exception.Message;
                bool isEmptyOrDuplicated = string.IsNullOrEmpty(exceptionMessage) || message.Contains(exceptionMessage);
                if (!isEmptyOrDuplicated)
                    message += $"{exceptionMessage}{SEPARATOR}";
                exception = exception.InnerException;
            }
            return message;
        }

        private static string removeLastSeparator(string message)
        {
            if (message.Contains(SEPARATOR))
                return message.Substring(0, message.Length - SEPARATOR.Length);
            return message;
        }
    }
}

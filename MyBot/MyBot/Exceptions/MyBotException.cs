using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Exceptions
{
    public class MyBotException : Exception
    {
        public MyBotException(string message) 
            : base(message) { }

        public MyBotException(string message, Exception inner)
            : base(message, inner) { }

        public MyBotException(Exception inner)
            : this("", inner) { }
    }
}

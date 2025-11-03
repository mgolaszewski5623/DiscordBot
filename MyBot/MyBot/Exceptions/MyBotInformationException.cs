using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Exceptions
{
    public class MyBotInformationException : MyBotException
    {
        public MyBotInformationException(string message)
            : base(message) { }

        public MyBotInformationException(string message, Exception inner)
            : base(message, inner) { }

        public MyBotInformationException(Exception inner) 
            : base(inner) { }
    }
}

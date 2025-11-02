using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Enums
{
    public enum ExceptionType
    {
        CRITICAL = 0, // Errors that cause major failures or data loss
        ERROR = 1,    // Standard errors that need attention
        WARNING = 2,  // Potential issues that may lead to errors
        DEBUG = 3,    // Detailed information for debugging purposes

        INFORMATION = 100, // General information
    }
}

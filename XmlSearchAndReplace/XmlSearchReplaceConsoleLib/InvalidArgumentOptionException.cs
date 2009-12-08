using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class InvalidArgumentOptionException : Exception
    {
        public InvalidArgumentOptionException(string message)
            : base(message)
        {
        }
    }
}

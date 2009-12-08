using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class BaseException : Exception
    {
        public BaseException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        public override string ToString()
        {
            string msg = "An error occurred:\r\n{0}\r\nDetails of error:\r\n{1}";
            return String.Format(msg, base.Message, InnerException.ToString());
        }
    }
}

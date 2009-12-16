using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class RequiredParameterMissingException : Exception
    {
        CommandLineParameterCollection _MissingParams;
        public RequiredParameterMissingException(string message, CommandLineParameterCollection missingParameters)
            : base(message)
        {
            _MissingParams = missingParameters;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.Message);
            sb.AppendLine("The following parameters are missing:");
            foreach (CommandLineParameter param in _MissingParams)
            {
                sb.AppendLine(param.GetName());
            }
            return sb.ToString();
        }

        public CommandLineParameterCollection GetMissginParameters()
        {
            return _MissingParams;
        }
    }
}

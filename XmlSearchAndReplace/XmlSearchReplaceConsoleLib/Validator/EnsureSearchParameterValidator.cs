using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class EnsureSearchParameterWithoutParamFile : IApplicationParameterValidator
    {
        public bool IsValid(CommandLineParameterWithValueCollection parameters)
        {
            return parameters.Exists(p => String.Compare("P", p.GetName(), true) == 0)
                || parameters.Exists(p => String.Compare("S", p.GetName(), true) == 0);
        }

        public string GetValidationMessage()
        {
            return "Missing search string (/S) in command line";
        }
    }
}

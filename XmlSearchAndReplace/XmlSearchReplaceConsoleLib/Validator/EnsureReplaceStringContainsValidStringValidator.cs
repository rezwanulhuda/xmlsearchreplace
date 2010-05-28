using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;
using System.Text.RegularExpressions;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class EnsureReplaceStringContainsValidStringValidator : IApplicationParameterValidator
    {
        public bool IsValid(CommandLineParameterWithValueCollection parameters)
        {
            ApplicationParameters param = new ApplicationParameters(parameters);

            if (param.GetLocationOptions().IsSet(SearchReplaceLocationOptions.ReplaceAttributeName) || param.GetLocationOptions().IsSet(SearchReplaceLocationOptions.ReplaceElementName))
            {                
                foreach (string str in param.GetReplaceString())
                {
                    if (!Utility.IsValidXmlName(str)) return false;
                }
            }

            return true;
        }

        

        public string GetValidationMessage()
        {
            return "One or more replace string contains invalid element or attribute names. When replace location contains element/attribute name, replace strings must be valid names suitable for element/attribute names";
        }
    }
}

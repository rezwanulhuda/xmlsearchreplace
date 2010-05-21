using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class EnsureEqualSearchReplaceStringValidator : IApplicationParameterValidator
    {
        

        public bool IsValid(CommandLineParameterWithValueCollection parameters)
        {
            ApplicationParameters param = new ApplicationParameters(parameters);

            return param.GetSearchString().Count == param.GetReplaceString().Count;
            //return parameters.Exists(p => String.Compare(p.GetName(), "L", true) == 0);
            //{

            //}
        }

        public string GetValidationMessage()
        {
            string msg = "Number of search and replace string must be equal. Check parameter file contains search and replace string in all of the lines.";
            return msg;
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class CheckSupportedParametersValidator
    {
        CommandLineParameterCollection _SupportedParameters;
        public CheckSupportedParametersValidator(CommandLineParameterCollection supportedParameters)
        {
            this._SupportedParameters = supportedParameters;
        }

        public bool Validate(CommandLineParameterWithValueCollection parameters)
        {
            return parameters.FindAll(p => !_SupportedParameters.Exists(q => String.Compare(q.GetName(), p.GetName(), true) == 0)).Count == 0;

        }
    }
}

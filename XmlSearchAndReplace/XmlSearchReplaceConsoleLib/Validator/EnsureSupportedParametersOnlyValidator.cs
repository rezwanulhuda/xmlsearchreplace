using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class EnsureSupportedParametersOnlyValidator : IApplicationParameterValidator
    {
        CommandLineParameterCollection _SupportedParameters;
        string _Message;
        public EnsureSupportedParametersOnlyValidator(CommandLineParameterCollection supportedParameters, string message)
        {
            this._SupportedParameters = supportedParameters;
            _Message = message;
        }
        
        public bool IsValid(CommandLineParameterWithValueCollection parameters)
        {
            return parameters.FindAll(p => !_SupportedParameters.Exists(q => String.Compare(q.GetName(), p.GetName(), true) == 0)).Count == 0;
        }

        public string GetValidationMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_Message);
            foreach (CommandLineParameter param in _SupportedParameters)
            {
                sb.AppendLine(param.GetName());
            }

            return sb.ToString();
            
        }        
    }
}

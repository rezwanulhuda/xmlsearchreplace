using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class EnsureAllMandatoryParametersArePresentValidator : IApplicationParameterValidator
    {
        CommandLineParameterCollection _MandatoryParameters;        

        public EnsureAllMandatoryParametersArePresentValidator(CommandLineParameterCollection mandatoryParameters)
        {
            _MandatoryParameters = mandatoryParameters;
        }
        public bool IsValid(CommandLineParameterWithValueCollection parameters)
        {
            return _MandatoryParameters.FindAll(p => !parameters.Exists(q => String.Compare(q.GetName(), p.GetName(), true) == 0)).Count == 0;
        }

        public string GetValidationMessage()
        {
            throw new NotImplementedException();
        }
    }
}

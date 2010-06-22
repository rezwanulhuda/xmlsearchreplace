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
            foreach (CommandLineParameter mandatoryParam in _MandatoryParameters)
            {
                CommandLineParameterWithValue suppliedParam = parameters.Find(q => String.Compare(q.GetName(), mandatoryParam.GetName(), true) == 0);
                if (suppliedParam == null) return false;
                if (String.IsNullOrEmpty(suppliedParam.GetValue())) return false;
            }
            return true;
        }

        public string GetValidationMessage()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("Following parameters are either missing or does not contain value:");
            foreach (CommandLineParameter p in _MandatoryParameters)
            {
                sb.Append(" /" + p.GetName() + ",");
            }            
            return sb.ToString().TrimEnd(new char[] {','});
        }
    }
}

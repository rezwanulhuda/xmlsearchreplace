using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class ApplicationParameterValidator
    {
        
        
        public static CommandLineParameterCollection GetMissingMandatoryParams(CommandLineParameterCollection mandatoryParams, CommandLineParameterWithValueCollection currentAppParams)
        {
            CommandLineParameterCollection missingRequiredParams = new CommandLineParameterCollection();
            foreach (CommandLineParameter param in mandatoryParams)
            {
                if (param.IsMandatory && currentAppParams.Find(p => String.Compare(p.GetName(), param.GetName(), true) == 0) == null)
                {
                    missingRequiredParams.Add(param);
                }
            }

            return missingRequiredParams;
        }
    }
}

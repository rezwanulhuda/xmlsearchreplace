using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterValueCollection : List<CommandLineParameterValue>
    {
        public CommandLineParameterCollection GetMissingMandatoryParams(CommandLineParameterCollection mandatoryParams)
        {
            CommandLineParameterCollection missingRequiredParams = new CommandLineParameterCollection();
            foreach (CommandLineParameter param in mandatoryParams)
            {
                if (param.IsMandatory && this.Find(p => String.Compare(p.GetName(), param.GetName(), true) == 0) == null)
                {
                    missingRequiredParams.Add(param);
                }
            }

            return missingRequiredParams;
        }
    }
}

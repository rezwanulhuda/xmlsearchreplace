using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterCollection : List<CommandLineParameter>
    {
        public static CommandLineParameterCollection SupporedParams
        {
            get
            {
                CommandLineParameterCollection supportedParams = new CommandLineParameterCollection();
                supportedParams.Add(new CommandLineParameter("C", String.Empty));
                supportedParams.Add(new CommandLineParameter("I", String.Empty));
                supportedParams.Add(new CommandLineParameter("F", String.Empty));
                supportedParams.Add(new CommandLineParameter("S", String.Empty));
                supportedParams.Add(new CommandLineParameter("R", String.Empty));
                supportedParams.Add(new CommandLineParameter("O", String.Empty));
                supportedParams.Add(new CommandLineParameter("W", String.Empty));

                return supportedParams;
            }
        }

        public bool IsParameterSupported(string param)
        {
            return this.Exists(p => String.Compare(p.GetName(), param, true) == 0);
        }
    }
}

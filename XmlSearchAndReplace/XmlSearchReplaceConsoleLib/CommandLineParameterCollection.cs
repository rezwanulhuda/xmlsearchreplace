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
                supportedParams.Add(new CommandLineParameter("C"));
                supportedParams.Add(new CommandLineParameter("I"));
                supportedParams.Add(new CommandLineParameter("F"));
                supportedParams.Add(new CommandLineParameter("S"));
                supportedParams.Add(new CommandLineParameter("R"));
                supportedParams.Add(new CommandLineParameter("O"));
                supportedParams.Add(new CommandLineParameter("W"));

                return supportedParams;
            }
        }

        public bool IsParameterSupported(string param)
        {
            return this.Exists(p => String.Compare(p.GetName(), param, true) == 0);
        }
    }
}

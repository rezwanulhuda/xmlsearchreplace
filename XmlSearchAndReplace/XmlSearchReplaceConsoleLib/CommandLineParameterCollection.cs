using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterCollection : List<CommandLineParameter>
    {

        private static CommandLineParameterCollection _SupportedParams;
        public static CommandLineParameterCollection SupporedParams
        {
            get
            {
                if (_SupportedParams == null)
                {
                    _SupportedParams = new CommandLineParameterCollection();
                    _SupportedParams.Add(new CommandLineParameter("S", @"""search""", HelpText.SearchString));
                    _SupportedParams.Add(new CommandLineParameter("R", @"""replace""", HelpText.ReplaceString));
                    _SupportedParams.Add(new CommandLineParameter("O", @"en,ev,an,av", HelpText.Option));
                    _SupportedParams.Add(new CommandLineParameter("F", @"""C:\Files\*.xml""", HelpText.FileName));
                    _SupportedParams.Add(new CommandLineParameter("C", String.Empty, HelpText.ContinueOnError));
                    _SupportedParams.Add(new CommandLineParameter("I", String.Empty, HelpText.IgnoreCase));
                    _SupportedParams.Add(new CommandLineParameter("W", String.Empty, HelpText.WholeWordOnly));
                }
                
                return _SupportedParams;
            }
        }

        public bool IsParameterSupported(string param)
        {
            return this.Exists(p => String.Compare(p.GetName(), param, true) == 0);
        }

        public static string GetUsage()
        {
            string usage = String.Empty;

            foreach (CommandLineParameter param in SupporedParams)
            {
                string paramUsage = param.GetUsage();
                usage += String.Format("/{0}{1} ", param.GetName(), String.IsNullOrEmpty(paramUsage) ? String.Empty : "=" + paramUsage);
            }

            return usage.Trim();
        }

        public static string GetHelpText()
        {
            string helpText = String.Empty;

            foreach (CommandLineParameter param in SupporedParams)
            {
                string paramHelp = param.GetHelpText();
                helpText += String.Format("{0} - {1}{2}", param.GetName(), paramHelp, Environment.NewLine);
            }

            helpText += HelpText.MoreInfo;
            return helpText;
        }
    }
}

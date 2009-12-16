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
                    _SupportedParams.Add(new CommandLineParameter("S", @"""search""", HelpText.SearchString, true));
                    _SupportedParams.Add(new CommandLineParameter("R", @"""replace""", HelpText.ReplaceString, true));
                    _SupportedParams.Add(new CommandLineParameter("O", @"en,ev,an,av", HelpText.Option, true));
                    _SupportedParams.Add(new CommandLineParameter("F", @"""C:\Files\*.xml""", HelpText.FileName, true));
                    _SupportedParams.Add(new CommandLineParameter("C", String.Empty, HelpText.ContinueOnError, false));
                    _SupportedParams.Add(new CommandLineParameter("I", String.Empty, HelpText.IgnoreCase, false));
                    _SupportedParams.Add(new CommandLineParameter("W", String.Empty, HelpText.WholeWordOnly, false));
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
                string paramName = param.GetName();
                paramName = GetUsageName(param, paramName);

                string paramUsage = param.GetUsage();
                usage += String.Format("{0}{1} ", paramName, String.IsNullOrEmpty(paramUsage) ? String.Empty : "=" + paramUsage);
            }

            return usage.Trim();
        }

        private static string GetUsageName(CommandLineParameter param, string paramName)
        {
            if (!param.IsMandatory)
                paramName = "[/" + paramName + "]";
            else
                paramName = "/" + paramName;
            return paramName;
        }

        public static string GetHelpText()
        {
            string helpText = String.Empty;

            foreach (CommandLineParameter param in SupporedParams)
            {                
                string paramHelp = param.GetHelpText();
                helpText += String.Format("{0} - {1}{2}{3}", param.GetName(), paramHelp, param.IsMandatory ? String.Empty : " (optional)", Environment.NewLine);
            }

            helpText += HelpText.MoreInfo;
            return helpText;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class ApplicationParameterCollection : List<ApplicationParameter>
    {

        private static ApplicationParameterCollection _SupportedParams;
        public static ApplicationParameterCollection SupporedParams
        {
            get
            {
                if (_SupportedParams == null)
                {
                    _SupportedParams = new ApplicationParameterCollection();
                    _SupportedParams.Add(new ApplicationParameter("S", @"""search""", HelpText.SearchString, false));
                    _SupportedParams.Add(new ApplicationParameter("R", @"""replace""", HelpText.ReplaceString, false));
                    _SupportedParams.Add(new ApplicationParameter("O", @"en,ev,an,av", HelpText.Option, true));
                    _SupportedParams.Add(new ApplicationParameter("F", @"""C:\Files\*.xml""", HelpText.FileName, true));
                    _SupportedParams.Add(new ApplicationParameter("C", String.Empty, HelpText.ContinueOnError, false));
                    _SupportedParams.Add(new ApplicationParameter("I", String.Empty, HelpText.IgnoreCase, false));
                    _SupportedParams.Add(new ApplicationParameter("W", String.Empty, HelpText.WholeWordOnly, false));
                    _SupportedParams.Add(new ApplicationParameter("D", String.Empty, HelpText.RecurseSubDir, false));
                    _SupportedParams.Add(new ApplicationParameter("L", String.Empty, HelpText.ReplaceSearchStringByLowerCase, false));
                    _SupportedParams.Add(new ApplicationParameter("P", @"""C:\Files\params.txt""", HelpText.ParameterFile, false));
                }
                
                return _SupportedParams;
            }
        }
        
        public static string GetUsage()
        {
            string usage = String.Empty;

            foreach (ApplicationParameter param in SupporedParams)
            {
                string paramName = param.GetName();
                paramName = GetDisplayName(param, paramName);

                string sampleValue = param.GetSampleValue();
                usage += String.Format("{0}{1} ", paramName, String.IsNullOrEmpty(sampleValue) ? String.Empty : "=" + sampleValue);
            }

            return usage.Trim();
        }

        private static string GetDisplayName(ApplicationParameter param, string paramName)
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

            foreach (ApplicationParameter param in SupporedParams)
            {                
                string paramHelp = param.GetHelpText();
                helpText += String.Format("{0} - {1}{2}{3}", param.GetName(), paramHelp, param.IsMandatory ? String.Empty : " (optional)", Environment.NewLine);
            }

            helpText += HelpText.MoreInfo;
            return helpText;
        }
    }
}

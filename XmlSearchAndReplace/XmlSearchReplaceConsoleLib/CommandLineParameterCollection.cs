﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterCollection : List<CommandLineParameter>
    {

        private static CommandLineParameterCollection _SupportedParams;

        public CommandLineParameterCollection()
            : this(null)
        {

        }

        public CommandLineParameterCollection(List<CommandLineParameter> defaultLoad)
        {
            if (defaultLoad != null)
            {
                foreach (CommandLineParameter p in defaultLoad)
                {
                    this.Add(p);
                }
            }
        }
        public static CommandLineParameterCollection SupporedParams
        {
            get
            {
                if (_SupportedParams == null)
                {
                    _SupportedParams = new CommandLineParameterCollection();
                    _SupportedParams.Add(new CommandLineParameter("S", @"""search""", HelpText.SearchString, false));
                    _SupportedParams.Add(new CommandLineParameter("R", @"""replace""", HelpText.ReplaceString, false));
                    _SupportedParams.Add(new CommandLineParameter("O", @"en,ev,an,av,va,ve", HelpText.Option, true));
                    _SupportedParams.Add(new CommandLineParameter("F", @"""C:\Files\*.xml""", HelpText.FileName, true));
                    _SupportedParams.Add(new CommandLineParameter("C", String.Empty, HelpText.ContinueOnError, false));
                    _SupportedParams.Add(new CommandLineParameter("I", String.Empty, HelpText.IgnoreCase, false));
                    _SupportedParams.Add(new CommandLineParameter("W", String.Empty, HelpText.WholeWordOnly, false));
                    _SupportedParams.Add(new CommandLineParameter("D", String.Empty, HelpText.RecurseSubDir, false));
                    _SupportedParams.Add(new CommandLineParameter("L", String.Empty, HelpText.ReplaceSearchStringByLowerCase, false));
                    _SupportedParams.Add(new CommandLineParameter("P", @"""C:\Files\params.txt""", HelpText.ParameterFile, false));
                }
                
                return _SupportedParams;
            }
        }
        
        public static string GetUsage()
        {
            string usage = String.Empty;

            foreach (CommandLineParameter param in SupporedParams)
            {
                string paramName = param.GetName();
                paramName = GetDisplayName(param, paramName);

                string sampleValue = param.GetSampleValue();
                usage += String.Format("{0}{1} ", paramName, String.IsNullOrEmpty(sampleValue) ? String.Empty : "=" + sampleValue);
            }

            return usage.Trim();
        }

        private static string GetDisplayName(CommandLineParameter param, string paramName)
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

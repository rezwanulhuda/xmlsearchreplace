using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XmlSearchReplaceLib;
using XmlSearchReplaceConsoleLib;

namespace XmlSearchReplaceConsoleLib
{
    public class ArgumentParser
    {
        CommandLineParameterValueCollection _Params;
        const string _ArgsKeyValueSeparatorCharacter = "=";

        public ArgumentParser(string[] commandLineArgs)
            : this(String.Join(" ", commandLineArgs))
        {
        }

        public ArgumentParser(string commandLine)
        {
            _Params = new CommandLineParameterValueCollection();
            CreateKeys(GetAppArgsFromCommandLine(commandLine));
        }

        private static string[] GetAppArgsFromCommandLine(string commandLine)
        {            
            string[] applicatioinArgs = commandLine.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return applicatioinArgs;
        }

        private string GetStringValue(string key)
        {
            return _Params.Find(delegate(CommandLineParameterValue k) { return String.Compare(k.GetName(), key, true) == 0; }).GetValue();
        }

        private bool GetBoolValue(string key)
        {
            CommandLineParameterValue value = _Params.Find(delegate(CommandLineParameterValue k) { return String.Compare(k.GetName(), key, true) == 0; });
            if (value == null)
                return false;
            return true;
        }
        

        private void CreateKeys(string[] allApplicatioinArgs)
        {
            foreach (string param in allApplicatioinArgs)
            {

                string arg = GetArgumentFromWholeParam(param);
                string val = GetValueFromWholeParam(param);

                CommandLineParameter commandLineParam = CommandLineParameterCollection.SupporedParams.Find(p => String.Compare(p.GetName(), arg, true) == 0);


                if (commandLineParam == null)
                    throw new ArgumentException(String.Format("Parameter '{0}' (/{1}) is not supported", arg, param));

                _Params.Add(new CommandLineParameterValue(commandLineParam, val));
            }
        }

        private string GetValueFromWholeParam(string param)
        {
            int indexOfEquals = param.IndexOf('=');
            if (indexOfEquals >= 0)
            {
                return param.Substring(indexOfEquals + 1).Trim();
            }
            return String.Empty;
        }

        private string GetArgumentFromWholeParam(string param)
        {
            int indexOfEquals = param.IndexOf('=');
            if (indexOfEquals >= 0)
            {
                return param.Substring(0, param.IndexOf('=')).Trim();
            }
            return param.Trim();
        }                
        
        public SearchReplaceLocationOptions GetLocationOptions()
        {

            string optionsS = GetStringValue("O").ToLower();            
            SearchReplaceLocationOptions options = SearchReplaceLocationOptions.ReplaceNone;
            if (optionsS.Contains("ev"))
            {
                options |= SearchReplaceLocationOptions.ReplaceElementValue;
            }

            if (optionsS.Contains("av")) 
            {
                options |= SearchReplaceLocationOptions.ReplaceAttributeValue;
            }

            if (optionsS.Contains("en"))
            {
                options |= SearchReplaceLocationOptions.ReplaceElementName;
            }

            if (optionsS.Contains("an"))
            {
                options |= SearchReplaceLocationOptions.ReplaceAttributeName;
            }

            return options;
        }

        public SearchReplaceOperationOptions GetOperationOptions()
        {
            SearchReplaceOperationOptions opOptions = SearchReplaceOperationOptions.CaseSensitivePartialWord;
            if (GetBoolValue("W"))
            {
                opOptions |= SearchReplaceOperationOptions.WholeWordOnly;
            }

            if (GetBoolValue("I"))
            {
                opOptions |= SearchReplaceOperationOptions.CaseInsensitive;
            }

            return opOptions;
        }

        public string GetFileName()
        {
            return TrimDoubleQuotes(GetStringValue("F"));
        }

        public string GetSearchString()
        {
            return TrimDoubleQuotes(GetStringValue("S"));
        }

        public string GetReplaceString()
        {
            return TrimDoubleQuotes(GetStringValue("R"));            
        }

        private string TrimDoubleQuotes(string input)
        {
            if (input.Length == 0) return input;

            if (!SurroundedByQuotes(input)) return input;

            string trimmed = input.Substring(1);
            trimmed = trimmed.Substring(0, trimmed.Length - 1);
            return trimmed;                            
            
        }

        private bool SurroundedByQuotes(string input)
        {
            return input[0] == '"' && input[input.Length - 1] == '"';                
        }

        public bool ContinueOnError
        {
            get 
            {
                return GetBoolValue("C"); 
            
            }
        }

        
    }
}

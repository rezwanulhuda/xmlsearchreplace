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

        private List<string> GetAppArgsFromCommandLine(string commandLine)
        {
            List<string> applicationArgs = GetArgumentsFromString(commandLine);
            return applicationArgs;
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
        

        private void CreateKeys(List<string> allApplicatioinArgs)
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

        public static List<string> GetArgumentsFromString(string commandLine)
        {

            List<string> argsWithValues = new List<string>();

            int x = commandLine.IndexOf('/', 0);

            while (x >= 0 && x < commandLine.Length)
            {
                int next = x + 1;
                if (next >= commandLine.Length) break;

                string arg = String.Empty;
                int iSlash = commandLine.IndexOf('/', next);
                int iEqual = commandLine.IndexOf('=', next);

                if (iSlash < 0 && next < commandLine.Length)
                {
                    arg = commandLine.Substring(next);
                    argsWithValues.Add(arg);
                    break;
                }
                else if (iEqual < 0 || iSlash < iEqual)
                {
                    arg = commandLine.Substring(next, iSlash - next).Trim();
                    argsWithValues.Add(arg);
                    x = iSlash;
                }
                else
                {
                    if (commandLine[iEqual + 1] == '"')
                    {

                        int iDblQuote = findClosingDblQuote(commandLine, iEqual + 2);                         
                        if (iDblQuote < 0)
                            throw new Exception("invalid commandline");
                        else
                        {
                            arg = commandLine.Substring(next, iDblQuote - next + 1).Trim();
                            arg = arg.Replace(@"\""", @"""");                            
                            argsWithValues.Add(arg);
                            x = iDblQuote;
                        }

                    }
                    else
                    {
                        arg = commandLine.Substring(next, iSlash - next).Trim();
                        argsWithValues.Add(arg);
                        x = iSlash;
                    }
                }
            }

            argsWithValues.RemoveAll(s => String.IsNullOrEmpty(s));

            return argsWithValues;
        }

        private static int findClosingDblQuote(string commandLine, int start)
        {
            
            while (true)
            {
                int iDblQuote = commandLine.IndexOf('"', start);
                if (iDblQuote < 0)
                    return iDblQuote;
                if (commandLine[iDblQuote - 1] == '\\')
                {
                    start = iDblQuote + 1;
                }
                else
                {
                    return iDblQuote;
                }
            }

        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XmlSearchReplaceLib;
using XmlSearchReplaceConsoleLib;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandlineParser
    {
        ApplicationParameterWithValueCollection _Params;
        const string _ArgsKeyValueSeparatorCharacter = "=";

        public CommandlineParser(string[] commandLineArgs)
            : this(String.Join(" ", commandLineArgs))
        {
        }

        public CommandlineParser(string commandLine)
        {
            _Params = new ApplicationParameterWithValueCollection();
            CreateKeys(GetAppArgsFromCommandLine(commandLine));
            
            ApplicationParameterCollection missingParams = _Params.GetMissingMandatoryParams(ApplicationParameterCollection.SupporedParams);
            if (missingParams.Count > 0)
            {
                throw new RequiredParameterMissingException("Required parameter missing", missingParams);
            }
        }

        private List<string> GetAppArgsFromCommandLine(string commandLine)
        {
            List<string> applicationArgs = GetArgumentsFromString(commandLine);
            return applicationArgs;
        }

        
        

        private void CreateKeys(List<string> allApplicatioinArgs)
        {
            foreach (string param in allApplicatioinArgs)
            {

                string arg = GetArgumentFromWholeParam(param);
                string val = GetValueFromWholeParam(param);

                ApplicationParameter commandLineParam = ApplicationParameterCollection.SupporedParams.Find(p => String.Compare(p.GetName(), arg, true) == 0);


                if (commandLineParam == null)
                    throw new ArgumentException(String.Format("Parameter '{0}' (/{1}) is not supported", arg, param));

                _Params.Add(new ApplicationParameterWithValue(commandLineParam, val));
            }
        }

        private string GetValueFromWholeParam(string param)
        {
            int indexOfEquals = param.IndexOf('=');
            if (indexOfEquals >= 0)
            {
                return TrimDoubleQuotes(param.Substring(indexOfEquals + 1).Trim());
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

                        int iDblQuote = FindClosingDblQuote(commandLine, iEqual + 2);
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

        private static int FindClosingDblQuote(string commandLine, int start)
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

        private string TrimDoubleQuotes(string input)
        {
            if (input.Length == 0) return input;

            if (!SurroundedByQuotes(input)) return input;

            return input.Substring(1, input.Length - 2);
        }

        private bool SurroundedByQuotes(string input)
        {
            return input[0] == '"' && input[input.Length - 1] == '"';
        }      
  
        public ApplicationParameterWithValueCollection GetParamsAndValues()
        {
            return this._Params;
        }
    }
}

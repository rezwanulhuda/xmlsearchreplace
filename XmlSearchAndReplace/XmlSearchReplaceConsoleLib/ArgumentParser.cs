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
        List<KeyValuePair<string, string>> _Keys;
        const string _ArgsKeyValueSeparatorCharacter = "=";

        public ArgumentParser(string[] commandLineArgs)
            : this(String.Join(" ", commandLineArgs))
        {
            
                        
        }



        public ArgumentParser(string commandLine)
        {
            _Keys = new List<KeyValuePair<string, string>>();
            CreateKeys(GetAppArgsFromCommandLine(commandLine));
        }

        private static string[] GetAppArgsFromCommandLine(string commandLine)
        {            
            string[] applicatioinArgs = commandLine.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return applicatioinArgs;
        }

        private string GetStringValue(string key)
        {
            return _Keys.Find(delegate(KeyValuePair<string, string> k) { return String.Compare(k.Key, key, true) == 0; }).Value;
        }

        private bool GetBoolValue(string key)
        {
            if (_Keys.Exists(delegate(KeyValuePair<string, string> k) { return String.Compare(k.Key, key, true) == 0; }))
                return true;

            return false;
        }
        

        private void CreateKeys(string[] allApplicatioinArgs)
        {
            foreach (string param in allApplicatioinArgs)
            {
                string[] argParts = param.Split(_ArgsKeyValueSeparatorCharacter.ToCharArray());

                if (argParts.Length > 2)
                    throw new InvalidArgumentOptionException(String.Format("The command line parameter '{0}' is invalid", param));
                                
                string arg = argParts[0].Trim();

                if (String.IsNullOrEmpty(arg))
                {
                    throw new InvalidArgumentOptionException(String.Format("The command line parameter '{0}' has an empty option", param));
                }

                string val = String.Empty;
                if (argParts.Length > 1)
                {
                    val = argParts[1].Trim();
                }
                
                _Keys.Add(new KeyValuePair<string,string>(arg, val));
            }
        }        

        //public KeyValuePair<string, string> this[int index]
        //{
        //    get 
        //    {
        //        return _Keys[index];
        //    }           
        //}
        
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
            return input.TrimEnd("\"".ToCharArray()).TrimStart("\"".ToCharArray());
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

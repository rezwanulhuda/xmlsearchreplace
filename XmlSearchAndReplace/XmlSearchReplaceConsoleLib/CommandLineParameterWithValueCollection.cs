using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;
using System.IO;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterWithValueCollection : List<CommandLineParameterWithValue>, ISearchReplaceParameter
    {                        
        private string GetStringValue(string paramName)
        {
            CommandLineParameterWithValue found = this.Find(delegate(CommandLineParameterWithValue k) { return String.Compare(k.GetName(), paramName, true) == 0; });

            if (found != null) return found.GetValue();

            throw new ArgumentException("Parameter {0} could not be found", paramName);
            //return found != null ? found.GetValue() : String.Empty;
        }

        private bool GetBoolValue(string paramName)
        {
            CommandLineParameterWithValue value = this.Find(delegate(CommandLineParameterWithValue k) { return String.Compare(k.GetName(), paramName, true) == 0; });
            if (value == null)
                return false;
            return true;
        }

        public SearchReplaceLocationOptions GetLocationOptions()
        {

            string optionsS = this.GetStringValue("O").ToLower();
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
            if (this.GetBoolValue("W"))
            {
                opOptions |= SearchReplaceOperationOptions.WholeWordOnly;
            }

            if (this.GetBoolValue("I"))
            {
                opOptions |= SearchReplaceOperationOptions.CaseInsensitive;
            }

            return opOptions;
        }

        public string GetFileName()
        {
            return this.GetStringValue("F");
        }

        public List<string> GetSearchString()
        {
            LoadValuesFromParamFileIfPresent();
            string paramFile = GetParamFileName();

            if (!String.IsNullOrEmpty(paramFile))
            {
                FileParamReader reader = new FileParamReader(paramFile);
                return reader.GetAllSearchStrings();
            }
        

            return new List<string>() { GetStringValue("S") };
        }

        private string GetParamFileName()
        {
            if (HasParamFile()) return GetStringValue("P");
            return String.Empty;
        }

        FileParamReader _Reader = null;

        private void LoadValuesFromParamFileIfPresent()
        {
            if (!HasParamFile()) return;
            if (_Reader != null) return;
            
            _Reader = new FileParamReader(GetStringValue("P"));
        }

        bool? _HasParamFile;

        private bool HasParamFile()
        {
            //return !String.IsNullOrEmpty(GetStringValue("P"));
            if (_HasParamFile.HasValue) return _HasParamFile.Value;
            try
            {
                GetStringValue("P");
                _HasParamFile = true;
            }
            catch (ArgumentException)
            {
                _HasParamFile = false;
            }

            return _HasParamFile.Value;
        }

        public List<string> GetReplaceString()
        {
            LoadValuesFromParamFileIfPresent();
            List<string> replaceStrings = null;

            if (HasParamFile())
            {
                if (GetBoolValue("L"))
                {
                    replaceStrings = ToLowerArray(_Reader.GetAllSearchStrings());
                }
                else
                {
                    replaceStrings = _Reader.GetAllReplaceStrings();
                }
            }            
            else
            {
                if (this.GetBoolValue("L"))
                {
                    replaceStrings = ToLowerArray(new List<string>() { this.GetStringValue("S")});
                }
                else
                {
                    replaceStrings = new List<string>() { this.GetStringValue("R") };
                }
            }


            return replaceStrings;
        }

        public bool ContinueOnError
        {
            get
            {
                return this.GetBoolValue("C");

            }
        }

        public bool RecurseSubDir
        {
            get
            {
                return this.GetBoolValue("D");
            }
        }

        private List<string> ToLowerArray(List<string> values)
        {
            List<string> lowered = new List<string>();
            foreach (string value in values)
            {
                lowered.Add(value.ToLower());
                
            }
            return lowered;
            
        }

    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;

namespace XmlSearchReplaceConsoleLib
{
    public class ApplicationParameters : ISearchReplaceParameter
    {

        CommandLineParameterWithValueCollection _Parameters;
        public ApplicationParameters(CommandLineParameterWithValueCollection parameters)
        {
            this._Parameters = parameters;
        }

        public bool ContinueOnError
        {
            get
            {
                return _Parameters.GetBoolValue("C");

            }
        }

        public bool RecurseSubDir
        {
            get
            {
                return _Parameters.GetBoolValue("D");
            }
        }

        public List<string> GetReplaceString()
        {
            LoadValuesFromParamFileIfPresent();
            List<string> replaceStrings = null;

            if (HasParamFile())
            {
                //if (_Parameters.GetBoolValue("L"))
                //{
                //    replaceStrings = ToLowerArray(_Reader.GetAllSearchStrings());
                //}
                //else
                //{
                    
                //}
                replaceStrings = _Reader.GetAllReplaceStrings();
            }
            else
            {
                if (_Parameters.GetBoolValue("L"))
                {
                    replaceStrings = ToLowerArray(new List<string>() { _Parameters.GetStringValue("S") });
                }
                else
                {
                    replaceStrings = new List<string>() { _Parameters.GetStringValue("R") };
                }
            }


            return replaceStrings;
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

        private string GetParamFileName()
        {
            if (HasParamFile()) return _Parameters.GetStringValue("P");
            return String.Empty;
        }

        FileParamReader _Reader = null;

        private void LoadValuesFromParamFileIfPresent()
        {
            if (!HasParamFile()) return;
            if (_Reader != null) return;

            _Reader = new FileParamReader(_Parameters.GetStringValue("P"));
        }

        bool? _HasParamFile;

        private bool HasParamFile()
        {
            //return !String.IsNullOrEmpty(GetStringValue("P"));
            if (_HasParamFile.HasValue) return _HasParamFile.Value;
            try
            {
                _Parameters.GetStringValue("P");
                _HasParamFile = true;
            }
            catch (ArgumentException)
            {
                _HasParamFile = false;
            }

            return _HasParamFile.Value;
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


            return new List<string>() { _Parameters.GetStringValue("S") };
        }

        public string GetFileName()
        {
            return _Parameters.GetStringValue("F");
        }

        public SearchReplaceOperationOptions GetOperationOptions()
        {
            SearchReplaceOperationOptions opOptions = SearchReplaceOperationOptions.CaseSensitivePartialWord;
            if (_Parameters.GetBoolValue("W"))
            {
                opOptions |= SearchReplaceOperationOptions.WholeWordOnly;
            }

            if (_Parameters.GetBoolValue("I"))
            {
                opOptions |= SearchReplaceOperationOptions.CaseInsensitive;
            }

            return opOptions;
        }

        public SearchReplaceLocationOptions GetLocationOptions()
        {

            string optionsS = _Parameters.GetStringValue("O").ToLower();
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

    }
}

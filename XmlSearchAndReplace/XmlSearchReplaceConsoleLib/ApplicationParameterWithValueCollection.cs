using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;

namespace XmlSearchReplaceConsoleLib
{
    public class ApplicationParameterWithValueCollection : List<ApplicationParameterWithValue>, ISearchReplaceParameter
    {
        public ApplicationParameterCollection GetMissingMandatoryParams(ApplicationParameterCollection mandatoryParams)
        {
            ApplicationParameterCollection missingRequiredParams = new ApplicationParameterCollection();
            foreach (ApplicationParameter param in mandatoryParams)
            {
                if (param.IsMandatory && this.Find(p => String.Compare(p.GetName(), param.GetName(), true) == 0) == null)
                {
                    missingRequiredParams.Add(param);
                }
            }

            return missingRequiredParams;
        }        

        private string GetStringValue(string paramName)
        {
            return this.Find(delegate(ApplicationParameterWithValue k) { return String.Compare(k.GetName(), paramName, true) == 0; }).GetValue();
        }

        private bool GetBoolValue(string paramName)
        {
            ApplicationParameterWithValue value = this.Find(delegate(ApplicationParameterWithValue k) { return String.Compare(k.GetName(), paramName, true) == 0; });
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
            return new List<string>() { this.GetStringValue("S") };
        }

        public List<string> GetReplaceString()
        {
            if (this.GetBoolValue("L"))
                return new List<string>() { this.GetSearchString()[0].ToLower()};
            else
                return new List<string>() { this.GetStringValue("R")};
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

    }
}

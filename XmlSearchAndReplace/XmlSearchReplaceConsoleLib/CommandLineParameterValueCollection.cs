using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterValueCollection : List<CommandLineParameterValue>, ISearchReplaceParameter
    {
        public CommandLineParameterCollection GetMissingMandatoryParams(CommandLineParameterCollection mandatoryParams)
        {
            CommandLineParameterCollection missingRequiredParams = new CommandLineParameterCollection();
            foreach (CommandLineParameter param in mandatoryParams)
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
            return this.Find(delegate(CommandLineParameterValue k) { return String.Compare(k.GetName(), paramName, true) == 0; }).GetValue();
        }

        private bool GetBoolValue(string paramName)
        {
            CommandLineParameterValue value = this.Find(delegate(CommandLineParameterValue k) { return String.Compare(k.GetName(), paramName, true) == 0; });
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

        public string GetSearchString()
        {
            return this.GetStringValue("S");
        }

        public string GetReplaceString()
        {
            return this.GetStringValue("R");
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

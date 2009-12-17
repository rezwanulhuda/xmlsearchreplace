using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;

namespace XmlSearchReplaceConsoleLib
{
    public class SearchReplaceParameter
    {
        CommandLineParameterValueCollection _ParamsWithValues;
        public SearchReplaceParameter(CommandLineParameterValueCollection paramsWithValues)
        {
            this._ParamsWithValues = paramsWithValues;
        }

        public SearchReplaceLocationOptions GetLocationOptions()
        {

            string optionsS = _ParamsWithValues.GetStringValue("O").ToLower();
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
            if (_ParamsWithValues.GetBoolValue("W"))
            {
                opOptions |= SearchReplaceOperationOptions.WholeWordOnly;
            }

            if (_ParamsWithValues.GetBoolValue("I"))
            {
                opOptions |= SearchReplaceOperationOptions.CaseInsensitive;
            }

            return opOptions;
        }



        public string GetFileName()
        {
            //return TrimDoubleQuotes(_Params.GetStringValue("F"));
            return _ParamsWithValues.GetStringValue("F");
        }

        public string GetSearchString()
        {
            return _ParamsWithValues.GetStringValue("S");
        }

        public string GetReplaceString()
        {
            return _ParamsWithValues.GetStringValue("R");
        }

        public bool ContinueOnError
        {
            get
            {
                return _ParamsWithValues.GetBoolValue("C");

            }
        }        

    }
}

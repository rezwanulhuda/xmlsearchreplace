using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib.Engine
{
    public abstract class ReplacerEngine : IReplacerEngine
    {

        public static IReplacerEngine CreateEngine(ReplacerEngineType type, SearchReplaceOperationOptions options)
        {
            switch (type)
            {
                case ReplacerEngineType.StringEngine:
                    return new StringReplacerEngine(options);
                default:
                    throw new ArgumentException(String.Format("Invalid type '{0}' specified when creating search replace engine.", type));
            }
        }
        
        protected ReplacementOptionValidatorList _Validators;

        public ReplacerEngine ()
	    {
            _Validators = new ReplacementOptionValidatorList();

	    }

        public string Replace(string actualString, string searchString, string replaceString)
        {
            if (_Validators.IsValidForReplacement(actualString, searchString, replaceString))
                return ReplaceString(actualString, searchString, replaceString);
            else
                return actualString;
        }

        protected bool HasOperationOption(SearchReplaceOperationOptions availableOptions, SearchReplaceOperationOptions checkOption)
        {
            return ((availableOptions & checkOption) == checkOption);
        }

        protected abstract string ReplaceString(string actualString, string searchString, string replaceString);
    }
}

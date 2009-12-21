using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class StringReplacerEngine : IReplacerEngine
    {
        protected ReplacementOptionValidatorList _Validators;
        public StringReplacerEngine(ReplacementOptionValidatorList validators)
        {
            _Validators = validators;
        }

        public string Replace(string actualString, string searchString, string replaceString)
        {
            if (_Validators.IsValidForReplacement(actualString, searchString, replaceString))
                return ReplaceString(actualString, searchString, replaceString);
            else
                return actualString;
        }

        private string ReplaceString(string actualString, string searchString, string replaceString)
        {
            string replaced;
            int start = actualString.ToLower().IndexOf(searchString.ToLower());
            replaced = actualString.Remove(start, searchString.Length);
            replaced = replaced.Insert(start, replaceString);
            return replaced;
        }
    }
}

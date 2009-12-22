using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class StringCaseSensitiveValidator : IReplacementOptionValidator
    {
        public bool IsValidForReplacement(string actualString, string searchString, string replaceString)
        {
            return actualString.Contains(searchString);
        }
    }
}

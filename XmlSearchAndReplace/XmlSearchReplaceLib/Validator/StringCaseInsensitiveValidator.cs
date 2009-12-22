using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class StringCaseInsensitiveValidator : IReplacementOptionValidator
    {
        public bool IsValidForReplacement(string actualString, string searchString, string replaceString)
        {
            return actualString.ToLower().Contains(searchString.ToLower());
        }
    }
}

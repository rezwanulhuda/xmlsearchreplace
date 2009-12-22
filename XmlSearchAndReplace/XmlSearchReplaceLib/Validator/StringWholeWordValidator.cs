using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class StringWholeWordValidator : IReplacementOptionValidator
    {
        public bool IsValidForReplacement(string actualString, string searchString, string replaceString)
        {
            return String.Compare(actualString, searchString, true) == 0;
        }
    }
}

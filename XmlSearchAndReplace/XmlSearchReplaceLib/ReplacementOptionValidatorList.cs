using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class ReplacementOptionValidatorList : List<IReplacementOptionValidator>
    {
        public bool IsValidForReplacement(string actualString, string searchString, string replaceString)
        {
            foreach (IReplacementOptionValidator validator in this)
            {
                if (!validator.IsValidForReplacement(actualString, searchString, replaceString))
                    return false;
            }
            return true;
        }
    }
}

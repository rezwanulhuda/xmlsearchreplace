using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class ReplacementOptionValidatorList : List<IReplacementOptionValidator>
    {
        public bool IsValidForReplacement(string actualString, string searchString)
        {
            foreach (IReplacementOptionValidator validator in this)
            {
                if (!validator.IsValidForReplacement(actualString, searchString))
                    return false;
            }
            return true;
        }
    }
}

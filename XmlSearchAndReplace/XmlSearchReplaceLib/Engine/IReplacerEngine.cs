using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public interface IReplacerEngine
    {
        string Replace(string actualString, string searchString, string replaceString);
        bool IsValidForReplacement(string actualString, string searchString);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    [Flags]
    public enum SearchReplaceOperationOptions
    {   
        CaseSensitivePartialWord = 0x0
        , WholeWordOnly = 0x1
        , CaseInsensitive = 0x2
    }
}

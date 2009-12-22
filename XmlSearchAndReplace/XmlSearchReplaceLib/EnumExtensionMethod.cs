using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public static class EnumExtensionMethod
    {
        public static bool IsSet(this SearchReplaceLocationOptions options, SearchReplaceLocationOptions option)
        {            
            return (options & option) == option;
        }

        public static bool IsSet(this SearchReplaceOperationOptions options, SearchReplaceOperationOptions option)
        {
            return (options & option) == option;
        }
    }
}

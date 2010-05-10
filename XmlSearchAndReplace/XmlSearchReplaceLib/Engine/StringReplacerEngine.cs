using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib.Engine;

namespace XmlSearchReplaceLib
{
    public class StringReplacerEngine : AbstractReplacerEngine
    {
        public StringReplacerEngine(SearchReplaceOperationOptions options)
        {            

            if (options.IsSet(SearchReplaceOperationOptions.CaseInsensitive))
                _Validators.Add(new StringCaseInsensitiveValidator());
            else
                _Validators.Add(new StringCaseSensitiveValidator());

            if (options.IsSet(SearchReplaceOperationOptions.WholeWordOnly))
                _Validators.Add(new StringWholeWordValidator());
            else
                _Validators.Add(new StringPartialWordValidator());

        }        

        protected override string ReplaceString(string actualString, string searchString, string replaceString)
        {
            string replaced;
            int start = actualString.ToLower().IndexOf(searchString.ToLower());
            replaced = actualString.Remove(start, searchString.Length);
            replaced = replaced.Insert(start, replaceString);
            return replaced;
        }

        
    }
}

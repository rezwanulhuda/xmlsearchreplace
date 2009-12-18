using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;

namespace XmlSearchReplaceConsoleLib
{
    public interface ISearchReplaceParameter
    {
        bool ContinueOnError { get; }
        string GetReplaceString();
        string GetSearchString();
        string GetFileName();
        SearchReplaceOperationOptions GetOperationOptions();
        SearchReplaceLocationOptions GetLocationOptions();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public static class HelpText
    {
        public const string Option = "Options for searching. Can have the following values: ev - element value, en - element name, an - attribute name, av - attribute value, va - value of attribute matching search string, ve - value of element matching search string. Separate options with comma, semicolon or space";
        public const string SearchString = "String to search in the xml. If string contains front slash (/) then it must be surrounded by double quotes(\"). If \" needs to be searched, then use \\\" with the entire string inside \"";
        public const string ReplaceString = "String to replace with in the xml. If string contains front slash (/) then it must be surrounded by double quotes(\"). If \" needs to be searched, then use \\\" with the entire string inside \"";
        public const string WholeWordOnly = "Whole word search. When searching values, whole word means the entire value.";
        public const string IgnoreCase = "Ignore case. Default is case sensitive search.";
        public const string FileName = "File name to perform search and replace on. Must be a valid xml file. Supports common windows wild cards * and ?. If wild cards are specified, searches all sub directories.";
        public const string ContinueOnError = "Continue on error when processing multiple files.";
        public const string RecurseSubDir = "Recurse sub directories.";
        public const string MoreInfo = "For more information - please visit the project page at CodePlex (http://xmlsearchreplace.codeplex.com/documentation)";
        public const string ReplaceSearchStringByLowerCase = "Replace the search string by a lower case version of the search string.";
        public const string ParameterFile = "Specify file name containing multiple search and replace strings. Each line in file should contain a search and replace string. Only /S, /R and /L options are supported. Syntax is same as command line.";
    }
}

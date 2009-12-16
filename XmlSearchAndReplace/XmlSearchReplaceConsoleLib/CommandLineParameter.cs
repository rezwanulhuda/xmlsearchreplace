using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameter
    {        
        string _Name;
        string _Usage;
        string _HelpText;

        public CommandLineParameter(string name, string usage, string helpText)
        {        
            this._Name = name;
            this._Usage = usage;
            this._HelpText = helpText;
        }     
        public string GetName() { return _Name; }

        public string GetUsage()
        {
            return _Usage;
        }

        public string GetHelpText()
        {
            return _HelpText;
        }
    }
}

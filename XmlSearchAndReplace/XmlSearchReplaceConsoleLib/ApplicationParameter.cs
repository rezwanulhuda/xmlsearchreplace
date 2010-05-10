using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class ApplicationParameter
    {        
        string _Name;
        string _Usage;
        string _HelpText;
        bool _IsMandatory;

        public ApplicationParameter(string name, string usage, string helpText, bool isMandatory)
        {        
            this._Name = name;
            this._Usage = usage;
            this._HelpText = helpText;
            this._IsMandatory = isMandatory;
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

        public bool IsMandatory
        {
            get { return _IsMandatory; }
        }
    }
}

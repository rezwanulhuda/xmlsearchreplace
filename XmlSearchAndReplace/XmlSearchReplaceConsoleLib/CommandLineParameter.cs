using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameter
    {        
        string _Name;

        public CommandLineParameter(string name)
        {        
            this._Name = name;
        }     
        public string GetName() { return _Name; }
    }
}

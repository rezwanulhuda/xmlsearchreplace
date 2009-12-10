using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameter
    {
        string _Value;
        string _Name;

        public CommandLineParameter(string name, string value)
        {
            this._Value = value;
            this._Name = name;
        }

        public string GetValue() { return _Value; }
        public string GetName() { return _Name; }        


    }
}

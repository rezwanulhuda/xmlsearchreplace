using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterValue
    {
        protected CommandLineParameter _Parameter;
        protected string _Value;

        public CommandLineParameterValue(CommandLineParameter parameter, string value)
        {
            this._Parameter = parameter;
            this._Value = value;
        }

        public string GetValue()
        {
            return _Value;
        }        

        public string GetName()
        {
            return this._Parameter.GetName();
        }
    }
}

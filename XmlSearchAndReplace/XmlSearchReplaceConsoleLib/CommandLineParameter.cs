﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameter
    {        
        string _Name;
        string _SampleValue;
        string _HelpText;
        bool _IsMandatory;

        public CommandLineParameter(string name)
            : this(name, String.Empty, String.Empty, false)
        {

        }
        public CommandLineParameter(string name, string sampleValue, string helpText, bool isMandatory)
        {        
            this._Name = name;
            this._SampleValue = sampleValue;
            this._HelpText = helpText;
            this._IsMandatory = isMandatory;
        }     
        public string GetName() { return _Name; }

        public string GetSampleValue()
        {
            return _SampleValue;
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

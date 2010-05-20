using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceLib;
using System.IO;

namespace XmlSearchReplaceConsoleLib
{
    public class CommandLineParameterWithValueCollection : List<CommandLineParameterWithValue>//, ISearchReplaceParameter
    {                        
        public string GetStringValue(string paramName)
        {
            CommandLineParameterWithValue found = this.Find(delegate(CommandLineParameterWithValue k) { return String.Compare(k.GetName(), paramName, true) == 0; });

            if (found != null) return found.GetValue();

            throw new ArgumentException("Parameter {0} could not be found", paramName);
            //return found != null ? found.GetValue() : String.Empty;
        }

        public bool GetBoolValue(string paramName)
        {
            CommandLineParameterWithValue value = this.Find(delegate(CommandLineParameterWithValue k) { return String.Compare(k.GetName(), paramName, true) == 0; });
            if (value == null)
                return false;
            return true;
        }

        

        

        

        

        

        

    }
    
}

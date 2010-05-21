using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    

    public class DefaultParameterValidator : AbstractParameterValidator
    {
        public DefaultParameterValidator()
        {
            this.Add(new EnsureEqualSearchReplaceStringValidator());
        }        
    }

    
}

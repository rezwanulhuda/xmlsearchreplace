using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public interface IApplicationParameterValidator
    {
        bool IsValid(CommandLineParameterWithValueCollection parameters);
        string GetValidationMessage();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class EnsureSearchParameterWithParamFileValidator : IApplicationParameterValidator
    {
        
        public bool IsValid(CommandLineParameterWithValueCollection parameters)
        {
            if (parameters.Exists(p => String.Compare("P", p.GetName(), true) == 0))
            {
                string paramFile = parameters.GetStringValue("P");
                FileParamReader reader = new FileParamReader(paramFile);
                return (reader.GetAllSearchStrings().Count == File.ReadAllLines(paramFile).Length);
            }
            return true;
        }

        public string GetValidationMessage()
        {
            throw new NotImplementedException();
        }
    }
}

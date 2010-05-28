using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public abstract class AbstractParameterValidatorGroup : List<IApplicationParameterValidator>
    {
        public void CheckParameters(CommandLineParameterWithValueCollection parameters)
        {
            foreach (IApplicationParameterValidator validator in this)
            {
                if (!validator.IsValid(parameters))
                {
                    throw new InvalidArgumentOptionException(validator.GetValidationMessage());
                }
            }
        }
    }
}

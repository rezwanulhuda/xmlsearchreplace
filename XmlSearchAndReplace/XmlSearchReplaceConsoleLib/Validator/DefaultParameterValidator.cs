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
            this.Add(new EnsureAllMandatoryParametersArePresentValidator(
                new CommandLineParameterCollection(CommandLineParameterCollection.SupporedParams.FindAll(p => p.IsMandatory)))                
                );
            this.Add(new EnsureSearchParameterWithoutParamFile());
            this.Add(new EnsureSearchParameterWithParamFileValidator());
            this.Add(new EnsureEqualSearchReplaceStringValidator());
        }        
    }

    
}

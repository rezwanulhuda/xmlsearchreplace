using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    

    public class DefaultParameterValidator : AbstractParameterValidatorGroup
    {
        public DefaultParameterValidator()
        {
            this.Add(new EnsureSupportedParametersOnlyValidator(CommandLineParameterCollection.SupporedParams
                , "Application only supports the following parameters:"));
            this.Add(new EnsureAllMandatoryParametersArePresentValidator(
                new CommandLineParameterCollection(
                    CommandLineParameterCollection.SupporedParams.FindAll(p => p.IsMandatory)))
                );
            this.Add(new EnsureSearchParameterWithoutParamFileValidator());
            this.Add(new EnsureSearchParameterWithParamFileValidator());
            this.Add(new EnsureEqualSearchReplaceStringValidator());
            //this.Add(new EnsureReplaceStringContainsValidStringValidator());
        }        
    }

    
}

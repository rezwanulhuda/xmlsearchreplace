using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class FileParameterReaderValidator : AbstractParameterValidator
    {               
        public FileParameterReaderValidator()
        {
            this.Add(new SupportedParametersValidator(
                    new CommandLineParameterCollection() { 
                        new CommandLineParameter("S")
                        , new CommandLineParameter("R")}
                    , "Parameter file only supports the following parameters:"));
        }
    }
}

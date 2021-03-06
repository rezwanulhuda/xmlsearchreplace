﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib.Validator
{
    public class FileParameterReaderValidator : AbstractParameterValidatorGroup
    {               
        public FileParameterReaderValidator()
        {
            this.Add(new EnsureSupportedParametersOnlyValidator(
                    new CommandLineParameterCollection() { 
                        new CommandLineParameter("S")
                        , new CommandLineParameter("R")
                        , new CommandLineParameter("L") }
                    , "Parameter file only supports the following parameters:"));
        }
    }
}

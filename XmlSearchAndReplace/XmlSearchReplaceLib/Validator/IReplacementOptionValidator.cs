﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public interface IReplacementOptionValidator
    {
        bool IsValidForReplacement(string actualString, string searchString);
    }
}

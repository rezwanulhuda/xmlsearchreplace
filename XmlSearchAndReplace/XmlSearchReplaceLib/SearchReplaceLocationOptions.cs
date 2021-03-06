﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    [Flags]
    public enum SearchReplaceLocationOptions
    {
        ReplaceNone = 0x0
        , ReplaceAll = SearchReplaceLocationOptions.ReplaceAttributeName
          | SearchReplaceLocationOptions.ReplaceAttributeValue
          | SearchReplaceLocationOptions.ReplaceElementName
          | SearchReplaceLocationOptions.ReplaceElementValue
          | SearchReplaceLocationOptions.ReplaceValueOfElement
          | SearchReplaceLocationOptions.ReplaceValueOfAttribute
        , ReplaceElementValue = 0x1
        , ReplaceAttributeValue = 0x2
        , ReplaceElementName = 0x4
        , ReplaceAttributeName = 0x8
        , ReplaceValueOfElement = 0x10
        , ReplaceValueOfAttribute = 0x20

    }    
}

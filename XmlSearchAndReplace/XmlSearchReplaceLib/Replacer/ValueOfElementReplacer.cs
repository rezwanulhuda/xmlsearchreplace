using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlSearchReplaceLib
{
    class ValueOfElementReplacer : IXmlSearchAndReplacer
    {
        

        public void Replace(XmlNode node, string searchString, string replaceString, IReplacerEngine engine)
        {
            if (node.NodeType != XmlNodeType.Element) return;           


            if (engine.IsValidForReplacement(node.LocalName, searchString))
                node.InnerText = replaceString;
        }

        
    }
}

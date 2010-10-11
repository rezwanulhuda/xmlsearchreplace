using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlSearchReplaceLib
{
    public class ValueOfAttributeReplacer : IXmlSearchAndReplacer
    {        

        public void Replace(XmlNode node, string searchString, string replaceString, IReplacerEngine engine)
        {
            if (node.Attributes == null) return;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                ReplaceValueOfAttribute(attribute, searchString, replaceString, engine);
            } 
        }

        private void ReplaceValueOfAttribute(XmlAttribute attribute, string searchString, string replaceString, IReplacerEngine engine)
        {
            if (engine.IsValidForReplacement(attribute.LocalName, searchString))
            {
                attribute.Value = replaceString;
            }
        }
    }
}

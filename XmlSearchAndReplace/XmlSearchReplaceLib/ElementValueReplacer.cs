using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlSearchReplaceLib
{
    public class ElementValueReplacer : IXmlSearchAndReplacer
    {
        public void Replace(XmlNode node, string searchString, string replaceString, IReplacerEngine engine)
        {
            if (node.NodeType != XmlNodeType.Text) return;

            //IReplacerEngine engine;
            //if (wholeWordOnly)
            //    engine = new WholeWordOnlyReplacerEngine();
            //else
            //    engine = new PartialWordReplacerEngine();

            node.InnerText = engine.Replace(node.InnerText, searchString, replaceString);
        }
    }
}

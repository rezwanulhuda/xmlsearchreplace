using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlSearchReplaceLib
{
    public interface IXmlSearchAndReplacer
    {
        void Replace(XmlNode node, string searchString, string replaceString, IReplacerEngine engine);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlSearchReplaceLib
{
    class AttributeNameReplacer : IXmlSearchAndReplacer
    {
        public void Replace(XmlNode node, string searchString, string replaceString, IReplacerEngine engine)
        {
            if (node.Attributes == null) return;

            List<KeyValuePair<XmlAttribute, XmlAttribute>> replacedAttributes = new List<KeyValuePair<XmlAttribute, XmlAttribute>>();

            FindReplacableAttributes(node, searchString, replaceString, engine, replacedAttributes);

            ReplaceAttributes(node, replacedAttributes);
        }

        private static void ReplaceAttributes(XmlNode node, List<KeyValuePair<XmlAttribute, XmlAttribute>> replacedAttributes)
        {
            foreach (KeyValuePair<XmlAttribute, XmlAttribute> attributes in replacedAttributes)
            {
                node.Attributes.InsertAfter(attributes.Value, attributes.Key);
                node.Attributes.Remove(attributes.Key);
            }
        }

        private static void FindReplacableAttributes(XmlNode node, string searchString, string replaceString, IReplacerEngine engine, List<KeyValuePair<XmlAttribute, XmlAttribute>> replacedAttributes)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                string newAttributeName = engine.Replace(attribute.Name, searchString, replaceString);


                if (String.Compare(newAttributeName, attribute.Name) != 0)
                {
                    XmlAttribute newAttribute = node.OwnerDocument.CreateAttribute(newAttributeName);
                    newAttribute.Value = attribute.Value;
                    replacedAttributes.Add(new KeyValuePair<XmlAttribute, XmlAttribute>(attribute, newAttribute));
                }
            }
        }

        //void ReplaceValueOfAttribute(XmlAttribute attribute, string searchString, string replaceString)
        //{
        //    attribute.Value = attribute.Value.Replace(searchString, replaceString);
        //}
    }
}

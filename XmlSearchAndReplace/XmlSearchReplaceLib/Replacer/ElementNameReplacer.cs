using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlSearchReplaceLib
{
    public class ElementNameReplacer : IXmlSearchAndReplacer
    {
        public void Replace(XmlNode node, string searchString, string replaceString, IReplacerEngine engine)
        {
            if (node.NodeType != XmlNodeType.Element) return;           

            string newNodeName = engine.Replace(node.Name, searchString, replaceString);
            
            if (String.Compare(newNodeName, node.Name) != 0)
            {
                ReplaceNode(node, newNodeName);
            }
        }

        private void ReplaceNode(XmlNode oldNode, string newNodeName)
        {
            
            
            XmlDocument document = oldNode.OwnerDocument;

            if (String.IsNullOrEmpty(newNodeName))
            {
                if (oldNode.ParentNode != null)
                {
                    oldNode.ParentNode.RemoveChild(oldNode);
                }
                else
                {
                    document.RemoveChild(oldNode);
                }
            }
            else
            {
                CreateSubstitueNodeWithNewValue(oldNode, newNodeName, document);
            }
        }

        private void CreateSubstitueNodeWithNewValue(XmlNode oldNode, string newNodeName, XmlDocument document)
        {
            XmlNode newNode = document.CreateElement(newNodeName);
            newNode.InnerXml = oldNode.InnerXml;
            CopyAttributes(newNode, oldNode);
            XmlNode parent = oldNode.ParentNode;
            parent.ReplaceChild(newNode, oldNode);
        }

        private void CopyAttributes(XmlNode newNode, XmlNode node)
        {
            if (node.Attributes == null) return;
            foreach (XmlAttribute attribute in node.Attributes)
            {
                XmlAttribute newAttrib = newNode.OwnerDocument.CreateAttribute(attribute.Name);
                newAttrib.Value = attribute.Value;
                newNode.Attributes.Append(newAttrib);
            }
        }

    }
}

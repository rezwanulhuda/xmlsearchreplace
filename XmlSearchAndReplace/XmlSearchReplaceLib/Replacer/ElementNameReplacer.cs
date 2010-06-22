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

            string newNodeName = engine.Replace(node.LocalName, searchString, replaceString);
            
            if (String.Compare(newNodeName, node.LocalName) != 0)
            {
                ReplaceNode(node, newNodeName);
            }
        }

        private void ReplaceNode(XmlNode oldNode, string newNodeName)
        {
            
            
            XmlDocument document = oldNode.OwnerDocument;

            if (String.IsNullOrEmpty(newNodeName))
            {
                DeleteNode(document, oldNode);
            }
            else
            {
                CreateSubstitueNodeWithNewValue(oldNode, newNodeName, document);
            }
        }

        private static void DeleteNode(XmlDocument document, XmlNode nodeToDelete)
        {
            if (nodeToDelete.ParentNode != null)
            {
                nodeToDelete.ParentNode.RemoveChild(nodeToDelete);
            }
            else
            {
                document.RemoveChild(nodeToDelete);
            }
        }

        private void CreateSubstitueNodeWithNewValue(XmlNode oldNode, string newNodeName, XmlDocument document)
        {            
            XmlNode newNode = document.CreateElement(oldNode.Prefix, newNodeName, oldNode.NamespaceURI);
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

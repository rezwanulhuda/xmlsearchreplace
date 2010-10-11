using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlSearchReplaceLib.Engine;

namespace XmlSearchReplaceLib
{
    public class XmlSearchReplace
    {
        XmlDocument _Document;
        List<IXmlSearchAndReplacer> _Processors;
        SearchReplaceLocationOptions _LocationOptions;
        SearchReplaceOperationOptions _OperationOptions;
        List<string> _SearchString;
        List<string> _ReplaceString;        
        IReplacerEngine _Engine;

        public XmlSearchReplace(SearchReplaceLocationOptions locOptions, SearchReplaceOperationOptions opOptions, List<string> searchString, List<string> replaceString)
        {            
            _Processors = new List<IXmlSearchAndReplacer>();
            _LocationOptions = locOptions;
            _SearchString = searchString;
            _ReplaceString = replaceString;
            _OperationOptions = opOptions;

            PrepareReplacerEngine();
            SetupReplacerFilters();
        }

        private void PrepareReplacerEngine()
        {
            _Engine = AbstractReplacerEngine.CreateEngine(ReplacerEngineType.StringEngine, _OperationOptions);
        }

        private void SetupReplacerFilters()
        {
            
            if (_LocationOptions.IsSet(SearchReplaceLocationOptions.ReplaceAttributeValue))
            {
                _Processors.Add(new AttributeValueReplacer());
            }

            if (_LocationOptions.IsSet(SearchReplaceLocationOptions.ReplaceElementValue))
            {
                _Processors.Add(new ElementValueReplacer());
            }

            if (_LocationOptions.IsSet(SearchReplaceLocationOptions.ReplaceElementName))
            {
                _Processors.Add(new ElementNameReplacer());
            }

            if (_LocationOptions.IsSet(SearchReplaceLocationOptions.ReplaceAttributeName))
            {
                _Processors.Add(new AttributeNameReplacer());
            }

            if (_LocationOptions.IsSet(SearchReplaceLocationOptions.ReplaceValueOfElement))
            {
                _Processors.Add(new ValueOfElementReplacer());
            }

            if (_LocationOptions.IsSet(SearchReplaceLocationOptions.ReplaceValueOfAttribute))
            {
                _Processors.Add(new ValueOfAttributeReplacer());
            }
        }

        public XmlDocument Replace(XmlDocument doc)
        {
            _Document = doc.Clone() as XmlDocument;

            for (int x = 0; x < _SearchString.Count; ++x)
            {
                foreach (IXmlSearchAndReplacer replacer in _Processors)
                {
                    ReplaceElements(_Document, replacer, _SearchString[x], _ReplaceString[x]);
                }
            }
            
            
            
            return _Document;
        }

        private void ReplaceElements(XmlNode node, IXmlSearchAndReplacer replacer, string searchString, string replaceString)
        {
            if (node == null) return;
            int totalCount = node.ChildNodes.Count;
            for (int x = 0; x < totalCount; ++x)
            {

                replacer.Replace(node.ChildNodes[x], searchString, replaceString, _Engine);
                if (node.ChildNodes.Count < totalCount)
                {                    
                    totalCount = node.ChildNodes.Count;
                    --x;
                    continue;
                }
                ReplaceElements(node.ChildNodes[x], replacer, searchString, replaceString);
            }
        }        
    }
}

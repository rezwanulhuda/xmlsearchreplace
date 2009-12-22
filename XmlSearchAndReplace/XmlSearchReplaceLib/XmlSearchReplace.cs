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
        string _SearchString;
        string _ReplaceString;        
        IReplacerEngine _Engine;

        public XmlSearchReplace(SearchReplaceLocationOptions locOptions, SearchReplaceOperationOptions opOptions, string searchString, string replaceString)
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
            _Engine = ReplacerEngine.CreateEngine(ReplacerEngineType.StringEngine, _OperationOptions);
        }

        private void SetupReplacerFilters()
        {
            if (HasLocationOption(_LocationOptions, SearchReplaceLocationOptions.ReplaceAttributeValue))
            {
                _Processors.Add(new AttributeValueReplacer());
            }

            if (HasLocationOption(_LocationOptions, SearchReplaceLocationOptions.ReplaceElementValue))
            {
                _Processors.Add(new ElementValueReplacer());
            }

            if (HasLocationOption(_LocationOptions, SearchReplaceLocationOptions.ReplaceElementName))
            {
                _Processors.Add(new ElementNameReplacer());
            }

            if (HasLocationOption(_LocationOptions, SearchReplaceLocationOptions.ReplaceAttributeName))
            {
                _Processors.Add(new AttributeNameReplacer());
            }
        }

        private bool HasLocationOption(SearchReplaceLocationOptions availableOptions, SearchReplaceLocationOptions checkOption)
        {
            return ((availableOptions & checkOption) == checkOption);
        }

        public XmlDocument Replace(XmlDocument doc)
        {
            _Document = doc.Clone() as XmlDocument;            

            foreach (IXmlSearchAndReplacer replacer in _Processors)
            {
                ReplaceElements(_Document, replacer);
            }
            
            return _Document;
        }

        private void ReplaceElements(XmlNode node, IXmlSearchAndReplacer replacer)
        {
            for (int x = 0; x < node.ChildNodes.Count; ++x)
            {
                replacer.Replace(node.ChildNodes[x], _SearchString, _ReplaceString, _Engine);
                ReplaceElements(node.ChildNodes[x], replacer);
            }
        }        
    }
}

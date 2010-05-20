using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceLib.Engine;

namespace XmlSnRTest.XmlSearchReplaceLibTest
{
    /// <summary>
    /// Summary description for AbstractReplacerEngineTest
    /// </summary>
    [TestClass]
    public class AbstractReplacerEngineTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateEngine_WithRegExEngine_ThrowsArgumentException()
        {
            AbstractReplacerEngine.CreateEngine(XmlSearchReplaceLib.ReplacerEngineType.RegexEngine, XmlSearchReplaceLib.SearchReplaceOperationOptions.CaseInsensitive);
        }
    }
}

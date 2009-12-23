using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using XmlSearchReplaceLib;
using System.IO;

namespace XmlSnPTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class XmlSearchReplaceTest
    {
        XmlSearchReplace _Replacer;

        public XmlSearchReplaceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        XmlDocument _Document = new XmlDocument();

        private void InitializeReplacer(string xml, SearchReplaceLocationOptions options, string searchString, string replaceString)
        {            
            InitializeReplacer(xml, options, searchString, replaceString, false, false);
        }

        private void InitializeReplacer(string xml, SearchReplaceLocationOptions options, string searchString, string replaceString, bool wholeWordOnly, bool caseInsensitive)
        {
            _Document.LoadXml(xml);
            SearchReplaceOperationOptions opOptions = SearchReplaceOperationOptions.CaseSensitivePartialWord;

            if (wholeWordOnly)
                opOptions |= SearchReplaceOperationOptions.WholeWordOnly;
            if (caseInsensitive)
                opOptions |= SearchReplaceOperationOptions.CaseInsensitive;
            _Replacer = new XmlSearchReplace(options, opOptions, searchString, replaceString);
        }

        private void AssertFirstElementValue(XmlDocument doc, string elementName, int expectedCount, string expectedFirstvalue)
        {
            string xPath = @"//" + elementName;
            XmlNodeList nodes = doc.SelectNodes(xPath);
            Assert.AreEqual(expectedCount, nodes.Count, String.Format("Could not fine element {0}", elementName));

            if (expectedCount > 0)
            {
                Assert.AreEqual(expectedFirstvalue, doc.SelectSingleNode(xPath).SelectSingleNode("text()").InnerText);
            }
        }

        private void AssertFirstAttributeValue(XmlDocument document, string attributeName, string expectedValue)
        {
            string xPath = @"//@" + attributeName;
            
            XmlNode node = document.SelectSingleNode(xPath);
            Assert.IsNotNull(node);
            Assert.AreEqual(expectedValue, node.Value);
            
        }

        private void AssertAttributeCount(XmlDocument document, string attributeName, int expectedCount)
        {
            string xPath = @"//@" + attributeName;

            XmlNodeList nodes = document.SelectNodes(xPath);
            
            Assert.AreEqual(expectedCount, nodes.Count);

        }

        private void AssertAttributeValueInFirstNamedElement(XmlDocument actualDoc, string elementName, string attributeName, string expectedValue)
        {
            XmlNode node = actualDoc.SelectSingleNode("//" + elementName + "/@" + attributeName);

            Assert.IsNotNull(node, String.Format("Could not find attribute {0} in element {1}", elementName, attributeName));
            Assert.AreEqual(expectedValue, node.Value, String.Format("Attribute value mismatch for attribute {0}", attributeName));
        }

        [TestMethod]
        public void Replace_InSide1ElementWholeWord()
        {
            InitializeReplacer("<document>Hello</document>", SearchReplaceLocationOptions.ReplaceElementValue, "Hello", "Hi");            
            
            XmlDocument actualDocument = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDocument, "document", 1, "Hi");            
        }

        [TestMethod]
        public void Replace_InSide2ElementWholeWord()
        {
            InitializeReplacer("<document><Page>Hello</Page></document>", SearchReplaceLocationOptions.ReplaceElementValue, "Hello", "Hi");

            XmlDocument actualDocument = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDocument, "Page", 1, "Hi");
            
        }

        [TestMethod]
        public void Replace_2ElemsHavingSameThing()
        {
            InitializeReplacer("<document>Hello<Page>Hello</Page></document>", SearchReplaceLocationOptions.ReplaceElementValue, "Hello", "Hi");

            XmlDocument actualDocument = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDocument, "document", 1, "Hi");
            AssertFirstElementValue(actualDocument, "Page", 1, "Hi");

        }

        [TestMethod]
        public void Replace_ElemsAndValueHavingSameThing()
        {
            InitializeReplacer("<document><Page>Page 1</Page></document>", SearchReplaceLocationOptions.ReplaceElementValue, "Page", "Paper");

            XmlDocument actualDocument = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDocument, "Page", 1, "Paper 1");

        }

        [TestMethod]
        public void Replace_ElemsAndValueHavingSameThing2Places()
        {
            InitializeReplacer("<document>Pages<Page>Page 1</Page></document>", SearchReplaceLocationOptions.ReplaceElementValue, "Page", "Paper");

            XmlDocument actualDocument = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDocument, "document", 1, "Papers");
            AssertFirstElementValue(actualDocument, "Page", 1, "Paper 1");                        
        }

        [TestMethod]
        public void Replace_Attributes()
        {
            InitializeReplacer(@"<document name=""Pages""><Page pageName=""StartPage""></Page></document>", SearchReplaceLocationOptions.ReplaceAttributeValue | SearchReplaceLocationOptions.ReplaceElementValue, "Page", "Paper");

            XmlDocument actualDocument = _Replacer.Replace(_Document);
            AssertFirstAttributeValue(actualDocument, "name", "Papers");
            AssertFirstAttributeValue(actualDocument, "pageName", "StartPaper");
        }

        [TestMethod]
        public void Replace_AttributesAndElements()
        {
            InitializeReplacer(@"<document name=""Pages"">Page<Page pageName=""StartPage"">Page</Page></document>", SearchReplaceLocationOptions.ReplaceAttributeValue | SearchReplaceLocationOptions.ReplaceElementValue, "Page", "Paper");

            XmlDocument actualDocument = _Replacer.Replace(_Document);
            AssertFirstAttributeValue(actualDocument, "name", "Papers");
            AssertFirstAttributeValue(actualDocument, "pageName", "StartPaper");
            AssertFirstElementValue(actualDocument, "document", 1, "Paper");
            AssertFirstElementValue(actualDocument, "Page", 1, "Paper");
        }

        [TestMethod]
        public void Replace_ElementsOnlyButNoAttributes()
        {
            InitializeReplacer(@"<document name=""Pages"">Page<Page pageName=""StartPage"">Page</Page></document>", SearchReplaceLocationOptions.ReplaceElementValue, "Page", "Paper");

            XmlDocument actualDocument = _Replacer.Replace(_Document);
            AssertFirstAttributeValue(actualDocument, "name", "Pages");
            AssertFirstAttributeValue(actualDocument, "pageName", "StartPage");
            AssertFirstElementValue(actualDocument, "document", 1, "Paper");
            AssertFirstElementValue(actualDocument, "Page", 1, "Paper");
        }       

        [TestMethod]
        public void Replace_MoreThan1ElementName()
        {
            InitializeReplacer("<document>hello<pages><page>some value</page><page></page></pages><pages><page></page></pages></document>", SearchReplaceLocationOptions.ReplaceElementName, "page", "paper");
            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "paper", 3, "some value");
        }

        [TestMethod]
        public void Replace_ValueInLastChild()
        {
            InitializeReplacer("<document>hello<pages><page>some value</page><page>some value</page></pages><pages><page>some value</page></pages></document>", SearchReplaceLocationOptions.ReplaceElementValue, "some value", "blah blah");
            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "page", 3, "blah blah");
        }

        [TestMethod]
        public void Replace_All()
        {
            InitializeReplacer(@"<document>Pages<Page Page=""Page1"">Page</Page></document>", SearchReplaceLocationOptions.ReplaceAll, "Page", "Paper");
            XmlDocument actualDoc = _Replacer.Replace(_Document);
            string actualXml = actualDoc.OuterXml;
            string expectedXml = @"<document>Papers<Paper Paper=""Paper1"">Paper</Paper></document>";
            Assert.AreEqual(expectedXml, actualXml);            
        }

        [TestMethod]
        public void Replace_ElementNameContainingAttributes()
        {
            InitializeReplacer(@"<document>Pages<Page id=""Page1"">Page</Page></document>", SearchReplaceLocationOptions.ReplaceElementName, "Page", "Paper");
            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDoc, "Paper", 1, "Page");
            AssertFirstAttributeValue(actualDoc, "id", "Page1");            
        }

        [TestMethod]
        public void Replace_AttributeNameIn1Element()
        {
            InitializeReplacer(@"<document>Pages<Page id=""Page1"">Page</Page></document>", SearchReplaceLocationOptions.ReplaceAttributeName, "id", "pageId");
            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDoc, "Page", 1, "Page");
            AssertFirstAttributeValue(actualDoc, "pageId", "Page1");
        }

        [TestMethod]
        public void Replace_AttributeNameIn2Elements()
        {
            InitializeReplacer(@"
<document>Pages
    <Book id=""1"">
        <Page id=""Page1"">Page</Page>
    </Book>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeName, "id", "pageId");

            XmlDocument actualDoc = _Replacer.Replace(_Document);            
            AssertFirstAttributeValue(actualDoc, "pageId", "1");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "pageId", "1");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Page", "pageId", "Page1");
        }

        [TestMethod]
        public void Replace_PartialAttributeNameIn2Elements()
        {
            InitializeReplacer(@"
<document>Pages
    <Book BookIdInBook=""1"">
        <Page BookIdInPage=""100"">Page</Page>
    </Book>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeName, "Id", "ElementId");

            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "BookElementIdInBook", "1");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Page", "BookElementIdInPage", "100");
        }



        [TestMethod]
        public void Replace_FullElementNameSameElementIn2Elements()
        {
            InitializeReplacer(@"
<document>Pages
    <Book BookIdInBook=""1"">Test<Book BookIdInPage=""100"">Page</Book>
    </Book>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "Book", "Kook");

            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDoc, "Kook", 2, "Test");
        }

        [TestMethod]
        public void Replace_PartialElementNameIn2Elements()
        {
            InitializeReplacer(@"
<document>Pages
    <Book BookIdInBook=""1"">Test<Book BookIdInPage=""100"">Page</Book>
    </Book>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "Bo", "Ko");

            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDoc, "Kook", 2, "Test");
        }

        [TestMethod]
        public void Replace_WholeWordOnlyFromElementValue()
        {
            InitializeReplacer(@"
<document>Pages
    <Book BookIdInBook=""1"">Test<Page BookIdInPage=""100"">TestPage</Page>
    </Book>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementValue, "Test", "Hello", true, false);

            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertFirstElementValue(actualDoc, "Book", 1, "Hello");
            AssertFirstElementValue(actualDoc, "Page", 1, "TestPage");
        }


        [TestMethod]
        public void Replace_WholeWordOnlyFromAttributeValue()
        {
            InitializeReplacer(@"
<document>Pages
    <Book BookIdBook=""ThisIsWhole"">Test<Page BookIdInPage=""ThisIsWholeWord"">TestPage</Page>
    </Book>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeValue, "ThisIsWhole", "BlahBlat", true, false);

            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "BookIdBook", "BlahBlat");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Page", "BookIdInPage", "ThisIsWholeWord");

        }

        [TestMethod]
        public void Replace_WholeWordOnlyFromElementName()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookIdBook=""ThisIsWhole"">Test<Book BookIdInPage=""ThisIsWholeWord"">TestPage</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "Book", "Novel", true, false);

            XmlDocument actualDoc = _Replacer.Replace(_Document);            
            AssertFirstElementValue(actualDoc, "Novel", 1, "TestPage");
            AssertFirstElementValue(actualDoc, "Books", 1, "Test");

        }

        [TestMethod]
        public void Replace_WholeWordOnlyFromAttributeName()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">Test<Book BookIdInPage=""ThisIsWholeWord"">TestPage</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeName, "BookId", "Novel", true, false);

            XmlDocument actualDoc = _Replacer.Replace(_Document);
            AssertAttributeValueInFirstNamedElement(actualDoc, "Books", "Novel", "ThisIsWhole");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "BookIdInPage", "ThisIsWholeWord");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_PartialWord_ElementValue()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">Test<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementValue, "Test", "Hello", false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);
            
            AssertFirstElementValue(actualDoc, "Books", 1, "Hello");
            AssertFirstElementValue(actualDoc, "Book", 1, "Hello");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_WholeWord_ElementValue()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementValue, "TEST", "Hello", true, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "Books", 1, "TestOptions");
            AssertFirstElementValue(actualDoc, "Book", 1, "Hello");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_WholeWord_AttributeValue()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeValue, "THISISWHOLE", "Hello", true, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertAttributeValueInFirstNamedElement(actualDoc, "Books", "BookId", "Hello");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "BookIdInPage", "ThisIsWholeWord");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_PartialWord_AttributeValue()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeValue, "THISISWHOLE", "Hello", false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertAttributeValueInFirstNamedElement(actualDoc, "Books", "BookId", "Hello");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "BookIdInPage", "HelloWord");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_PartialWord_AttributeName()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeName, "BOOK", "Hello", false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertAttributeValueInFirstNamedElement(actualDoc, "Books", "HelloId", "ThisIsWhole");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "HelloIdInPage", "ThisIsWholeWord");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_WholeWord_AttributeName()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeName, "BOOKID", "Hello", true, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertAttributeValueInFirstNamedElement(actualDoc, "Books", "Hello", "ThisIsWhole");
            AssertAttributeValueInFirstNamedElement(actualDoc, "Book", "BookIdInPage", "ThisIsWholeWord");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_WholeWord_ElementName()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "BOOK", "Hello", true, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "Books", 1, "TestOptions");
            AssertFirstElementValue(actualDoc, "Hello", 1, "test");
        }

        [TestMethod]
        public void Replace_CaseInsensitive_PartialWord_ElementName()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "BOOK", "Hello", false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "Hellos", 1, "TestOptions");
            AssertFirstElementValue(actualDoc, "Hello", 1, "test");
        }

        [TestMethod]
        public void Replace_ElementNameWithEmptyReplaceString_WillRemoveElement()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "Book", String.Empty, false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "Book", 0, String.Empty);
            //AssertFirstElementValue(actualDoc, "Hello", 1, "test");
        }

        [TestMethod]
        public void Replace_ElementNameWithEmptyReplaceStringWithMultipleNodes_WillRemoveAllOfThem()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    <Book BookIdInPage=""ThisIsWholeWord"">blah blah</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "Book", String.Empty, false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "Book", 0, String.Empty);
            //AssertFirstElementValue(actualDoc, "Hello", 1, "test");
        }

        [TestMethod]
        public void Replace_ElementNameWithEmptyReplaceStringWith3Nodes_WillRemoveAllOfThem()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    <Book BookIdInPage=""ThisIsWholeWord"">blah blah</Book>
    <Book BookIdInPage=""ThisIsWholeWord"">ahem</Book>
    </Books>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "Book", String.Empty, false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "Book", 0, String.Empty);
            //AssertFirstElementValue(actualDoc, "Hello", 1, "test");
        }

        [TestMethod]
        public void Replace_ElementNameWithEmptyReplaceStringAtMultipleElements_WillRemoveAllOfThem()
        {
            InitializeReplacer(@"
<document>Pages
    <Books BookId=""ThisIsWhole"">TestOptions<Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    <Book BookIdInPage=""ThisIsWholeWord"">blah blah</Book>
    <Book BookIdInPage=""ThisIsWholeWord"">ahem</Book>
    </Books>
    <Catalogues>
    <Book BookIdInPage=""ThisIsWholeWord"">test</Book>
    <Book BookIdInPage=""ThisIsWholeWord"">blah blah</Book>
    <Book BookIdInPage=""ThisIsWholeWord"">ahem</Book>
    <Book BookIdInPage=""ThisIsWholeWord""><Book BookIdInPage=""ThisIsWholeWord"">ahem<Book BookIdInPage=""ThisIsWholeWord"">ahem</Book></Book></Book>
    
    </Catalogues>
</document>"
                , SearchReplaceLocationOptions.ReplaceElementName, "Book", String.Empty, false, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertFirstElementValue(actualDoc, "Book", 0, String.Empty);            
        }

        [TestMethod]
        public void Replace_AttributeNameInOneElement_WillRemoveElement()
        {
            InitializeReplacer(@"
<document>
    <Book id=""1"" />
</document>"
                , SearchReplaceLocationOptions.ReplaceAttributeName, "id", String.Empty, true, true);

            XmlDocument actualDoc = _Replacer.Replace(_Document);

            AssertAttributeCount(actualDoc, "id", 0);
        }  
    }
}
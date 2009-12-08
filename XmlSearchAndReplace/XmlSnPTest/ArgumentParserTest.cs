using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceLib;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest
{
    /// <summary>
    /// Summary description for ArgumentParserTest
    /// </summary>
    [TestClass]
    public class ArgumentParserTest
    {
        public ArgumentParserTest()
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

        [TestMethod]
        public void CheckArgumentsAreFine()
        {
            string[] args = { "/O=", "av,ev,en,an", "/R=replace ", "/F=c:\\documents", "and", "settings\\desktop\\file.xml", "/S=search /W /i /c" };

            ArgumentParser argParser = new ArgumentParser(args);            
            Assert.AreEqual(SearchReplaceLocationOptions.ReplaceAll, argParser.GetLocationOptions());            
            Assert.AreEqual("c:\\documents and settings\\desktop\\file.xml", argParser.GetFileName());
            Assert.AreEqual("search", argParser.GetSearchString());
            Assert.AreEqual("replace", argParser.GetReplaceString());
            Assert.AreEqual(SearchReplaceOperationOptions.CaseInsensitive | SearchReplaceOperationOptions.WholeWordOnly, argParser.GetOperationOptions());
            Assert.AreEqual(true, argParser.ContinueOnError);
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidArgumentOptionException))]
        public void CheckArguments_InvalidValueWithDoubleEquals()
        {
            string[] args = { "/O=", "av,ev,=en,an"};

            
            try
            {
                ArgumentParser argParser = new ArgumentParser(args);
            }
            catch (InvalidArgumentOptionException ex)
            {
                Assert.AreEqual("The command line parameter 'O= av,ev,=en,an' is invalid", ex.Message);
                throw;
            }

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidArgumentOptionException))]
        public void CheckArguments_InvalidValueWithOptionEmpty()
        {
            string[] args = { "/=", "av,ev,en,an" };


            try
            {
                ArgumentParser argParser = new ArgumentParser(args);
            }
            catch (InvalidArgumentOptionException ex)
            {
                Assert.AreEqual("The command line parameter '= av,ev,en,an' has an empty option", ex.Message);
                throw;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void CheckArgumentsAreFineWithActualValue()
        {
            string[] args = { "/O=", "av,ev,en", "/S=\\\\server04\\spfiles", "/R=L:", "/F=c:\\documents", "and", "settings\\desktop\\file.xml" };

            ArgumentParser argParser = new ArgumentParser(args);
            Assert.AreEqual(SearchReplaceLocationOptions.ReplaceAll, argParser.GetLocationOptions() | SearchReplaceLocationOptions.ReplaceAttributeName);
            Assert.AreEqual("c:\\documents and settings\\desktop\\file.xml", argParser.GetFileName());
            Assert.AreEqual("\\\\server04\\spfiles", argParser.GetSearchString());
            Assert.AreEqual("L:", argParser.GetReplaceString());
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSpaceBeforeAndAfterWithoutSurroundingDoubleQutoes_ShouldTrimInput()
        {
            string argument = "/S= hello world /R= how are you /F=  c:\\documents and settings\\blah blah.html  ";

            ArgumentParser argParser = new ArgumentParser(argument);
            
            Assert.AreEqual("hello world", argParser.GetSearchString());
            Assert.AreEqual("how are you", argParser.GetReplaceString());
            Assert.AreEqual("c:\\documents and settings\\blah blah.html", argParser.GetFileName());
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSurroundingDoubleQutoes_ShouldGiveStringWithoutQuotes()
        {
            string argument = @"/S=""hello world"" /R=""how are you"" /F=""c:\documents and settings\blah blah.html""";

            ArgumentParser argParser = new ArgumentParser(argument);

            Assert.AreEqual("hello world", argParser.GetSearchString());
            Assert.AreEqual("how are you", argParser.GetReplaceString());
            Assert.AreEqual("c:\\documents and settings\\blah blah.html", argParser.GetFileName());
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSurroundingDoubleQutoesButSpaceBeforeAndAfter_ShouldTrimAndGiveStringWithoutQuotes()
        {
            string argument = @"/S=  ""hello world""   /R=   ""how are you""   /F=   ""c:\documents and settings\blah blah.html""   ";

            ArgumentParser argParser = new ArgumentParser(argument);

            Assert.AreEqual("hello world", argParser.GetSearchString());
            Assert.AreEqual("how are you", argParser.GetReplaceString());
            Assert.AreEqual("c:\\documents and settings\\blah blah.html", argParser.GetFileName());
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSurroundingDoubleQutoesAndSpaceInsideQuotes_ShouldNotTrimAndGiveStringWithoutQuotes()
        {
            string argument = @"/S=  "" hello world ""   /R=   "" how are you ""   /F=   "" c:\documents and settings\blah blah.html ""   ";

            ArgumentParser argParser = new ArgumentParser(argument);

            Assert.AreEqual(" hello world ", argParser.GetSearchString());
            Assert.AreEqual(" how are you ", argParser.GetReplaceString());
            Assert.AreEqual(" c:\\documents and settings\\blah blah.html ", argParser.GetFileName());
        }

        
    }
}

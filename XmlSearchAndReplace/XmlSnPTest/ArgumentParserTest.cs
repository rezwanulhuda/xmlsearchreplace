using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceLib;
using XmlSearchReplaceConsoleLib;
using System;

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

        public static ISearchReplaceParameter GetParameters(string[] commandLine)
        {
            return GetParameters(String.Join(" ", commandLine));
        }
        public static ISearchReplaceParameter GetParameters(string commandLine)
        {
            CommandlineParser parser = new CommandlineParser(commandLine);
            return parser.GetParamsAndValues();
        }

        [TestMethod]
        public void CheckArgumentsAreFine()
        {
            string[] args = { "/O=", "av,ev,en,an", "/R=replace ", "/F=c:\\documents", "and", "settings\\desktop\\file.xml", "/S=search /W /i /c" };
            
            ISearchReplaceParameter param = GetParameters(args);
            Assert.AreEqual(SearchReplaceLocationOptions.ReplaceAll, param.GetLocationOptions());
            Assert.AreEqual("c:\\documents and settings\\desktop\\file.xml", param.GetFileName());
            Assert.AreEqual("search", param.GetSearchString()[0]);
            Assert.AreEqual("replace", param.GetReplaceString()[0]);
            Assert.AreEqual(SearchReplaceOperationOptions.CaseInsensitive | SearchReplaceOperationOptions.WholeWordOnly, param.GetOperationOptions());
            Assert.AreEqual(true, param.ContinueOnError);
        }

        [TestMethod]
        public void StringArgumentsWithMoreThanOneEqualsSignShouldWork()
        {
            string[] args = { "/O=", "av,ev,en,an", @"/R==", "/F=c:\\documents", "and", "settings\\desktop\\file.xml", "/S=Equals /W /i /c" };

            ISearchReplaceParameter parameters = GetParameters(args);
            Assert.AreEqual(SearchReplaceLocationOptions.ReplaceAll, parameters.GetLocationOptions());
            Assert.AreEqual("c:\\documents and settings\\desktop\\file.xml", parameters.GetFileName());
            Assert.AreEqual("Equals", parameters.GetSearchString()[0]);
            Assert.AreEqual("=", parameters.GetReplaceString()[0]);
            Assert.AreEqual(SearchReplaceOperationOptions.CaseInsensitive | SearchReplaceOperationOptions.WholeWordOnly, parameters.GetOperationOptions());
            Assert.AreEqual(true, parameters.ContinueOnError);
        }

        

        [TestMethod]        
        public void CheckArguments_InvalidValueWithOptionEmpty()
        {
            string[] args = { "/=", "av,ev,en,an" };


            try
            {
                CommandlineParser argParser = new CommandlineParser(args);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Parameter '' (/= av,ev,en,an) is not supported", ex.Message);
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void CheckArgumentsAreFineWithActualValue()
        {
            string[] args = { "/O=", "av,ev,en", "/S=\\\\server04\\spfiles", "/R=L:", "/F=c:\\documents", "and", "settings\\desktop\\file.xml" };

            ISearchReplaceParameter argParser = GetParameters(args);
            Assert.AreEqual(SearchReplaceLocationOptions.ReplaceAll, argParser.GetLocationOptions() | SearchReplaceLocationOptions.ReplaceAttributeName);
            Assert.AreEqual("c:\\documents and settings\\desktop\\file.xml", argParser.GetFileName());
            Assert.AreEqual("\\\\server04\\spfiles", argParser.GetSearchString()[0]);
            Assert.AreEqual("L:", argParser.GetReplaceString()[0]);
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSpaceBeforeAndAfterWithoutSurroundingDoubleQutoes_ShouldTrimInput()
        {
            string argument = "/S= hello world /R= how are you /F=  c:\\documents and settings\\blah blah.html  /O=av";

            ISearchReplaceParameter argParser = GetParameters(argument);

            Assert.AreEqual("hello world", argParser.GetSearchString()[0]);
            Assert.AreEqual("how are you", argParser.GetReplaceString()[0]);
            Assert.AreEqual("c:\\documents and settings\\blah blah.html", argParser.GetFileName());
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSurroundingDoubleQutoes_ShouldGiveStringWithoutQuotes()
        {
            string argument = @"/S=""hello world"" /R=""how are you"" /F=""c:\documents and settings\blah blah.html"" /O=av";

            ISearchReplaceParameter argParser = GetParameters(argument);

            Assert.AreEqual("hello world", argParser.GetSearchString()[0]);
            Assert.AreEqual("how are you", argParser.GetReplaceString()[0]);
            Assert.AreEqual("c:\\documents and settings\\blah blah.html", argParser.GetFileName());
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSurroundingDoubleQutoesButSpaceBeforeAndAfter_ShouldTrimAndGiveStringWithoutQuotes()
        {
            string argument = @"/S=  ""hello world""   /R=   ""how are you""   /F=   ""c:\documents and settings\blah blah.html""   /O=av";

            ISearchReplaceParameter argParser = GetParameters(argument);

            Assert.AreEqual("hello world", argParser.GetSearchString()[0]);
            Assert.AreEqual("how are you", argParser.GetReplaceString()[0]);
            Assert.AreEqual("c:\\documents and settings\\blah blah.html", argParser.GetFileName());
        }

        [TestMethod]
        public void CheckSearchStringReplaceFileName_WithSurroundingDoubleQutoesAndSpaceInsideQuotes_ShouldNotTrimAndGiveStringWithoutQuotes()
        {
            string argument = @"/S=  "" hello world ""   /R=   "" how are you ""   /F=   "" c:\documents and settings\blah blah.html ""   /O=av";

            ISearchReplaceParameter argParser = GetParameters(argument);

            Assert.AreEqual(" hello world ", argParser.GetSearchString()[0]);
            Assert.AreEqual(" how are you ", argParser.GetReplaceString()[0]);
            Assert.AreEqual(" c:\\documents and settings\\blah blah.html ", argParser.GetFileName());
        }

        [TestMethod]
        public void CheckUnsupportedParametersAreReported()
        {
            string argument = @"/Q";

            try
            {
                CommandlineParser argParser = new CommandlineParser(argument);
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Parameter 'Q' (/Q) is not supported", ex.Message);
                return;
            }
            Assert.Fail();
        }


        [TestMethod]
        public void DblQuoteInSearchString_WillReturnStringWithDblQuote()
        {
            string argument = @"/O=av/F=something/S=""hello\""world\""how are you""/R=""hello world "" how are you?""""";

            ISearchReplaceParameter argParser = GetParameters(argument);

            Assert.AreEqual(@"hello""world""how are you", argParser.GetSearchString()[0]);
            Assert.AreEqual(@"hello world "" how are you?""", argParser.GetReplaceString()[0]);
        }

        [TestMethod]
        public void EqualsSignInSearchReplaceString_WillReturnStringWithEqual()
        {
            string argument = @"/O=av/F=something/S=""hello=world how are you""/R=""hello world = "" how are you? """"";

            ISearchReplaceParameter argParser = GetParameters(argument);

            Assert.AreEqual(@"hello=world how are you", argParser.GetSearchString()[0]);
            Assert.AreEqual(@"hello world = "" how are you? """, argParser.GetReplaceString()[0]);
        }

        [TestMethod]
        public void GetArgumentsFromString_EmptyString_WillReturn0Args()
        {
            string commandLine = String.Empty;

            Assert.AreEqual(0, CommandlineParser.GetArgumentsFromString(commandLine).Count);
        }

        [TestMethod]
        public void GetArgumentsFromString_1BoolArgument_WillReturn1Args()
        {
            string commandLine = "/W";

            Assert.AreEqual(1, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual("W", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
        }



        [TestMethod]
        public void GetArgumentsFromString_2BoolArgument_WillReturn2Args()
        {
            string commandLine = "/W /Q";

            Assert.AreEqual(2, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual("W", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual("Q", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
        }

        [TestMethod]
        public void GetArgumentsFromString_2BoolArgumentWithoutSpace_WillReturn2Args()
        {
            string commandLine = "/W/Q";

            Assert.AreEqual(2, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual("W", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual("Q", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
        }

        [TestMethod]
        public void GetArgumentsFromString_1StringArgumentWithoutSpecialChars_WillReturn1Arg()
        {
            string commandLine = "/S=helloworld";

            Assert.AreEqual(1, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual("S=helloworld", CommandlineParser.GetArgumentsFromString(commandLine)[0]);            
        }

        [TestMethod]
        public void GetArgumentsFromString_2StringArgumentWithoutSpecialChars_WillReturn2Arg()
        {
            string commandLine = "/S=helloworld /R=howareyou";

            Assert.AreEqual(2, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual("S=helloworld", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual("R=howareyou", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
        }

        [TestMethod]
        public void GetArgumentsFromString_1StringArgumentWithFrontSlashInValue_WillReturn2Args()
        {
            string commandLine = @"/S=hello/world";

            Assert.AreEqual(2, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual("S=hello", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual("world", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
        }

        [TestMethod]
        public void GetArgumentsFromString_1StringArgumentWithDblQuotes_WillReturn1Args()
        {
            string commandLine = @"/S=""hello/world""";

            Assert.AreEqual(1, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello/world""", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
        }

        [TestMethod]
        public void GetArgumentsFromString_1StringArgumentContainingDblQuotes_WillReturn1Args()
        {
            string commandLine = @"/S=""hello\""/world""";

            Assert.AreEqual(1, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello""/world""", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
        }

        [TestMethod]
        public void GetArgumentsFromString_1StringArgumentContainingBackSlash_WillReturn1Args()
        {
            string commandLine = @"/S=hello\world";

            Assert.AreEqual(1, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=hello\world", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
        }

        [TestMethod]
        public void GetArgumentsFromString_3StringArgument_WillReturn3Args()
        {
            string commandLine = @"/S=""hello world"" /R=How are u doing /F=""c:\dellshare.com\*.csproj""";

            Assert.AreEqual(3, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello world""", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual(@"R=How are u doing", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
            Assert.AreEqual(@"F=""c:\dellshare.com\*.csproj""", CommandlineParser.GetArgumentsFromString(commandLine)[2]);
        }        

        [TestMethod]
        public void GetArgumentsFromString_CombinationOfAll()
        {
            string commandLine = @"/S=""hello ""world"" /R=How are u doing /F=""c:\dellshare.com\*.csproj"" /W /C /""I"" /O=""en,ev,an,av"" /L";

            Assert.AreEqual(9, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello """, CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual(@"world""", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
            Assert.AreEqual(@"R=How are u doing", CommandlineParser.GetArgumentsFromString(commandLine)[2]);
            Assert.AreEqual(@"F=""c:\dellshare.com\*.csproj""", CommandlineParser.GetArgumentsFromString(commandLine)[3]);
            Assert.AreEqual(@"W", CommandlineParser.GetArgumentsFromString(commandLine)[4]);
            Assert.AreEqual(@"C", CommandlineParser.GetArgumentsFromString(commandLine)[5]);
            Assert.AreEqual(@"""I""", CommandlineParser.GetArgumentsFromString(commandLine)[6]);
            Assert.AreEqual(@"O=""en,ev,an,av""", CommandlineParser.GetArgumentsFromString(commandLine)[7]);
            Assert.AreEqual(@"L", CommandlineParser.GetArgumentsFromString(commandLine)[8]);
        }

        [TestMethod]
        public void GetArgumentsFromString_InvalidInput1_UnterminatedString()
        {
            string commandLine = @"/S=""hello ""world"" /R=How are u doing /F=""c:\dellshare.com\*.csproj""";

            Assert.AreEqual(4, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello """, CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual(@"world""", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
            Assert.AreEqual(@"R=How are u doing", CommandlineParser.GetArgumentsFromString(commandLine)[2]);
            Assert.AreEqual(@"F=""c:\dellshare.com\*.csproj""", CommandlineParser.GetArgumentsFromString(commandLine)[3]);
        }

        [TestMethod]
        public void GetArgumentsFromString_InvalidInput2_InvalidArgument()
        {
            string commandLine = @"/S=""hello \""world"" /R=How are /u doing /F=""c:\dellshare.com\*.csproj""";

            Assert.AreEqual(4, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello ""world""", CommandlineParser.GetArgumentsFromString(commandLine)[0]);            
            Assert.AreEqual(@"R=How are", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
            Assert.AreEqual(@"u doing", CommandlineParser.GetArgumentsFromString(commandLine)[2]);
            Assert.AreEqual(@"F=""c:\dellshare.com\*.csproj""", CommandlineParser.GetArgumentsFromString(commandLine)[3]);
        }

        [TestMethod]
        public void GetArgumentsFromString_InvalidInput3_UnendingDblQuoteAtEnd()
        {
            string commandLine = @"/S=""hello \""world"" /R=How are /u doing /F=""c:\dellshare.com\*.csproj";

            Assert.AreEqual(4, CommandlineParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello ""world""", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual(@"R=How are", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
            Assert.AreEqual(@"u doing", CommandlineParser.GetArgumentsFromString(commandLine)[2]);
            Assert.AreEqual(@"F=""c:\dellshare.com\*.csproj", CommandlineParser.GetArgumentsFromString(commandLine)[3]);
        }

        [TestMethod]
        public void GetArgumentsFromString_InvalidInput4_UnendingDblQuoteAtEnd()
        {
            string commandLine = @"/S=""hello \""world"" /R=""How are u doing /F=""c:\dellshare.com\*.csproj";

            //Assert.AreEqual(4, ArgumentParser.GetArgumentsFromString(commandLine).Count);
            Assert.AreEqual(@"S=""hello ""world""", CommandlineParser.GetArgumentsFromString(commandLine)[0]);
            Assert.AreEqual(@"R=""How are u doing /F=""", CommandlineParser.GetArgumentsFromString(commandLine)[1]);
            Assert.AreEqual(@"c:\dellshare.com\*.csproj", CommandlineParser.GetArgumentsFromString(commandLine)[2]);            
        }

        [TestMethod]
        public void GetArgumentsFromString_CheckMandatoryParameters()
        {
            string commandLine = @"/S=""hello \""world"" /R=""How are u doing""";

            try
            {
                CommandlineParser parser = new CommandlineParser(commandLine);
            }
            catch (RequiredParameterMissingException ex)
            {
                Assert.AreEqual(2, ex.GetMissginParameters().Count);
                Assert.IsTrue(ex.GetMissginParameters().Exists(p => String.Compare(p.GetName(), "O", true) == 0));
                Assert.IsTrue(ex.GetMissginParameters().Exists(p => String.Compare(p.GetName(), "F", true) == 0));
                return;
            }

            Assert.Fail();
        }
        
        [TestMethod]
        public void GetArgumentsFromString_WithoutRButContainingL_ShouldNotListRAsAMandatoryParam()
        {
            string commandLine = @"/S=""hello \""world"" /L";

            try
            {
                CommandlineParser parser = new CommandlineParser(commandLine);                
            }
            catch (RequiredParameterMissingException ex)
            {
                Assert.AreEqual(2, ex.GetMissginParameters().Count);
                Assert.IsTrue(ex.GetMissginParameters().Exists(p => String.Compare(p.GetName(), "O", true) == 0));
                Assert.IsTrue(ex.GetMissginParameters().Exists(p => String.Compare(p.GetName(), "F", true) == 0));                                
                return;
            }

            Assert.Fail();
        }


        
    }
}

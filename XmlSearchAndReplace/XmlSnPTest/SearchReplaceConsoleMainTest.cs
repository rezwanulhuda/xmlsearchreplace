using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XmlSearchReplaceConsoleLib;
using XmlSearchReplaceLib;
using System.Xml;

namespace XmlSnRTest
{
    /// <summary>
    /// Summary description for SearchReplaceConsoleMainTest
    /// </summary>
    [TestClass]
    public class SearchReplaceConsoleMainTest
    {
        public SearchReplaceConsoleMainTest()
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

        string _TmpFolder;
        string _SrcFile;

        [TestInitialize()]
        public void TestInitialize()
        {
            _TmpFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(_TmpFolder);
            string xmlSrc = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <Books>
    <Book id=""1"" category=""Category1"">
      <Title>Title 1</Title>
      <Author>Author 1</Author>
    </Book>
    <Book id=""2"" category=""Category2"">
      <Title>Title 2 - book</Title>
      <Author>Author 2 - Book</Author>
    </Book>
  </Books>  
</Library>";

            _SrcFile = Path.Combine(_TmpFolder, "file.xml");

            File.WriteAllText(_SrcFile, xmlSrc);

        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Directory.Delete(_TmpFolder, true);
        }

        private void TestAndAssertExpectationsAreMet(string xmlExpected, string commandLine)
        {
            SearchReplaceConsoleMain srMain = new SearchReplaceConsoleMain(ArgumentParserTest.GetParameters(commandLine));
            srMain.ProcessAll();

            string xmlActual = File.ReadAllText(_SrcFile);

            Assert.IsTrue(File.Exists(Utility.GetBackupFileName(_SrcFile)), "Backup file was not created.");
            Assert.AreEqual(xmlExpected, xmlActual);
        }

        [TestMethod]
        public void ProcessAll_EnEvAnAv_CaseInsensitive_WholeWordOnly()
        {
            
            string xmlExpected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <Books>
    <LibraryBook id=""1"" category=""Category1"">
      <Title>Title 1</Title>
      <Author>Author 1</Author>
    </LibraryBook>
    <LibraryBook id=""2"" category=""Category2"">
      <Title>Title 2 - book</Title>
      <Author>Author 2 - Book</Author>
    </LibraryBook>
  </Books>
</Library>";
            

            string commandLine = String.Format(@"/F={0} /O=en,ev,av,an /S=""Book"" /R=""LibraryBook"" /I /W", _SrcFile);            
            TestAndAssertExpectationsAreMet(xmlExpected, commandLine);
        }

        [TestMethod]
        public void ProcessAll_EnEvAnAv_CaseSensitive_PartialWord()
        {

            string xmlExpected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <LibraryBooks>
    <LibraryBook id=""1"" category=""Category1"">
      <Title>Title 1</Title>
      <Author>Author 1</Author>
    </LibraryBook>
    <LibraryBook id=""2"" category=""Category2"">
      <Title>Title 2 - book</Title>
      <Author>Author 2 - LibraryBook</Author>
    </LibraryBook>
  </LibraryBooks>
</Library>";


            string commandLine = String.Format(@"/F={0} /O=en,ev,av,an /S=""Book"" /R=""LibraryBook""", _SrcFile);
            TestAndAssertExpectationsAreMet(xmlExpected, commandLine);
        }

        [TestMethod]
        public void ProcessAll_EnEvAnAv_ContinueOnError()
        {   
            string commandLine = String.Format(@"/F={0} /O=en,ev,av,an /S=""Book"" /R=""LibraryBook"" /I /W /C", _SrcFile);
            File.SetAttributes(_SrcFile, FileAttributes.ReadOnly);

            SearchReplaceConsoleMain main = new SearchReplaceConsoleMain(ArgumentParserTest.GetParameters(commandLine));
            main.ProcessAll();
            
            Assert.IsTrue(File.Exists(Utility.GetBackupFileName(_SrcFile)));

            File.SetAttributes(_SrcFile, FileAttributes.Normal);
            File.SetAttributes(Utility.GetBackupFileName(_SrcFile), FileAttributes.Normal);
        }

        [TestMethod]        
        public void ProcessAll_EnEvAnAv_InvalidXmlFile()
        {
            
            string commandLine = String.Format(@"/F={0} /O=en,ev,av,an /S=""Book"" /R=""LibraryBook"" /I /W", _SrcFile);

            File.WriteAllText(_SrcFile, String.Empty);

            SearchReplaceConsoleMain main = new SearchReplaceConsoleMain(ArgumentParserTest.GetParameters(commandLine));
            try
            {
                main.ProcessAll();
            }
            catch
            {
                Assert.IsFalse(File.Exists(Utility.GetBackupFileName(_SrcFile)));
                return;
            }

            Assert.Fail();            
        }

        [TestMethod]
        public void GetUsageTest()
        {

            string expectedUsage = "tt.exe " + CommandLineParameterCollection.GetUsage() + Environment.NewLine;
            expectedUsage += CommandLineParameterCollection.GetHelpText();

            Assert.AreEqual(expectedUsage, SearchReplaceConsoleMain.GetUsage("tt.exe"));
        }

        
    }
}

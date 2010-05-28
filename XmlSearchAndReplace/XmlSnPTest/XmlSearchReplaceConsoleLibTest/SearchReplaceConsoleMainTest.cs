﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;
using System.IO;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest
{
    [TestClass]
    public class SearchReplaceConsoleMainTest
    {

        string _TestFile;
        string _ExpectedFile;

        private void AssertFilesMatched()
        {
            Assert.AreEqual(File.ReadAllText(_ExpectedFile), File.ReadAllText(_TestFile));
        }

        [TestInitialize]
        public void InitializeTest()
        {
            string fileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <Books>
    <Book BookId=""1"">
      <Title>Bourne Superemecy</Title>
      <Author>Robert Ludlum</Author>
    </Book>
  </Books>
</Library>";

            SetupTestFile(fileContent);
        }

        private void SetupTestFile(string content)
        {
            _TestFile = Path.GetTempFileName();
            File.WriteAllText(_TestFile, content);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            if (File.Exists(_TestFile))
            {
                File.Delete(_TestFile);
                string backup = XmlSearchReplaceLib.Utility.GetBackupFileName(_TestFile);
                if (File.Exists(backup))
                {
                    File.Delete(backup);
                }
            }

            if (File.Exists(_ExpectedFile))
            {
                File.Delete(_ExpectedFile);
            }

            TestHelper.DeleteLastParameterFile();
        }

        private void SetupExpectedFile(string content)
        {
            _ExpectedFile = Path.GetTempFileName();
            File.WriteAllText(_ExpectedFile, content);
        }

        private void SetupAndAssert(string expectedContent, string commandLine)
        {
            SetupExpectedFile(expectedContent);

            SearchReplaceConsoleMain main = new SearchReplaceConsoleMain();

            main.Start(commandLine);

            AssertFilesMatched();
        }
        
        [TestMethod]
        public void Start_1File_ElementNameChange()
        {
            string expectedContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<BookLibrary>
  <Books>
    <Book BookId=""1"">
      <Title>Bourne Superemecy</Title>
      <Author>Robert Ludlum</Author>
    </Book>
  </Books>
</BookLibrary>";
            
            
            string commandLine = String.Format(@"/F=""{0}"" /S=Library /R=BookLibrary /O=en", _TestFile);

            SetupAndAssert(expectedContent, commandLine);
        }

        [TestMethod]
        public void Start_1File_AttributeNameChangeLowerCaseIgnoreCase()
        {
            string expectedContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <Books>
    <Book bookid=""1"">
      <Title>Bourne Superemecy</Title>
      <Author>Robert Ludlum</Author>
    </Book>
  </Books>
</Library>";
            

            string commandLine = String.Format(@"/F=""{0}"" /S=BOOKID /O=an /L /I", _TestFile);

            SetupAndAssert(expectedContent, commandLine);
        }

        [TestMethod]
        public void Start_1File_ElementNameWholeWordOnlyParamFile()
        {
            string expectedContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <Books>
    <Book BookId=""1"">
      <BookTitle>Bourne Superemecy</BookTitle>
      <BookAuthor>Robert Ludlum</BookAuthor>
    </Book>
  </Books>
</Library>";

            string paramFile = TestHelper.CreateParameterFile(new string[] {"/S=Title /R=BookTitle", "/S=Author /R=BookAuthor" });

            string commandLine = String.Format(@"/F=""{0}"" /O=en /P={1} /W", _TestFile, paramFile);
            
            SetupAndAssert(expectedContent, commandLine);
        }

        [TestMethod]
        public void Start_1File_ElementValueWholeWordOnlyParamFile()
        {
            string expectedContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <Books>
    <Book BookId=""1"">
      <Title>Bourne Identity</Title>
      <Author>Robert Ludlum</Author>
    </Book>
  </Books>
</Library>";

            string paramFile = TestHelper.CreateParameterFile(new string[] { @"/S=""Bourne Superemecy"" /R=""Bourne Identity""", @"/S=Robert /R=WontBeFound" });

            string commandLine = String.Format(@"/F=""{0}"" /O=ev /P={1} /W", _TestFile, paramFile);

            SetupAndAssert(expectedContent, commandLine);
        }

        [TestMethod]
        public void Start_ElementNameParamFileLowerCaseIgnoreCase()
        {
            string expectedContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Library>
  <books>
    <book bookId=""1"">
      <Title>Bourne Superemecy</Title>
      <Author>Robert Ludlum</Author>
    </book>
  </books>
</Library>";

            string paramFile = TestHelper.CreateParameterFile(new string[] { @"/S=""book""" });

            string commandLine = String.Format(@"/F=""{0}"" /O=en,an /P={1} /L /I", _TestFile, paramFile);

            SetupAndAssert(expectedContent, commandLine);
        }

//        [TestMethod]
//        public void Start_ElementNameWithNameSpaceParamFileLowerCaseIgnoreCase()
//        {
//            string testFileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
//<h:Library xmlns:h=""http://www.w3.org/TR/html4/"">
//  <h:Books>
//    <h:Book bookId=""1"">
//      <h:Title>Bourne Superemecy</h:Title>
//      <h:Author>Robert Ludlum</h:Author>
//    </h:Book>
//  </h:Books>
//</h:Library>";

//            SetupTestFile(testFileContent);
            
//            string expectedContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
//<h:Library xmlns:h=""http://www.w3.org/TR/html4/"">
//  <h:books>
//    <h:book bookId=""1"">
//      <h:Title>Bourne Superemecy</h:Title>
//      <h:Author>Robert Ludlum</h:Author>
//    </h:book>
//  </h:books>
//</h:Library>";

//            string paramFile = TestHelper.CreateParameterFile(new string[] { @"/S=h:book" });

//            string commandLine = String.Format(@"/F=""{0}"" /O=en,an /P={1} /I /L", _TestFile, paramFile);

//            SetupAndAssert(expectedContent, commandLine);
//        }

        
    }
}

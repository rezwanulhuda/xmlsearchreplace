using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;
using System.IO;

namespace XmlSnRTest
{
    /// <summary>
    /// Summary description for FileParamReaderTest
    /// </summary>
    [TestClass]
    public class FileParamReaderTest
    {       
        
        [TestCleanup]
        public void TestCleanup()
        {
            DeleteParameterFile();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentOptionException), "Parameter file name cannot be empty")]
        public void Ctor_DoesNotAcceptEmptyString()
        {
            FileParamReader fpr = new FileParamReader(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentOptionException), "Parameter file does not exist")]
        public void Ctor_ValidatesExistenceOfParameterFile()
        {
            FileParamReader fpr = new FileParamReader("a:\\blah.txt");
        }
        
        FileParamReader _Fpr;

        void SetUpParameterFile(params string[] values)
        {            
            _Fpr = new FileParamReader(TestHelper.CreateParameterFile(values));
        }

        void DeleteParameterFile()
        {
            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void GetAllSearchStrings_1Line_Returns1SearchString()
        {
            SetUpParameterFile(new string[] { @"/S=""Hello""" });

            List<string> searchStrings = _Fpr.GetAllSearchStrings();

            Assert.AreEqual(1, searchStrings.Count);
            Assert.AreEqual("Hello", searchStrings[0]);            
        }

        [TestMethod]
        public void GetAllReplaceStrings_1Line_Returns1ReplaceString()
        {
            SetUpParameterFile(new string[] { @"/R=""Hello""" });
            
            List<string> replaceStrings = _Fpr.GetAllReplaceStrings();

            Assert.AreEqual(1, replaceStrings.Count);
            Assert.AreEqual("Hello", replaceStrings[0]);            
        }

        [TestMethod]
        public void GetAllSearchStrings_2Line_Returns2SearchString()
        {
            SetUpParameterFile(new string[] { @"/S=hello", @"/S=world" });
            List<string> searchStrings = _Fpr.GetAllSearchStrings();
            Assert.AreEqual(2, searchStrings.Count);
            Assert.AreEqual("hello", searchStrings[0]);
            Assert.AreEqual("world", searchStrings[1]);            

        }

        [TestMethod]
        public void GetAllReplaceStrings_2Line_Returns2ReplaceString()
        {
            SetUpParameterFile(new string[] { @"/R=hello", @"/R=world" });
            List<string> replaceStrings = _Fpr.GetAllReplaceStrings();
            Assert.AreEqual(2, replaceStrings.Count);
            Assert.AreEqual("hello", replaceStrings[0]);
            Assert.AreEqual("world", replaceStrings[1]);

        }

        [TestMethod]
        public void GetAllReplaceStrings_2LineWithLParam_Returns2ReplaceString()
        {
            SetUpParameterFile(new string[] { @"/S=HELLO /L", @"/R=world" });
            List<string> replaceStrings = _Fpr.GetAllReplaceStrings();
            Assert.AreEqual(2, replaceStrings.Count);
            Assert.AreEqual("hello", replaceStrings[0]);
            Assert.AreEqual("world", replaceStrings[1]);

        }


    }
}

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
    /// Summary description for CommandLineParameterValueCollectionTest
    /// </summary>
    [TestClass]
    public class ApplicationParameterWithValueCollectionTest
    {        

        private TestContext testContextInstance;

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
        

        [TestMethod]
        public void CheckMissingParamsAreDetected()
        {
            CommandLineParameterCollection mandatoryParams = new CommandLineParameterCollection();
            mandatoryParams.Add(new CommandLineParameter("A", String.Empty, String.Empty, true));
            mandatoryParams.Add(new CommandLineParameter("B", String.Empty, String.Empty, false));

            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(mandatoryParams[1], String.Empty));



            Assert.AreEqual(1, ApplicationParameterValidator.GetMissingMandatoryParams(mandatoryParams, values).Count);
            Assert.AreEqual("A", ApplicationParameterValidator.GetMissingMandatoryParams(mandatoryParams, values)[0].GetName());

        }

        [TestMethod]
        public void GetReplaceString_WithLParam_WillReturnSearchStringInLowerCase()
        {
            CommandLineParameterCollection mandatoryParams = new CommandLineParameterCollection();
            mandatoryParams.Add(new CommandLineParameter("S", String.Empty, String.Empty, true));


            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(mandatoryParams[0], "HelloWorld"));
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("L", String.Empty, String.Empty, false), string.Empty));



            Assert.AreEqual("HelloWorld", values.GetSearchString()[0]);
            Assert.AreEqual("helloworld", values.GetReplaceString()[0]);
        }

        [TestMethod]
        public void GetSearchString_WithPParam_ReturnListOfSearchStringsFromFileSpecified()
        {

            string paramFile = TestHelper.CreateParameterFile(new string[1]{@"/S=""Book"" /R=""SmallBook"""});

            

            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("P", String.Empty, String.Empty, false), paramFile));

            Assert.AreEqual(1, values.GetSearchString().Count);
            Assert.AreEqual("Book", values.GetSearchString()[0]);

            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void GetReplaceString_WithPParam_ReturnListOfReplaceStringsFromFileSpecified()
        {

            string paramFile = TestHelper.CreateParameterFile(new string[] { @"/S=""Book"" /R=""SmallBook""" , @"/S=""Library"" /R=""Oxford""" });



            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("P", String.Empty, String.Empty, false), paramFile));
            
            Assert.AreEqual(2, values.GetReplaceString().Count);
            Assert.AreEqual("SmallBook", values.GetReplaceString()[0]);
            Assert.AreEqual("Oxford", values.GetReplaceString()[1]);

            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void GetReplaceString_WithPLParam_ReturnListOfReplaceStringsFromFileSpecifiedInLowerCase()
        {

            string paramFile = TestHelper.CreateParameterFile(new string[] { @"/S=""Book"" /R=""SmallBook""", @"/S=""Library"" /R=""Oxford""" });



            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("P", String.Empty, String.Empty, false), paramFile));
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("L", String.Empty, String.Empty, false), String.Empty));            

            Assert.AreEqual(2, values.GetReplaceString().Count);
            Assert.AreEqual("book", values.GetReplaceString()[0]);
            Assert.AreEqual("library", values.GetReplaceString()[1]);

            TestHelper.DeleteLastParameterFile();
        }
    }
}

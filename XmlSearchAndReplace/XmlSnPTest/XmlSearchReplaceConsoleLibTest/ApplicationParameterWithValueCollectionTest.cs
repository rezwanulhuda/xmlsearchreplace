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

            CommandLineParameterWithValueCollection param = new CommandLineParameterWithValueCollection();
            param.Add(new CommandLineParameterWithValue(mandatoryParams[0], "HelloWorld"));
            param.Add(new CommandLineParameterWithValue(new CommandLineParameter("L", String.Empty, String.Empty, false), string.Empty));
            ApplicationParameters values = new ApplicationParameters(param);


            Assert.AreEqual("HelloWorld", values.GetSearchString()[0]);
            Assert.AreEqual("helloworld", values.GetReplaceString()[0]);
        }

        [TestMethod]
        public void GetSearchString_WithPParam_ReturnListOfSearchStringsFromFileSpecified()
        {

            string paramFile = TestHelper.CreateParameterFile(new string[1]{@"/S=""Book"" /R=""SmallBook"""});

            

            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("P", String.Empty, String.Empty, false), paramFile));

            ApplicationParameters appParams = new ApplicationParameters(values);
            Assert.AreEqual(1, appParams.GetSearchString().Count);
            Assert.AreEqual("Book", appParams.GetSearchString()[0]);

            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void GetReplaceString_WithPParam_ReturnListOfReplaceStringsFromFileSpecified()
        {

            string paramFile = TestHelper.CreateParameterFile(new string[] { @"/S=""Book"" /R=""SmallBook""" , @"/S=""Library"" /R=""Oxford""" });



            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("P", String.Empty, String.Empty, false), paramFile));

            ApplicationParameters appParams = new ApplicationParameters(values);

            Assert.AreEqual(2, appParams.GetReplaceString().Count);
            Assert.AreEqual("SmallBook", appParams.GetReplaceString()[0]);
            Assert.AreEqual("Oxford", appParams.GetReplaceString()[1]);

            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void GetReplaceString_WithPLParam_ReturnListOfReplaceStringsFromFileSpecifiedInLowerCase()
        {

            string paramFile = TestHelper.CreateParameterFile(new string[] { @"/S=""Book"" /R=""SmallBook""", @"/S=""Library"" /R=""Oxford""" });



            CommandLineParameterWithValueCollection values = new CommandLineParameterWithValueCollection();
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("P", String.Empty, String.Empty, false), paramFile));
            values.Add(new CommandLineParameterWithValue(new CommandLineParameter("L", String.Empty, String.Empty, false), String.Empty));

            ApplicationParameters appParams = new ApplicationParameters(values);

            Assert.AreEqual(2, appParams.GetReplaceString().Count);
            Assert.AreEqual("book", appParams.GetReplaceString()[0]);
            Assert.AreEqual("library", appParams.GetReplaceString()[1]);

            TestHelper.DeleteLastParameterFile();
        }
    }
}

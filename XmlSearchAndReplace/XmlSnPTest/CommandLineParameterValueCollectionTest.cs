using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest
{
    /// <summary>
    /// Summary description for CommandLineParameterValueCollectionTest
    /// </summary>
    [TestClass]
    public class CommandLineParameterValueCollectionTest
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
            ApplicationParameterCollection mandatoryParams = new ApplicationParameterCollection();
            mandatoryParams.Add(new ApplicationParameter("A", String.Empty, String.Empty, true));
            mandatoryParams.Add(new ApplicationParameter("B", String.Empty, String.Empty, false));

            ApplicationParameterWithValueCollection values = new ApplicationParameterWithValueCollection();
            values.Add(new ApplicationParameterWithValue(mandatoryParams[1], String.Empty));

            Assert.AreEqual(1, values.GetMissingMandatoryParams(mandatoryParams).Count);
            Assert.AreEqual("A", values.GetMissingMandatoryParams(mandatoryParams)[0].GetName());

        }

        [TestMethod]
        public void GetReplaceString_WithLParam_WillReturnSearchStringInLowerCase()
        {
            ApplicationParameterCollection mandatoryParams = new ApplicationParameterCollection();
            mandatoryParams.Add(new ApplicationParameter("S", String.Empty, String.Empty, true));


            ApplicationParameterWithValueCollection values = new ApplicationParameterWithValueCollection();
            values.Add(new ApplicationParameterWithValue(mandatoryParams[0], "HelloWorld"));
            values.Add(new ApplicationParameterWithValue(new ApplicationParameter("L", String.Empty, String.Empty, false), string.Empty));



            Assert.AreEqual("HelloWorld", values.GetSearchString()[0]);
            Assert.AreEqual("helloworld", values.GetReplaceString()[0]);
        }

        [TestMethod, Ignore]
        public void GetSearchString_WithPParam_ReturnListOfSearchStringsFromFileSpecified()
        {            
        }
    }
}

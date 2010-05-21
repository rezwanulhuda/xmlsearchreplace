using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest.ValidatorTest
{
    /// <summary>
    /// Summary description for EnsureSearchParameterWithoutParamFileTest
    /// </summary>
    [TestClass]
    public class EnsureSearchParameterWithoutParamFileTest
    {

        private void SetupAndAssert(bool expectedResult, string commandLine)
        {
            EnsureSearchParameterWithoutParamFile validator = new EnsureSearchParameterWithoutParamFile();
            Assert.AreEqual(expectedResult, validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));
        }

        [TestMethod]
        public void IsValid_When_P_Missing_S_Present_ReturnsTrue()
        {
            SetupAndAssert(true, "/S=hello world");
        }

        [TestMethod]
        public void IsValid_When_P_Present_S_Present_ReturnsTrue()
        {
            SetupAndAssert(true, "/S=hello world /P");            
        }

        [TestMethod]
        public void IsValid_When_SP_Missing_ReturnsFalse()
        {
            SetupAndAssert(false, String.Empty);
        }
        
    }
}

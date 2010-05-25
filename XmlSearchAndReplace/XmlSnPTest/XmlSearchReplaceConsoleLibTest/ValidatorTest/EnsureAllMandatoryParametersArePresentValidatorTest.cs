using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest.ValidatorTest
{
    /// <summary>
    /// Summary description for EnsureAllMandatoryParametersArePresent
    /// </summary>
    [TestClass]
    public class EnsureAllMandatoryParametersArePresentValidatorTest
    {

        private void SetupAndAssert(bool expectedResult, string commandLine, params string[] mandatoryParams)
        {
            
            
            CommandLineParameterCollection mandatoryList = new CommandLineParameterCollection();
            
            foreach(string s in mandatoryParams)
            {
                mandatoryList.Add(new CommandLineParameter(s));
            }
            EnsureAllMandatoryParametersArePresentValidator validator = new EnsureAllMandatoryParametersArePresentValidator(mandatoryList);
            Assert.AreEqual(expectedResult, validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));
        }


        [TestMethod]
        public void IsValid_WithAllMandatoryParamsPresent_ReturnsTrue()
        {
            SetupAndAssert(true, "/P=abc /Q=bbc /R=ddd", "P", "Q", "R");
        }

        [TestMethod]
        public void IsValid_WithSomeMandatoryParamsPresent_ReturnsFalse()
        {
            SetupAndAssert(false, "/P /Q", "P", "Q", "R");
        }

        [TestMethod]
        public void IsValid_MandatoryParamsPresentButEmptyValue_ReturnsFalse()
        {
            SetupAndAssert(false, "/P", "P");
        }
    }
}

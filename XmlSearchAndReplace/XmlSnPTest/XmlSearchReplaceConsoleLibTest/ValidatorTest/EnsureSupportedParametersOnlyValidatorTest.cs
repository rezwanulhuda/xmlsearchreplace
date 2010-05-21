using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest
{   
    [TestClass]
    public class EnsureSupportedParametersOnlyValidatorTest
    {
        private void SetupAndAssert(bool expectedResult, string commandLine, params string[] supportedParams)
        {
            CommandLineParameterCollection supportedParamsColl = new CommandLineParameterCollection();

            foreach (string s in supportedParams)
            {
                supportedParamsColl.Add(new CommandLineParameter(s));
            }

            EnsureSupportedParametersOnlyValidator validator = new EnsureSupportedParametersOnlyValidator(supportedParamsColl, String.Empty);
            Assert.AreEqual(expectedResult, validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));

        }

        [TestMethod]
        public void Validate_WhenApplicationParameterContainsUnsupportedParameters_ReturnsFalse()
        {
            SetupAndAssert(false, "/S", "P");
        }

        [TestMethod]
        public void Validate_WhenApplicationParameterContainsSupportedParameters_ReturnsTrue()
        {
            SetupAndAssert(true, "/S", "S");
        }

        [TestMethod]
        public void Validate_WhenApplicationParameterContainsMoreUnsupportedParameters_ReturnsTrue()
        {
            SetupAndAssert(false, "/S /R", "P", "S");
        }
    }
}

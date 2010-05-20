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
    public class CheckSupportedParametersValidatorTest
    {
        [TestMethod]
        public void Validate_WhenApplicationParameterContainsUnsupportedParameters_ReturnsFalse()
        {
            string commandLineParams = "/S";
            CommandLineParameterWithValueCollection paramsWithValues = TestHelper.GetCommandLineParameters(commandLineParams);
            CommandLineParameterCollection supportedParams = new CommandLineParameterCollection();
            supportedParams.Add(new CommandLineParameter("P", String.Empty, String.Empty, false));

            CheckSupportedParametersValidator validator = new CheckSupportedParametersValidator(supportedParams);
            Assert.IsFalse(validator.Validate(paramsWithValues));


        }

        [TestMethod]
        public void Validate_WhenApplicationParameterContainsSupportedParameters_ReturnsTrue()
        {
            string commandLineParams = "/S";
            CommandLineParameterWithValueCollection paramsWithValues = TestHelper.GetCommandLineParameters(commandLineParams);
            CommandLineParameterCollection supportedParams = new CommandLineParameterCollection();
            supportedParams.Add(new CommandLineParameter("S", String.Empty, String.Empty, false));

            CheckSupportedParametersValidator validator = new CheckSupportedParametersValidator(supportedParams);
            Assert.IsTrue(validator.Validate(paramsWithValues));


        }
    }
}

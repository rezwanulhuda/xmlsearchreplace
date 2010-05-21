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
    public class SupportedParametersValidatorTest
    {
        [TestMethod]
        public void Validate_WhenApplicationParameterContainsUnsupportedParameters_ReturnsFalse()
        {
            string commandLineParams = "/S";
            CommandLineParameterWithValueCollection paramsWithValues = TestHelper.GetCommandLineParameters(commandLineParams);
            CommandLineParameterCollection supportedParams = new CommandLineParameterCollection();
            supportedParams.Add(new CommandLineParameter("P", String.Empty, String.Empty, false));

            SupportedParametersValidator validator = new SupportedParametersValidator(supportedParams, String.Empty);
            Assert.IsFalse(validator.IsValid(paramsWithValues));


        }

        [TestMethod]
        public void Validate_WhenApplicationParameterContainsSupportedParameters_ReturnsTrue()
        {
            string commandLineParams = "/S";
            CommandLineParameterWithValueCollection paramsWithValues = TestHelper.GetCommandLineParameters(commandLineParams);
            CommandLineParameterCollection supportedParams = new CommandLineParameterCollection();
            supportedParams.Add(new CommandLineParameter("S", String.Empty, String.Empty, false));

            SupportedParametersValidator validator = new SupportedParametersValidator(supportedParams, String.Empty);
            Assert.IsTrue(validator.IsValid(paramsWithValues));


        }
    }
}

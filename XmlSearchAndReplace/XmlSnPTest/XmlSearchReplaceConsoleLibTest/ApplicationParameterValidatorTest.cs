using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest
{
    /// <summary>
    /// Summary description for ApplicationParameterValidatorTest
    /// </summary>
    [TestClass]
    public class ApplicationParameterValidatorTest
    {
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
    }
}

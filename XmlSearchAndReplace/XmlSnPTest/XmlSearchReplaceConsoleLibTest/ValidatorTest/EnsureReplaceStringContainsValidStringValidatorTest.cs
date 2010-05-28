using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest.ValidatorTest
{
    [TestClass]
    public class EnsureReplaceStringContainsValidStringValidatorTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            TestHelper.DeleteLastParameterFile();
        }

        private void SetupAndAssert(bool expectedResult, string commandLine)
        {
            EnsureReplaceStringContainsValidStringValidator validator = new EnsureReplaceStringContainsValidStringValidator();
            Assert.AreEqual(expectedResult, validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));
        }

        [TestMethod]
        public void IsValid_When_O_ContainsEN_S_ContainsSpace_ReturnsFalse()
        {
            SetupAndAssert(false, "/O=en /R=hello world");
        }

        [TestMethod]
        public void IsValid_When_O_ContainsAN_S_ContainsSpace_ReturnsFalse()
        {
            SetupAndAssert(false, "/O=an /R=hello world");
        }

        [TestMethod]
        public void IsValid_When_O_ContainsANAV_S_Valid_ReturnsTrue()
        {
            SetupAndAssert(true, "/O=anav /R=helloworld");
        }


    }
}

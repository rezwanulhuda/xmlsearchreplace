using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest.ValidatorTest
{
    /// <summary>
    /// Summary description for EnsureEqualSearchReplaceStringValidatorTest
    /// </summary>
    [TestClass]
    public class EnsureEqualSearchReplaceStringValidatorTest
    {

        [TestCleanup]
        public void Cleanup()
        {
            TestHelper.DeleteLastParameterFile();
        }

        private void SetupAndAssert(bool expectedResult, string commandLine)
        {
            EnsureEqualSearchReplaceStringValidator validator = new EnsureEqualSearchReplaceStringValidator();
            Assert.AreEqual(expectedResult, validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));
        }

        private void SetupAndAssert(bool expectedResult, string additionalCommandLineWithoutP, string[] paramFileValues)
        {
            EnsureEqualSearchReplaceStringValidator validator = new EnsureEqualSearchReplaceStringValidator();
            string paramFile = TestHelper.CreateParameterFile(paramFileValues);
            string commandLine = additionalCommandLineWithoutP + String.Format("/P={0}", paramFile);            
            Assert.AreEqual(expectedResult, validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));
        }

        [TestMethod]
        public void IsValid_L_Missing_RS_Present_ShouldReturnTrue()
        {
            SetupAndAssert(true, "/S=hello /R=world");
        }

        [TestMethod]
        public void IsValid_LS_Present_R_Missing_ShouldReturnTrue()
        {
            
            string commandLine = "/S=hello /L";

            SetupAndAssert(true, commandLine);

        }

        [TestMethod]
        public void IsValid_P_PresentWithAllSearchReplaceStrings_RSL_Missing_ShouldReturnTrue()
        {
            SetupAndAssert(true, String.Empty, new String[] { "/R=abc /S=bbc", "/R=1 /S=2" });            
        }

        [TestMethod]
        public void IsValid_P_PresentWithSomeReplaceStringMissing_RSL_Missing_ShouldReturnFalse()
        {
            SetupAndAssert(false, String.Empty, new String[] { "/R=abc /S=bbc", "/S=2" });
        }

        [TestMethod]
        public void IsValid_PL_PresentWithAllReplaceStringMissing_RSL_Missing_ShouldReturnTrue()
        {            
            SetupAndAssert(true, "/L", new String[] { "/S=bbc", "/S=2" });
        }    
    }
}
